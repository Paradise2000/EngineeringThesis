$("#register").on('click', function() {
    
    var isValid = true;
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if($("#nick").val().length < 3) {
        $("#nickError").text("Nick nie ma przynajmniej 3 znaków");
        isValid = false;
    } else {
        $("#nickError").text("");
    }

    if(!emailRegex.test($("#email").val())) {
        $("#emailError").text("adres email jest nieprawidłowy");
        isValid = false;
    } else {
        $("#emailError").text("");
    }

    if ($("#password").val() != $("#repeatPassword").val()) {
        $("#passwordError").text("Hasła nie są takie same");
        isValid = false;
    } else if ($("#password").val().length < 5) {
        $("#passwordError").text("Hasło nie ma przynajmniej 5 znaków");
        isValid = false;
    } else {
        $("#passwordError").text("");
    }

    if(isValid) {
        const jsonData = JSON.stringify({
            nickName: $("#nick").val(),
            email: $("#email").val(),
            password: $("#password").val()
        });

        console.log("TEST");

        fetch('https://localhost:7002/api/account/register', {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json',
    },
    body: jsonData
})
    .then(response => {
        if (!response.ok) {
            // Obsługa błędów dla statusu HTTP różnego niż 200
            return response.json().then(errorData => {
                // Przetwarzanie danych błędu
                if (errorData.errors) {
                    errorData.errors.forEach(element => {
                        if (element.errorCode === "EmailAlreadyTaken") {
                            $("#emailError").text("Ten adres email jest już zajęty");
                        }

                        if (element.errorCode === "NickAlreadyTaken") {
                            $("#nickError").text("Ten nick jest już zajęty");
                        }
                    });
                }

                // Rzucenie błędu z komunikatem
                throw new Error('Błąd żądania: ' + response.status);
            });
        }

        // Jeśli wszystko jest w porządku, przetwórz odpowiedź jako JSON
        return response.json();
    })
    .then(data => {
        // Tutaj możesz korzystać z danych w formie obiektu JSON
        console.log(data);
    })
    .catch(error => {
        // Obsługa błędów związaną z samym żądaniem lub przetwarzaniem odpowiedzi
        console.error('Wystąpił błąd:', error);
    });

    }


});