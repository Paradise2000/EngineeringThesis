import { isUserLogged } from "./services/authService.js";
import { API_BASE_URL } from "./services/functionService.js";

if(isUserLogged() == true) {
    window.location.href = "panelUzytkownika.html";
} else {
    $("#menu").load("menu_unlogged.html");
}
$("#footer").load("footer.html");

$("#register").on('click', function() {
    
    var isValid = true;
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if($("#nick").val().length < 3) {
        $("#nickError").text("Nick nie ma przynajmniej 3 znaków");
        isValid = false;
    } else {
        $("#nickError").text("");
    }

    if(!emailRegex.test($("#email").val())) {
        $("#emailError").text("adres email jest nieprawidłowy");
        isValid = false;
    } else {
        $("#emailError").text("");
    }

    if ($("#password").val() != $("#repeatPassword").val()) {
        $("#passwordError").text("Hasła nie są takie same");
        isValid = false;
    } else if ($("#password").val().length < 5) {
        $("#passwordError").text("Hasło nie ma przynajmniej 5 znaków");
        isValid = false;
    } else {
        $("#passwordError").text("");
    }

    if(isValid) {
        const jsonData = JSON.stringify({
            nickName: $("#nick").val(),
            email: $("#email").val(),
            password: $("#password").val()
        });

        console.log("TEST");

        fetch(`${API_BASE_URL}/api/account/register`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: jsonData
        })
            .then(response => {
                if (!response.ok) {
                    return response.json().then(errorData => {
                        if (errorData.errors) {
                            errorData.errors.forEach(element => {
                                if (element.errorCode === "EmailAlreadyTaken") {
                                    $("#emailError").text("Ten adres email jest już zajęty");
                                }

                                if (element.errorCode === "NickAlreadyTaken") {
                                    $("#nickError").text("Ten nick jest już zajęty");
                                }
                            });
                        }
                        
                        throw new Error('Błąd żądania: ' + response.status);
                    });
                }

                return response.json();
            });
    }
});