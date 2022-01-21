import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { TradeService } from '../trade.service';
import { TradeRecord } from '../TradeRecord'
@Component({
  selector: 'app-tradehistory',
  templateUrl: './tradehistory.component.html',
  styleUrls: ['./tradehistory.component.css'],
})
export class TradehistoryComponent implements OnInit {


  trades: TradeRecord[] = [];

  constructor(private location: Location,
    private tradeService: TradeService) { }


  ngOnInit(): void {
    this.getTrades();
  }

  getTrades(): void {
    this.tradeService.getTrades()
      .subscribe(trades => this.trades);
  }
  goBack(): void {
    this.location.back();
  }
}
