import { Router } from '@angular/router';
import { BasicUtils } from './../../../utils/basic-utils';
import { Component, EventEmitter, Input, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Mission } from 'src/app/models/mission/mission.models';

@Component({
  selector: 'app-mission-card',
  templateUrl: './mission-card.component.html',
  styleUrls: ['./mission-card.component.css']
})
export class MissionCardComponent implements OnInit {

  @Input() mission: Mission;
  @Input() content: any;

  constructor(private router: Router, private basicUtils: BasicUtils) { }

  ngOnInit(): void {

  }


  showOnMap() {
    this.router.navigate(["/map/showMap"], { queryParams: { x: this.mission.X, y: this.mission.Y }});
  }

  printMissionPercentagesWithColors(diff: number, str: number, dex: number, int: number)
  {
    const text = this.basicUtils.printMissionPercentagesWithColors(diff,str,dex,int);
    return text;
  }

}
