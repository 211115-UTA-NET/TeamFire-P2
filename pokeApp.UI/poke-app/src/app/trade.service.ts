import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { lastValueFrom, Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { TradeRecord } from './TradeRecord';
import { MessageService } from './message.service'


@Injectable({
  providedIn: 'root',
})
export class TradeService {
  private tradeUrl = 'https://211115pokemonapp.azurewebsites.net/api/trade/'; // URL to web api

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  constructor(private http: HttpClient,
              private messageService: MessageService ) { }

  /** GET trades from the server */
  getTrades(): Observable<TradeRecord[]> {
    return this.http.get<TradeRecord[]>(this.tradeUrl)

      //combine tap and catchError into one function
      .pipe(
        //pass message to messageservice
        tap(_ => this.log('got trades')),
        catchError(this.handleError<TradeRecord[]>('getTrades', []))
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

    return this.http.get<TradeRecord[]>(`${this.tradeUrl}/?pokemon=${term}`)
      .pipe(
        tap(x => x.length ?
          this.log(`found trade containing pokemon "${term}"`) :
          this.log(`no trades containing pokemon "${term}`)),
        catchError(this.handleError<TradeRecord[]>('searchTradesbyPokemon', []))
      );
  }

  searchTradesbyUser(term: string): Observable<TradeRecord[]> {
    if (!term.trim()) {

      return of([]);
    }

    return this.http.get<TradeRecord[]>(`${this.tradeUrl}/?offeredBy=${term}`)
      .pipe(
        tap(x => x.length ?
          this.log(`found trade intiated by "${term}"`) :
          this.log(`no trades intiated by "${term}`)),
        catchError(this.handleError<TradeRecord[]>('searchTradesbyUser', []))
      );
  }

  GetTradeInfo() {
    // GET
    const url = `${this.tradeUrl}/getall`;
    return lastValueFrom(this.http.get<TradeRecord[]>(url));
  }

  private log(message: string) {
    this.messageService.add(`tradeService: ${message}`);
  }

}
