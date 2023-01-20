import { NewsService } from './../../../services/news/news.service';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { News } from 'src/app/models/news/news.models';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-create-news',
  templateUrl: './create-news.component.html',
  styleUrls: ['./create-news.component.css']
})
export class CreateNewsComponent implements OnInit {

  news: News

  @ViewChild('newsModal') newsModal : TemplateRef<any>;

  constructor(private newsService: NewsService, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.news = {Priority: 0, HTMLContent: "", Subject: ""};
  }

  showNews(){
    this.modalService.open(this.newsModal, {ariaLabelledBy: 'modal-basic-title'});
  }

  createNews(){
    this.newsService.createNews(this.news).subscribe
    (
      x=>
      {
        alert("News created")
      }
    );
  }

  closeAndRefresh() {
    this.modalService.dismissAll();
  }


}
