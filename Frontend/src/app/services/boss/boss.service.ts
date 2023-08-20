import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import { environment } from 'src/environments/environment';
import { Boss, BossWithPosition } from 'src/app/models/boss/boss.models';

@Injectable({
  providedIn: 'root'
})
export class BossService {

  readonly APIUrl = environment.APIEndpoint + '/boss'

  constructor(private http:HttpClient) { }

  getBoss():Observable<BossWithPosition>
  {
    return this.http.get<BossWithPosition>(this.APIUrl+'/datas');
  }

  findBossNamesStartingWith(startingWith:string):Observable<string[]>
  {
    return this.http.get<string[]>(this.APIUrl+'/similarNames?startingWith='+ startingWith);
  }

  getBestBosses(from: number, to: number):Observable<BossWithPosition[]>
  {
    return this.http.get<BossWithPosition[]>(this.APIUrl+'/best?from='+from+'&to='+to);
  }
}
