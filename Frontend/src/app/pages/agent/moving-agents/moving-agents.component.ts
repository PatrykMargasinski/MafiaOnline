import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { MovingAgent } from 'src/app/models/agent/agent.models';
import { Point } from 'src/app/models/map/point.models';
import { AgentService } from 'src/app/services/agent/agent.service';
import { TokenService } from 'src/app/services/auth/token.service';

@Component({
  selector: 'app-moving-agents',
  templateUrl: './moving-agents.component.html',
  styleUrls: ['./moving-agents.component.css']
})
export class MovingAgentsComponent implements OnInit {

  constructor(private shared: AgentService, private tokenService: TokenService, private router: Router) { }
  MovingAgents:MovingAgent[];
  bossId: number;

  ngOnInit(): void {
    this.bossId = this.tokenService.getBossId();
    this.refresh();
  }

  refresh(){
    this.shared.getMovingAgents(this.bossId).subscribe(data=>{
      this.MovingAgents=data;
    });
  }

  getPosition(point: Point)
  {
    if(point == null) return "Unknown";
    else return "["+point.X+","+point.Y+"]"
  }

  showOnMap(point: Point) {
    this.router.navigate(["/map/showMap"], { queryParams: { x: point.X, y: point.Y }});
  }
}
