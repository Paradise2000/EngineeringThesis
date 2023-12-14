import Cookies from "../JSPackages/js.cookie.min.mjs";

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

