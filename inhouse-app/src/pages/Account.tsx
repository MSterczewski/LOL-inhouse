import React, { useState, useEffect } from "react";
import Layout from "../Layout.tsx";
import Global from "../config.tsx";
import { ERank } from "../models/enums/ERank.ts";
import { AccountDTO, EEditAccountError } from "../models/Account.ts";
import { ERole } from "../models/LoginCookie.ts";
import axios from "axios";
import Cookie from "../components/functional/Cookie.tsx";
import RoleField from "../components/lobby/RoleField.tsx";
import Button from "../components/shared/Button.tsx";
import "../styles/accountStyles.css";
import Select from "react-select";
import Input from "../components/shared/Input.tsx";

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

interface Rank {
  rank: ERank;
}

export default function Account() {
  const [account, setAccount] = useState<AccountDTO>(DefaultAccount());
  const [accountEdited, setAccountEdited] = useState<AccountDTO>(
    DefaultAccount()
  );

  const cookie = Cookie();

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
      setAccountEdited(data);
    });
  }, []);
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setErrorMessage(null);
    const { name, value } = e.target;
    setAccountEdited((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handlePriorityChange = (priority: number, position: string) => {
    setErrorMessage(null);
    setAccountEdited((prevData) => ({
      ...prevData,
      [position]: priority,
    }));
  };

  const handleRankChange = (rank: ERank) => {
    setErrorMessage(null);
    setAccountEdited((prevData) => ({
      ...prevData,
      rank: rank,
    }));
  };

  const [isEdit, setIsEdit] = useState<boolean>(false);
  function handleAccountEdit() {
    setIsEdit(true);
  }

  function handleAccountCancelEdit() {
    setAccountEdited(account);
    setIsEdit(false);
  }
  async function handleSubmit() {
    await axios<void>({
      url: Global.serverUrl + "Account/Edit",
      method: "POST",
      data: accountEdited,
    })
      .then(() => {
        setIsEdit(false);
        setAccount(accountEdited);
      })
      .catch((error) => {
        if (error.response.status == 400) {
          console.log(error.response.data);
          switch (error.response.data) {
            case EEditAccountError.NicknameDuplicate:
              handleError("There is a player with this nickname already");
              break;
            case EEditAccountError.PlayerNotFound:
              handleError("Player not found");
              break;
          }
        } else throw new Error("Unhandled response code");
      });
  }
  function handleError(message: string) {
    setErrorMessage(message);
  }
  const [errorMessage, setErrorMessage] = useState<string | null>();

  // Helper
  const StringIsNumber = (value: string) => isNaN(Number(value)) === false;

  const ranks: Rank[] = [];
  Object.keys(ERank)
    .filter(StringIsNumber)
    .forEach((element) => {
      ranks.push({ rank: Number.parseInt(element) });
    });

  return (
    <Layout>
      <div className="account-wrapper">
        <div className="account-header">YOUR ACCOUNT</div>
        <div className="account-form">
          <div className="account-input-wrapper">
            Nickname:
            <div className="account-input">
              <Input
                id="nickname"
                name="nickname"
                value={accountEdited.nickname}
                onChange={handleChange}
                disabled={!isEdit}
                placeholder="Your nickname"
              />
            </div>
          </div>
          <div className="account-input-wrapper">
            Rank:
            <div className="account-input">
              <Select
                options={ranks}
                value={{ rank: accountEdited.rank }}
                onChange={(p) => (p == null ? null : handleRankChange(p.rank))}
                formatOptionLabel={(p) => ERank[p.rank]}
                getOptionValue={(p) => ERank[p.rank]}
                isDisabled={!isEdit}
                className="account-input-rank-selector"
              />
            </div>
          </div>
          <RoleField
            name="top"
            onChange={handlePriorityChange}
            priority={accountEdited.top}
            disabled={!isEdit}
          />
          <br />
          <RoleField
            name="jng"
            onChange={handlePriorityChange}
            priority={accountEdited.jng}
            disabled={!isEdit}
          />
          <br />
          <RoleField
            name="mid"
            onChange={handlePriorityChange}
            priority={accountEdited.mid}
            disabled={!isEdit}
          />
          <br />
          <RoleField
            name="bot"
            onChange={handlePriorityChange}
            priority={accountEdited.bot}
            disabled={!isEdit}
          />
          <br />
          <RoleField
            name="supp"
            onChange={handlePriorityChange}
            priority={accountEdited.supp}
            disabled={!isEdit}
          />
        </div>
        {errorMessage != null && (
          <div className="login-error-message">{errorMessage}</div>
        )}
        {!isEdit && <Button onClick={handleAccountEdit}>Edit</Button>}
        {isEdit && (
          <>
            <Button onClick={handleSubmit}>Save</Button>
            <br />
            <Button onClick={handleAccountCancelEdit}>Cancel</Button>
          </>
        )}
      </div>
    </Layout>
  );
}
