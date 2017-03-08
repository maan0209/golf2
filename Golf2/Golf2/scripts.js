/* ALL JS-kod att läggas inom denna. Tvingar DOM:en att läses in före körning av någon kod */
$(function () {

    

});

// funktion för att lagra golfids som reserveras i bookingschedule
function confirmbooking(elementName, confirmbutton)
{
    // Knappen
    var button = document.getElementById();
    // Hämta variabler
    var golfid1 = button.getAttribute('reservation0');
    var golfid2 = button.getAttribute('reservation1');
    var golfid3 = button.getAttribute('reservation2');
    var golfid4 = button.getAttribute('reservation3');
    var bookingtime = button.getAttribute('bookingtime');
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
}
