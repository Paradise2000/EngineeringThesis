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

    if(isValid) {
        const url = 'https://localhost:7002/api/attraction/create';
        const token = '';

        const jsonData = JSON.stringify({
            city: document.getElementById('city').value,
            duration: document.getElementById('duration').value + ":00",
            price: parseFloat(document.getElementById('price').value),
            name: document.getElementById('name').value,
            description: document.getElementById('description').value,
            coordinateX: parseFloat(lat),
            coordinateY: parseFloat(lng),
            coordinateZ: parseFloat(16)
        });

        fetch(url, {
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