import { AgentQuery } from './../../models/agent/agent.models';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Agent, AgentOnMission, AgentForSale, MovingAgent, AmbushingAgent } from 'src/app/models/agent/agent.models';
import { DismissAgentRequest, PatrolRequest, RecruitAgentRequest } from 'src/app/models/agent/agent.requests';
import { Point } from 'src/app/models/map/point.models';
import { Mission } from 'src/app/models/mission/mission.models';
import { environment } from 'src/environments/environment';
@Injectable({
  providedIn: 'root'
})
export class AgentService {

  readonly APIUrl = environment.APIEndpoint + '/agent'

  constructor(private http:HttpClient) { }


  getAgentsByQuery(filters: AgentQuery): Observable<Agent[]> {
    return this.http.post<Agent[]>(this.APIUrl + '/query', filters);
  }

  getAvailableAgents(): Observable<Agent[]> {
    return this.http.get<Agent[]>(this.APIUrl + '/active');
  }

  getAgentsOnMission(): Observable<AgentOnMission[]> {
    return this.http.get<AgentOnMission[]>(this.APIUrl + '/onMission');
  }

  getAgentsForSaleList(): Observable<AgentForSale[]> {
    return this.http.get<AgentForSale[]>(this.APIUrl + '/forSale');
  }

  getMovingAgents(): Observable<MovingAgent[]> {
    return this.http.get<MovingAgent[]>(this.APIUrl + '/moving');
  }

  getAmbushingAgents(): Observable<AmbushingAgent[]> {
    return this.http.get<AmbushingAgent[]>(this.APIUrl + '/ambushing');
  }
}
