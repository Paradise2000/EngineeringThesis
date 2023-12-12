import { isUserLogged } from "../../services/authService.js";
import { API_BASE_URL ,getHour } from "../../services/functionService.js";

var InitialdataSource = `${API_BASE_URL}/api/attraction/getAttractions`;
var paginationOptions = {
    dataSource: InitialdataSource,
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
            let newAttraction = `
                <div class="container attraction-container">
                    <div onclick="window.location.href='getAttractionDetails.html?id=${item.id}'" class="attraction" style="cursor: pointer;">
                        <div class="col-one-third">
                            <img class="attraction-img" src="${API_BASE_URL}/api/file/download/${item.mainImagePath}"/>
                        </div>
                        <div class="col-two-third">
                            <div class="info">
                                <div class="info element-info"><img src="../images/marker.png" class="icon"><p>${item.city}</p></div>
                                <div class="info element-info"><img src="../images/stopwatch.png" class="icon"><p>${getHour(item.duration)}h</p></div>
                                <div class="info element-info"><img src="../images/piggy-bank.png" class="icon"><p>${item.price}zł</p></div>
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
    }
};

if(isUserLogged() == true) {
    $("#menu").load("menu_logged.html");
  } else {
    $("#menu").load("menu_unlogged.html");
}
$("#footer").load("footer.html");

$('#pagination-container').pagination(paginationOptions);

$('#filter').on('click', function() {
    
    const params = new URLSearchParams();

    if($('#city').val() != '') {
        params.append('City', $('#city').val());
    }

    if($('#categories').val() != 'no_option') {
        params.append('CategoryId', $('#categories').find(':selected').attr('id'));
    }

    if($('#name').val() != '') {
        params.append('Name', $('#name').val());
    }

    paginationOptions.dataSource = InitialdataSource + '?' + params;

    $('#pagination-container').pagination('destroy');
    $('#data-container').empty();

    $('#pagination-container').pagination(paginationOptions);
});

fetch(`${API_BASE_URL}/api/attraction/getCategories`)
    .then(response => response.json())
    .then(dataFromAPI => {
        var select = $('#categories');
        $.each(dataFromAPI, function (index, item) {
            select.append('<option value="' + item.name + '" id="' + item.id + '">' + item.name + '</option>');
        });
    })