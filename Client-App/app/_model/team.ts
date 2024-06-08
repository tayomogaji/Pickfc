import { Comp } from "./comp"

export interface Team{
  id: number
  compID: number
  name: string
  pic: string
  rating: number
  compsCount: number
  club: boolean
  inComp: boolean
  hasFixture: boolean
  comps: Comp[]
  timestamped: Date
}
