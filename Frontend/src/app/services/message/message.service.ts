import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Message, MessageToSend } from 'src/app/models/message/message.models';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  readonly APIUrl = environment.APIEndpoint + '/message'

  constructor(private http:HttpClient) { }

  getMessageCount(bossId: number):Observable<number>{
    return this.http.get<number>(this.APIUrl+'/bossMessageCount?bossId='+bossId);
  }

  getReportCount(bossId: number):Observable<number>{
    return this.http.get<number>(this.APIUrl+'/reportCount?bossId='+bossId);
  }

  getAllMessages(bossId: number, fromRange: number, toRange:number, bossNameFilter):Observable<Message[]>{
    return this.http.get<Message[]>(this.APIUrl+'/bossMessagesTo?bossId=' + bossId + '&fromRange=' + fromRange + '&toRange=' + toRange
     + (bossNameFilter==='' ? '' : '&bossNameFilter=' + bossNameFilter));
  }

  getAllReports(bossId: number, fromRange: number, toRange:number, bossNameFilter):Observable<Message[]>{
    return this.http.get<Message[]>(this.APIUrl+'/reportsTo?bossId=' + bossId + '&fromRange=' + fromRange + '&toRange=' + toRange
     + (bossNameFilter==='' ? '' : '&bossNameFilter=' + bossNameFilter));
  }

  deleteMessage(val:number){
    return this.http.delete(this.APIUrl+'?messageId='+val);
  }

  deleteMessages(val:number[]){
    return this.http.delete(this.APIUrl+'/messages?messageIds='+val.join('i'));
  }

  sendMessage(val: MessageToSend){
    return this.http.post(this.APIUrl+'/send',val);
  }

  getMessageContent(val: number){
    return this.http.get<string>(this.APIUrl+'/content?messageId='+val);
  }

  setSeen(val: number){
    return this.http.get<string>(this.APIUrl+'/seen?messageId='+val);
  }
}
