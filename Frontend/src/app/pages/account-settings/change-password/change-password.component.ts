import { ChangePasswordRequest } from './../../../models/player/player.requests';
import { PlayerService } from './../../../services/player/player.service';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TokenService } from 'src/app/services/auth/token.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {

  constructor(private http: HttpClient, private tokenService: TokenService, private playerService: PlayerService) { }

  oldPass: string
  newPass: string
  newPass2: string

  ngOnInit(): void {
  }

  changePassword(){

      var changeModel: ChangePasswordRequest=
      {
        PlayerId: this.tokenService.getPlayerId(),
        OldPassword: this.oldPass,
        NewPassword: this.newPass,
        RepeatedNewPassword: this.newPass2
      }

      this.playerService.changePassword(changeModel)
      .subscribe(data=>{
        alert("Password changed");
      });
  }
}
