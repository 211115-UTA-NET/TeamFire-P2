import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { TradeService } from '../trade.service';
import { TradeRecord } from '../TradeRecord';
import { Observable, Subject } from 'rxjs';

@Component({
  selector: 'app-tradehistory',
  templateUrl: './tradehistory.component.html',
  styleUrls: ['./tradehistory.component.css'],
})
export class TradehistoryComponent implements OnInit {
  trades: TradeRecord[] = [];

  constructor(private location: Location, private tradeService: TradeService) {}

  ngOnInit(): void {
    this.getTrades();
  }
  goBack(): void {
    this.location.back();
  }

  getTrades(): void {
    this.tradeService.getTrades().subscribe((trades) => (this.trades = trades));
  }
}
