export interface MapField
{
  Id : number,
  X: number,
  Y: number,
  TerrainType: TerrainTypes,
  MapElementType : MapElementType,
  Owner: number,
  Description: string
}

export enum TerrainTypes
{
  Road, BuildUpArea
}


export enum MapModes
{
  Nothing, CreatingMissionPath, SettingAmbush, CreatingPatrolPath
}

export enum Directions
{
  Up, Right, Down, Left
}


export enum MapOperations
{
  Cancel,
  StartSettingMissionPath,
  StartSettingAmbush,
  StartSettingPatrolPath,
  MissionPathReady,
  AmbushPointReady,
  PatrolPathReady,
  MissionChosen
}

export enum MapElementType
{
  None,
  Headquarters,
  Mission,
  Ambush
}

export class MapButton
{
  constructor(text: string, operation: MapOperations)
  {
    this.Text=text;
    this.Operation=operation;
  }

  Text: string
  Operation: MapOperations
}
