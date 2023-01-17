import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Agent, AgentOnMission, AgentForSale, MovingAgent } from 'src/app/models/agent/agent.models';
import { DismissAgentRequest, PatrolRequest, RecruitAgentRequest } from 'src/app/models/agent/agent.requests';
import { Point } from 'src/app/models/map/point.models';
import { environment } from 'src/environments/environment';
@Injectable({
  providedIn: 'root'
})
export class AgentService {

  readonly APIUrl = environment.APIEndpoint + '/agent'

  constructor(private http:HttpClient) { }


  getAvailableAgents(bossId: number): Observable<Agent[]> {
    return this.http.get<Agent[]>(this.APIUrl + '/active');
  }

  getAgentsOnMission(bossId: number): Observable<AgentOnMission[]> {
    return this.http.get<AgentOnMission[]>(this.APIUrl + '/onMission');
  }

  getAgentsForSaleList(): Observable<AgentForSale[]> {
    return this.http.get<AgentForSale[]>(this.APIUrl + '/forSale');
  }

  getMovingAgents(bossId: number): Observable<MovingAgent[]> {
    return this.http.get<MovingAgent[]>(this.APIUrl + '/moving');
  }

  recruitAgent(bossId: number, agentId: number) {
    const request: RecruitAgentRequest = {AgentId: agentId, BossId: bossId}
    return this.http.post(this.APIUrl + "/recruit", request);
  }

  dismissAgent(agentId: number) {
    const request: DismissAgentRequest = {AgentId: agentId}
    return this.http.post(this.APIUrl + "/dismiss", request);
  }

  moveToPatrol(agentId: number, path: Point[])
  {
    const request: PatrolRequest = {Path: path, AgentId: agentId}
    return this.http.post(this.APIUrl + '/patrol', request);
  }
}
