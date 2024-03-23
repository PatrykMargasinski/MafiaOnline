import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DismissAgentRequest, PatrolRequest, RecruitAgentRequest } from 'src/app/models/agent/agent.requests';
import { Point } from 'src/app/models/map/point.models';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AgentActionsService {

  readonly APIUrl = environment.APIEndpoint + '/agentactions'

  constructor(private http:HttpClient) { }

  sendToPatrol(agentId: number, path: Point[])
  {
    const request: PatrolRequest = {Path: path, AgentId: agentId}
    return this.http.post(this.APIUrl + '/patrol', request);
  }

  cancelAgentAmbush(agentId: number)
  {
    return this.http.get(this.APIUrl + '/cancelAmbush?agentId=' + agentId);
  }

  getAgentPosition(agentId: number)
  {
    return this.http.get<Point>(this.APIUrl + '/position?agentId=' + agentId);
  }

  recruitAgent(agentId: number) {
    const request: RecruitAgentRequest = {AgentId: agentId, BossId: 0}
    return this.http.post(this.APIUrl + "/recruit", request);
  }

  dismissAgent(agentId: number) {
    console.log(agentId);
    const request: DismissAgentRequest = {AgentId: agentId}
    return this.http.post(this.APIUrl + "/dismiss", request);
  }
}
