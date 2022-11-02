import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Message } from 'src/app/models/message/message.models';
import { TokenService } from 'src/app/services/auth/token.service';
import { MessageService } from 'src/app/services/message/message.service';

@Component({
  selector: 'app-show-reports',
  templateUrl: './show-reports.component.html',
  styleUrls: ['./show-reports.component.css']
})
export class ShowReportsComponent implements OnInit {
  constructor(private shared: MessageService, private router: Router, private tokenService: TokenService) { }
  ReportList: Message[];
  ReportFilteredList: Message[];
  PageNumbers: number[];
  ReportIdsForActions: number[];
  OnlyUnseen:boolean = false;

  ngOnInit(): void {
    this.refreshReportList()
  }

  stringDateConvert(stringDate: string) {
    var d = new Date(stringDate);
    let ye = new Intl.DateTimeFormat('en', { year: 'numeric' }).format(d);
    let mo = new Intl.DateTimeFormat('en', { month: 'short' }).format(d);
    let da = new Intl.DateTimeFormat('en', { day: '2-digit' }).format(d);
    return `${da}-${mo}-${ye}`
  }

  clearFilters(){
    this.OnlyUnseen=false;
    this.refreshReportList();
  }

  refreshReportList(){
    this.ReportIdsForActions=new Array<number>();
    const bossId = Number(this.tokenService.getBossId())
    this.shared.getReportCount(bossId).subscribe(data=>{
      let bossReportsCount = data;
      this.PageNumbers=new Array<number>();
      for(let i=0;i<bossReportsCount;i+=5)
      {
        this.PageNumbers.push(i)
      }
    })
    this.getNextPage(0,5)
  }

  getNextPage(fromRange: number, toRange: number)
  {
    const bossId = Number(this.tokenService.getBossId())
    this.shared.getAllReports(
      bossId,
      fromRange,
      toRange,
      this.OnlyUnseen)
    .subscribe(data=>{
      this.ReportList=data;
      this.ReportFilteredList=data;
    });
  }

  checkboxClicked(ev, ReportId){
    if(ev.target.checked==true)
      this.ReportIdsForActions.push(ReportId);
    else
    {
      const index = this.ReportIdsForActions.indexOf(ReportId);
      if (index > -1) {
        this.ReportIdsForActions.splice(index, 1);
      }
    }
  }

  showContent(content: string, reportId: number){
    this.shared.setSeen(reportId).subscribe(
      x=>
      {
        alert(content);
        this.refreshReportList();
      })
  }

  deleteReport(reportId: number): void{
    if(confirm('Are you sure??')){
      this.shared.deleteMessage(reportId).subscribe(data=>{
        alert(data.toString());
        this.refreshReportList();
      });
    }
  }

  deleteSelected(): void{
    if(this.ReportIdsForActions.length==0)
    {
      confirm('There is no selected reports')
    }
    else
    {
      if(confirm('Do you want to delete '+this.ReportIdsForActions.length+' reports??')){
        this.shared.deleteMessages(this.ReportIdsForActions).subscribe(data=>{
          alert(data.toString());
          this.refreshReportList();
        });
      }
    }
  }
}
