import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageToSend } from 'src/app/models/message/message.models';
import { TokenService } from 'src/app/services/auth/token.service';
import { BossService } from 'src/app/services/boss/boss.service';
import { MessageService } from 'src/app/services/message/message.service';

@Component({
  selector: 'app-send-message',
  templateUrl: './send-message.component.html',
  styleUrls: ['./send-message.component.css']
})
export class SendMessageComponent implements OnInit {

  constructor(private mesService: MessageService, private bossService: BossService, private route: ActivatedRoute, private router: Router, private tokenService: TokenService) { }

  similarBossNames: string[]
  messageToSend: MessageToSend

  findSimilarBossNames(text: string)
  {
    if(text.length>=3)
    {
      this.bossService.findBossNamesStartingWith(text).subscribe(x=>{this.similarBossNames=x;})
    }
    else
      this.similarBossNames=[]
  }

  returnToMessageList()
  {
    this.router.navigate(["/message/messages"]);
  }

  sendMessage()
  {

    this.mesService.sendMessage(this.messageToSend).subscribe(x=>
      {
        alert("Message sent");
        this.returnToMessageList();
      })
  }

  ngOnInit(): void {
    this.messageToSend = {
      FromBossId: Number(this.tokenService.getBossId()),
      ToBossFullName: "",
      Content: "",
      Subject: ""
    }

    this.route.queryParams.subscribe(params => {
      if(params['bossName'])
        this.messageToSend.ToBossFullName=params['bossName']
      if(params['subject'])
        this.messageToSend.Subject=params['subject']
    });
  }

}
