import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Agent, AgentOnMission, AgentForSale } from 'src/app/models/agent/agent.models';
import { RecruitAgentRequest } from 'src/app/models/agent/agent.requests';
import { environment } from 'src/environments/environment';
@Injectable({
  providedIn: 'root'
})
export class AgentService {

  readonly APIUrl = environment.APIEndpoint + '/agent'

  constructor(private http:HttpClient) { }


  getAvailableAgents(bossId: number): Observable<Agent[]> {
    return this.http.get<Agent[]>(this.APIUrl + '/available/' + bossId);
  }

  getAgentsOnMission(bossId: number): Observable<AgentOnMission[]> {
    return this.http.get<AgentOnMission[]>(this.APIUrl + '/onMission/' + bossId);
  }

  getAgentsForSaleList(): Observable<AgentForSale[]> {
    return this.http.get<AgentForSale[]>(this.APIUrl + '/forSale');
  }

  recruitAgent(bossId: number, agentId: number) {
    const request: RecruitAgentRequest = {AgentId: agentId, BossId: bossId}
    return this.http.post(this.APIUrl + "/recruit", request);
  }
}
