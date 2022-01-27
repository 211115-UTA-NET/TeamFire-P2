
import { TradeRecord } from '../TradeRecord';
import { TradeService } from '../trade.service'
import { Observable, Subject } from 'rxjs';
import { AuthService } from '@auth0/auth0-angular';
import { DOCUMENT } from '@angular/common';
import { Component, Inject, Injectable, OnInit } from '@angular/core';

import {
  debounce,
  debounceTime, distinctUntilChanged, switchMap
} from 'rxjs/operators';


@Component({
  selector: 'app-trade-search',
  templateUrl: './tradesearch.component.html',
  styleUrls: ['./tradesearch.component.css']
})
export class TradesearchComponent implements OnInit {

  trades$!: Observable<TradeRecord[]>;
  private searchTerms = new Subject<string>();

  constructor(private tradeService: TradeService,
              public auth: AuthService,
               @Inject(DOCUMENT) private doc: Document,  ) { }

  search(term: string): void {
    this.searchTerms.next(term);
  }
  ngOnInit(): void {

    this.trades$ = this.searchTerms.pipe(

      debounceTime(400),
      distinctUntilChanged(),
      switchMap((term: string) => this.tradeService.searchTradesbyUser(term)),

    );
  }

  logout(): void {
    console.log(this.doc.location);
    this.auth.logout({ returnTo: this.doc.location.origin });
    alert('Successfully logout!');
  }

}
