import React from "react";
import Global from "../../config.tsx";
import { Player } from "../../models/Player.ts";
import JoinLobbyForm from "../../components/lobby/JoinLobbyForm.tsx";
import "../../styles/homePageStyles.css";
import Button from "../../components/shared/Button.tsx";
import ResetLobbyButton from "../../components/buttons/ResetLobbyButton.tsx";
import axios from "axios";
import { LoginCookie } from "../../models/LoginCookie.ts";

export default function LobbySidebar({
  players,
  cookie,
}: {
  players: Player[];
  cookie: LoginCookie;
}) {
  async function handleLeaveLobby() {
    await axios({
      url: Global.serverUrl + "Lobby/Leave",
      method: "POST",
      data: {
        id: cookie.id,
      },
    });
  }
  async function handleFillWithBots() {
    await axios({
      url: Global.serverUrl + "LobbyAdmin/FillWithBots",
      method: "POST",
    });
  }
  return (
    <div className="home-page-player-info">
      <>
        {players.some((p) => p.nickname == cookie.nickname) ? (
          <div>
            <Button onClick={handleLeaveLobby}>Leave</Button>
          </div>
        ) : (
          <JoinLobbyForm cookie={cookie} canJoin={players.length < 10} />
        )}
        {cookie.isAdmin && (
          <>
            <ResetLobbyButton />
            <br />
            <Button onClick={handleFillWithBots}>Fill with bots</Button>
          </>
        )}
      </>
    </div>
  );
}
