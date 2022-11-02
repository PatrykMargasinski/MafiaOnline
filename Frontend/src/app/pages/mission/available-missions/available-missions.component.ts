import { Component, Input, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Mission } from 'src/app/models/mission/mission.models';
import { MissionService } from 'src/app/services/mission/mission.service';

@Component({
  selector: 'app-available-missions',
  templateUrl: './available-missions.component.html',
  styleUrls: ['./available-missions.component.css']
})
export class AvailableMissionsComponent implements OnInit {

  @Input() missions: Mission[]
  missionId: number = 0

  constructor(
    private missionSerice: MissionService,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    this.refreshMissions()

  }

  private refreshMissions() {
    this.missionSerice.getAvailableMissions().subscribe(data => {
      this.missions = data
    })
  }


  closeAndRefresh() {
    this.modalService.dismissAll();
    this.refreshMissions();
  }

  changeMissionId(missionId: number){
    this.missionId = missionId
  }
}
