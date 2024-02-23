import React from "react";
import { PlayerInfo } from "../../models/MatchPlayers.ts";
import MatchSinglePlayer from "./MatchSinglePlayer.tsx";
import "../../styles/matchStyles.css";

export default function MatchPosition({
  redPlayer,
  bluePlayer,
  positionName,
}: {
  redPlayer: PlayerInfo;
  bluePlayer: PlayerInfo;
  positionName: string;
}) {
  return (
    <>
      <tr>
        <td className="match-position-cell">{positionName}:</td>
        <MatchSinglePlayer player={bluePlayer} />
        <MatchSinglePlayer player={redPlayer} />
      </tr>
    </>
  );
}
