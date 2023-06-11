import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Job } from 'src/app/models/job/job.models';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class JobService {

  readonly APIUrl = environment.APIEndpoint + '/job'

  constructor(private http:HttpClient) { }

  getAllActiveJobs():Observable<Job[]>
  {
    return this.http.get<Job[]>(this.APIUrl+'/active');
  }
}
