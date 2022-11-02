import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
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

  constructor(private modalService: NgbModal) { }

  ngOnInit(): void {

  }


  open(content) {
    this.getMissionId.emit(this.mission.Id)
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {

    }, (reason) => {

    });
  }

}
