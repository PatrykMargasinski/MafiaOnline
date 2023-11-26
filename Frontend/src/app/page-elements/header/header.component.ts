import { TokenService } from 'src/app/services/auth/token.service';
import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { PlayerService } from 'src/app/services/player/player.service';
import { MessageService } from 'src/app/services/message/message.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor(private jwtHelper: JwtHelperService, private tokenService: TokenService, private playerService: PlayerService, private messageService: MessageService) { }

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
    let roles = this.tokenService.getRoles();
    let requiredRoles : Array<string> = rolesStr.split(',');
    return roles.some(item => requiredRoles.includes(item));
  }

  isPlayerNotActivated(){
    return sessionStorage.getItem("notActivated")=="1";
  }

  hasUnseenMessages(){
    return sessionStorage.getItem("unseenMessages")=="1";
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
