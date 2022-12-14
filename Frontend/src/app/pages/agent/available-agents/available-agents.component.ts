import { Component, OnInit } from '@angular/core';
import { Agent } from 'src/app/models/agent/agent.models';
import { AgentService } from 'src/app/services/agent/agent.service';
import { TokenService } from 'src/app/services/auth/token.service';

@Component({
  selector: 'app-available-agents',
  templateUrl: './available-agents.component.html',
  styleUrls: ['./available-agents.component.css']
})
export class AvailableAgentsComponent implements OnInit {

  constructor(private shared: AgentService, private tokenService: TokenService) { }
  AgentList:Agent[];

  ngOnInit(): void {
    this.refreshAgentList();
  }

  refreshAgentList(){
    const bossId = Number(this.tokenService.getBossId())
    this.shared.getAvailableAgents(bossId).subscribe(data=>{
      this.AgentList=data;
    });
  }

  dismissAgent(agentId: number): void
  {
    if(confirm('Are you sure??')){
        this.shared.dismissAgent(agentId).subscribe(data=>{
          alert('Agent dismissed successfully');
          this.refreshAgentList();
        })
    }
  }
}
