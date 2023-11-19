export function getStars(number, code) {
    var stars = '';
    for(var i=0; i < number; i++) {
        stars += code;
    }

    return stars;
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