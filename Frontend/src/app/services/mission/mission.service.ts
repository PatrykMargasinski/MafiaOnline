import { StartMissionRequest } from './../../models/mission/mission.requests';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Mission, PerformingMission } from 'src/app/models/mission/mission.models';
import { environment } from 'src/environments/environment';
import { Point } from 'src/app/models/map/point.models';

@Injectable({
  providedIn: 'root'
})
export class MissionService {

  constructor(private http: HttpClient) { }

  readonly APIUrl = environment.APIEndpoint + '/mission'

  getMissionByMapElement(mapElementId: number): Observable<Mission> {
    return this.http.get<Mission>(this.APIUrl + '/mapElement?mapElementId='+mapElementId);
  }

  getMissionDetails(): Observable<Mission[]> {
    return this.http.get<Mission[]>(this.APIUrl + '/available');
  }

  getAvailableMissions(): Observable<Mission[]> {
    return this.http.get<Mission[]>(this.APIUrl + '/available');
  }

  getPerformingMissions(bossId: number): Observable<PerformingMission[]> {
    return this.http.get<PerformingMission[]>(this.APIUrl + '/performing?bossId='+bossId);
  }

  getMissionById(missionId: number): Observable<Mission> {
    return this.http.get<Mission>(this.APIUrl + '/?missionId='+missionId);
  }

  startMission(agentId: number, missionId: number, path: Point[]){
    const request: StartMissionRequest = {MissionId: missionId, AgentId: agentId, Path: path}
    return this.http.post<PerformingMission[]>(this.APIUrl + '/start', request);
  }

  moveOnMission(agentId: number, missionId: number, path: Point[]){
    const request: StartMissionRequest = {MissionId: missionId, AgentId: agentId, Path: path}
    return this.http.post<PerformingMission[]>(this.APIUrl + '/move', request);
  }
}
