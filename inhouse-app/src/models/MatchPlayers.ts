import { ERank } from "./enums/ERank";
export interface MatchPlayers {
  blue: Team;
  red: Team;
  unassignedPlayers: number;
}
interface Team {
  players: {
    Top: PlayerInfo;
    Jng: PlayerInfo;
    Mid: PlayerInfo;
    Bot: PlayerInfo;
    Supp: PlayerInfo;
  };
}
export interface PlayerInfo {
  id: number;
  rank: ERank;
  nickname: string;
}
