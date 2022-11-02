export interface LoginRequest
{
  Nick: string,
  Password: string
}

export interface RegisterRequest
{
  Nick: string,
  Password: string,
  BossFirstName: string,
  BossLastName: string,
  AgentNames: string[]
}

export interface ChangePasswordRequest
{
  PlayerId: number,
  OldPassword: string,
  NewPassword: string,
  RepeatedNewPassword: string
}

export interface DeleteAccountRequest
{
  PlayerId: number,
  Password: string
}
