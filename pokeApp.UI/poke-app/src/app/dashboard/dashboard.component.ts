import { Component, Inject, OnInit, Renderer2 } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { DOCUMENT } from '@angular/common';
import { User } from '../User';
import { HttpClient } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';
import { TradeRecord } from '../TradeRecord';

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
    private http: HttpClient
  ) {}
  profileJson: string = '';
  user: User = {
    userID: -1,
    userName: '',
    password: '',
    email: '',
  };
  trades: TradeRecord[] = [];
  ngOnInit(): void {
    // this.renderer.setStyle(document.body, 'background-color', 'white');
    this.auth.user$.subscribe(
      (profile) => (this.profileJson = JSON.stringify(profile, null, 2))
    );

    // this.http
    //   .post<User>('https://211115pokemonapp.azurewebsites.net/api/user', {
    //     title: 'Added User to database',
    //   })
    //   .subscribe((data) => {
    // (this.user.userID = data.userID),
    //   (this.user.userName = data.userName),
    //   (this.user.password = data.password),
    //   (this.user.email = data.email);
    //   });
    lastValueFrom(
      this.http.get<TradeRecord[]>(
        'https://211115pokemonapp.azurewebsites.net/api/trade/getall'
      )
    ).then((data) => {
      this.trades = data;
    });
  }
  logout(): void {
    console.log(this.doc.location);
    this.auth.logout({ returnTo: this.doc.location.origin });
    alert('Successfully logout!');
  }
}
