import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { lastValueFrom, Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { Card } from './Card';

@Injectable({
  providedIn: 'root',
})
export class CardService {
  private cardUrl = 'https://211115pokemonapp.azurewebsites.net/api/card'; // URL to web api

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  constructor(private http: HttpClient) {}

  GetTradableCards() {
    const url = `${this.cardUrl}/trading`;
    return lastValueFrom(this.http.get<Card[]>(url));
  }
}
