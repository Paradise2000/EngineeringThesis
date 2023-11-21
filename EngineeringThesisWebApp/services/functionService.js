export function getStars(number, code) {
    var stars = '';
    for(var i=0; i < number-1; i++) {
        stars += '<img src="../images/star.png" class="star">';
    }

    if(number % 1 != 0) {
        console.log("TEST");
        stars += `<img src="../images/star.png" style="clip-path: inset(0 ${(number % 1)*100}% 0 0" class="star">`;
    } else {
        stars += '<img src="../images/star.png" class="star">';
    }

    return stars;
}

export function generateStarRating(containerId, defaultStars = 0) {
    var starRating = {
        selectedStars: 0,

        generate: function () {
            for (var i = 1; i <= 5; i++) {
                $('#' + containerId).append(`<img src="../images/star.png" style="filter: grayscale(1);" data-star="${i}" class="star">`);
            }

            $("#" + containerId + " .star").on("click", this.handleClick.bind(this));

            if (defaultStars > 0 && defaultStars <= 5) {
                this.selectedStars = defaultStars;
                this.updateStars();
            }
        },

        handleClick: function (event) {
            this.selectedStars = parseInt($(event.target).attr("data-star"));
            this.updateStars();
            console.log("Liczba gwiazdek:", this.selectedStars);
        },

        updateStars: function () {
            $("#" + containerId + " .star").css("filter", "grayscale(1)");

            for (var i = 1; i <= this.selectedStars; i++) {
                $("#" + containerId + " .star[data-star='" + i + "']").css("filter", "");
            }
        }
    };

    starRating.generate();

    return starRating;
}

export function getDate(date) {
    const DateToFormat = new Date(date);
    
    return `${DateToFormat.getDate()}.${DateToFormat.getMonth()}.${DateToFormat.getFullYear()}`;
}

export function getHour(hour) {
    const [hours, minutes] = hour.split(":").map(Number);

    return `${hours < 10 ? "0" + hours : hours}:${minutes < 10 ? "0" + minutes : minutes}`;
}

export function opinionForm(number) {
    if (number == 1) {
        return "opinia";
    } else if (number >= 2 && number <= 4) {
        return "opinie";
    } else {
        return "opinii";
    }
}