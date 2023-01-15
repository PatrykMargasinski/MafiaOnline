import { Headquarters } from './../../../models/headquarters/headquarters.models';
import { HeadquartersService } from './../../../services/headquarters/headquarters.service';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-headquarters-details',
  templateUrl: './headquarters-details.component.html',
  styleUrls: ['./headquarters-details.component.css']
})
export class HeadquartersDetailsComponent implements OnInit {

  constructor(private service: HeadquartersService) { }

  @Input() headquartersId: number
  headquarters: Headquarters

  ngOnInit(): void {
    this.service.getHeadquartersDetails(this.headquartersId).subscribe(x=>
      {
        this.headquarters = x;
      })
  }

}
