import { useCookies } from "react-cookie";
import { LoginCookie } from "../../models/LoginCookie";

export default function Cookie(): LoginCookie {
  const [cookies, ,] = useCookies();
  const cookie = cookies.inhouse as LoginCookie;
  return cookie;
}
