import { ERank } from "./enums/ERank";
export interface Player {
  nickname: string;
  id: number;
  rank: ERank;
  priorities: Priorities;
}

export interface Priorities {
  top: number;
  jng: number;
  mid: number;
  bot: number;
  supp: number;
}
export interface WaitingPlayersDTO {
  players: Player[];
  isMatchReady: boolean;
}
