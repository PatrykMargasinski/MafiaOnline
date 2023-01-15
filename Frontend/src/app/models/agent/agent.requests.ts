import { Point } from "../map/point.models"

export interface RecruitAgentRequest
{
  BossId: number,
  AgentId: number
}

export interface DismissAgentRequest
{
  AgentId: number
}

export interface PatrolRequest
{
  AgentId: number,
  Path: Point[]
}


