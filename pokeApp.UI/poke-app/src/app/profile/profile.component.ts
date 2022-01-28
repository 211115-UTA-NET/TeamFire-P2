import { Component, Injectable, Input, OnInit, Output } from '@angular/core';
import { Location } from '@angular/common';
import { AuthService } from '@auth0/auth0-angular';
import { User } from '../User';
import { UserService } from '../user.service';
import { UserDto } from '../UserDto';
import { Card } from '../Card';

@Injectable({
  providedIn: 'root',
})
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  constructor(private location: Location, public auth: AuthService) {}

  @Input() dbUserInfo: User[] = [];
  @Input() userCards: Card[] = [];
  ngOnInit(): void {
    // this.auth.user$.subscribe(
    //   (profile) => (this.profileJson = JSON.stringify(profile, null, 2))
    // );
    // this.AddUser();
    console.log(this.dbUserInfo);
  }

  // AddUser(): void {
  //   this.auth.user$.subscribe((profile) => {
  //     let body = <UserDto>{
  //       name: profile?.nickname,
  //       pw: 'psw',
  //       email: profile?.email,
  //     };
  //     // debugger;
  //     return this.userService.addUser(body).then((data) => {
  //       // console.log(data);
  //       // debugger;
  //       this.dbUserInfo = data;
  //       this.userService.userid = data[0].userID;
  //       // debugger;
  //     });
  //   });
  // }
  buttonName: string = 'My Pokemon Collection';
  isShow: boolean = true;
  goBack(): void {
    this.location.back();
  }
  toggle(): void {
    this.isShow = !this.isShow;
    if (!this.isShow) this.buttonName = 'Profile';
    else this.buttonName = 'My Pokemon Collection';
  }
}
