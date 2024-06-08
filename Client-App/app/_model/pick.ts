import { Team } from "./team"

export interface Pick{
  id: number
  roundID: number
  gameID: number
  playerID: number
  teamID: number
  roundNumber: number
  playerName: string
  playerPic: string
  result: string
  time: Date
  timestamped: Date
  team: Team
}
