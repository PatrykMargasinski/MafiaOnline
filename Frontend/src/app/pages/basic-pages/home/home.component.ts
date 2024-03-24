import { NewsService } from './../../../services/news/news.service';
import { Component, OnInit } from '@angular/core';
import { News } from 'src/app/models/news/news.models';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private newsService: NewsService) { }
  newsList: News[]
  ngOnInit(): void {
    this.newsService.getLastNews().subscribe(x => {
      this.newsList=x;
    })

  }

}
