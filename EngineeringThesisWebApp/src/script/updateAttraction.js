import { isUserLogged, getJWTtoken } from "../../services/authService.js";
import {getHour} from "../../services/functionService.js"

var APIdata;
var ImagesToDelete = [];
var ImagesToPost = [];

var token;
var FileDeleteEndpoint = "https://localhost:7002/api/File/delete";
const PostAttractionEndpoint = 'https://localhost:7002/api/attraction/create';
var MainPhotoName;

var lat;
var lng;

const urlParams = new URLSearchParams(window.location.search);

if(isUserLogged() == true) {
    $("#menu").load("menu_logged.html");
} else {
    window.location.href = "login.html";
}
token = getJWTtoken();

await fetch(`https://localhost:7002/api/attraction/getAttraction?id=${urlParams.get('id')}`)
.then(response => response.json())
.then(dataFromAPI => {
    APIdata = dataFromAPI;
    $('#city').val(dataFromAPI.city);
    $('#duration').val(getHour(dataFromAPI.duration));
    $('#price').val(dataFromAPI.price);
    $('#category').val(dataFromAPI.categoryName);
    $('#name').val(dataFromAPI.name);
    $('#description').val(dataFromAPI.description);
})

var map = L.map('map').setView([APIdata.coordinateX, APIdata.coordinateY], APIdata.coordinateZ);
        
L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);
var marker = L.marker([APIdata.coordinateX, APIdata.coordinateY]).addTo(map);

function onMapClick(e) {
    lat = e.latlng.lat;
    lng = e.latlng.lng;

    if (marker) {
        map.removeLayer(marker);
    }

    marker = L.marker(e.latlng).addTo(map);
}
    
map.on('click', onMapClick);

await fetch('https://localhost:7002/api/attraction/getCategories')
.then(response => response.json())
.then(dataFromAPI => {
    var select = $('#categories');
    $.each(dataFromAPI, function (index, item) {
        select.append('<option value="' + item.name + '" id="' + item.id + '">' + item.name + '</option>');
    });
})

Dropzone.autoDiscover = false;
const myDropzone = new Dropzone("#my-awesome-dropzone", {
    paramName: "file",
    maxFilesize: 5,
    addRemoveLinks: true,
    acceptedFiles: 'image/jpeg,image/jpg,image/png',
    dictDefaultMessage: "Przeciągnij zdjęcia lub kliknij tutaj",
    init: function() {
        APIdata.imagePaths.forEach(path => {
            var mockFile = {name: path, size: 12345, accepted: true};

            this.emit("addedfile", mockFile);
            this.emit("thumbnail", mockFile, `https://localhost:7002/api/file/download/${path}`);
            this.emit("complete", mockFile);
            this.files.push(mockFile);

            if(path == APIdata.mainImagePath) {
                var previews = document.querySelectorAll(".dz-preview");
                $(previews).find(".caption").remove();
                $(mockFile.previewElement).append(`<p class="caption">Zdjęcie główne</p>`);

                MainPhotoName = path;

                console.log("Wybrane zdjęcie główne: ", path);
            }

            mockFile.previewElement.addEventListener("click", function () {
                var previews = document.querySelectorAll(".dz-preview");
                $(previews).find(".caption").remove();
                $(mockFile.previewElement).append(`<p class="caption">Zdjęcie główne</p>`);

                MainPhotoName = path;

                console.log("Wybrane zdjęcie główne: ", path);
            });
        });
    },

    sending: function(file, xhr, formData) {
        xhr.setRequestHeader("Authorization", "Bearer " + token);
    },

    success: function(file, response) {
        
        console.log("WYSŁANO");
        file.newFileName = response;
        ImagesToPost.push(file.newFileName);

        file.previewElement.addEventListener("click", function () {

            var previews = document.querySelectorAll(".dz-preview");
            $(previews).find(".caption").remove();
            $(file.previewElement).append(`<p class="caption">Zdjęcie główne</p>`);

            MainPhotoName = file.newFileName;

            console.log("Wybrane zdjęcie główne: ", file.newFileName);
        });
    },

    removedfile: function(file) {
        if(APIdata.imagePaths.includes(file.name)) {
            ImagesToDelete.push(file.name);

            if(MainPhotoName == file.name) {
                MainPhotoName = null;
            }

            console.log(ImagesToDelete);
            console.log(MainPhotoName);

            file.previewElement.remove();
        } else {
            ImagesToPost = ImagesToPost.filter(function(element) {
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
    }
});

$("#form").on('submit', function(e) {
    e.preventDefault();

    console.log("To delete:" + ImagesToDelete);

    APIdata.imagePaths.forEach(path => {
        if(!ImagesToDelete.includes(path)) {
            ImagesToPost.push(path);
        }
    });

    console.log("To post:" + ImagesToPost);
    console.log("main photo " + MainPhotoName);

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

    if(MainPhotoName == null) {
        $("#MainPhotoError").text("Wybierz zdjęcie główne");
        isValid = false;
    } else {
        $("#MainPhotoError").text("");
    }

    if(lat == undefined || lng == undefined) {
        lat = APIdata.coordinateX;
        lng = APIdata.coordinateY;
    }

    if(isValid) {
        fetch("https://localhost:7002/api/attraction/updateAttraction", {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({
            attractionId: urlParams.get('id'),
            city: $("#city").val(),
            duration: $("#duration").val() + ":00",
            price: parseFloat($("#price").val()),
            categoryId: parseFloat($('#categories').find(':selected').attr('id')),
            name: $("#name").val(),
            description: $("#description").val(),
            coordinateX: parseFloat(lat),
            coordinateY: parseFloat(lng),
            coordinateZ: parseFloat(16),
            imagesPaths: ImagesToPost,
            mainImagePath: MainPhotoName
        })
        })
        .then(data => console.log(data))
        .catch(error => console.log(error))
    }
    
});