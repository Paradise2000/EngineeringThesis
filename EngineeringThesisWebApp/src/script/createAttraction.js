import { isUserLogged, getJWTtoken } from "../../services/authService.js";

var token;
var FileDeleteEndpoint = "https://localhost:7002/api/File/delete";
const PostAttractionEndpoint = 'https://localhost:7002/api/attraction/create';
var PhotosPaths = [];
var MainPhotoName;

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

$("#form").on('submit', function(e) {
    e.preventDefault();

    let isValid = true;

    if($("#city").val().length < 3) {
        $("#cityError").html("Miasto musi mieć długość conajmniej 3 znaków");
        isValid = false;
    } else {
        $("#cityError").html("");
    }

    if($("#name").val().length < 3) {
        $("#nameError").html("Nazwa atrakcji musi mieć długość conajmniej 3 znaków");
        isValid = false;
    } else {
        $("#nameError").html("");
    }

    if($("#description").val().length < 10) {
        $("#descriptionError").text("Atrakcja musi mieć opis mający przynajmniej 10 znaków");
        isValid = false;
    } else {
        $("#descriptionError").text("");
    }

    if(lat == null || lng == null) {
        $("#mapError").text("Wybierz zdjęcie główne");
        isValid = false;
    } else {
        $("#mapError").text("");
    }

    if(MainPhotoName == null) {
        $("#MainPhotoError").text("Wybierz zdjęcie główne");
        isValid = false;
    } else {
        $("#MainPhotoError").text("");
    }

    if(isValid) {
        fetch(PostAttractionEndpoint, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({
            city: $("#city").val(),
            duration: $("#duration").val() + ":00",
            price: parseFloat($("#price").val()),
            categoryId: parseFloat($('#categories').find(':selected').attr('id')),
            name: $("#name").val(),
            description: $("#description").val(),
            coordinateX: parseFloat(lat),
            coordinateY: parseFloat(lng),
            coordinateZ: parseFloat(16),
            imagesPaths: PhotosPaths,
            mainImagePath: MainPhotoName
        })
        })
        .then(data => console.log(data))
        .catch(error => console.log(error));
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