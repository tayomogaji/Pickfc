import { User } from "./user"

export interface Comp {
  id: number
  roundID: number
  name: string
  pic: string
  teamsCount: number
  teamsRemaining: number
  teamsTotal: number
  legacy: boolean
  active: boolean
  hasTeam: boolean
  openNotified: boolean
  open: Date
  timestamped: Date
  admin: User
}
