import React, { useState, useEffect } from "react";
import "../../styles/lobbySidebarStyles.css";
import { Priorities } from "../../models/Player.ts";
import { ERank } from "../../models/enums/ERank.ts";
import axios from "axios";
import Global from "../../config.tsx";
import Button from "../shared/Button.tsx";
import RoleField from "./RoleField.tsx";
import { ERole, LoginCookie } from "../../models/LoginCookie.ts";
import { AccountDTO } from "../../models/Account.ts";

function DefaultAccount(): AccountDTO {
  return {
    id: 0,
    nickname: "placeholder",
    role: ERole.User,
    rank: ERank.Iron4,
    top: 3,
    jng: 3,
    mid: 3,
    bot: 3,
    supp: 3,
  };
}

export default function JoinLobbyForm({
  cookie,
  canJoin,
}: {
  cookie: LoginCookie;
  canJoin: boolean;
}) {
  const [priorities, setPriorities] = useState<Priorities>({
    top: 3,
    jng: 3,
    mid: 3,
    bot: 3,
    supp: 3,
  });
  async function fetchAccount(): Promise<AccountDTO> {
    const { data } = await axios<AccountDTO>({
      url: Global.serverUrl + "Account/Get",
      method: "POST",
      data: { id: cookie.id },
    });
    return data;
  }
  useEffect(() => {
    fetchAccount().then((data) => {
      setAccount(data);
      setPriorities({
        top: data.top,
        jng: data.jng,
        mid: data.mid,
        bot: data.bot,
        supp: data.supp,
      });
    });
  }, []);
  const [account, setAccount] = useState<AccountDTO>(DefaultAccount());

  const handlePriorityChange = (priority: number, position: string) => {
    setPriorities((prevData) => ({
      ...prevData,
      [position]: priority,
    }));
  };

  async function handleSubmit() {
    await axios({
      url: Global.serverUrl + "Lobby/Join",
      method: "POST",
      data: {
        priorities: priorities,
        id: cookie.id,
      },
    });
  }

  return (
    <div className="join-lobby">
      <div className="join-lobby-nickname">{account.nickname}</div>
      <div className="join-lobby-rank">{ERank[account.rank]}</div>
      <RoleField
        name="top"
        onChange={handlePriorityChange}
        priority={priorities.top}
      />
      <br />
      <RoleField
        name="jng"
        onChange={handlePriorityChange}
        priority={priorities.jng}
      />
      <br />
      <RoleField
        name="mid"
        onChange={handlePriorityChange}
        priority={priorities.mid}
      />
      <br />
      <RoleField
        name="bot"
        onChange={handlePriorityChange}
        priority={priorities.bot}
      />
      <br />
      <RoleField
        name="supp"
        onChange={handlePriorityChange}
        priority={priorities.supp}
      />
      <br />
      <div className="join-lobby-join-button-wrapper">
        <Button onClick={handleSubmit} disabled={!canJoin}>
          Join
        </Button>
      </div>
    </div>
  );
}
