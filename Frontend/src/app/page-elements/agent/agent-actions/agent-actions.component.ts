import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Agent } from 'src/app/models/agent/agent.models';
import { AgentActionsService } from 'src/app/services/agent/agent-actions.service';
import { AgentService } from 'src/app/services/agent/agent.service';
import { AmbushService } from 'src/app/services/ambush/ambush.service';

@Component({
  selector: 'app-agent-actions',
  templateUrl: './agent-actions.component.html',
  styleUrls: ['./agent-actions.component.css']
})
export class AgentActionsComponent implements OnInit {

  constructor(private agentActionsService: AgentActionsService, private router: Router) { }

  @Input()
  agent: Agent

  @Output() actionResponse = new EventEmitter<string>();

  ngOnInit(): void {

  }

  recruitAgent(agent: Agent): void
  {
    if(confirm('Are you sure??'))
    {
        this.agentActionsService.recruitAgent(agent.Id).subscribe(data=>{
          alert(data.toString());
        })
    }
  }

  dismissAgent(agent: Agent): void
  {
    if(confirm('Are you sure??'))
    {
        this.agentActionsService.dismissAgent(agent.Id).subscribe(data=>{
          this.emitActionResponse("Agent dismissed");
        })
    }
  }

  cancelAmbush(agent: Agent): void
  {
    this.agentActionsService.cancelAgentAmbush(agent.Id).subscribe(data =>
    {
      this.emitActionResponse("Ambush canceled");
    })
  }

  showAgentOnMap(agent: Agent)
  {
    this.agentActionsService.getAgentPosition(agent.Id).subscribe(data =>
      {
        this.router.navigate(["/map/showMap"], { queryParams: { x: data.X, y: data.Y }});
      })
  }

  agentContainsState(states: [])
  {
    let agentState = this.agent.Substate == null ? this.agent.State : [this.agent.State, this.agent.Substate]
    return states.some(subArray => JSON.stringify(subArray) === JSON.stringify(agentState));
  }

  emitActionResponse(response: string)
  {
    this.actionResponse.emit(response)
  }
}
