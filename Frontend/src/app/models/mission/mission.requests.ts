import { Point } from "../map/point.models";

export interface StartMissionRequest
{
  MissionId: number,
  AgentId: number,
  Path: Point[]
}
