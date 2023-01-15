import { Component, Input, OnInit } from '@angular/core';
import { Mission } from 'src/app/models/mission/mission.models';
import { MissionService } from 'src/app/services/mission/mission.service';

@Component({
  selector: 'app-mission-details',
  templateUrl: './mission-details.component.html',
  styleUrls: ['./mission-details.component.css']
})
export class MissionDetailsComponent implements OnInit {

  constructor(private service: MissionService) { }

  @Input() missionId: number
  mission: Mission

  ngOnInit(): void {
    this.service.getMissionByMapElement(this.missionId).subscribe(x=>
      {
        this.mission = x;
      })
  }

}
