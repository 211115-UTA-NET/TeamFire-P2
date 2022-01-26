import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { lastValueFrom, Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { AuthService } from '@auth0/auth0-angular';
import { UserDto } from './UserDto';
import { User } from './User';

@Injectable({
  providedIn: 'root',
})
export class UserService implements OnInit {
  private userUrl = 'https://211115pokemonapp.azurewebsites.net/api/user';
  constructor(private http: HttpClient, public auth: AuthService) {}
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };
  userid: number = -1;
  ngOnInit(): void {}

  addUser(userInfo: UserDto) {
    return lastValueFrom(
      this.http.post<User[]>(this.userUrl, userInfo, this.httpOptions)
    );
  }
}
