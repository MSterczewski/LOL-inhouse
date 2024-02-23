import React from "react";
import Global from "../../config.tsx";
import { Player } from "../../models/Player.ts";
import "../../styles/waitingPlayersStyles.css";
import { ERank } from "../../models/enums/ERank.ts";
import Cookie from "../functional/Cookie.tsx";
import Button from "../shared/Button.tsx";
import axios from "axios";
import PriorityIcon from "../shared/PriorityIcon.tsx";

export default function WaitingPlayersTable({
  players,
}: {
  players: Player[];
}) {
  const cookie = Cookie();

  const placeholders: Array<Player> = [];
  for (let i = players.length; i < 10; i++) {
    placeholders.push({
      id: -i,
      nickname: "a",
      priorities: { top: 3, jng: 3, mid: 3, bot: 3, supp: 3 },
      rank: 1,
    });
  }
  return (
    <div className="home-page-waiting-players-container">
      Count: {players.length}/10
      {cookie?.isAdmin && (
        <div className="home-page-waiting-players-start-matchmaking-button">
          <Button onClick={handleMatchmake} disabled={players.length != 10}>
            Start matchmaking
          </Button>
        </div>
      )}
      <table className="home-page-waiting-players-table">
        <tbody>
          {players.map((user) => (
            <tr
              key={user.id}
              className="home-page-waiting-players-single-player"
            >
              <td className="home-page-waiting-players-single-player-avatar-column">
                <img
                  src={cookie.imageUrl}
                  className="home-page-waiting-players-single-player-avatar"
                  alt={user.nickname}
                />
              </td>
              <td>
                <div className="home-page-waiting-players-single-player-nickname">
                  {user.nickname}
                </div>
                {ERank[user.rank]}
              </td>

              <td className="home-page-waiting-players-single-player-priority">
                {priorityIcon(user.priorities.top, "Top: ")}
              </td>
              <td className="home-page-waiting-players-single-player-priority">
                {priorityIcon(user.priorities.jng, "Jng: ")}
              </td>
              <td className="home-page-waiting-players-single-player-priority">
                {priorityIcon(user.priorities.mid, "Mid: ")}
              </td>
              <td className="home-page-waiting-players-single-player-priority">
                {priorityIcon(user.priorities.bot, "Bot: ")}
              </td>
              <td className="home-page-waiting-players-single-player-priority">
                {priorityIcon(user.priorities.supp, "Supp: ")}
              </td>
              {cookie?.isAdmin && (
                <td className="home-page-waiting-players-single-player-remove-player">
                  <button
                    className="home-page-waiting-players-single-player-remove-player-button"
                    onClick={() => {
                      handleRemovePlayer(user.id);
                    }}
                  >
                    <img
                      src="https://uxwing.com/wp-content/themes/uxwing/download/checkmark-cross/cross-icon.png"
                      className="home-page-waiting-players-single-player-remove-player-image"
                      alt="Remove player"
                    />
                  </button>
                </td>
              )}
            </tr>
          ))}
          {placeholders.map((user) => (
            <tr
              key={user.id}
              className="home-page-waiting-players-single-player"
            >
              <td className="home-page-waiting-players-single-player-avatar-column">
                <img
                  src={Global.defaultPlayerImage}
                  className="home-page-waiting-players-single-player-avatar-placeholder"
                  alt={user.nickname}
                />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

async function handleRemovePlayer(id: number) {
  await axios({
    url: Global.serverUrl + "LobbyAdmin/RemovePlayer",
    method: "POST",
    data: {
      id,
    },
  });
  window.location.reload();
}

async function handleMatchmake() {
  await axios({
    url: Global.serverUrl + "Matchmaking/Matchmake",
    method: "POST",
  });
}

function priorityIcon(priority: number, label: string) {
  return (
    <div className="home-page-waiting-players-single-player-priorities-grouped-indicators">
      {label}
      {PriorityIcon({ priority, size: 1 })}
    </div>
  );
}
