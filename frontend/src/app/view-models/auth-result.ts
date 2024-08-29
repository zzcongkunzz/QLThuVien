import {User} from "./user";

export interface AuthResult {
  token: string;
  refresh_token: string;
  expiresAt: Date;
  userInformation : User
}
