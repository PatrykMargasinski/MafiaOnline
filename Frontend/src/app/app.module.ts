import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { JwtModule } from '@auth0/angular-jwt';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {HttpClientModule} from '@angular/common/http';
import {FormsModule} from '@angular/forms';
import { HeaderComponent } from './page-elements/header/header.component';
import { FooterComponent } from './page-elements/footer/footer.component';
import { LoginComponent } from './pages/basic-pages/login/login.component';
import { RegisterComponent } from './pages/basic-pages/register/register.component';
import { HomeComponent } from './pages/basic-pages/home/home.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CustomErrorHandler } from './handlers/customErrorHandler';
import { environment } from 'src/environments/environment';
import { BossComponent } from './pages/boss/boss.component';
import { AboutBossComponent } from './pages/boss/about-boss/about-boss.component';

export function tokenGetter(){
  return sessionStorage.getItem("jwtToken");
}

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    AboutBossComponent,
    BossComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatAutocompleteModule,
    HttpClientModule,
    FormsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: [environment.APIEndpoint],
        disallowedRoutes: []
      }
    }),
    NgbModule
  ],
  providers: [ { provide: ErrorHandler, useClass: CustomErrorHandler }],
  bootstrap: [AppComponent]
})
export class AppModule { }
