import {
  Component,
  Inject,
  Injectable,
  OnInit,
  Renderer2,
} from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { DOCUMENT } from '@angular/common';
import { User } from '../User';
import {
  HttpClient,
  HttpParams,
  HttpParamsOptions,
} from '@angular/common/http';
import { lastValueFrom } from 'rxjs';
import { TradeRecord } from '../TradeRecord';
import { TradeService } from '../trade.service';
@Injectable({
  providedIn: 'root',
})
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  constructor(
    public auth: AuthService,
    @Inject(DOCUMENT) private doc: Document,
    private renderer: Renderer2,
    private http: HttpClient,
    private tradeService: TradeService
  ) {}
  profileJson: string = '';
  /**
   * type assertion on user -> will autocomplete Employee properties
   * Compiler will provide autocomplete properties,
   * but will not give an error if you forgot to add the properties
   **/
  trades: TradeRecord[] = [];
  params: any;
  ngOnInit(): void {
    this.GetTrades();
  }

  GetTrades(): void {
    this.tradeService.GetTradeInfo().then((data) => {
      this.trades = data;
    });
  }

  logout(): void {
    console.log(this.doc.location);
    this.auth.logout({ returnTo: this.doc.location.origin });
    alert('Successfully logout!');
  }
}
