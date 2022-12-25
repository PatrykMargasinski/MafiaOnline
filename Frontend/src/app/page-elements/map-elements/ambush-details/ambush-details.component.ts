import { AmbushService } from './../../../services/ambush/ambush.service';
import { Component, Input, OnInit } from '@angular/core';
import { Ambush } from 'src/app/models/ambush/ambush.models';

@Component({
  selector: 'app-ambush-details',
  templateUrl: './ambush-details.component.html',
  styleUrls: ['./ambush-details.component.css']
})
export class AmbushDetailsComponent implements OnInit {

  constructor(private service: AmbushService) { }
  @Input() mapElementId: number
  ambush: Ambush
  ngOnInit(): void {
    this.service.getAmbushDetails(this.mapElementId).subscribe(x=>
      {
        this.ambush = x
      });
  }

}
