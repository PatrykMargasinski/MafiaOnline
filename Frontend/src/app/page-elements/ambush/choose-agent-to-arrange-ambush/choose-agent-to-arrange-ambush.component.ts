import { AmbushService } from './../../../services/ambush/ambush.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Agent } from 'src/app/models/agent/agent.models';
import { AgentService } from 'src/app/services/agent/agent.service';
import { TokenService } from 'src/app/services/auth/token.service';
import { MapUtils } from 'src/app/utils/map-utils';

@Component({
  selector: 'app-choose-agent-to-arrange-ambush',
  templateUrl: './choose-agent-to-arrange-ambush.component.html',
  styleUrls: ['./choose-agent-to-arrange-ambush.component.css']
})
export class ChooseAgentToArrangeAmbushComponent implements OnInit {

  @Input() missionId: number
  @Input() mapElementId: number
  @Output() someEvent = new EventEmitter<number>();
  agents: Agent[]
  agentPathSet: boolean = false

  constructor(
    private agentService: AgentService,
    private tokenService: TokenService,
    private ambushService: AmbushService,
    private mapUtils: MapUtils
    ) {

    }

  ngOnInit(): void {
    this.getAgents();
    this.agentPathSet = this.mapUtils.pathExists();
  }

  getAgents(){
    const bossId = Number(this.tokenService.getBossId())
    this.agentService.getAvailableAgents(bossId).subscribe(data=>{
      this.agents = data
    })
  }

  moveToArrangeAmbush(agentId: number){
    let point = this.mapUtils.getPath()
    this.ambushService.moveToArrangeAmbush(agentId, point[0]).subscribe(it=>{
      alert("Agent sent to arrange ambush")
      this.someEvent.next(2)
    })
  }
}


