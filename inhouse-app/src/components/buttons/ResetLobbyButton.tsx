import React from "react";
import Button from "../shared/Button.tsx";
import Global from "../../config.tsx";
import { useNavigate } from "react-router-dom";
import axios from "axios";

export default function ResetLobbyButton({
  redirectUrl = null,
}: {
  redirectUrl?: string | null;
}) {
  const navigate = useNavigate();

  async function handleResetLobby() {
    await axios({
      url: Global.serverUrl + "LobbyAdmin/Reset",
      method: "POST",
    });
    if (redirectUrl) navigate(redirectUrl);
  }

  return <Button onClick={handleResetLobby}>Reset lobby</Button>;
}
