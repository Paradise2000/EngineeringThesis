import Cookies from "../node_modules/js-cookie/dist/js.cookie.min.mjs";

var isUserLoggedEndpoint = "https://localhost:7002/api/account/isUserLogged";

export const getJWTtoken = () => {
    return Cookies.get('TokenJWT');
}

export const logout = (redirectPath) => {
    Cookies.remove('TokenJWT');
    window.location.href = redirectPath;
}

export function isUserLogged() {
    const token = Cookies.get('TokenJWT');

    if (token != undefined) {
        return true;
    } else {
        return false;
    }
}

