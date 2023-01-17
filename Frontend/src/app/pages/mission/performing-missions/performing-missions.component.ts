import { Router } from '@angular/router';
import { TokenService } from 'src/app/services/auth/token.service';
import { Component, Input, OnInit } from '@angular/core';
import { interval, Subscription } from 'rxjs';
import { Mission, PerformingMission } from 'src/app/models/mission/mission.models';
import { MissionService } from 'src/app/services/mission/mission.service';

@Component({
  selector: 'app-performing-missions',
  templateUrl: './performing-missions.component.html',
  styleUrls: ['./performing-missions.component.css']
})
export class PerformingMissionsComponent implements OnInit {

  @Input() performingMissions: PerformingMission[]

  constructor(private missionService: MissionService, private tokenService: TokenService, private router: Router) { }

  private refreshPerformingMissions() {
    this.missionService.getPerformingMissions().subscribe(data => {
      this.performingMissions = data
      this.countdown()
    })
  }

  private countdown() {
    this.performingMissions.forEach(it => {
      interval(1000)
        .subscribe(x => {this.setTime()})
    })
  }

  private setTime() {
    this.performingMissions.forEach(mission => {
      mission.SecondsLeft = Math.ceil((new Date(mission.CompletionTime).getTime() - Date.now())/1000)
      if(mission.SecondsLeft <= 0) {
        this.refreshPerformingMissions()
      }
    });
  }

  showOnMap(mission: PerformingMission) {
    this.router.navigate(["/map/showMap"], { queryParams: { x: mission.X, y: mission.Y }});
  }

  ngOnInit(): void {
    this.refreshPerformingMissions()
  }
}
