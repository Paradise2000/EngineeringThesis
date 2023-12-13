import { isUserLogged, getJWTtoken } from "../../services/authService.js";
import { API_BASE_URL } from "../../services/functionService.js";

var token = getJWTtoken();

if(isUserLogged() == true) {
    $("#menu").load("menu_logged.html");
} else {
    window.location.href = "login.html";
}
$("#footer").load("footer.html");

async function userPanel()
{
    await renderLinks();
    await renderData();
}

async function getUserData()
{
    var response = await fetch(`${API_BASE_URL}/api/account/getUserData`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        }
    })

    return response.json();
}

async function changePassword(jsonData)
{
    var response = await fetch(`${API_BASE_URL}/api/account/setNewPassword`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        },
        body: jsonData
    })

    return response;
}

async function renderData()
{
    var userData = await getUserData();

    $("#titleNick").text(`Cześć ${userData.nickName}! Gotowy na kolejną przygodę?`);
    $("#nickName").text(`${userData.nickName}`);
    $("#email").text(`${userData.email}`);

    $("#changePassword").on('click', async function() {
        $("#message1").text("");
        $("#message2").text("");

        var isValid = true;

        if($("#newPassword").val().length < 3) {
            $("#message2").text("Nowe hasło musi mieć przynajmniej 3 znaki").css("color", "red");
            isValid = false;
        } else {
            $("#message2").text("");
        }

        if(isValid)
        {
            const jsonData = JSON.stringify({
                currentPassword: $("#currentPassword").val(),
                newPassword: $("#newPassword").val(),
                retypedNewPassword: $("#retypedNewPassword").val()
            });
    
            var response = await changePassword(jsonData);
    
            if (!response.ok) {
                return response.json().then(errorData => {
                    if (errorData.errors) {
                        errorData.errors.forEach(element => {
                            if (element.errorCode == "WrongPassword") {
                                $("#message1").text("Stare hasło nie jest prawidłowe").css("color", "red");
                            }
    
                            if (element.errorCode == "PasswordNotSame") {
                                $("#message2").text("Nowe hasła nie są takie same").css("color", "red");
                            }
                        });
                    }
                });
            } else {
                $("#message2").text("Hasło zostało zmienione").css("color", "green");
            }
        }
    });
}

async function renderLinks()
{
    const addAttractionDiv = document.getElementById('addAttraction');
    const myAdventureDiv = document.getElementById('myAdventure');
    const myAttractionsDiv = document.getElementById('myAttractions');
    
    // Obsługa kliknięcia dla każdego diva
    addAttractionDiv.addEventListener('click', function() {
    window.location.href = './createAttraction.html';
    });
    
    myAdventureDiv.addEventListener('click', function() {
    window.location.href = './myAdventure.html';
    });
    
    myAttractionsDiv.addEventListener('click', function() {
    window.location.href = './myAttractions.html';
    });
}

userPanel();