import {
  Component,
  Inject,
  Injectable,
  OnInit,
  Renderer2,
} from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { DOCUMENT } from '@angular/common';
import { CardService } from '../card.service';
import { Card } from '../Card';
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
    private cardService: CardService
  ) {}
  profileJson: string = '';
  /**
   * type assertion on user -> will autocomplete Employee properties
   * Compiler will provide autocomplete properties,
   * but will not give an error if you forgot to add the properties
   **/
  tradableCards: Card[] = [];
  params: any;
  ngOnInit(): void {
    this.GetTradableCards();
  }

  GetTradableCards(): void {
    this.cardService.GetTradableCards().then((data) => {
      this.tradableCards = data;
    });
  }

  logout(): void {
    console.log(this.doc.location);
    this.auth.logout({ returnTo: this.doc.location.origin });
    alert('Successfully logout!');
  }
}
