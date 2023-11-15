import { isUserLogged, getJWTtoken } from "../../services/authService.js";

var token = getJWTtoken();

if(isUserLogged() == true) {
    $("#menu").load("menu_logged.html");
  } else {
    $("#menu").load("menu_unlogged.html");
}

const urlParams = new URLSearchParams(window.location.search);

fetch(`https://localhost:7002/api/attraction/getAttraction?id=${urlParams.get('id')}`)
    .then(response => response.json())
    .then(dataFromAPI => {
        $('#city').html(dataFromAPI.city);
        $('#duration').html(dataFromAPI.duration + 'h');
        $('#price').html(dataFromAPI.price + 'zł');
        $('#category').val(dataFromAPI.categoryName);
        $('#title').html(dataFromAPI.name);
        
        for(i=0; i < dataFromAPI.avgReview; i++) {
            $('#avgReview').append('<img src="../images/star.png" class="star">');
        }

        dataFromAPI.imagePaths.forEach(function(path, index) {
            $('#slideshow').append(
                `<div class="mySlides fade">
                <div class="numbertext"> ${index+1} / ${dataFromAPI.imagePaths.length}</div>
                <img src="https://localhost:7002/api/file/download/${path}" style="width:100%">
                </div>`
            );
        });
        $.getScript("../script/slider.js");

        $('#Description').html(dataFromAPI.description);

        var map = L.map('map').setView([dataFromAPI.coordinateX, dataFromAPI.coordinateY], dataFromAPI.coordinateZ);
        
        L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        }).addTo(map);
        L.marker([dataFromAPI.coordinateX, dataFromAPI.coordinateY]).addTo(map);

        setTimeout(function () {
            window.dispatchEvent(new Event("resize"));
         }, 500);

        const sumRefiew = [dataFromAPI.numberOf5StarReviews, dataFromAPI.numberOf4StarReviews,
                           dataFromAPI.numberOf3StarReviews, dataFromAPI.numberOf2StarReviews,
                           dataFromAPI.numberOf1StarReviews];
        
        function opinionForm(number) {
                if (number == 1) {
                    return "opinia";
                } else if (number >= 2 && number <= 4) {
                    return "opinie";
                } else {
                    return "opinii";
                }
        }

        $('#reviewSum').append(`
                <div class="info">
                <h1 class="title21">Razem</h1>
                    <div class="info" name="star-container">
                        <img src="../images/star.png" class="star">
                        <img src="../images/star.png" class="star">
                        <img src="../images/star.png" class="star">
                        <img src="../images/star.png" class="star">
                        <img src="../images/star.png" class="star">
                    </div>
                    <h1 class="title21">${dataFromAPI.avgReview} ${opinionForm(dataFromAPI.avgReview)}</h1>
                </div>
        `)

        sumRefiew.forEach(function(element, index) {
            var stars = '';
            for(i = 0; i < 5-index; i++) {
                stars += '<img src="../images/star.png" class="star">';
            }

            var form = opinionForm(element);

            $('#reviewSum').append(`
                <div class="info">
                    <div class="info" name="star-container">
                        ${stars}
                    </div>
                    <h1 class="title21">${element} ${form}</h1>
                </div>
            `)
        });               
    });

if(isUserLogged() == true) {
    $("#opinion").html(
        `<h1 class="title21" style="text-align: center;">Byłeś już tutaj?<br>Pomóż innym
        wyrażając swoją opinię!</h1>
    <input type="button" class="button margin" id="commentButton" value="Dodaj opinię">`
    );
} else {
    $("#opinion").html(
    `<h1 class="title21" style="text-align: center;">Zaloguj się aby dodać opinię !</h1>`
    );
}    

const commentButton = document.getElementById('commentButton');
const commentSection = document.getElementById('commentSection');
var selectedStars;

for(var i=1; i<=5; i++) {
    $('#commentRating').append(`<img src="../images/star.png" style="filter: grayscale(1);" data-star="${i}" class="star">`);
}

$("#commentRating .star").on("click", function() {
    selectedStars = parseInt($(this).attr("data-star"));

    $("#commentRating .star").css("filter", "grayscale(1)");
    
    for (var i = 1; i <= selectedStars; i++) {
      $("#commentRating .star[data-star='" + i + "']").css("filter", "");
    }
});

commentButton.addEventListener('click', function () {
    // Po kliknięciu przycisku pokaż sekcję komentarzy
    commentSection.style.display = commentSection.style.display === 'none' ? 'block' : 'none';
});

$("#commentForm").on('submit', function(e) {
    e.preventDefault();
    console.log(selectedStars);

    let isValid = true;

    if(selectedStars >= 1 && selectedStars <= 5) {
        $("#ratingError").html("");
    } else {
        $("#ratingError").html("Wybierz ilość gwiazdek między 1, a 5")
        isValid = false;
    }

    if(isValid) {

        const jsonData = JSON.stringify({
            attractionId: urlParams.get('id'),
            title: "brak tytulu",
            rating: selectedStars,
            description: $("#comment").val(),
        });

        fetch('https://localhost:7002/api/attraction/addComment', {
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

$('#pagination-container').pagination({
    dataSource: `https://localhost:7002/api/attraction/getComment?attractionId=${urlParams.get('id')}`,
    locator: 'items',
    totalNumberLocator: function (response) {
        return response.totalPages * 5;
    },
    alias: {
        pageNumber: 'pageIndex',
        pageSize: 'pageSize'
    },
    pageSize: 5,
    callback: function(data, pagination) {
        $('#data-container').empty();

        data.forEach(function(item) {

            var stars = '';
            for(i = 0; i < item.rating; i++) {
                stars += '<img src="../images/star.png" class="star">';
            }

            const date = new Date(item.date);
            const formattedDate = `${date.getDay()}.${date.getDay()}.${date.getFullYear()}`;

            let newComment = `
            <div class="attraction column margin">
                <div class="info">
                    <img src="../images/image.PNG" class="profile" alt="Obrazek zastępczy" />
                    <h1 class="title21">${item.author}</h1>
                    <div class="align-right"><input type="submit" class="category" value="Napisano ${formattedDate}"></div>
                </div>
                <div class="info stars">
                    ${stars}
                </div>
                <p class="text14">${item.description}</p>
            </div>
            `

        $('#data-container').append(newComment);
        });
    }
})

