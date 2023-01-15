import { Headquarters } from './../../models/headquarters/headquarters.models';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HeadquartersService {

  constructor(private http:HttpClient) { }

  readonly APIUrl = environment.APIEndpoint + '/headquarters'

  getHeadquartersDetails(id: number):Observable<Headquarters>
  {
    return this.http.get<Headquarters>(this.APIUrl+'?id='+id);
  }
}
