import { Router } from '@angular/router';
import { PlayerService } from './../../../services/player/player.service';
import { DeleteAccountRequest } from './../../../models/player/player.requests';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TokenService } from 'src/app/services/auth/token.service';

@Component({
  selector: 'app-delete-account',
  templateUrl: './delete-account.component.html',
  styleUrls: ['./delete-account.component.css']
})
export class DeleteAccountComponent implements OnInit {
  password: string

  constructor(private http: HttpClient, private tokenService: TokenService, private playerService: PlayerService, private router: Router) { }

  ngOnInit(): void {
  }

  deleteAccount()
  {
    if(confirm('Are you sure you want to delete your account?'))
    {
      const playerId = this.tokenService.getPlayerId();
      const request: DeleteAccountRequest =
      {
        Password: this.password,
        PlayerId: playerId
      }
      this.playerService.deleteAccount(request).subscribe
      (
        x=>
        {
          alert("Your account was successfully deleted. Thank you for trying Mafia Online")
          this.logOut();
        }
      )
    }
  }

  logOut(){
    this.tokenService.removeTokens();
    this.router.navigateByUrl('/')
  }

}
