$('#pagination-container').pagination({
    dataSource: 'https://localhost:7002/api/attraction/get',
    locator: 'items',
    totalNumberLocator: function (response) {
        return response.totalPages;
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
                    <div class="attraction">
                        <div class="col-one-third">
                            <img src="https://localhost:7002/api/file/download/${item.mainImagePath}"/>
                        </div>
                        <div class="col-two-third">
                            <div class="info">
                                <div class="info element-info"><img src="../images/marker.png" class="icon"><p>${item.city}</p></div>
                                <div class="info element-info"><img src="../images/stopwatch.png" class="icon"><p>${item.duration}</p></div>
                                <div class="info element-info"><img src="../images/piggy-bank.png" class="icon"><p>${item.price}</p></div>
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
    }
})