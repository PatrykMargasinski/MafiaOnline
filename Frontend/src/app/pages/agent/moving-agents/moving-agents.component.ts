import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { MovingAgent } from 'src/app/models/agent/agent.models';
import { Point } from 'src/app/models/map/point.models';
import { AgentService } from 'src/app/services/agent/agent.service';
import { TokenService } from 'src/app/services/auth/token.service';
import { interval } from 'rxjs';

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
      this.countdown()
    });
  }

  getPosition(point: Point)
  {
    if(point == null) return "Unknown";
    else return "["+point.X+","+point.Y+"]"
  }

  showOnMap(point: Point) {
    console.log(point)
    this.router.navigate(["/map/showMap"], { queryParams: { x: point.X, y: point.Y }});
  }

  private countdown() {
    this.MovingAgents.forEach(it => {
        interval(1000)
        .subscribe(x => {this.setTime()})
    })
  }

  private setTime() {
    this.MovingAgents.forEach(agent => {
      agent.SecondsLeft = Math.ceil((new Date(agent.ArrivalTime).getTime() - Date.now())/1000)
      if(agent.SecondsLeft <= 0) {
        this.refresh()
      }
    });
  }
}
