import { ChangePasswordRequest, LoginRequest, RegisterRequest, DeleteAccountRequest, CreateResetPasswordCodeRequest, ResetPasswordRequest } from './../../models/player/player.requests';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  constructor(private http: HttpClient) { }

  login(request: LoginRequest)
  {
    return this.http.post(environment.APIEndpoint + "/login", request)
  }

  register(request: RegisterRequest)
  {
    return this.http.post(environment.APIEndpoint + "/register", request)
  }

  changePassword(request: ChangePasswordRequest)
  {
    return this.http.post(environment.APIEndpoint + "/changePassword", request)
  }

  deleteAccount(request: DeleteAccountRequest)
  {
    return this.http.post(environment.APIEndpoint + "/deleteAccount", request)
  }

  resendActivationLink()
  {
    return this.http.get(environment.APIEndpoint + "/resendActivationLink")
  }

  createResetPasswordCode(request: CreateResetPasswordCodeRequest)
  {
    return this.http.post(environment.APIEndpoint + "/resetPasswordCode", request)
  }

  resetPassword(request: ResetPasswordRequest)
  {
    return this.http.post(environment.APIEndpoint + "/resetPassword", request)
  }
}
