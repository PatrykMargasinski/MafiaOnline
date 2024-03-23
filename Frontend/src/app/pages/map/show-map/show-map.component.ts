import { MapUtils } from './../../../utils/map-utils';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Directions, MapField, MapModes, Operations, TerrainTypes } from 'src/app/models/map/mapField.models';
import { MapService } from 'src/app/services/map/map.service';
import { TokenService } from 'src/app/services/auth/token.service';
import { ActivatedRoute } from '@angular/router';
import { Point } from 'src/app/models/map/point.models';

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
  mapMode: MapModes = MapModes.Nothing //0 - nothing, 1 - creating path back from mission, 2 - set ambush, 3 - create path to patrol
  @ViewChild('mapElementModal') mapElementModal : TemplateRef<any>;

  uniquelyColouredFields: Map<string, string> = new Map<string, string>()

  mapElementDescription: string = "Select map element to get it's description"

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
        if(params['x']!=null && params['y']!=null)
        {
          let requestedX = Number(params['x']), requestedY = Number(params['y']);
          this.uniquelyColouredFields.set(`${requestedX}|${requestedY}`,'green');
          this.edgeX=requestedX-10;
          this.edgeY=requestedY-10;
          this.refreshMap();
          this.mapUtils.clearPath();
        }
        else
        {
          this.mapService.getEdgeForBoss().subscribe(x=>
            {
              this.edgeX=x[0]
              this.edgeY=x[1]
              this.refreshMap();
              this.mapUtils.clearPath();
            })
        }
      });
  }

  onMouseEnter(mapField: MapField)
  {
    this.setDescriptionOfMapElement(mapField);
  }

  setDescriptionOfMapElement(mapField: MapField)
  {
    let description: string = `Position: [${mapField.Y},${mapField.X}]`
    if(mapField.Description != null)
      description += `  ${mapField.Description}`;
    this.mapElementDescription = description;
  }

  closeAndRefresh(operation: number) {
    this.modalService.dismissAll();
    if(operation==1)
    {
      this.mapMode=MapModes.CreatingMissionPath
    }
    else
    {
      this.mapMode=MapModes.Nothing
      this.mapUtils.clearPath();
    }
    this.refreshMap();
  }

  setAmbushMode()
  {
    this.mapMode=MapModes.SettingAmbush
    this.mapUtils.clearPath();
    this.refreshMap();
  }

  setPatrolMode()
  {
    this.mapMode=MapModes.CreatingPatrolPath
    this.mapUtils.clearPath();
    this.refreshMap();
  }

  refreshMap(){
    this.mapService.getMap(this.edgeX,this.edgeY,20).subscribe(
      x=>
      {
        this.mapFields=x
      })
  }

  getTerrainColor(x: number, y: number, terrainTypeId: number): string{
    let color = this.uniquelyColouredFields.get(`${x}|${y}`)
    if(color)
    {
      return color;
    }
    switch(terrainTypeId)
    {
      case TerrainTypes.Road:
        return "gray";
      case TerrainTypes.BuildUpArea:
        return "darkred";
    }
  }

  getBossId()
  {
    return this.tokenService.getBossId();
  }

  moveMap(direction: Directions)
  {
    switch(direction)
    {
      case Directions.Up:
        this.edgeX--;
      break;
      case Directions.Right:
        this.edgeY++;
      break;
      case Directions.Down:
        this.edgeX++;
      break;
      case Directions.Left:
        this.edgeY--;
      break;
    }
    this.refreshMap();
  }

  mapElementClick(mapFieldId: number, elementType: number){
    this.chosenMapFieldId=mapFieldId
    this.chosenElementType=elementType
    this.modalService.open(this.mapElementModal, {ariaLabelledBy: 'modal-basic-title'});
  }

  mapFieldClick(mapFieldId: number, X:number, Y:number, mapElementType: number, terrainType: number){
    if(this.mapMode==MapModes.CreatingMissionPath)
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
    else if(this.mapMode == MapModes.SettingAmbush)
    {
      if(!this.mapUtils.isRoad(X,Y))
      {
        alert('This field is not a road')
        return
      }

      this.mapUtils.clearPath();

      if(!this.mapUtils.isPointPath(X,Y))
      {
        this.mapUtils.addPointToPath(X,Y);
      }
    }

    else if(this.mapMode == MapModes.CreatingPatrolPath)
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
    if(operation==Operations.RoadReady)
    {
      this.modalService.open(this.mapElementModal, {ariaLabelledBy: 'modal-basic-title'});
    }
    if(operation==Operations.Cancel)
    {
      this.mapMode=MapModes.Nothing
      this.mapUtils.clearPath();
    }
    if(operation==Operations.AmbushPointReady)
    {
      if(this.mapUtils.getPath().length!=1)
      {
        alert('You have to choose 1 point');
        return;
      }
      this.chosenElementType=0;
      this.modalService.open(this.mapElementModal, {ariaLabelledBy: 'modal-basic-title'});
    }

    if(operation==Operations.PatrolPathReady)
    {
      console.log("Test");
      this.chosenElementType=0;
      this.modalService.open(this.mapElementModal, {ariaLabelledBy: 'modal-basic-title'});
    }
  }
}
