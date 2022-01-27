
import { Location } from '@angular/common';
import { TradeService } from '../trade.service';
import { TradeRecord } from '../TradeRecord';
import { Observable, Subject } from 'rxjs';
import { AuthService } from '@auth0/auth0-angular';
import { DOCUMENT } from '@angular/common';
import { Component, Inject, Injectable, OnInit } from '@angular/core';

@Component({
  selector: 'app-tradehistory',
  templateUrl: './tradehistory.component.html',
  styleUrls: ['./tradehistory.component.css'],
})
export class TradehistoryComponent implements OnInit {
  trades: TradeRecord[] = [];

  constructor(private location: Location,
              private tradeService: TradeService,
              public auth: AuthService,
              @Inject(DOCUMENT) private doc: Document,) { }

  ngOnInit(): void {
    this.getTrades();
  }
  goBack(): void {
    this.location.back();
  }

  getTrades(): void {
    this.tradeService.getTrades().subscribe((trades) => (this.trades = trades));
  }

  isShow: boolean = false;
  logout(): void {
    console.log(this.doc.location);
    this.auth.logout({ returnTo: this.doc.location.origin });
    alert('Successfully logout!');
  }
}
