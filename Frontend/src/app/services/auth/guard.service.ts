import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { templateJitUrl } from '@angular/compiler';
import { MessageService } from '../message/message.service';

@Injectable({
  providedIn: 'root'
})
export class GuardService implements CanActivate{

  constructor(private router: Router, private jwtHelper: JwtHelperService, private http: HttpClient, private message: MessageService) { }

  async canActivate(){
    const token = sessionStorage.getItem("jwtToken");

    //message refreshing
    let hasUnreadMessages = this.message.hasUnseenMessages().subscribe(
      x=>
      {
        if(x == true)
          sessionStorage.setItem("unseenMessages", "1");
        else
          sessionStorage.setItem("unseenMessages", "0");
      }
    )

    if(token && !this.jwtHelper.isTokenExpired(token, 60)){
      return true
    }

    const isRefreshSuccess = await this.tryRefreshingTokens(token);
    if (!isRefreshSuccess) {
      this.router.navigateByUrl("");
    }
      return isRefreshSuccess;

  }

  async tryRefreshingTokens(token: string): Promise<boolean> {
    const refreshToken: string = sessionStorage.getItem("refreshToken");
    if (!token || !refreshToken) {
      return false;
    }
    const credentials = JSON.stringify({ Token: token, RefreshToken: refreshToken });
    let isRefreshSuccess: boolean;
    try {
      const response = await this.http.post(environment.APIEndpoint+"/refreshToken", credentials, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        }),
        observe: 'response'
        }).toPromise();
      const newToken = (<any>response).body.Token;
      const newRefreshToken = (<any>response).body.RefreshToken;
      sessionStorage.setItem("jwtToken", newToken);
      sessionStorage.setItem("refreshToken", newRefreshToken);
      isRefreshSuccess = true;
    }
    catch (ex) {
      isRefreshSuccess = false;
    }
    return isRefreshSuccess;
  }
}
