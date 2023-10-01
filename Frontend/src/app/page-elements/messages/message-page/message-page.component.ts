import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Message } from 'src/app/models/message/message.models';
import { MessageService } from 'src/app/services/message/message.service';

@Component({
  selector: 'app-message-page',
  templateUrl: './message-page.component.html',
  styleUrls: ['./message-page.component.css']
})
export class MessagePageComponent implements OnInit {

  constructor(private route: ActivatedRoute, private messageService: MessageService) { }

  @Input()
  messageId: number
  message: Message

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if(params['id'])
        this.messageService.getMessageContent(Number(params['id'])).subscribe(x=>{
          this.message = x
      })
    })
  }

}
