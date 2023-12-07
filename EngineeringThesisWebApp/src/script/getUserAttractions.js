import { isUserLogged, getJWTtoken } from "../../services/authService.js";
import { getHour } from "../../services/functionService.js";

var token;
token = getJWTtoken();

await fetch(`https://localhost:7002/api/attraction/getUserAttractions`, {
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
                        <img class="attraction-img" src="https://localhost:7002/api/file/download/${item.mainImagePath}"/>
                    </div>
                    <div class="col-two-third">
                        <div class="info">
                            <div class="info element-info"><img src="../images/marker.png" class="icon"><p>${item.city}</p></div>
                            <div class="info element-info"><img src="../images/stopwatch.png" class="icon"><p>${getHour(item.duration)}h</p></div>
                            <div class="info element-info"><img src="../images/piggy-bank.png" class="icon"><p>${item.price}zł</p></div>
                            <div><input type="submit" class="category" value="${item.categoryName}"></div>
                        </div>
                        <h3>${item.name}</h3><br>
                        <p>${item.description}</p>
                    </div>
                </div>
            </div>
            `;

    $('#data-container').append(newAttraction);
    });
})

if(isUserLogged() == true) {
    $("#menu").load("menu_logged.html");
  } else {
    $("#menu").load("menu_unlogged.html");
}