export interface Boss
{
  BossId: number,
  Name: string,
  Money: number,
  LastSeen: string
}

export interface BossWithPosition
{
  BossId: number,
  Name: string,
  Money: number,
  LastSeen: string,
  Position: number
}
