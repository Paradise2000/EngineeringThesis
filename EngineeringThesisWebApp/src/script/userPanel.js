import { isUserLogged, getJWTtoken } from "../../services/authService.js";

if(isUserLogged() == true) {
    $("#menu").load("menu_logged.html");
} else {
    window.location.href = "login.html";
}
$("#footer").load("footer.html");

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