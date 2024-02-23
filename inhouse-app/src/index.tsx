import * as React from "react";
// import App from './App.tsx'
import Login from "./pages/Login.tsx";
import Lobby from "./pages/Lobby.tsx";
import { createRoot } from "react-dom/client";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Match from "./pages/Match.tsx";
import PrivateRoute from "./PrivateRoute.tsx";
import Account from "./pages/Account.tsx";

const container = document.getElementById("root");

const root = createRoot(container!);
root.render(
  // <div id="root">
  <BrowserRouter>
    <Routes>
      <Route path="" element={<PrivateRoute outlet={<Lobby />} />} />
      <Route path="/" element={<PrivateRoute outlet={<Lobby />} />} />
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Login isRegister={true} />} />
      <Route path="/match" element={<PrivateRoute outlet={<Match />} />} />
      <Route path="/account" element={<PrivateRoute outlet={<Account />} />} />
    </Routes>
  </BrowserRouter>
  // </div>
);
