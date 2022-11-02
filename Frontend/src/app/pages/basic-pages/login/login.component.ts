import { LoginRequest } from './../../../models/player/player.requests';
import { PlayerService } from './../../../services/player/player.service';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  invalidLogin: boolean;
  errorMessage: string;

  constructor(private router: Router, private http: HttpClient, private playerService: PlayerService) { }

  login(form: NgForm) {
    const request: LoginRequest = {
      'Nick': form.value.nick,
      'Password': form.value.password
    }

    this.playerService.login(request)
      .subscribe(response => {
        const token = (<any>response).Token;
        const refreshToken = (<any>response).RefreshToken;
        sessionStorage.setItem("jwtToken", token);
        sessionStorage.setItem("refreshToken", refreshToken);
        this.invalidLogin = false;
        this.router.navigate(["/boss"]);
      })
  }
}
