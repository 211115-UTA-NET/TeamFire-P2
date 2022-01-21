import { TradeRecord } from './TradeRecord';
import { HttpClientModule } from '@angular/common/http'
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { Injectable } from '@angular/core';
import { MessageService } from './message.service';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';


@Injectable({ providedIn: 'root' })
export class TradeService {

  private tradeUrl = 'api/TradeRecord'

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };


  constructor(
    private http: HttpClient,
    private messageService: MessageService,
    private location: Location  ) { }


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

  /*SOURCE: Tour of Heroes */
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
 

  private log(message: string) {
    this.messageService.add(`TradeService: ${message}`);
  }
 }


