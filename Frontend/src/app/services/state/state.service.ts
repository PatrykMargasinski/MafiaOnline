import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { State } from 'src/app/models/state/state';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StateService {

  constructor(private http: HttpClient) { }

  readonly APIUrl = environment.APIEndpoint + '/state'

  getAvailableAgentStates()
  {
    return this.http.get<State[]>(this.APIUrl + "/agent")
  }
}
