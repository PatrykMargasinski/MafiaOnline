import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Agent } from 'src/app/models/agent/agent.models';
import { AgentActionsService } from 'src/app/services/agent/agent-actions.service';
import { AgentService } from 'src/app/services/agent/agent.service';
import { AmbushService } from 'src/app/services/ambush/ambush.service';
import { TokenService } from 'src/app/services/auth/token.service';
import { MapUtils } from 'src/app/utils/map-utils';

@Component({
  selector: 'app-choose-agent-to-patrol',
  templateUrl: './choose-agent-to-patrol.component.html',
  styleUrls: ['./choose-agent-to-patrol.component.css']
})
export class ChooseAgentToPatrolComponent implements OnInit {

  @Input() missionId: number
  @Output() someEvent = new EventEmitter<number>();
  agents: Agent[];

  constructor(
    private agentService: AgentService,
    private agentActionsService: AgentActionsService,
    private mapUtils: MapUtils
    ) {

    }

  ngOnInit(): void {
    this.getAgents();
  }

  getAgents(){
    this.agentService.getAvailableAgents().subscribe(data=>{
      this.agents = data
    })
  }

  sendToPatrol(agentId: number){
    let path= this.mapUtils.getPath()
    this.agentActionsService.sendToPatrol(agentId, path).subscribe(it=>{
      alert("Agent sent to patrol")
      this.someEvent.next(2)
    })
  }
}


