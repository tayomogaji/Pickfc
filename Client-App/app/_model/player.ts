import { User } from "./user"
import { Pick } from "./pick"

export interface Player {
  id: number
  gameID: number
  pickID: number

  hitByID: number
  hitsTotal: number
  hitsPlayed: number
  boostTotal: number
  boostPlayed: number

  life: number
  pos: number
  pts: number
  roundPts: number
  streak: number
  champs: number
  pickTime: number
  roundPickTime: number

  name: string
  hitByPic: string
  hitByName: string

  eliminated: boolean
  admin: boolean
  active: boolean

  timestamped: Date

  user: User
  pick: Pick
}
