import { Component, Input, OnInit } from '@angular/core';
import { News } from 'src/app/models/news/news.models';

@Component({
  selector: 'app-news-card',
  templateUrl: './news-card.component.html',
  styleUrls: ['./news-card.component.css']
})
export class NewsCardComponent implements OnInit {

  constructor() { }
  @Input()
  news: News

  ngOnInit(): void {
  }

}
