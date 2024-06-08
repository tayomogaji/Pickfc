import { Art } from "./art"

export interface User {
  id: number
  artID: number
  firstName: string
  lastName: string
  fullName: string
  email: string
  password: string
  code: string
  notify: boolean
  admin: boolean
  fullAdmin: boolean
  canAdd: boolean
  verifyTime?: Date
  codeExpires?: Date
  art: Art
}
