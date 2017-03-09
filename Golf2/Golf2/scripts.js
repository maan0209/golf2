/* ALL automatiserad JS-kod att läggas inom denna. Tvingar DOM:en att läses in före körning av någon kod */
$(function () {

    /* Håller datumet uppdaterat i bokningsschemats header                                              */
    /*Källa: http://stackoverflow.com/questions/1309452/how-to-replace-innerhtml-of-a-div-using-jquery  */
    $(function () {
            if (sessionStorage.getItem("currDate") == null) {
                        var todaysDate = new Date();
                        sessionStorage.setItem("currDate", todaysDate)
                        
            }
            
            var anyDate = new Date(sessionStorage.getItem("currDate"));
            var processedDate = formatTheDate(anyDate);
            var updateDate = document.getElementById('cdate');
            updateDate.innerHTML = processedDate;
    });

    /* Ändrar datumet en dag bakåt i tiden */
    $('#ContentPlaceHolder1_Button1').click(function () {
        var goForwards = "False";
        pickAnotherDay(goForwards);
    });

    /* Ändrar datumet en dag framåt i tiden */
    $('#ContentPlaceHolder1_Button2').click(function () {
        var goForwards = "True";
        pickAnotherDay(goForwards);
    });

    /* Tar emot en BOOL och ändrar datumet antingen en dag framåt eller bakåt */
    function pickAnotherDay(goForwards) {
        
        var changeTheDate = new Date(sessionStorage.getItem("currDate"));

        if (goForwards == 'True') {
            changeTheDate.setDate(changeTheDate.getDate() + 1);
        }
        else {
            changeTheDate.setDate(changeTheDate.getDate() - 1);
        }

        sessionStorage.setItem("currDate", changeTheDate);
        var HiddenChangeDateVariable = document.getElementById('ContentPlaceHolder1_HiddenChangeDateVariable');
        HiddenChangeDateVariable.value = formatTheDate(changeTheDate);
    }

    /* En lite trevligare formatering av javascript-datum*/
    function formatTheDate(dateIn) {
        var d = new Date();
        d = d.setDate = dateIn;
        var yyyymmdd;
        var year = d.getFullYear();
        var month = (d.getMonth() + 101).toString().slice(-2);
        var dateOfMonth = (d.getDate() + 100).toString().slice(-2);
        yyyymmdd = year.toString() + "-" + month.toString() + "-" + dateOfMonth.toString();
        return yyyymmdd;
    };

});

/* Används för att trigga en postback*/
function fulfix(theTimeToBook) {
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
    if (concatenateAString == "") {
        return;
    }
    else {
        concatenateAString += "#" + fakeButton.getAttribute("currBookingTime");
    }
    for (var i = 0; i < 50; i++) {
        console.log(concatenateAString.toString());
    }
    
    fakeButton.setAttribute("CommandArgument", concatenateAString);
    var hdnfldVariable = document.getElementById('ContentPlaceHolder1_hdnfldVariable');
    hdnfldVariable.value = concatenateAString;
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
};



    /* Trycker man på 'stäng' så rensas alla reserverationer - allt återställs */
function clearAllReservations(elementName, reservationButton, confirmButton) {
    for (var i = 0; i < 4; i++) {
        var whichElementToChange = document.getElementById(elementName + i);
        whichElementToChange.innerHTML = 'Ledig plats';
        document.getElementById(reservationButton + i).value = "Reservera";
        document.getElementById(confirmButton).setAttribute("reservation" + i, "");
    };
    document.getElementById(confirmButton).setAttribute("currBookingTime", "");
};

//Hitta positon för där ny tee läggs till och lägger till ny kolumn i Scorekortet 

function Table2() {

    $('#Table3').find('tr').each(function () {
        $(this).find('td').eq(1).after('<td>cell 1a</td>')
    });

};
// Hitta positon för där ny tee läggs till och lägger till ny kolumn i Scorekortet + sparar hur många kolumner som lagts till 
function Table3() {

    var varde;

    if (sessionStorage.getItem("kolumn") == null || sessionStorage.getItem("kolumn") == 0) {
        sessionStorage.setItem("kolumn", 1);
        varde = sessionStorage.getItem("kolumn");
        var hamtaknapp = document.getElementById('deleteknapp')
        hamtaknapp.disabled = false;
    }
    else {
        varde = sessionStorage.getItem("kolumn");
        varde++;
        sessionStorage.setItem("kolumn", varde);
    }

    var t = $('#Tee').attr("colspan");
    t++;

    $('#Tee').attr("colspan", t)
    $('#Table2').find('tr').eq(1).find('td').eq(0).after('<td>cell 1a</td>');
    Table2();
    Table4();
};


function Table4() {

    $('#Table4').find('tr').each(function () {
        $(this).find('td').eq(1).after('<td>cell 1a</td>')
    });
};

// Tar bort Tees om man ångrat sig 

function Delete() {

    var t = $('#Tee').attr("colspan");
    t--;

    $('#Tee').attr("colspan", t);
    $('#Table2').find('tr').eq(1).find('td').eq(1).remove();

    $('#Table4').find('tr').each(function () {
        $(this).find('td').eq(2).remove()
    });

    var varde = sessionStorage.getItem("kolumn");
    varde--;
    sessionStorage.setItem("kolumn", varde);

    if (sessionStorage.getItem("kolumn") == 0) {
        var hamtaknapp = document.getElementById('deleteknapp')
        hamtaknapp.disabled = true
    }

    Delete2();
};




function Delete2() {

    $('#Table3').find('tr').each(function () {
        $(this).find('td').eq(2).remove()

    });
};


