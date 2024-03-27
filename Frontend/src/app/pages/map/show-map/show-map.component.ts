import { MapElementType as MapElementTypes } from './../../../models/map/mapField.models';
import { MapUtils } from './../../../utils/map-utils';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Directions, MapField, MapModes, MapOperations, TerrainTypes } from 'src/app/models/map/mapField.models';
import { MapService } from 'src/app/services/map/map.service';
import { TokenService } from 'src/app/services/auth/token.service';
import { ActivatedRoute } from '@angular/router';
import { CreatingMissionPathMapModeStrategy, CreatingPatrolPathMapModeStrategy, MapModeStrategy, NothingMissionPathMapModeStrategy, SettingAmbushMapModeStrategy } from 'src/app/utils/map/strategies/mapModeStrategy';

@Component({
  selector: 'app-show-map',
  templateUrl: './show-map.component.html',
  styleUrls: ['./show-map.component.css']
})
export class ShowMapComponent implements OnInit {

  edgeX : number
  edgeY : number

  constructor(private mapService: MapService, private tokenService: TokenService, private modalService: NgbModal, private mapUtils: MapUtils, private activatedRoute: ActivatedRoute)
  {
    this.mapModeStrategies.set(MapModes.Nothing, new NothingMissionPathMapModeStrategy(mapUtils));
    this.mapModeStrategies.set(MapModes.CreatingMissionPath, new CreatingMissionPathMapModeStrategy(mapUtils));
    this.mapModeStrategies.set(MapModes.SettingAmbush, new SettingAmbushMapModeStrategy(mapUtils));
    this.mapModeStrategies.set(MapModes.CreatingPatrolPath, new CreatingPatrolPathMapModeStrategy(mapUtils));
  }

  mapFields: MapField[]
  chosenMapFieldId: number
  chosenElementType: MapElementTypes
  mapMode: MapModes = MapModes.Nothing; //0 - nothing, 1 - creating path back from mission, 2 - set ambush, 3 - create path to patrol
  mapModeStrategies: Map <MapModes, MapModeStrategy> = new Map();

  @ViewChild('mapElementModal') mapElementModal : TemplateRef<any>;

  //enums (To be able to use them in the html file I need to output them here)
  MapOperations = MapOperations
  MapModes = MapModes
  TerrainTypes = TerrainTypes
  MapElementTypes = MapElementTypes
  Directions = Directions

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
    switch(operation)
    {
      case MapOperations.MissionChosen:
      case MapOperations.Cancel:
        this.setMode(MapModes.Nothing)
      break;
      case MapOperations.StartSettingRoad:
        this.setMode(MapModes.CreatingMissionPath)
      break;
    }
    this.refreshMap();
  }

  setMode(mode: MapModes)
  {
    this.mapMode=mode;
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

  mapElementClick(mapFieldId: number, elementType: MapElementTypes){
    if(this.mapMode == MapModes.Nothing)
    {
      this.chosenMapFieldId=mapFieldId
      this.chosenElementType=elementType
      this.modalService.open(this.mapElementModal, {ariaLabelledBy: 'modal-basic-title'});
    }
  }

  mapFieldClick(X:number, Y:number){
    this.mapModeStrategies.get(this.mapMode).mapFieldClick(X, Y);
  }

  isChosenAsAgentPath(X: number, Y: number)
  {
    return this.mapUtils.isPointPath(X, Y);
  }

  pathReadyOperations(operation: MapOperations)
  {
    switch(operation)
    {
      case MapOperations.RoadReady:
        this.modalService.open(this.mapElementModal, {ariaLabelledBy: 'modal-basic-title'});
        break;
      case MapOperations.Cancel:
        this.setMode(MapModes.Nothing);
        break;
      case MapOperations.AmbushPointReady:
        if(this.mapUtils.getPath().length != 1)
        {
          alert('You have to choose 1 point');
          return;
        }
        this.chosenElementType = MapElementTypes.None;
        this.modalService.open(this.mapElementModal, {ariaLabelledBy: 'modal-basic-title'});
        break;
      case MapOperations.PatrolPathReady:
        this.chosenElementType = MapElementTypes.None;
        this.modalService.open(this.mapElementModal, {ariaLabelledBy: 'modal-basic-title'});
        break;
    }
  }
}
