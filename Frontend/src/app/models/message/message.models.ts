export interface Message
{
  Id: number,
  FromBossId: number,
  ToBossId: number,
  FromBossName: string,
  ToBossName: string,
  Subject: string,
  Content: string,
  ReceivedDate: Date,
  Seen: boolean
}

export interface MessageToSend
{
  ToBossFullName: string,
  FromBossId: number,
  Content: string,
  Subject: string
}
