/* ALL JS-kod att läggas inom denna. Tvingar DOM:en att läses in före körning av någon kod */
$(function () {



});

function reservation(elementName, golfid) {
    var whichElementToChange = document.getElementById(elementName);
    if (whichElementToChange.innerHTML != 'Ledig plats') {
        whichElementToChange.innerHTML = 'Ledig plats';
    }
    else {
        whichElementToChange.innerHTML = 'Reserverad för: ' + golfid;
    }

}
