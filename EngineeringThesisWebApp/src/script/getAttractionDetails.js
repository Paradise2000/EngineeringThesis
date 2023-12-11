import { isUserLogged, getJWTtoken } from "../../services/authService.js";
import {getStars, generateStarRating, getDate, opinionForm, getHour} from "../../services/functionService.js"

var token = getJWTtoken();
const urlParams = new URLSearchParams(window.location.search);

if(isUserLogged() == true) {
    $("#menu").load("menu_logged.html");
  } else {
    $("#menu").load("menu_unlogged.html");
}

async function getAttraction() {
    const response = await fetch(`https://localhost:7002/api/attraction/getAttraction?id=${urlParams.get('id')}`, {
        headers: {
            'Authorization': 'Bearer ' + token
        }
    });
    const dataFromAPI = await response.json();

    await renderAttractionDetails(dataFromAPI);
    await renderCommentSection(dataFromAPI);
    await handlePagination(); 
}

async function addComment(jsonData) {
    await fetch('https://localhost:7002/api/attraction/addComment', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: jsonData
        });

    location.reload();
}

async function deleteComment() {
    await fetch(`https://localhost:7002/api/attraction/deleteComment/${urlParams.get('id')}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });

    location.reload();
}

async function updateComment(jsonData) {
    await fetch('https://localhost:7002/api/attraction/updateComment', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: jsonData
        });

    location.reload();
}

async function renderAttractionDetails(dataFromAPI) {
    //sprawdzenie czy użytkownik jest zalogowany
    if(dataFromAPI.isUserAttracion == true) {
        $('#editDelete').append(`
        <input type="button" class="button margin" value="Edytuj" id="edit">
        <input type="button" class="button red margin" value="Usuń atrakcję" id="delete">`);

        $('#edit').on('click', function() {
            window.location.href = `http://127.0.0.1:5500/src/html/updateAttraction.html?id=${urlParams.get('id')}`;
        });

        $('#delete').on('click', function() {
            //do zrobienia
        });
    }

    //przypisanie danych do zmiennych
    $('#city').html(dataFromAPI.city);
    $('#duration').html(getHour(dataFromAPI.duration) + 'h');
    $('#price').html(dataFromAPI.price + 'zł');
    $('#category').val(dataFromAPI.categoryName);
    $('#title').html(dataFromAPI.name);
    $('#avgReview').append(getStars(dataFromAPI.avgReview));
    $('#Description').html(dataFromAPI.description);

    //wyświetlenie zdjęć
    dataFromAPI.imagePaths.forEach(function(path, index) {
        $('#slideshow').append(
            `<div class="mySlides fade">
            <div class="numbertext"> ${index+1} / ${dataFromAPI.imagePaths.length}</div>
            <img src="https://localhost:7002/api/file/download/${path}" style="width:100%">
            </div>`
        );
    });
    $.getScript("../script/slider.js");

    //stworzenie mapy
    var map = L.map('map').setView([dataFromAPI.coordinateX, dataFromAPI.coordinateY], dataFromAPI.coordinateZ); 
    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);
    L.marker([dataFromAPI.coordinateX, dataFromAPI.coordinateY]).addTo(map);
    setTimeout(function () {
        window.dispatchEvent(new Event("resize"));
    }, 500);

    //Dodanie sekcji ocen
    const sumRefiew = [dataFromAPI.numberOf5StarReviews, dataFromAPI.numberOf4StarReviews,
    dataFromAPI.numberOf3StarReviews, dataFromAPI.numberOf2StarReviews, dataFromAPI.numberOf1StarReviews];
    $('#reviewSum').append(`
            <div class="info">
            <h1 class="title21">Średnio</h1>
                <div class="info" name="star-container">
                    ${getStars(dataFromAPI.avgReview, '<img src="../images/star.png" class="star">')}
                </div>
                <h1 class="title21">${dataFromAPI.numberOfReviews} ${opinionForm(dataFromAPI.numberOfReviews)}</h1>
            </div>
    `)
    sumRefiew.forEach(function(element, index) {
        var form = opinionForm(element);
        $('#reviewSum').append(`
            <div class="info">
                <div class="info" name="star-container">
                    ${getStars(5-index, '<img src="../images/star.png" class="star">')}
                </div>
                <h1 class="title21">${element} ${form}</h1>
            </div>
        `)
    });
}

