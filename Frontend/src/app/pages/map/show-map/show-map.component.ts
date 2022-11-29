import { MapUtils } from './../../../utils/map-utils';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MapField } from 'src/app/models/map/mapField.models';
import { MapService } from 'src/app/services/map/map.service';
import { TokenService } from 'src/app/services/auth/token.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-show-map',
  templateUrl: './show-map.component.html',
  styleUrls: ['./show-map.component.css']
})
export class ShowMapComponent implements OnInit {

  edgeX : number
  edgeY : number

  constructor(private mapService: MapService, private tokenService: TokenService, private modalService: NgbModal, private mapUtils: MapUtils, private activatedRoute: ActivatedRoute) { }
  mapFields: MapField[]
  chosenMapFieldId: number
  chosenElementType: number
  creatingAgentPathMode: boolean = false
  @ViewChild('mapElementModal') mapElementModal : TemplateRef<any>;

  ngOnInit(): void {
    this.mapService.getEdgeForBoss(this.tokenService.getBossId()).subscribe(x=>
      {
        this.edgeX=x[0]
        this.edgeY=x[1]
        this.refreshMap();
        this.mapUtils.clearAgentPath();
      })
  }

  closeAndRefresh(operation: number) {
    this.modalService.dismissAll();
    this.refreshMap();
    if(operation==1)
    {
      this.creatingAgentPathMode = true;
    }
  }

  refreshMap(){
    this.mapService.getMap(this.edgeX,this.edgeY,20).subscribe(
      x=>
      {
        this.mapFields=x
      })
  }

  getTerrainColor(terrainTypeId: number): string{
    switch(terrainTypeId)
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
    this.edgeX--;
    this.refreshMap();
  }

  moveDown()
  {
    this.edgeX++;
    this.refreshMap();
  }

  mapElementClick(mapFieldId: number, elementType: number){
    this.chosenMapFieldId=mapFieldId
    this.chosenElementType=elementType
    console.log(elementType);
    this.modalService.open(this.mapElementModal, {ariaLabelledBy: 'modal-basic-title'});
  }

  mapFieldClick(mapFieldId: number, X:number, Y:number, mapElementType: number, terrainType: number){
    if(this.creatingAgentPathMode == true)
    {
      if(!this.mapUtils.isRoad(X,Y))
      {
        alert('This field is not a road')
        return
      }

      var lastElement = this.mapUtils.getLastElementOfAgentPath();

      if(lastElement!=null && lastElement.X==X && lastElement.Y==Y)
      {
        this.mapUtils.removeAgentPath(X,Y);
        return
      }

      if(this.mapUtils.getAgentPath().filter(el=>el.X==X && el.Y==Y).length!=0)
      {
        alert('You can remove only last set element')
        return
      }

      if(lastElement!=null && !this.mapUtils.areAdjacent(X,Y,lastElement.X,lastElement.Y))
      {
        alert('This field is not adjacent with last field of agent road')
        return
      }

      if(!this.mapUtils.isPointAgentPath(X,Y))
      {
        this.mapUtils.addRoadToAgentPath(X,Y);
      }
    }
  }

  isChosenAsAgentPath(X: number, Y: number)
  {
    return this.mapUtils.isPointAgentPath(X, Y);
  }

  roadReadyOperations(operation: number)
  {
    console.log(operation)
    if(operation==1)
    {
      this.modalService.open(this.mapElementModal, {ariaLabelledBy: 'modal-basic-title'});
    }
    if(operation==2)
    {
      this.creatingAgentPathMode = false
      this.mapUtils.clearAgentPath();
    }
  }
}
