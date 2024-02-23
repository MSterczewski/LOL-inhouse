import React, { useState } from "react";
import Layout from "../Layout.tsx";
import Global from "../config.tsx";
import axios from "axios";
import "../styles/loginStyles.css";
import Button from "../components/shared/Button.tsx";
import { useCookies } from "react-cookie";
import { useNavigate } from "react-router-dom";
import {
  ELoginErrorCode,
  LoginData,
  LoginCookie,
  ERole,
} from "../models/LoginCookie.ts";
import Input from "../components/shared/Input.tsx";

export default function Login({
  isRegister = false,
}: {
  isRegister?: boolean;
}) {
  const [input, setInput] = useState<LoginData>({
    nickname: "",
    password: "",
  });
  const inputChangeHandler = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setInput({ ...input, [name]: value });
    setErrorMessage(null);
  };
  const [, setCookie] = useCookies();
  const navigate = useNavigate();

  const [errorMessage, setErrorMessage] = useState<string | null>();

  const submit = async () => {
    const { data } = await axios<LoginCookie>({
      url:
        Global.serverUrl + (isRegister ? "Account/Register" : "Account/Login"),
      data: input,
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
    }).catch((error) => {
      if (error.response.status == 400) {
        if (isRegister) {
        } else {
          switch (error.response.data) {
            case ELoginErrorCode.DataInvalid:
              setErrorMessage("Data invalid");
            case ELoginErrorCode.PlayerNotFound:
              setErrorMessage("Player not found");
            case ELoginErrorCode.PasswordInvalid:
              setErrorMessage("Password invalid");
          }
        }
      } else throw new Error("Unhandled response code");
      return {
        data: null,
      };
    });
    if (data == null) return;
    data.isAdmin = data.role == ERole.Admin;
    setCookie(Global.cookieName, data);
    if (isRegister) navigate("/account");
    else navigate("/");
    return data;
  };
  const text = isRegister ? "REGISTER" : "LOG IN";
  const submitText = isRegister ? "Register" : "Log in";

  return (
    <Layout>
      <div className="login-container">
        <div className="login-form">
          <h1>{text}</h1>
          <label>
            Nickname:
            <Input
              id="nickname"
              name="nickname"
              value={input.nickname}
              onChange={inputChangeHandler}
              placeholder="Your nickname"
              //TODO: VALIDATE IF REGISTER
            />
          </label>
          <br />
          <label>
            Password:
            <Input
              id="password"
              name="password"
              value={input.password}
              onChange={inputChangeHandler}
              placeholder="Your password"
              //TODO: VALIDATE IF REGISTER
            />
          </label>
          <br />
          <br />
          {errorMessage != null && (
            <div className="login-error-message">{errorMessage}</div>
          )}
          <Button onClick={submit}>{submitText}</Button> <br />
          {!isRegister && (
            <Button
              onClick={() => {
                navigate("/register");
              }}
            >
              Create an account
            </Button>
          )}
          {isRegister && (
            <Button
              onClick={() => {
                navigate("/login");
              }}
            >
              Login instead
            </Button>
          )}
        </div>
      </div>
    </Layout>
  );
}
