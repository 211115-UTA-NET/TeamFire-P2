import { Component, OnInit } from '@angular/core';
import { TradeRecord } from '../TradeRecord';
import { TradeService } from '../trade.service'
import { Observable, Subject } from 'rxjs';

import {
    debounce,
  debounceTime, distinctUntilChanged, switchMap
} from 'rxjs/operators';


@Component({
  selector: 'app-trade-search',
  templateUrl: './trade-search.component.html',
  styleUrls: ['./trade-search.component.css']
})
export class TradeSearchComponent implements OnInit {

  trades$!: Observable<TradeRecord[]>;
  private searchTerms = new Subject<string>();

  constructor(private tradeService : TradeService) { }

  search(term: string): void {
    this.searchTerms.next(term);
  }
  ngOnInit(): void {

    this.trades$ = this.searchTerms.pipe(

      debounceTime(400),
      distinctUntilChanged(),
      switchMap((term: string) => this.tradeService.searchTradesbyPokemon(term)),

    );
  }

}
