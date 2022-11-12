import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MapField } from 'src/app/models/map/mapField.models';
import { MapService } from 'src/app/services/map/map.service';
import { TokenService } from 'src/app/services/auth/token.service';

@Component({
  selector: 'app-show-map',
  templateUrl: './show-map.component.html',
  styleUrls: ['./show-map.component.css']
})
export class ShowMapComponent implements OnInit {

  edgeX : number
  edgeY : number

  constructor(private mapService: MapService, private tokenService: TokenService, private modalService: NgbModal) { }
  mapFields: MapField[]
  chosenMapFieldId: number
  chosenElementType: number

  ngOnInit(): void {
    this.edgeX=0
    this.edgeY=0
    this.refreshMap();
  }

  closeAndRefresh() {
    this.modalService.dismissAll();
    this.refreshMap();
  }

  refreshMap(){
    this.mapService.getMap(this.edgeX,this.edgeY,20).subscribe(
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

  getBossId()
  {
    return this.tokenService.getBossId();
  }

  moveLeft()
  {
    this.edgeY--;
    this.refreshMap();
  }

  moveRight()
  {
    this.edgeY++;
    this.refreshMap();
  }

  moveUp()
  {
    console.log("test")
    this.edgeX--;
    this.refreshMap();
  }

  moveDown()
  {
    this.edgeX++;
    this.refreshMap();
  }

  divClick(mapFieldId: number, elementType: number, mapElementContent: any){
    this.chosenMapFieldId=mapFieldId
    this.chosenElementType=elementType
    this.modalService.open(mapElementContent, {ariaLabelledBy: 'modal-basic-title'});
  }
}
