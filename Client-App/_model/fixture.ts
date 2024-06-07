import { Team } from "./team"

export interface Fixture{
  id: number
  roundID: number
  homeID: number
  awayID: number
  homeResult: string
  awayResult: string
  resultReset: boolean
  timeStamped: Date
  home: Team
  away: Team
}
