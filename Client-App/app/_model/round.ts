import { Fixture } from "./fixture"
import { Pick } from "./pick"

export interface Round {
  id: number
  compID: number
  number: number
  deadlineMsecs: number
  deadlineDays: number
  name: string
  show: boolean
  current: boolean
  startNotified: boolean
  deadlineNotified: boolean
  start: Date
  deadline: Date
  timestamped: Date
  fixtures: Fixture[]
  picks: Pick[]
}
