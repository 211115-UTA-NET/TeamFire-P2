import { Component, Input, OnInit } from '@angular/core';
import { Card } from '../Card';
import { CardService } from '../card.service';
import { Requests } from '../Requests';
import { TradeService } from '../trade.service';
import { User } from '../User';

@Component({
  selector: 'app-request',
  templateUrl: './request.component.html',
  styleUrls: ['./request.component.css'],
})
export class RequestComponent implements OnInit {
  constructor(
    private tradeService: TradeService,
    private cardService: CardService
  ) {}
  sendRequests: Requests[] = [];
  rcvRequests: Requests[] = [];
  @Input() dbUserInfo: User[] = [];
  @Input() userCards: Card[] = [];
  columnsToDisplay: string[] = [
    'requestID',
    'userID',
    'offerCardID',
    'cardID',
    'pokeID',
    'pokemon',
    // 'Timestamp',
    'status',
    'manageReq',
  ];

  columnsToDisplaySend: string[] = [
    'requestID',
    'userID',
    'offerCardID',
    'cardID',
    'pokeID',
    'pokemon',
    // 'Timestamp',
    'status',
  ];
  ngOnInit(): void {
    this.GetSendRequest();
    this.GetReceivedRequest();
  }
  badgenum: number | null = null;
  isShow: boolean = false;
  togglesend(): void {
    this.isShow = false;
  }
  togglercv(): void {
    this.isShow = true;
  }
  // Todo
  GetSendRequest() {
    return this.tradeService
      .GetSendRequest(this.dbUserInfo[0].userID)
      .subscribe((data) => {
        this.sendRequests = data;
        // console.log(this.sendRequests);
      });
  }
  // Todo
  GetReceivedRequest() {
    return this.tradeService
      .GetReceivedRequest(this.dbUserInfo[0].userID)
      .subscribe((data) => {
        this.rcvRequests = data;
        this.badgenum = this.badge();
        if (this.badgenum == 0) {
          this.badgenum = null;
        }
        console.log(this.rcvRequests);
      });
  }

  //accept trade
  AcceptRequest(
    requestID: number,
    status: string,
    redeemUserID: number,
    redeemCardID: number, // what i get from anthor user
    offerCardID: number // I offer this card id
  ) {
    if (status == 'Accepted') {
      alert('This trade request is no longer avaliable...');
    } else if (status == 'Rejected') {
      alert(
        'The trade request has been rejected. This trade request no longer avaliable.'
      );
    } else if (status == 'Expired') {
      alert('This trade request is no longer avaliable...');
    } else {
      this.cardService.CheckCardOwner(offerCardID).subscribe((data) => {
        if (data[0].userID == this.dbUserInfo[0].userID) {
          console.log("I'm the card owner");
          this.cardService.CheckCardOwner(redeemCardID).subscribe((data) => {
            // redeemer is the redeem card owner
            if (data[0].userID == redeemUserID) {
              if (status == 'pending') {
                this.UpdateRequest(
                  requestID,
                  offerCardID,
                  redeemCardID,
                  redeemUserID
                );
              }
            } else {
              this.tradeService
                .UpdataRequestStatus(requestID, 'Expired')
                .subscribe((_) => {
                  alert(
                    'The redeemer are no longer the card owner. Trade request expired...'
                  );
                  location.reload();
                });
            }
          });
        } else {
          this.tradeService
            .UpdataRequestStatus(requestID, 'Expired')
            .subscribe((_) => {
              alert(
                'You are no longer the card owner. Trade request expired...'
              );
              location.reload();
            });
        }
      });
    }
  }

  UpdateRequest(
    requestid: number,
    offerCardid: number,
    redeemCardid: number,
    redeemUserid: number
  ) {
    //update the request status
    return this.tradeService
      .UpdataRequestStatus(requestid, 'Accepted')
      .subscribe((data) => {
        if (data == 1) {
          alert('You Accepted the Trade!');
          // add the trade record
          this.AddCompletedTrade(
            this.dbUserInfo[0].userID,
            redeemUserid
          ).subscribe((tradeid) => {
            // add trade details for offered user
            this.AddTradeDetails(
              tradeid,
              offerCardid,
              this.dbUserInfo[0].userID
            ).subscribe((data) => {
              // update card owner...
              this.UpdateCardOwner(offerCardid, redeemUserid).subscribe(
                (data) => {
                  // console.log('change owner: ' + data);
                  // add trade details for redeemed user
                  this.AddTradeDetails(
                    tradeid,
                    redeemCardid,
                    redeemUserid
                  ).subscribe((data) => {
                    // finally... update card owner...
                    this.UpdateCardOwner(
                      redeemCardid,
                      this.dbUserInfo[0].userID
                    ).subscribe((data) => {
                      // console.log('change owner: ' + data);
                      alert('Trasaction is complete. Refreshing the page...');
                      location.reload();
                    });
                  });
                }
              );
            });
          });
        } else {
          alert(
            'You already Accepted the trade. This trade request no longer avaliable.'
          );
        }
      });
  }

  AddCompletedTrade(offerbyid: number, redeembyid: number) {
    return this.tradeService.AddCompletedTrade(offerbyid, redeembyid);
  }

  // who gave what
  AddTradeDetails(tradeid: number, Cardid: number, Userid: number) {
    return this.tradeService.AddTradeDetail(tradeid, Cardid, Userid);
  }

  UpdateCardOwner(carid: number, newOwnerid: number) {
    return this.cardService.UpdateCardOwner(carid, newOwnerid);
  }

  // reject trade request
  RejectRequest(requestID: number, status: string) {
    if (status == 'Accepted') {
      alert('This trade request is no longer avaliable...');
    } else if (status == 'Rejected') {
      alert('You already rejected the trade request...');
    } else if (status == 'Expired') {
      alert('This trade request is expired...');
    } else {
      this.tradeService
        .UpdataRequestStatus(requestID, 'Rejected')
        .subscribe((data) => {
          if (data == 1) {
            alert('You successfully rejected the trade request!');
            location.reload();
          } else {
            alert('This trade request is no longer avaliable...');
          }
        });
    }
  }

  badge(): number {
    return this.rcvRequests.filter((req) => req.status == 'pending').length;
  }
}
