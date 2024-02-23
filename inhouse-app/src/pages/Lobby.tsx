import Global from "../config.tsx";
import React, { useState, useEffect } from "react";
import { WaitingPlayersDTO } from "../models/Player.ts";
import WaitingPlayersTable from "../components/lobby/WaitingPlayersTable.tsx";
import Layout from "../Layout.tsx";
import LobbySidebar from "../components/lobby/LobbySidebar.tsx";
import "../styles/homePageStyles.css";
import { useNavigate } from "react-router-dom";
import { useModal, Modal } from "../components/shared/Modal.tsx";
import Cookie from "../components/functional/Cookie.tsx";
import axios from "axios";
import * as signalR from "@microsoft/signalr";
import Button from "../components/shared/Button.tsx";

export default function Lobby() {
  const cookie = Cookie();
  const navigate = useNavigate();

  const [waitingPlayers, setWaitingPlayers] = useState<WaitingPlayersDTO>({
    players: [],
    isMatchReady: false,
  });

  const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(Global.serverUrl + "lobbyHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

  const { isOpen, toggle } = useModal();

  async function fetchWaitingPlayers(): Promise<WaitingPlayersDTO> {
    const { data } = await axios<WaitingPlayersDTO>({
      url: Global.serverUrl + "Lobby/Get",
      method: "GET",
    });
    setWaitingPlayers(data);
    if (data.isMatchReady) toggle();
    return data;
  }

  const navigateMatch = () => {
    navigate("/match");
  };

  useEffect(() => {
    fetchWaitingPlayers();
    hubConnection.start();
    hubConnection.on("refreshLobby", () => {
      fetchWaitingPlayers();
    });
    hubConnection.on("matchReady", () => {
      toggle();
    });
  }, []);

  return (
    <Layout>
      <div>
        <table className="home-page-layout-table">
          <tbody>
            <tr>
              <td className="home-page-layout-table-player-info-column">
                <LobbySidebar
                  cookie={cookie}
                  players={waitingPlayers.players}
                />
              </td>
              <td className="home-page-layout-table-waiting-players-column">
                <WaitingPlayersTable players={waitingPlayers.players} />
              </td>
            </tr>
          </tbody>
        </table>
        <Modal isOpen={isOpen}>
          <div className="home-page-match-found">
            <div>MATCH FOUND</div>
            <Button onClick={navigateMatch}>Go to match</Button>
          </div>
        </Modal>
      </div>
    </Layout>
  );
}
