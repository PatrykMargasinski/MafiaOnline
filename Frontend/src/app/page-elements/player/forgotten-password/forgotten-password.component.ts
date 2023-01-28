import { PlayerService } from 'src/app/services/player/player.service';
import { CreateResetPasswordCodeRequest, ResetPasswordRequest } from './../../../models/player/player.requests';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-forgotten-password',
  templateUrl: './forgotten-password.component.html',
  styleUrls: ['./forgotten-password.component.css']
})
export class ForgottenPasswordComponent implements OnInit {

  constructor(private playerService: PlayerService) { }

  emailProvided: boolean;

  createResetPasswordCodeRequest: CreateResetPasswordCodeRequest
  resetPasswordRequest: ResetPasswordRequest

  ngOnInit(): void {
    this.emailProvided = false;
    this.createResetPasswordCodeRequest = {Email: ""}
    this.resetPasswordRequest = {Code: "", Password: "", RepeatedPassword: ""}
  }

  createResetPasswordCode()
  {
    console.log(this.createResetPasswordCodeRequest)
    this.playerService.createResetPasswordCode(this.createResetPasswordCodeRequest).subscribe
    (
      x=>
      {
        alert("Code sent");
        this.emailProvided = true;
      }
    );
  }

  resetPassword()
  {
    this.playerService.resetPassword(this.resetPasswordRequest).subscribe
    (
      x=>alert("Password reseted")
    )
  }
}
