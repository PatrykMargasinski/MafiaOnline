import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Ambush } from 'src/app/models/ambush/ambush.models';
import { ArrangeAmbushRequest, CancelAmbushRequest } from 'src/app/models/ambush/ambush.requests';
import { Point } from 'src/app/models/map/point.models';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AmbushService {

  readonly APIUrl = environment.APIEndpoint + '/ambush'

  constructor(private http:HttpClient) { }

  moveToArrangeAmbush(agentId: number, point: Point)
  {
    const request: ArrangeAmbushRequest = {Point: point, AgentId: agentId}
    return this.http.post(this.APIUrl + '/arrange', request);
  }

  getAmbushDetails(mapElementId: number)
  {
    return this.http.get<Ambush>(this.APIUrl + '?mapElementId=' + mapElementId);
  }

  cancelAmbush(mapElementId: number)
  {
    const request: CancelAmbushRequest = {MapElementId: mapElementId}
    return this.http.post(this.APIUrl + '/cancel', request);
  }
}
