import { isUserLogged, getJWTtoken } from "../../services/authService.js";
import {API_BASE_URL, getHour } from "../../services/functionService.js";

var token;
token = getJWTtoken();

var map = L.map('map').setView([52, 19], 5);

L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);

await fetch(`${API_BASE_URL}/api/attraction/getAttractionPlan`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + token
    }
  })
.then(response => response.json())
.then(dataFromAPI => {
    dataFromAPI.attractions.forEach(function(item) {
        let newAttraction = `
            <div class="container attraction-container">
                <div onclick="if (!event.target.matches('input')) window.location.href='getAttractionDetails.html?id=${item.id}'" class="attraction" style="cursor: pointer;">
                    <div class="col-one-third">
                        <img class="attraction-img" src="${API_BASE_URL}/api/file/download/${item.mainImagePath}"/>
                    </div>
                    <div class="col-two-third">
                        <div class="info">
                            <div class="info element-info"><img src="../images/marker.png" class="icon"><p>${item.city}</p></div>
                            <div class="info element-info"><img src="../images/stopwatch.png" class="icon"><p>${getHour(item.duration)}h</p></div>
                            <div class="info element-info"><img src="../images/piggy-bank.png" class="icon"><p>${item.price}zł</p></div>
                            <div><input type="submit" class="category" value="${item.categoryName}"></div>
                            <div><input type="button" class="button red margin" data-group="delete" data-id="${item.id}" value="Usuń z planu" ></div>
                        </div>
                        <h3 class="title24">${item.name}</h3><br>
                        <div class="text-crop"><p class="main-text">${item.description}</p></div>
                    </div>
                </div>
                <hr class="visible">
            </div>
            `;

    $('#data-container').append(newAttraction);
    });

    dataFromAPI.locationSummary.forEach(function(attraction) {
        var marker = L.marker([attraction.coordinateX, attraction.coordinateY]).addTo(map);
    
        marker.bindPopup(`<b>${attraction.name}</b>`);
    }); 

    $('#totalTime').html(getHour(dataFromAPI.totalTime) + "h");
    $('#totalPrice').html(dataFromAPI.totalPrice + "zł");
})

if(isUserLogged() == true) {
    $("#menu").load("menu_logged.html");
  } else {
    $("#menu").load("menu_unlogged.html");
}
$("#footer").load("footer.html");

$("input").on('click', async function() {
    var dataGroup = $(this).data('group');
    var dataId = $(this).data('id');

    if(dataGroup == 'delete') {
        console.log(dataId);
        await fetch(`${API_BASE_URL}/api/attraction/deleteAttractionFromPlan/${dataId}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + token
        }
        });
        location.reload();
    }
});