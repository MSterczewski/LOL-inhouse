import React from "react";
import { Navigate } from "react-router-dom";
import Cookie from "./components/functional/Cookie.tsx";

export default function PrivateRoute({ outlet }: { outlet: JSX.Element }) {
  if (Cookie() != null) {
    return outlet;
  } else {
    return <Navigate to={{ pathname: "/login" }} />;
  }
}
