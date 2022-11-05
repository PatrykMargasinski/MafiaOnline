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
  @Output() getMissionId = new EventEmitter<number>();

  constructor(private modalService: NgbModal, private basicUtils: BasicUtils) { }

  ngOnInit(): void {

  }


  open(content) {
    this.getMissionId.emit(this.mission.Id)
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {

    }, (reason) => {

    });
  }

  printMissionPercentagesWithColors(diff: number, str: number, dex: number, int: number)
  {
    const text = this.basicUtils.printMissionPercentagesWithColors(diff,str,dex,int);
    return text;
  }

}
