import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { News } from 'src/app/models/news/news.models';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NewsService {

  constructor(private http: HttpClient) { }

  readonly APIUrl = environment.APIEndpoint + '/news'

  getLastNews(): Observable<News[]> {
    return this.http.get<News[]>(this.APIUrl);
  }
}
