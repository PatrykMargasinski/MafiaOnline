import { Point } from "../map/point.models";

export interface ArrangeAmbushRequest
{
  AgentId: number,
  Point: Point
}

export interface CancelAmbushRequest
{
  MapElementId: number
}
