import { BasicUtils } from './../../../utils/basic-utils';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Message} from 'src/app/models/message/message.models';
import { TokenService } from 'src/app/services/auth/token.service';
import { MessageService } from 'src/app/services/message/message.service';


@Component({
  selector: 'app-show-message',
  templateUrl: './show-messages.component.html',
  styleUrls: ['./show-messages.component.css']
})
export class ShowMessagesComponent implements OnInit {
  constructor(private shared: MessageService, private router: Router, private tokenService: TokenService, private basicUtils: BasicUtils) { }
  MessageFilteredList: Message[];
  ReceiverFilterText: string = "";
  PageNumbers: number[];
  MessageIdsForActions: number[];

  ngOnInit(): void {
    this.refreshMessageList()
  }

  navigateToSendMessageComponentWithParams(bossName: string, subject: string){
    this.router.navigate(["/message/send"], { queryParams: { bossName: bossName, subject: 'Re:'+subject }});
  }

  navigateToSendMessageComponent(){
    this.router.navigate(["/message/send"]);
  }

  clearFilters(){
    this.ReceiverFilterText="";
    this.refreshMessageList();
  }

  refreshMessageList(){
    this.MessageIdsForActions=new Array<number>();
    const bossId = this.tokenService.getBossId();
    this.shared.getMessageCount(bossId).subscribe(data=>{
      let bossMessagesCount = data;
      this.PageNumbers=new Array<number>();
      for(let i=0;i<bossMessagesCount;i+=5)
      {
        this.PageNumbers.push(i)
      }
    })
    this.getNextPage(0,5)
  }

  getNextPage(fromRange: number, toRange: number)
  {
    const bossId = this.tokenService.getBossId()
    this.shared.getAllMessages
    (
      bossId,
      fromRange,
      toRange,
      this.ReceiverFilterText.toLowerCase()
    )
    .subscribe(data=>{
      this.MessageFilteredList=data;
    });
  }

  checkboxClicked(ev, MessageId){
    if(ev.target.checked==true)
      this.MessageIdsForActions.push(MessageId);
    else
    {
      const index = this.MessageIdsForActions.indexOf(MessageId);
      if (index > -1) {
        this.MessageIdsForActions.splice(index, 1);
      }
    }
  }

  showContent(content: string, messageId: number){
    this.shared.setSeen(messageId).subscribe(
      x=>
      {
        alert(content);
        this.refreshMessageList();
      })
  }

  deleteMessage(messageId: number): void{
    if(confirm('Are you sure??')){
      this.shared.deleteMessage(messageId).subscribe(data=>{
        this.refreshMessageList();
      });
    }
  }

  deleteSelected(): void{
    if(this.MessageIdsForActions.length==0)
    {
      confirm('There is no selected messages')
    }
    else
    {
      if(confirm('Do you want to delete '+this.MessageIdsForActions.length+' messages??')){
        this.shared.deleteMessages(this.MessageIdsForActions).subscribe(data=>{
          this.refreshMessageList();
        });
      }
    }
  }
}
