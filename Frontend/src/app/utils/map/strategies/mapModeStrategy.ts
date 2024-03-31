import { MapButton, MapOperations } from "src/app/models/map/mapField.models";
import { MapUtils } from "../../map-utils";

export interface MapModeStrategy {
  mapFieldClick(X: number, Y: number): void;
  getButtons(): MapButton[];
}

export class CreatingMissionPathMapModeStrategy implements MapModeStrategy {
  mapUtils: MapUtils;

  constructor(mapUtils: MapUtils) {
      this.mapUtils = mapUtils;
  }

  getButtons(): MapButton[] {
    var buttons: MapButton[] =
    [
      new MapButton("Mission path ready", MapOperations.MissionPathReady),
      new MapButton("Cancel", MapOperations.Cancel)
    ]
    return buttons;
  }

  mapFieldClick(X: number, Y: number): void {
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

export class NothingMissionPathMapModeStrategy implements MapModeStrategy {
  mapUtils: MapUtils;

  constructor(mapUtils: MapUtils) {
      this.mapUtils = mapUtils;
  }

  getButtons(): MapButton[] {
    var buttons: MapButton[] =
    [
      new MapButton("Set ambush", MapOperations.StartSettingAmbush),
      new MapButton("Patrol", MapOperations.StartSettingPatrolPath)
    ]
    return buttons;
  }

  mapFieldClick(X: number, Y: number): void {

  }
}

export class SettingAmbushMapModeStrategy implements MapModeStrategy {
  mapUtils: MapUtils;

  constructor(mapUtils: MapUtils) {
      this.mapUtils = mapUtils;
  }

  getButtons(): MapButton[] {
    var buttons: MapButton[] =
    [
      new MapButton("Ambush point ready", MapOperations.AmbushPointReady),
      new MapButton("Cancel", MapOperations.Cancel)
    ]
    return buttons;
  }


  mapFieldClick(X: number, Y: number): void {
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
}

export class CreatingPatrolPathMapModeStrategy implements MapModeStrategy {
  mapUtils: MapUtils;

  constructor(mapUtils: MapUtils) {
      this.mapUtils = mapUtils;
  }
  getButtons(): MapButton[] {
    var buttons: MapButton[] =
    [
      new MapButton("Patrol path ready", MapOperations.PatrolPathReady),
      new MapButton("Cancel", MapOperations.Cancel)
    ]
    return buttons;
  }

  mapFieldClick(X: number, Y: number): void {
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
