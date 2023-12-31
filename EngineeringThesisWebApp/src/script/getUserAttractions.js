import { isUserLogged, getJWTtoken } from "../services/authService.js";
import { API_BASE_URL, getHour } from "../services/functionService.js";

var token = getJWTtoken();

if(isUserLogged() == true) {
    $("#menu").load("menu_logged.html");
  } else {
    window.location.href = `login.html?forward=${location.href.split("/").slice(-1)}`;
}
$("#footer").load("footer.html");

await fetch(`${API_BASE_URL}/api/attraction/getUserAttractions`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + token
    }
  })
.then(response => response.json())
.then(dataFromAPI => {
    dataFromAPI.forEach(function(item) {
        let newAttraction = `
            <div class="container attraction-container">
                <div onclick="if (!event.target.matches('input')) window.location.href='getAttractionDetails.html?id=${item.id}'" class="attraction" style="cursor: pointer;">
                    <div class="col-one-third">
                        <img class="attraction-img" src="${API_BASE_URL}/api/file/download/${item.mainImagePath}"/>
                    </div>
                    <div class="col-two-third">
                        <div class="info">
                            <div class="info element-info"><img src="./images/marker.png" class="icon"><p>${item.city}</p></div>
                            <div class="info element-info"><img src="./images/stopwatch.png" class="icon"><p>${getHour(item.duration)}h</p></div>
                            <div class="info element-info"><img src="./images/piggy-bank.png" class="icon"><p>${item.price}zł</p></div>
                            <div><input type="submit" class="category" value="${item.categoryName}"></div>
                        </div>
                        <h3 class="title21">${item.name}</h3><br>
                        <div class="text-crop"><p class="main-text">${item.description}</p></div>
                    </div>
                </div>
                <hr class="visible">
            </div>
            `;

    $('#data-container').append(newAttraction);
    });
})