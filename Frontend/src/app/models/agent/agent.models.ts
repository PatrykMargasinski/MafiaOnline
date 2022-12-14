export interface Agent
{
  Id: number,
  BossId: number,
  LastName: string,
  FirstName: string,
  Strength: number,
  Dexterity: number,
  Intelligence: number,
  Upkeep: number
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
  Strength: number,
  Dexterity: number,
  Intelligence: number,
  SuccessChance: number,
  CompletionTime: Date,
  SecondsLeft: number
}
