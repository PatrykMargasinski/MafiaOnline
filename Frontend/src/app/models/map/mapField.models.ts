export interface MapField
{
  Id : number,
  X: number,
  Y: number,
  TerrainType: number,
  MapElementType : number,
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


export enum Operations
{
  Cancel, RoadReady, AmbushPointReady, PatrolPathReady
}
