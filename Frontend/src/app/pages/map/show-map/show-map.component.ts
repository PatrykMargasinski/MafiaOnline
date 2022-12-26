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
  mapMode: number = 0 //0 - nothing, 1 - creating path back from mission, 2 - set ambush, 3 - create path to patrol
  @ViewChild('mapElementModal') mapElementModal : TemplateRef<any>;

  ngOnInit(): void {
    this.mapService.getEdgeForBoss(this.tokenService.getBossId()).subscribe(x=>
      {
        this.edgeX=x[0]
        this.edgeY=x[1]
        this.refreshMap();
        this.mapUtils.clearPath();
      })
  }

  closeAndRefresh(operation: number) {
    this.modalService.dismissAll();
    if(operation==1)
    {
      this.mapMode=1
    }
    else
    {
      this.mapMode=0
      this.mapUtils.clearPath();
    }
    this.refreshMap();
  }

  setAmbushMode()
  {
    this.mapMode=2
    this.mapUtils.clearPath();
    this.refreshMap();
  }

  setPatrolMode()
  {
    this.mapMode=3
    this.mapUtils.clearPath();
    this.refreshMap();
  }

  refreshMap(){
    this.mapService.getMap(this.edgeX,this.edgeY,20, this.tokenService.getBossId()).subscribe(
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
    if(this.mapMode==1)
    {
      if(!this.mapUtils.isRoad(X,Y))
      {
        alert('This field is not a road')
        return
      }

      var lastElement = this.mapUtils.getLastElementOfPath();

      if(lastElement!=null && lastElement.X==X && lastElement.Y==Y)
      {
        this.mapUtils.removePath(X,Y);
        return
      }

      if(this.mapUtils.getPath().filter(el=>el.X==X && el.Y==Y).length!=0)
      {
        alert('You can remove only last set element')
        return
      }

      if(lastElement!=null && !this.mapUtils.areAdjacent(X,Y,lastElement.X,lastElement.Y))
      {
        alert('This field is not adjacent with last field of agent road')
        return
      }

      if(!this.mapUtils.isPointPath(X,Y))
      {
        this.mapUtils.addPointToPath(X,Y);
      }
    }
    else if(this.mapMode == 2)
    {
      if(!this.mapUtils.isRoad(X,Y))
      {
        alert('This field is not a road')
        return
      }

      if(!this.mapUtils.isPointPath(X,Y))
      {
        this.mapUtils.addPointToPath(X,Y);
      }
    }

    else if(this.mapMode == 3)
    {
      if(!this.mapUtils.isRoad(X,Y))
      {
        alert('This field is not a road')
        return
      }

      var lastElement = this.mapUtils.getLastElementOfPath();

      if(lastElement!=null && lastElement.X==X && lastElement.Y==Y)
      {
        this.mapUtils.removePath(X,Y);
        return
      }

      if(this.mapUtils.getPath().filter(el=>el.X==X && el.Y==Y).length!=0)
      {
        alert('You can remove only last set element')
        return
      }

      if(lastElement!=null && !this.mapUtils.areAdjacent(X,Y,lastElement.X,lastElement.Y))
      {
        alert('This field is not adjacent with last field of agent road')
        return
      }

      if(!this.mapUtils.isPointPath(X,Y))
      {
        this.mapUtils.addPointToPath(X,Y);
      }
    }
  }

  isChosenAsAgentPath(X: number, Y: number)
  {
    return this.mapUtils.isPointPath(X, Y);
  }

  pathReadyOperations(operation: number)
  {
    if(operation==1)
    {
      this.modalService.open(this.mapElementModal, {ariaLabelledBy: 'modal-basic-title'});
    }
    if(operation==2)
    {
      this.mapMode=0
      this.mapUtils.clearPath();
    }
    if(operation==3)
    {
      if(this.mapUtils.getPath().length!=1)
      {
        alert('You have to choose 1 point');
        return;
      }
      this.chosenElementType=0;
      this.modalService.open(this.mapElementModal, {ariaLabelledBy: 'modal-basic-title'});
    }

    if(operation==4)
    {
      this.chosenElementType=0;
      this.modalService.open(this.mapElementModal, {ariaLabelledBy: 'modal-basic-title'});
    }
  }
}
