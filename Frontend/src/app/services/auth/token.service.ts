import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor(private jwtHelper: JwtHelperService) { }

  getBossId(){
    const decoded = this.jwtHelper.decodeToken(sessionStorage.getItem("jwtToken"));
    return Number(decoded.bossId);
  }

  getPlayerId(){
    const decoded = this.jwtHelper.decodeToken(sessionStorage.getItem("jwtToken"));
    return Number(decoded.playerId);
  }

  getLogin(){
    const decoded = this.jwtHelper.decodeToken(sessionStorage.getItem("jwtToken"));
    return decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
  }

  getRoles(){
    const decoded = this.jwtHelper.decodeToken(sessionStorage.getItem("jwtToken"));
    var roles = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    if (typeof roles === 'string')
      roles = [roles];
    return roles;
  }

  removeTokens(){
    sessionStorage.removeItem("jwtToken");
    sessionStorage.removeItem("refreshToken");
  }
}
