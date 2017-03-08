/* ALL JS-kod att läggas inom denna. Tvingar DOM:en att läses in före körning av någon kod */
$(function () {



});

/* Används för att trigga en postback*/
function fulfix(theTimeToBook) {
    console.log('postback trigger');
    var fakeButton = document.getElementById('ContentPlaceHolder1_fakeSenderButton');
    fakeButton.setAttribute("currBookingTime", theTimeToBook);
    var concatenateAString = "";
    for (var i = 0; i < 4; i++) { 
        var bookingValue = fakeButton.getAttribute("reservation" + i);
        if (bookingValue != "") {
            if (concatenateAString != "") {
                concatenateAString += "#" + bookingValue.toString();
            }
            else {
                concatenateAString = bookingValue.toString();
            }
        }
    }
    if (concatenateAString = "") {
        return;
    }
    else {
        concatenateAString += "#" + fakeButton.getAttribute("currBookingTime");
    }
    fakeButton.setAttribute("CommandArgument", concatenateAString);
    
    $('#ContentPlaceHolder1_fakeSenderButton')[0].click();  

}

    /* En reservation skapas för en ledig bokningsplats. Golfid anges */
    function reservation(elementName, golfidElementName, confirmButton, r, reservationButton) {
        var whichElementToChange = document.getElementById(elementName);
        if (whichElementToChange.innerHTML != 'Ledig plats') {
        whichElementToChange.innerHTML = 'Ledig plats';
        var cButton = document.getElementById(confirmButton);
        cButton.attributes["reservation" + r].value = "";
        document.getElementById(reservationButton).value = "Reservera";
        }
        else {
        var golfid = document.getElementById(golfidElementName).value;
        if (golfid != "") {
            whichElementToChange.innerHTML = 'Reserverad för: ' + golfid.toString();
            var cButton = document.getElementById(confirmButton);
            cButton.attributes["reservation" + r].value = golfid;
            document.getElementById(reservationButton).value = "Ångra";
            document.getElementById(confirmButton).setAttribute("currBookingTime", "");
        }
}
}



    /* Trycker man på 'stäng' så rensas alla reserverationer - allt återställs */
    function clearAllReservations(elementName, reservationButton, confirmButton) {
        for (var i = 0; i < 4; i++) {
        var whichElementToChange = document.getElementById(elementName + i);
        whichElementToChange.innerHTML = 'Ledig plats';
        document.getElementById(reservationButton + i).value = "Reservera";
        document.getElementById(confirmButton).setAttribute("reservation" + i, "");

        }
        document.getElementById(confirmButton).setAttribute("currBookingTime", "");
}

