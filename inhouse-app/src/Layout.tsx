import React from "react";
import "./styles/layoutStyles.css";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { LoginCookie } from "./models/LoginCookie.ts";
import { useCookies } from "react-cookie";
import Global from "./config.tsx";

export default function Layout(props: { children: React.ReactNode }) {
  const [cookies, , removeCookie] = useCookies();
  const cookie = cookies.inhouse as LoginCookie;

  axios.defaults.headers.common["Authorization"] =
    cookie?.id?.toString() + "_" + cookie?.token;
  axios.defaults.headers.common["Content-Type"] = "application/json";

  const navigate = useNavigate();

  // axios.interceptors.response.use(undefined, function (error) {
  //   if (error.response.status === 401) navigate("/login");
  //   //return Promise.reject(error);
  // });
  async function handleLogout() {
    await axios({
      url: Global.serverUrl + "Account/Logout",
      method: "POST",
      data: {
        id: cookie.id,
      },
    }).catch((error) => {
      //if 401 from server, then there is no session stored in server, BUT cookie exists on FE
      if (error.response.status == 401) {
      }
    });
    removeCookie(Global.cookieName);
    window.location.reload();
  }
  return (
    <div className="main-layout">
      <div className="main-layout-nav-bar">
        <button
          className="main-layout-nav-bar-main-button"
          onClick={() => navigate("/")}
        >
          MAIN
        </button>
        {cookie == null ? (
          <></>
        ) : (
          <>
            <button
              onClick={() => navigate("/account")}
              className="main-layout-nav-bar-account-button"
            >
              <img
                src={cookie.imageUrl}
                className="main-layout-nav-bar-account-button-image"
              ></img>
            </button>
            <button
              onClick={handleLogout}
              className="main-layout-nav-bar-logout-button"
            >
              <img
                src="https://uxwing.com/wp-content/themes/uxwing/download/web-app-development/logout-line-icon.png"
                className="main-layout-nav-bar-logout-button-image"
              ></img>
            </button>
          </>
        )}
        {/* <div className="sidebar-heading">Sidebar Heading</div>
                <div className="list-group list-group-flush">
                    <a href="#" className="list-group-item list-group-item-action bg-light">Dashboard</a>
                    <a href="#" className="list-group-item list-group-item-action bg-light">Shortcuts</a>
                    <a href="#" className="list-group-item list-group-item-action bg-light">Overview</a>
                    <a href="#" className="list-group-item list-group-item-action bg-light">Events</a>
                    <a href="#" className="list-group-item list-group-item-action bg-light">Profile</a>
                    <a href="#" className="list-group-item list-group-item-action bg-light">Status</a>
                </div> */}
      </div>
      {props.children}
    </div>
  );
}
