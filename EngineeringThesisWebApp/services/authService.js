import Cookies from "../node_modules/js-cookie/dist/js.cookie.min.mjs";

var isUserLoggedEndpoint = "https://localhost:7002/api/account/isUserLogged";

export const isUserLogged = (redirectPath) => {

    const token = Cookies.get('TokenJWT');

    fetch(isUserLoggedEndpoint, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    })
    .then((response) => {
        if (response.status == 200) {
        } else {
            window.location.href = redirectPath;
        }
    })
};
