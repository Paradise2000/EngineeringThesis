import { isUserLogged, getJWTtoken } from "../../services/authService.js";

var token;
var FileDeleteEndpoint = "https://localhost:7002/api/File/delete";
const PostAttractionEndpoint = 'https://localhost:7002/api/attraction/create';
var PhotosPaths = [];
var MainPhotoName;

document.addEventListener("DOMContentLoaded", function() {

    if(isUserLogged() == true) {
        $("#menu").load("menu_logged.html");
    } else {
        window.location.href = "login.html";
    }

    token = getJWTtoken();

    fetch('https://localhost:7002/api/attraction/getCategories', {
        headers: {
            'Authorization': 'Bearer ' + token
        }
    })
    .then(response => response.json())
    .then(dataFromAPI => {
        var select = $('#categories');
        $.each(dataFromAPI, function (index, item) {
            select.append('<option value="' + item.name + '" id="' + item.id + '">' + item.name + '</option>');
        });
    })
});

var map = L.map('map').setView([52, 19], 5);

L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);

var marker;
var lat;
var lng;

function onMapClick(e) {
    lat = e.latlng.lat;
    lng = e.latlng.lng;

    if (marker) {
        map.removeLayer(marker);
    }

    marker = L.marker(e.latlng).addTo(map);
}

map.on('click', onMapClick);

const CityField = document.getElementById('city');
const CityError = document.getElementById('cityError');
const NameField = document.getElementById('name');
const NameError = document.getElementById('nameError');
const DescriptionField = document.getElementById('description');
const DescriptionError = document.getElementById('descriptionError');

const MapError = document.getElementById('mapError');

document.getElementById('form').addEventListener('submit', function(e) {
    e.preventDefault();

    let isValid = true;

    if(CityField.value.length < 3) {
        CityError.innerHTML = "Miasto musi mieć długość conajmniej 3 znaków";
        isValid = false;
    } else {
        CityError.innerHTML = "";
    }

    if(NameField.value.length < 3) {
        NameError.innerHTML = "Nazwa atrakcji musi mieć długość conajmniej 3 znaków";
        isValid = false;
    } else {
        NameError.innerHTML = "";
    }

    if(DescriptionField.value.length < 10) {
        DescriptionError.textContent = "Atrakcja musi mieć opis mający przynajmniej 10 znaków";
        isValid = false;
    } else {
        DescriptionError.textContent = "";
    }

    if(lat == null || lng == null) {
        MapError.textContent = "Wskaż lokalizacje atrakcji na mapie";
        isValid = false;
    } else {
        MapError.textContent = "";
    }

    if(MainPhotoName == null) {
        MainPhotoError.textContent = "Wybierz zdjęcie główne";
        isValid = false;
    } else {
        MapError.textContent = "";
    }

    if(isValid) {
        const jsonData = JSON.stringify({
            city: document.getElementById('city').value,
            duration: document.getElementById('duration').value + ":00",
            price: parseFloat(document.getElementById('price').value),
            categoryId: parseFloat($('#categories').find(':selected').attr('id')),
            name: document.getElementById('name').value,
            description: document.getElementById('description').value,
            coordinateX: parseFloat(lat),
            coordinateY: parseFloat(lng),
            coordinateZ: parseFloat(16),
            imagesPaths: PhotosPaths,
            mainImagePath: MainPhotoName
        });

        fetch(PostAttractionEndpoint, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: jsonData
        })
        .then(data => console.log(data))
        .catch(error => console.log(error))

        console.log(JSON.parse(jsonData));
    }
    
  });

  
Dropzone.autoDiscover = false;
const myDropzone = new Dropzone("#my-awesome-dropzone", {
    paramName: "file",
    maxFilesize: 5,
    addRemoveLinks: true,
    acceptedFiles: 'image/jpeg,image/jpg,image/png',
    dictDefaultMessage: "Przeciągnij zdjęcia lub kliknij tutaj",

    sending: function(file, xhr, formData) {
        xhr.setRequestHeader("Authorization", "Bearer " + token);
    },

    success: function(file, response) {
        
        console.log("WYSŁANO");
        file.newFileName = response;
        PhotosPaths.push(file.newFileName);

        file.previewElement.addEventListener("click", function () {

            var previews = document.querySelectorAll(".dz-preview");
            $(previews).find(".caption").remove();
            $(file.previewElement).append(`<p class="caption">Zdjęcie główne</p>`);

            MainPhotoName = file.newFileName;

            console.log("Wybrane zdjęcie główne: ", file.name);
        });
    },

    removedfile: function(file) {
        PhotosPaths = PhotosPaths.filter(function(element) {
            return element !== file.newFileName;
        });

        if(MainPhotoName == file.newFileName) {
            MainPhotoName = null;
        }

        fetch(`${FileDeleteEndpoint}/${file.newFileName}`, {
          method: 'DELETE',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
          }
        })
        .then(response => {
          file.previewElement.remove();
          console.log(response);
        })
        .catch(error => {
          console.error('Wystąpił błąd:', error);
        });
    }

});