import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class GuardService implements CanActivate{

  constructor(private router: Router, private jwtHelper: JwtHelperService, private http: HttpClient) { }

  async canActivate(){
    const token = sessionStorage.getItem("jwtToken");
    if(token && !this.jwtHelper.isTokenExpired(token)){
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
    const credentials = JSON.stringify({ accessToken: token, refreshToken: refreshToken });
    let isRefreshSuccess: boolean;
    try {
      const response = await this.http.post(environment.APIEndpoint+"/token/refresh", credentials, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        }),
        observe: 'response'
        }).toPromise();
      const newToken = (<any>response).body.accessToken;
      const newRefreshToken = (<any>response).body.refreshToken;
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
