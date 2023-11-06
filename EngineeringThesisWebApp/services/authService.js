import Cookies from "../node_modules/js-cookie/dist/js.cookie.min.mjs";

var isUserLoggedEndpoint = "https://localhost:7002/api/account/isUserLogged";

export const getJWTtoken = () => {
    return Cookies.get('TokenJWT');
}

export const logout = (redirectPath) => {
    Cookies.remove('TokenJWT');
    window.location.href = redirectPath;
}

export async function isUserLogged() {
    const token = Cookies.get('TokenJWT');

    const response = await fetch(isUserLoggedEndpoint, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    });

    if (response.status === 200) {
        return true;
    } else {
        return false;
    }
}

