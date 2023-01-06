export interface Mission
{
  Id: number,
  X: number,
  Y: number,
  Name: string,
  DifficultyLevel: number,
  StrengthPercentage: number,
  DexterityPercentage: number,
  IntelligencePercentage: number,
  Loot: number,
  Duration: number
}

export interface PerformingMission
{
  Id: number,
  X: number,
  Y: number,
  MissionId: number,
  AgentId: number,
  MissionName: string,
  AgentName: string,
  SuccessChance: number,
  Loot: number,
  CompletionTime: Date,
  SecondsLeft: number
}
