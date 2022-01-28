import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { lastValueFrom, Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { TradeRecord } from './TradeRecord';
import { MessageService } from './message.service';
import { Requests } from './Requests';
import { Card } from './Card';

@Injectable({
  providedIn: 'root',
})
export class TradeService {
  private tradeUrl = 'https://211115pokemonapp.azurewebsites.net/api/trade'; // URL to web api

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  constructor(
    private http: HttpClient,
    private messageService: MessageService
  ) {}

  /** GET trades from the server */
  getTrades(): Observable<TradeRecord[]> {
    return (
      this.http
        .get<TradeRecord[]>(`${this.tradeUrl}/GetAll`)

        //combine tap and catchError into one function
        .pipe(
          //pass message to messageservice
          tap((_) => this.log('got trades')),
          catchError(this.handleError<TradeRecord[]>('getTrades', []))
        )
    );
  }

  /*SOURCE TOUR OF HEROES*/
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  searchTradesbyPokemon(term: string): Observable<TradeRecord[]> {
    if (!term.trim()) {
      return of([]);
    }

    return this.http
      .get<TradeRecord[]>(`${this.tradeUrl}?pokemon=${term}`)
      .pipe(
        tap((x) =>
          x.length
            ? this.log(`found trade containing pokemon "${term}"`)
            : this.log(`no trades containing pokemon "${term}`)
        ),
        catchError(this.handleError<TradeRecord[]>('searchTradesbyPokemon', []))
      );
  }

  searchTradesbyUser(term: string): Observable<TradeRecord[]> {
    if (!term.trim()) {
      return of([]);
    }

    return this.http
      .get<TradeRecord[]>(`${this.tradeUrl}/GetByName?name=${term}`)
      .pipe(
        tap((x) =>
          x.length
            ? this.log(`found trade intiated by "${term}"`)
            : this.log(`no trades intiated by "${term}`)
        ),
        catchError(this.handleError<TradeRecord[]>('searchTradesbyUser', []))
      );
  }

  private log(message: string) {
    this.messageService.add(`tradeService: ${message}`);
  }

  // ----------- Check is Card still tradable in trading center ---------
  public CheckTradable(cardid: number) {
    return this.http.get<boolean>(
      `${this.tradeUrl}/isTradable?cardId=${cardid}`
    );
  }

  public SendTradeRequest(
    cardid: number,
    userid: number,
    offercardid: number,
    targetUserID: number
  ) {
    let body = {
      cardID: cardid,
      userID: userid,
      offerCardID: offercardid,
      targetUserID: targetUserID,
    };
    var url = this.tradeUrl + '/tradeRequest';
    return this.http.post<number>(url, body, this.httpOptions);
  }

  public GetSendRequest(userid: number) {
    return this.http.get<Requests[]>(
      `${this.tradeUrl}/sendrequest?userid=${userid}`
    );
  }

  public GetReceivedRequest(userid: number) {
    return this.http.get<Requests[]>(
      `${this.tradeUrl}/receivedrequest?userid=${userid}`
    );
  }

  public UpdataRequestStatus(requestid: number, status: string) {
    let body = {
      requestID: requestid,
      requestStatus: status,
    };
    return this.http.put<number>(
      `${this.tradeUrl}/UpdateStatus`,
      body,
      this.httpOptions
    );
  }

  public AddCompletedTrade(offerbyid: number, redeembyid: number) {
    let body = {
      offeredByID: offerbyid,
      recevedByID: redeembyid,
    };
    return this.http.post<number>(
      `${this.tradeUrl}/AddCompletedTrade`,
      body,
      this.httpOptions
    );
  }

  public AddTradeDetail(tradeid: number, cardid: number, offerbyid: number) {
    let body = {
      tradeId: tradeid,
      cardId: cardid,
      offeredId: offerbyid,
    };
    return this.http.post<TradeRecord[]>(
      `${this.tradeUrl}/AddTradeDetail`,
      body,
      this.httpOptions
    );
  }
}
