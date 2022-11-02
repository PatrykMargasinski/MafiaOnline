import { TokenService } from 'src/app/services/auth/token.service';
import { AgentOnMission } from 'src/app/models/agent/agent.models';
import { Component, OnInit } from '@angular/core';
import { AgentService } from 'src/app/services/agent/agent.service';
import { interval, Subscription } from 'rxjs';

@Component({
  selector: 'app-agents-on-mission',
  templateUrl: './agents-on-mission.component.html',
  styleUrls: ['./agents-on-mission.component.css']
})
export class AgentsOnMissionComponent implements OnInit {

  constructor(private shared: AgentService, private tokenService: TokenService) { }

  agentsOnMission: AgentOnMission[]

  ngOnInit(): void {
    this.refreshAgentsOnMission();
  }

  refreshAgentsOnMission() {
    this.shared.getAgentsOnMission(this.tokenService.getBossId()).subscribe(data => {
      this.agentsOnMission = data;
      this.countdown()
    })
  }

  private countdown() {
    this.agentsOnMission.forEach(it => {
        interval(1000)
        .subscribe(x => {this.setTime()})
    })
  }

  private setTime() {
    this.agentsOnMission.forEach(agent => {
      agent.SecondsLeft = Math.ceil((new Date(agent.CompletionTime).getTime() - Date.now())/1000)
      if(agent.SecondsLeft <= 0) {
        this.refreshAgentsOnMission()
      }
    });
  }

}
