import { Component, OnInit, Renderer2 } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  constructor(public auth: AuthService, private renderer: Renderer2) {}

  ngOnInit(): void {
    // document.body.classList.add('loginbg');
    // document.body.classList.add('container-fluid');
    this.renderer.setStyle(
      document.body,
      'background-image',
      'url(../../assets/login-background.jpg)'
    );
    this.renderer.setStyle(document.body, 'background-size', 'cover');
    // this.renderer.setStyle(document.body, 'background-repeat', 'no-repeat');
  }

  loginWithRedirect(): void {
    this.auth.loginWithRedirect({
      appState: { target: '/dashboard' },
    });
  }
}
