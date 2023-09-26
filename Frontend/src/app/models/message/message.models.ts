export class Message
{
  Id: number = 0;
  FromBossId: number = 0;
  ToBossId: number = 0;
  FromBossName: string = "";
  ToBossName: string = "";
  Subject: string = "";
  Content: string = "";
  ReceivedDate: Date = new Date();
  Seen: boolean = false;
  IsReport: boolean = true;
}

export interface MessageToSend
{
  ToBossFullName: string,
  FromBossId: number,
  Content: string,
  Subject: string
}
