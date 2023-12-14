import Cookies from "./JSPackages/js.cookie.min.mjs";
import { isUserLogged } from "./services/authService.js";
import { API_BASE_URL } from "./services/functionService.js";

if(isUserLogged() == true) {
    window.location.href = "panelUzytkownika.html";
} else {
    $("#menu").load("menu_unlogged.html");
}
$("#footer").load("footer.html");

var LoginEndpoint = `${API_BASE_URL}/api/account/login`;

const EmailField = document.getElementById('email');
const PasswordField = document.getElementById('password');

const EmailError = document.getElementById('emailError');
const PasswordError = document.getElementById('passwordError');

const ServerError = document.getElementById('serverError');

document.getElementById('form').addEventListener('submit', function(e) {
    e.preventDefault();

    let isValid = true;

    if (!/\S+@\S+\.\S+/.test(EmailField.value)) {
        EmailError.innerHTML = "Niepoprawny email, spróbuj ponownie";
        isValid = false;
    } else {
        EmailError.innerHTML = "";
    }

    if (PasswordField.value.length < 3) {
        PasswordError.innerHTML = "Hasło powinno mieć conajmniej 3 znaki";
        isValid = false;
    } else {
        PasswordError.innerHTML = "";
    }

    if(isValid) {
        const jsonData = JSON.stringify({
            email: EmailField.value,
            password: PasswordField.value
        });

        fetch(LoginEndpoint, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: jsonData
        })
        .then((response) => {
            if (response.status != 200) {
                ServerError.innerHTML = "Użytkownik nie istnieje lub email/hasło są nieprawidłowe";
                throw new Error('Błąd żądania ');
            }
            ServerError.innerHTML = "";
            return response.text();
        })
        .then((data) => {
            Cookies.set('TokenJWT', data, { expires: 7, secure: true, sameSite: 'strict' });
            window.location.href = "panelUzytkownika.html";
        })
    }

});