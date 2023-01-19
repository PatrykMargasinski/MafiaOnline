import { AgentService } from 'src/app/services/agent/agent.service';
import { Agent } from 'src/app/models/agent/agent.models';
import { TokenService } from 'src/app/services/auth/token.service';
import { AmbushService } from './../../../services/ambush/ambush.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Ambush } from 'src/app/models/ambush/ambush.models';

@Component({
  selector: 'app-ambush-details',
  templateUrl: './ambush-details.component.html',
  styleUrls: ['./ambush-details.component.css']
})
export class AmbushDetailsComponent implements OnInit {

  constructor(private service: AmbushService, private tokenService: TokenService, private agentService: AgentService) { }
  @Input() mapElementId: number
  @Output() someEvent = new EventEmitter<number>();
  ambush: Ambush
  yourAmbush: boolean = false
  agents: Agent[]
  ngOnInit(): void {
    this.service.getAmbushDetails(this.mapElementId).subscribe(x=>
      {
        this.ambush = x;
        let bossId = this.tokenService.getBossId();
        if(this.ambush.BossId==bossId)
        {
          this.yourAmbush = true
        }
        else
        {
          this.yourAmbush = false;
          this.agentService.getAvailableAgents().subscribe(data=>{
            this.agents = data
          })
        }
      });
  }

  cancelAmbush()
  {
    this.service.cancelAmbush(this.mapElementId).subscribe()
    {
      alert('Ambush cancelled');
      this.someEvent.next(2);
    };
  }

  attackAmbush(agentId: number)
  {
    this.service.attackAmbush(agentId, this.ambush.MapElementId).subscribe()
    {
      alert('Agent sent to attack an ambush');
      this.someEvent.next(2);
    };
  }

}
