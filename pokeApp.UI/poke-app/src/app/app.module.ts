import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { AppRoutingModule } from './app-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ProfileComponent } from './profile/profile.component';
import { CollectionComponent } from './collection/collection.component';
import { TradehistoryComponent } from './tradehistory/tradehistory.component';
import { ModalModule } from './_modal';

import { AuthModule } from '@auth0/auth0-angular';
import { environment as env } from '../environments/environment';
import { HttpClientModule } from '@angular/common/http';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { TradesearchComponent } from './tradesearch/tradesearch.component';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSelectModule } from '@angular/material/select';
import { MatListModule } from '@angular/material/list';
import { RequestComponent } from './request/request.component';
import { MatTableModule } from '@angular/material/table';
import { MatBadgeModule } from '@angular/material/badge';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    DashboardComponent,
    ProfileComponent,
    CollectionComponent,
    TradehistoryComponent,
    TradesearchComponent,
    RequestComponent,
  ],
  imports: [
    MatCardModule,
    ModalModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    MatGridListModule,
    MatSlideToggleModule,
    MatSelectModule,
    MatListModule,
    MatTableModule,
    MatBadgeModule,
    AuthModule.forRoot({
      ...env.auth,
    }),
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