async function renderCommentSection(dataFromAPI) {
    if(isUserLogged() == true) {
        if(dataFromAPI.userComment != null) {
            $('#opinion').toggle();

            var UserComment = `
                <div class="comment column margin">
                    <div class="info">
                        <h1 class="title21">Twoja opinia</h1>
                        <input type="button" class="button margin" id="commentEditButton" value="Edytuj/Usuń komentarz">
                        <div class="align-right"><input type="submit" class="category" value="Napisano ${getDate(dataFromAPI.userComment.date)}"></div>
                    </div>
                    <div id="commentRatingChange" class="info stars">
                        ${getStars(dataFromAPI.userComment.rating, '<img src="../images/star.png" class="star">')}
                    </div>
                    <p id="commentDescriptionChange" class="main-text">${dataFromAPI.userComment.description}</p>
                </div>`;

            var isCommentEditing = false;

            $('#useropinion-container').prepend(UserComment);
            
            function handleEditComment() {
                if (!isCommentEditing) {
                    $('#commentRatingChange').html('');
                    var selectedStars2 = generateStarRating("commentRatingChange", dataFromAPI.userComment.rating);
            
                    $('#commentDescriptionChange').replaceWith(`
                        <textarea id="commentDescriptionChange" class="comment-area">${dataFromAPI.userComment.description}</textarea>
                        <div class="info">
                        <input type="button" class="button margin" id="commentUpdateButton" value="Edytuj komentarz">
                        <input type="button" class="button margin" id="commentDeleteButton" value="Usuń komentarz">
                        </div>`);
                    
                    $("#commentUpdateButton").on('click', function () {
                        const jsonData = JSON.stringify({
                            attractionId: urlParams.get('id'),
                            title: "brak tytulu",
                            rating: selectedStars2.selectedStars,
                            description: $("#commentDescriptionChange").val(),
                        });

                        updateComment(jsonData);
                    });

                    $("#commentDeleteButton").on('click', function () {
                        deleteComment();
                    });
            
                    isCommentEditing = true;
                } else {
                    $('#useropinion-container').html(UserComment);
            
                    isCommentEditing = false;

                    $("#commentEditButton").on("click", handleEditComment);
                }
            }
            
            $("#commentEditButton").on("click", handleEditComment);

        } else {
            $("#opinion").html(
                `<h1 class="title21" style="text-align: center;">Byłeś już tutaj?<br>Pomóż innym
                wyrażając swoją opinię!</h1>
            <input type="button" class="button margin" id="commentButton" value="Dodaj opinię">`
            );

            var selectedStars2 = generateStarRating("commentRating", 0);

            $("#commentButton").on('click', function () {
                // Po kliknięciu przycisku pokaż sekcję komentarzy
                $("#commentSection").toggle();
            });

            $("#commentForm").on('submit', function(e) {
                e.preventDefault();

                let isValid = true;

                if(selectedStars2.selectedStars >= 1 && selectedStars2.selectedStars <= 5) {
                    $("#ratingError").html("");
                } else {
                    $("#ratingError").html("Wybierz ilość gwiazdek między 1, a 5")
                    isValid = false;
                }

                if(isValid) {

                    const jsonData = JSON.stringify({
                        attractionId: urlParams.get('id'),
                        title: "brak tytulu",
                        rating: selectedStars2.selectedStars,
                        description: $("#comment").val(),
                    });

                    addComment(jsonData);
                }
            });
        }
    } else {
        $("#opinion").html(
        `<h1 class="title21" style="text-align: center;">Zaloguj się aby dodać opinię !</h1>`
        );
    }
}

async function handlePagination() {
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
            $('#data-container').append(`
                <div class="comment column margin">
                    <div class="info">
                        <img src="../images/image.PNG" class="profile" alt="Obrazek zastępczy" />
                        <h1 class="title21">${item.author}</h1>
                        <div class="align-right"><input type="submit" class="category" value="Napisano ${getDate(item.date)}"></div>
                    </div>
                    <div class="info stars">
                        ${getStars(item.rating, '<img src="../images/star.png" class="star">')}
                    </div>
                    <p class="main-text">${item.description}</p>
                </div>
            `);
            });
        }
    })
}

getAttraction();