import { TokenService } from 'src/app/services/auth/token.service';
import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor(private jwtHelper: JwtHelperService, private tokenService: TokenService) { }

  isUserAuthenticated(){
    const token: string = sessionStorage.getItem("jwtToken");
    if(token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    else{
      return false;
    }
  }

  logOut(){
    this.tokenService.removeTokens();
  }

  ngOnInit(): void {
  }

}
