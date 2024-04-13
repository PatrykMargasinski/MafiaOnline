import { BaseQuery } from "../base/BaseQuery"
import { Point } from "../map/point.models"

export interface Agent
{
  Id: number,
  BossId: number,
  LastName: string,
  FirstName: string,
  FullName: string,
  Strength: number,
  Dexterity: number,
  Intelligence: number,
  Upkeep: number,
  State: AgentState,
  Substate?: AgentSubstate,
  StateName: string,
  SubstateName?: string
}

export interface VAgent
{
  Id: number,
  BossId: number,
  LastName: string,
  FirstName: string,
  FullName: string,
  Strength: number,
  Dexterity: number,
  Intelligence: number,
  Upkeep: number,
  State: AgentState,
  Substate?: AgentSubstate,
  StateName: string,
  SubstateName?: string,
  FinishTime?: Date
}

export interface AgentForSale
{
  Id: number,
  BossId: number,
  LastName: string,
  FirstName: string,
  Strength: number,
  Dexterity: number,
  Intelligence: number,
  Upkeep: number,
  Price: number
}

export interface AgentOnMission
{
  Id: number,
  PerformingMissionId: number,
  MissionId: number,
  AgentName: string,
  MissionName: string,
  MissionPosition: Point,
  Strength: number,
  Dexterity: number,
  Intelligence: number,
  SuccessChance: number,
  CompletionTime: Date,
  SecondsLeft: number
}

export interface MovingAgent
{
  Id: number,
  BossId: number,
  LastName: string,
  FirstName: string,
  Strength: number,
  Dexterity: number,
  Intelligence: number,
  Upkeep: number,
  DestinationDescription: string,
  CurrentPosition: Point,
  DestinationPosition: Point,
  ArrivalTime: Date,
  SecondsLeft: number
}

export interface AmbushingAgent
{
  Id: number,
  AmbushId: number,
  MapElementId: number,
  LastName: string,
  FirstName: string,
  Strength: number,
  Dexterity: number,
  Intelligence: number,
  Position: Point
}


export class AgentQuery extends BaseQuery
{
    Name: string = ""
    State: AgentState = null
}

export enum AgentState
{
  Renegate = 1,
  ForSale = 2,
  OnMission = 3,
  Active = 4,
  Moving = 5,
  Ambushing = 6
}

export enum AgentSubstate
{
    MovingOnMission = 1,
    Patrolling = 2,
    MovingWithLoot = 3
}
