import React, { useState, useEffect } from "react";
import Layout from "../Layout.tsx";
import { MatchPlayers, PlayerInfo } from "../models/MatchPlayers.ts";
import Global from "../config.tsx";
import { ERank } from "../models/enums/ERank.ts";
import MatchPosition from "../components/match/MatchPosition.tsx";
import ResetLobbyButton from "../components/buttons/ResetLobbyButton.tsx";
import axios from "axios";
import Button from "../components/shared/Button.tsx";
import { useNavigate } from "react-router-dom";
import * as signalR from "@microsoft/signalr";
import Cookie from "../components/functional/Cookie.tsx";

async function fetchMatch(): Promise<MatchPlayers> {
  const { data } = await axios<MatchPlayers>({
    url: Global.serverUrl + "Match/Get",
    method: "GET",
  });
  return data;
}

function DefaultPlayer(): PlayerInfo {
  return { id: 0, rank: ERank.Bronze1, nickname: "placeholer" };
}
function DefaultPlayers(): MatchPlayers {
  return {
    blue: {
      players: {
        Top: DefaultPlayer(),
        Jng: DefaultPlayer(),
        Mid: DefaultPlayer(),
        Bot: DefaultPlayer(),
        Supp: DefaultPlayer(),
      },
    },
    red: {
      players: {
        Top: DefaultPlayer(),
        Jng: DefaultPlayer(),
        Mid: DefaultPlayer(),
        Bot: DefaultPlayer(),
        Supp: DefaultPlayer(),
      },
    },
    unassignedPlayers: 0,
  };
}

export default function Match() {
  const [matchPlayers, setMatchPlayers] = useState<MatchPlayers>(
    DefaultPlayers()
  );

  const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(Global.serverUrl + "lobbyHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

  useEffect(() => {
    fetchMatch().then((data) => {
      setMatchPlayers(data);
    });
    hubConnection.start();
    hubConnection.on("refreshLobby", () => {
      navigate("/");
    });
  }, []);
  const navigate = useNavigate();

  async function handleReviewLobby() {
    await axios({
      url: Global.serverUrl + "LobbyAdmin/Review",
      method: "POST",
    });
    navigate("/");
  }

  const cookie = Cookie();

  return (
    <Layout>
      {cookie.isAdmin && (
        <>
          <ResetLobbyButton redirectUrl="/" />
          <Button onClick={handleReviewLobby}>Review lobby</Button>
        </>
      )}

      <table className="match-table">
        <thead>
          <th />
          <th>BLUE</th>
          <th>RED</th>
        </thead>
        <tbody>
          <MatchPosition
            bluePlayer={matchPlayers.blue.players.Top}
            redPlayer={matchPlayers.red.players.Top}
            positionName="TOP"
          />
          <MatchPosition
            bluePlayer={matchPlayers.blue.players.Jng}
            redPlayer={matchPlayers.red.players.Jng}
            positionName="JNG"
          />
          <MatchPosition
            bluePlayer={matchPlayers.blue.players.Mid}
            redPlayer={matchPlayers.red.players.Mid}
            positionName="MID"
          />
          <MatchPosition
            bluePlayer={matchPlayers.blue.players.Bot}
            redPlayer={matchPlayers.red.players.Bot}
            positionName="BOT"
          />
          <MatchPosition
            bluePlayer={matchPlayers.blue.players.Supp}
            redPlayer={matchPlayers.red.players.Supp}
            positionName="SUPP"
          />
        </tbody>
      </table>
    </Layout>
  );
}
