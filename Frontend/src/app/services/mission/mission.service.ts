import { StartMissionRequest } from './../../models/mission/mission.requests';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Mission, PerformingMission } from 'src/app/models/mission/mission.models';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MissionService {

  constructor(private http: HttpClient) { }

  readonly APIUrl = environment.APIEndpoint + '/mission'

  getAvailableMissions(): Observable<Mission[]> {
    return this.http.get<Mission[]>(this.APIUrl + '/available');
  }

  getPerformingMissions(bossId: number): Observable<PerformingMission[]> {
    return this.http.get<PerformingMission[]>(this.APIUrl + '/performing?bossId='+bossId);
  }

  startMission(agentId: number, missionId: number){
    const request: StartMissionRequest = {MissionId: missionId, AgentId: agentId}
    return this.http.post<PerformingMission[]>(this.APIUrl + '/start', request);
  }
}
