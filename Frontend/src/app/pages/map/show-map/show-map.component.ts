import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MapField } from 'src/app/models/map/mapField.models';
import { MapService } from 'src/app/services/map/map.service';

@Component({
  selector: 'app-show-map',
  templateUrl: './show-map.component.html',
  styleUrls: ['./show-map.component.css']
})
export class ShowMapComponent implements OnInit {


  constructor(private mapService: MapService) { }
  mapFields: MapField[]
  chosenMapFieldId: number
  ngOnInit(): void {
    this.refreshMap();
  }
  refreshMap(){
    this.mapService.getMap(1,1,20).subscribe(
      x=>
      {
        console.log(x)
        this.mapFields=x
      })
  }

  getTerrainColor(terrainId: number): string{
    switch(terrainId)
    {
      case 0:
        return "gray";
      case 1:
        return "darkred";
    }
  }
}
