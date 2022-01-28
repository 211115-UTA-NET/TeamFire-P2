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

  GetUserCards(userid: number) {
    const url = `${this.cardUrl}?userid=${userid}`;
    // console.log(url);
    return lastValueFrom(this.http.get<Card[]>(url));
  }

  public CheckCardOwner(carid: number) {
    return this.http.get<Card[]>(`${this.cardUrl}/checkInfo?cardID=${carid}`);
  }

  public UpdateCardOwner(carid: number, newOwnerid: number) {
    let body = {
      userId: newOwnerid,
      cardId: carid,
    };
    return this.http.put<Card[]>(
      `${this.cardUrl}/updateOwner`,
      body,
      this.httpOptions
    );
  }

  public TradableToggle(cardid: number) {
    let body = {
      cardId: cardid,
    };
    return this.http.put<Card[]>(
      `${this.cardUrl}/toggleTradable`,
      body,
      this.httpOptions
    );
  }

  public DrawCard(userid: number) {
    let body = {
      userId: userid,
    };
    return this.http.post<Card[]>(
      `${this.cardUrl}/NewCard`,
      body,
      this.httpOptions
    );
  }
}
