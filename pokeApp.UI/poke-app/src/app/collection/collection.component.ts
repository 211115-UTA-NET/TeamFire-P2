import { Component, Input, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { CardService } from '../card.service';
import { ProfileComponent } from '../profile/profile.component';
import { Card } from '../Card';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../user.service';
import { ThemePalette } from '@angular/material/core';

@Component({
  selector: 'app-collection',
  templateUrl: './collection.component.html',
  styleUrls: ['./collection.component.css'],
})
export class CollectionComponent implements OnInit {
  constructor(
    private location: Location,
    private cardService: CardService,
    private routeInfo: ActivatedRoute,
    private userService: UserService
  ) {}
  @Input() userCards: Card[] = [];
  isTradable = false;
  ngOnInit(): void {
    // this.GetUserCards();
    console.log(this.routeInfo.snapshot);
  }
  goBack(): void {
    this.location.back();
  }
  // GetUserCards() {
  //   console.log(this.userService.userid);
  //   this.cardService.GetUserCards(this.userService.userid).then((data) => {
  //     this.userCards = data;
  //   });
  // }
  IsTradable(trading: number): boolean {
    if (trading == 1) return true;
    else return false;
  }

  tradableToggle(cardid: number) {
    return this.cardService.TradableToggle(cardid).subscribe((data) => {
      console.log(data);
      if (data[0].trading == 1) {
        alert('Trading On');
        location.reload();
      } else {
        alert('Trading off');
        location.reload();
      }
    });
  }
}
