import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Agent } from 'src/app/models/agent/agent.models';
import { AgentService } from 'src/app/services/agent/agent.service';
import { TokenService } from 'src/app/services/auth/token.service';
import { MissionService } from 'src/app/services/mission/mission.service';

@Component({
  selector: 'app-choose-agent-on-mission',
  templateUrl: './choose-agent-on-mission.component.html',
  styleUrls: ['./choose-agent-on-mission.component.css']
})
export class ChooseAgentOnMissionComponent implements OnInit {

  @Input() missionId: number
  @Output() someEvent = new EventEmitter();
  agents: Agent[]

  constructor(
    private agentService: AgentService,
    private missionService: MissionService,
    private tokenService: TokenService
    ) {

    }

  ngOnInit(): void {
    this.getAgents();
  }

  getAgents(){
    const bossId = Number(this.tokenService.getBossId())
    this.agentService.getAvailableAgents(bossId).subscribe(data=>{
      this.agents = data
    })
  }

  doMission(agentId: number){
    this.missionService.startMission(agentId, this.missionId).subscribe(it=>{
      this.someEvent.next()
    })
  }
}


