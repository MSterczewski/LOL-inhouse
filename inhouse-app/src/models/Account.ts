import { ERole } from "./LoginCookie";
import { ERank } from "./enums/ERank";

export interface AccountDTO {
  nickname: string;
  id: number;
  rank: ERank;
  role: ERole;
  top: number;
  jng: number;
  mid: number;
  bot: number;
  supp: number;
}

export enum EEditAccountError {
  PlayerNotFound,
  NicknameDuplicate,
}
