import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import { environment } from 'src/environments/environment';
import { Boss } from 'src/app/models/boss/boss.models';

@Injectable({
  providedIn: 'root'
})
export class BossService {

  readonly APIUrl = environment.APIEndpoint + '/boss'

  constructor(private http:HttpClient) { }

  getBoss(id:number):Observable<Boss>
  {
    return this.http.get<Boss>(this.APIUrl+'/'+id);
  }

  findBossNamesStartingWith(name:string):Observable<string[]>
  {
    return this.http.get<string[]>(this.APIUrl+'/similarNames?name='+ name);
  }

  getBestBosses(from: number, to: number):Observable<Boss[]>
  {
    return this.http.get<Boss[]>(this.APIUrl+'/best?from='+from+'&to='+to);
  }
}
