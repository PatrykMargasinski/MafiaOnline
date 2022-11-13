import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MapField } from 'src/app/models/map/mapField.models';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MapService {

  readonly APIUrl = environment.APIEndpoint + '/map'

  constructor(private http:HttpClient) { }

  getEdgeForBoss(bossId: number):Observable<number[]>
  {
    return this.http.get<number[]>(this.APIUrl+'/edge?bossId=' + bossId);
  }

  getMap(x: number, y:number, size: number):Observable<MapField[]>
  {
    return this.http.get<MapField[]>(this.APIUrl+'?x=' + x + '&y=' + y +'&size=' + size);
  }
}
