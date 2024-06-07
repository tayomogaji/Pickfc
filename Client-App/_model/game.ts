import { Player } from "./player"
import { User } from "./user"
import { Comp } from "./comp"

export interface Game {
  id: number
  creatorID: number
  compID: number
  name: string
  pic: string
  code: string
  public: boolean
  legacy: boolean
  roundDeadlined: boolean
  deadline: boolean
  deadlineDate: Date
  timestamped: Date
  comp: Comp
  creator: User
  currentPlayer: Player
  players: Player[]
  admins: Player[]
}
