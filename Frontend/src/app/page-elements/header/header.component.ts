import { TokenService } from 'src/app/services/auth/token.service';
import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { PlayerService } from 'src/app/services/player/player.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor(private jwtHelper: JwtHelperService, private tokenService: TokenService, private playerService: PlayerService) { }

  isUserAuthenticated(){
    const token: string = sessionStorage.getItem("jwtToken");
    if(token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    else{
      return false;
    }
  }

  hasRole(rolesStr: string){
    let role = this.tokenService.getRole();
    let roles = rolesStr.split(',');
    return roles.includes(role);
  }

  isPlayerNotActivated(){
    return sessionStorage.getItem("notActivated")=="1";
  }

  logOut(){
    this.tokenService.removeTokens();
  }

  resendLink()
  {
    this.playerService.resendActivationLink().subscribe(x=>
      {
        alert("Link resended");
      }
    )
  }

  ngOnInit(): void {
  }

}
