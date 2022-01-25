import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { lastValueFrom, Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { TradeRecord } from './TradeRecord';

@Injectable({
  providedIn: 'root',
})
export class TradeService {
  private tradeUrl = 'https://211115pokemonapp.azurewebsites.net/api/trade/'; // URL to web api

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  constructor(private http: HttpClient) {}

  GetTradeInfo() {
    // GET
    const url = `${this.tradeUrl}/getall`;
    return lastValueFrom(this.http.get<TradeRecord[]>(url));
  }
}
