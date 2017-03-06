/* ALL JS-kod att läggas inom denna. Tvingar DOM:en att läses in före körning av någon kod */
$(function () {



});

function reservation(elementName) {
    alert(elementName);
    var whichElementToChange = document.getElementById(elementName);
    whichElementToChange.innerHTML = 'Reserverad';
}

