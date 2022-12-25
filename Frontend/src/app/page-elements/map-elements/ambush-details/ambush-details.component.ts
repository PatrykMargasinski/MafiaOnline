import { AmbushService } from './../../../services/ambush/ambush.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Ambush } from 'src/app/models/ambush/ambush.models';

@Component({
  selector: 'app-ambush-details',
  templateUrl: './ambush-details.component.html',
  styleUrls: ['./ambush-details.component.css']
})
export class AmbushDetailsComponent implements OnInit {

  constructor(private service: AmbushService) { }
  @Input() mapElementId: number
  @Output() someEvent = new EventEmitter<number>();
  ambush: Ambush
  ngOnInit(): void {
    this.service.getAmbushDetails(this.mapElementId).subscribe(x=>
      {
        this.ambush = x
      });
  }

  cancelAmbush()
  {
    this.service.cancelAmbush(this.mapElementId).subscribe()
    {
      alert('Ambush cancelled');
      this.someEvent.next(2);
    };
  }

}
