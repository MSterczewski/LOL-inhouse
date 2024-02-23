import React from "react";
import { PlayerInfo } from "../../models/MatchPlayers.ts";
import { ERank } from "../../models/enums/ERank.ts";
import "../../styles/matchStyles.css";

export default function Match({ player }: { player: PlayerInfo }) {
  return (
    <td className="match-player-cell">
      <table>
        <tbody>
          <tr>
            <td>{player.nickname}</td>
          </tr>
          <tr>
            <td>{ERank[player.rank]}</td>
          </tr>
        </tbody>
      </table>
    </td>
  );
}
