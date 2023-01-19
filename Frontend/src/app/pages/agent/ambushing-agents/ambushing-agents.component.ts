import { Point } from 'src/app/models/map/point.models';
import { Component, OnInit } from '@angular/core';
import { AmbushingAgent } from 'src/app/models/agent/agent.models';
import { AgentService } from 'src/app/services/agent/agent.service';
import { AmbushService } from 'src/app/services/ambush/ambush.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-ambushing-agents',
  templateUrl: './ambushing-agents.component.html',
  styleUrls: ['./ambushing-agents.component.css']
})
export class AmbushingAgentsComponent implements OnInit {

  constructor(private agentService: AgentService, private ambushService: AmbushService, private router: Router) { }
  AmbushingAgents:AmbushingAgent[];

  ngOnInit(): void {
    this.refresh();
  }

  refresh(){
    this.agentService.getAmbushingAgents().subscribe(data=>{
      this.AmbushingAgents=data;
    });
  }

  cancelAmbush(mapElementId: number): void
  {
    if(confirm('Are you sure??'))
    {
        this.ambushService.cancelAmbush(mapElementId).subscribe(data=>{
          alert("Ambush canceled");
          this.refresh();
        })
    }
  }

  showOnMap(point: Point): void
  {
    this.router.navigate(["/map/showMap"], { queryParams: { x: point.X, y: point.Y }});
  }
}
