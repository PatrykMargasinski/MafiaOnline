import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Agent } from 'src/app/models/agent/agent.models';
import { Point } from 'src/app/models/map/point.models';
import { AgentService } from 'src/app/services/agent/agent.service';
import { AmbushService } from 'src/app/services/ambush/ambush.service';

@Component({
  selector: 'app-agent-actions',
  templateUrl: './agent-actions.component.html',
  styleUrls: ['./agent-actions.component.css']
})
export class AgentActionsComponent implements OnInit {

  constructor(private agentService: AgentService, private ambushService: AmbushService, private router: Router) { }

  @Input()
  agent: Agent

  @Output() actionResponse = new EventEmitter<string>();

  ngOnInit(): void {

  }

  recruitAgent(agent: Agent): void
  {
    if(confirm('Are you sure??'))
    {
        this.agentService.recruitAgent(agent.Id).subscribe(data=>{
          alert(data.toString());
          this.emitActionResponse("test")
        })
    }
  }

  dismissAgent(agent: Agent): void
  {
    if(confirm('Are you sure??'))
    {
        this.agentService.dismissAgent(agent.Id).subscribe(data=>{
          this.emitActionResponse("Agent dismissed");
        })
    }
  }

  cancelAmbush(agent: Agent): void
  {
    this.agentService.cancelAgentAmbush(agent.Id).subscribe(data =>
    {
      this.emitActionResponse("Ambush canceled");
    })
  }

  showAgentOnMap(agent: Agent)
  {
    this.agentService.getAgentPosition(agent.Id).subscribe(data =>
      {
        this.router.navigate(["/map/showMap"], { queryParams: { x: data.X, y: data.Y }});
      })
  }

  emitActionResponse(response: string)
  {
    this.actionResponse.emit(response)
  }
}
