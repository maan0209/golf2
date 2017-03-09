<%@ Page Title="" Language="C#" MasterPageFile="~/Master1.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Golf2.Home1" %>   

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script>
        function printpage() {
            var getpanel = document.getElementById("<%= Scorecard.ClientID%>");
            var MainWindow = window.open('', '', 'height=850,width=1000');
            MainWindow.document.write('<html><head><link type="text/css" rel="stylesheet" href="Scorekort.css"/><title>Print Page</title>');
            MainWindow.document.write('</head><body>');
            MainWindow.document.write(getpanel.innerHTML);
            MainWindow.document.write('</body></html>');
            
            MainWindow.document.close();
            setTimeout(function(){
                MainWindow.print();
            }, 500);
            return false;
        }
</script>
   
        <!-- Karusellen börjar här -->
    
    <div id="myCarousel" class="carousel slide" data-ride="carousel">
    <!-- Indicators -->
    <ol class="carousel-indicators">
      <li data-target="#myCarousel" data-slide-to="0"></li>
      <li data-target="#myCarousel" data-slide-to="1"></li>
      <li data-target="#myCarousel" data-slide-to="2"></li>
        
    </ol>

    <!-- Wrapper for slides -->
    <div class="carousel-inner" role="listbox">
      <div class="item active">
        <img src="bilder/bild1.jpg" alt="Banan" width="100%" height="100%">
        <div class="carousel-caption">
          <h3>Banan</h3>
          <p>Banan är nu öppen och i toppenskick!</p>
        </div>      
      </div>

      <div class="item">
        <img src="bilder/bild3.jpg" alt="Klubbhuset" width="100%" height="100%">
        <div class="carousel-caption">
          <h3>Utslag hål 9</h3>
          <p>Vår bana har jättemånga hål.</p>
        </div>      
      </div>
        
        <div class="item">
        <img src="bilder/bild2.jpg" alt="Klubbhuset" width="100%" height="100%">
        <div class="carousel-caption">
          <h3>Hålslagets klubbhus</h3>
          <p>Välkommen in och ta en matbit. Öppet från 11.00 - 18.00</p>
        </div>      
      </div>
        
    </div>

    <!-- Vänster & Höger -->
    <a class="left carousel-control" href="#myCarousel" role="button" data-slide="prev">
      <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
      <span class="sr-only">Previous</span>
    </a>
    <a class="right carousel-control" href="#myCarousel" role="button" data-slide="next">
      <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
      <span class="sr-only">Next</span>
    </a>
</div>    
    
            <!-- About / Boka tid sektion börjar här -->
    
<div class="bg-primary" id="about">
        <div class="container">
            <div class="row">
                <div class="col-lg-8 col-lg-offset-2 text-center">
                    <h2 class="section-heading">Kom och spela golf hos oss!</h2>                    
                    <hr class="small">
                    <p class="text-faded">Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>                    
                    <a href="/BookingSchedule.aspx" class="btn btn-default">Boka tid</a>
                </div>
            </div>
        </div>
    </div>                                                                                     
    
    <!-- Nyheter börjar här -->
    
    <div class="container-fluid">
        <div id="nyheter" class="col-lg-10 col-lg-offset-1 text-center">       
    <h2>Nyheter</h2>      
      <hr class="small">
  
  <div class="row text-center slideanim">
    <div class="col-sm-4">
      <div class="thumbnail">
        <img src="bilder/nyhet1.jpg" alt="nyhet1" width="400" height="300">
        <p><strong>Sega Rundor</strong></p>
        <p>Här kommer det finnas lite text....</p>
      </div>
        </div>
    <div class="col-sm-4">
      <div class="thumbnail">
        <img src="bilder/nyhet2.jpg" alt="nyhet2" width="400" height="300">
        <p><strong>Skräp på banan</strong></p>
        <p>Jag dricker gärna Öl men när min boll.....</p>
      </div>
    </div>
    <div class="col-sm-4">
      <div class="thumbnail">
        <img src="bilder/nyhet3.jpg" alt="nyhet3" width="400" height="300">
        <p><strong>Inge kul, bara strul</strong></p>
        <p>texte text text text.....</p>
      </div>
    </div>      
      <a href="#morenews" class="btn btn-default">Mer Nyheter</a>
  </div>   
  </div>          
</div>
    
    <!-- Ban sektion börjar här -->
    
<div id="banan"> 
  <h2>Upplev vår bana i världsklass</h2>
    
  <!--Här börjar scorekortet och alla tabeller-->
<asp:Panel ID="Scorecard" runat="server">
<div id="div">

      <!-- Tabell Huvet-->  
<table id="Table1" width="1100" border="1">
	<tr>
	<td rowspan="2" id="l">Spelare</td>
		<td id="r">GolfId</td>
		<td id="t">Datum</td>
	</tr>
    
<tr>
<td id="r">Klubb</td>
<td id="t">Starttid</td>
</tr>

	<tr>
	<td rowspan="2" id="l">Tävling</td>
		<td id="r">ExaktHcp</td>
		<td id="t">Startordning</td>
	</tr>
    
<tr>
<td id="r">SpelHcp</td>
<td id="t">Klass</td>
</tr>

	
</table>
      <!-- Tabell huvudet del 2r -->
   
<table id="Table2" width="1100" border="1">
	<tr>
	<td rowspan="2" id ="Hål">Hål</td>
		<td colspan="2" id="Tee">Tee, längd i meter</td>
		<td rowspan="2" id="Par">Par</td>
		<td rowspan="2" id="Index">Index</td>
		<td rowspan="2" id="Erh">Erh slag</td>	
		<td colspan="2">Spelare 1</td>
		<td colspan="2">Spelare 2</td>
		<td colspan="2">Spelare 3</td>
		<td colspan="2">Markör</td>
		<td colspan="3">Personlig statestik*</td>
		
	</tr>
   
<tr id=>

<td id ="Gul">Gul</td>
<td id ="Röd">Röd</td>
<td id="Score">Score</td>
<td id="Score">Poäng</td>
<td id="Score">Score</td>
<td id="Score">Poäng</td>
<td id="Score">Score</td>
<td id="Score">Poäng</td>
<td id="Score">Score</td>
<td id="Score">Poäng</td>
<td id="Fw">Fw</td>
<td id="Gir">GIR</td>
<td id="Putt">Putt</td>
</table>
</br>

</tr>
      <!-- Hål sektion 1  -->
<table id="Table3" width="1100" border="1"> 

<tr id="rad">

<td id ="Hål">1</td>
<td id ="Gul">453</td>
<td id ="Röd">386</td>
<td id ="Par">5</td>
<td id ="Index">16</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>
</tr>

<tr>
<td id ="Hål">2</td>
<td id ="Gul">299</td>
<td id ="Röd">268</td>
<td id ="Par">4</td>
<td id ="Index">14</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>
</tr>

<tr>
<td id ="Hål">3</td>
<td id ="Gul">155</td>
<td id ="Röd">145</td>
<td id ="Par">3</td>
<td id ="Index">10</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>
</tr>

<tr>
<td id ="Hål">4</td>
<td id ="Gul">348</td>
<td id ="Röd">304</td>
<td id ="Par">4</td>
<td id ="Index">2</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>
</tr>

<tr>
<td id ="Hål">5</td>
<td id ="Gul">341</td>
<td id ="Röd">301</td>
<td id ="Par">4</td>
<td id ="Index">4</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>
</tr>

</tr>
<td id ="Hål">6</td>
<td id ="Gul">116</td>
<td id ="Röd">101</td>
<td id ="Par">3</td>
<td id ="Index">18</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>

</tr>
<tr>

<td id ="Hål">7</td>
<td id ="Gul">304</td>
<td id ="Röd">268</td>
<td id ="Par">4</td>
<td id ="Index">8</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>
</tr>
<tr>

<td id ="Hål">8</td>
<td id ="Gul">480</td>
<td id ="Röd">415</td>
<td id ="Par">5</td>
<td id ="Index">12</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>
</tr>
<tr>

<td id ="Hål">9</td>
<td id ="Gul">344</td>
<td id ="Röd">312</td>
<td id ="Par">4</td>
<td id ="Index">6</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>
</tr>
<tr>


<td id="Hål">Ut</td>
<td id="Hål">2768</td>
<td id="Hål">2500</td>
<td id="Hål">36</td>
<td id="Hål">Ut</td>
<td></td>
<td></td>
<td></td>
<td></td>
<td></td>
<td></td>
<td></td>
<td></td>
<td></td>
<td></td>
<td></td>
<td></td>
</tr>

</table>

</br>


 <!-- Hål sektion 2  -->
<table id="Table4" width="1100" border="1"> 
<tr>
<td id ="Hål">10</td>
<td id ="Gul">321</td>
<td id ="Röd">306</td>
<td id ="Par">4</td>
<td id ="Index">9</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>
</tr>

<tr>
<td id ="Hål">11</td>
<td id ="Gul">308</td>
<td id ="Röd">286</td>
<td id ="Par">4</td>
<td id ="Index">13</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>
</tr>

<tr>
<td id ="Hål">12</td>
<td id ="Gul">450</td>
<td id ="Röd">428</td>
<td id ="Par">5</td>
<td id ="Index">7</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>
</tr>

<tr>
<td id ="Hål">13</td>
<td id ="Gul">357</td>
<td id ="Röd">332</td>
<td id ="Par">4</td>
<td id ="Index">3</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>
</tr>

<tr>
<td id ="Hål">14</td>
<td id ="Gul">150</td>
<td id ="Röd">150</td>
<td id ="Par">3</td>
<td id ="Index">17</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>
</tr>

<tr>
<td id ="Hål">15</td>
<td id ="Gul">366</td>
<td id ="Röd">351</td>
<td id ="Par">4</td>
<td id ="Index">1</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>
</tr>

<tr>
<td id ="Hål">16</td>
<td id ="Gul">437</td>
<td id ="Röd">376</td>
<td id ="Par">5</td>
<td id ="Index">15</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>
</tr>

<tr>
<td id ="Hål">17</td>
<td id ="Gul">139</td>
<td id ="Röd">139</td>
<td id ="Par">3</td>
<td id ="Index">11</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>
</tr>

<tr>
<td id ="Hål">18</td>
<td id ="Gul">318</td>
<td id ="Röd">308</td>
<td id ="Par">4</td>
<td id ="Index">5</td>
<td id="Erh"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Fw"></td>
<td id="Gir"></td>
<td id="Putt"></td>
</tr>



<tr>
<td id="Hål">IN</td>
<td id="Hål">2846</td>
<td id="Hål">2676</td>
<td id="Hål">36</td>
<td id="Hål">IN</td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
</tr>

<tr>
<td id="Hål">UT</td>
<td id="Hål">2768</td>
<td id="Hål">2500</td>
<td id="Hål">36</td>
<td id="Hål">UT</td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
</tr>
    
<tr>
<td id="Hål">S:A</td>
<td id="Hål">5615</td>
<td id="Hål">5176</td>
<td id="Hål">72</td>
<td id="Hål">S:A</td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
<td id="Score"></td>
</tr> 
</table> 

  <!-- Scorekort footern   -->
<table id="Table5" width="1100" border="1"> 
<tr>
<td id="hej">Spelhandicap</td>
<td id="hej">Netto</td>
<td id="hej">Markör, sign</td>
<td id="hej">Markör, golfID</td>
<td id="hej">Spelare, sign</td> 
<td id="hej">Spelare, sign</td> 

</tr>
</table>
     
<table id="Table6" width="1100" border="1"> 
<tr>
<td></td>
<td id="Resultat" height="50">Resultat:</td>
</tr>

</table>

</div>
</asp:Panel>
<p>
<input type="button" value="Lägg till ny Tee" onclick="Table3()" /> <button id="deleteknapp" disabled="true" type="button" onclick="Delete()">Ta bort Tee</button><asp:Button ID="printScorecard" runat="server" OnClientClick="return printpage();" Text="Skriv ut scorekort"/>
</p>


 <p class="stars"><span class="glyphicon glyphicon-star"></span> <span class="glyphicon glyphicon-star"></span> <span class="glyphicon glyphicon-star"></span> <span class="glyphicon glyphicon-star"></span> <span class="glyphicon glyphicon-star"></span> </p>
    
    <hr class="small">
  <div class="row">
    <div class="col-md-4">
      <div class="portfolio-item">
        <a href="bilder/bild5.jpg" target="_blank">
          <img src="bilder/bild5.jpg" alt="golf" style="width:100%">

        </a>
      </div>
    </div>
      
    <div class="col-md-4">
      <div class="portfolio-item">
        <a href="bilder/bild6.jpg" target="_blank">
          <img src="bilder/bild6.jpg" alt="golf" style="width:100%">

        </a>
      </div>
    </div>
    <div class="col-md-4">
      <div class="portfolio-item">
        <a href="bilder/bild7.jpg" target="_blank">
          <img src="bilder/bild7.jpg" alt="golf" style="width:100%">

        </a>
      </div>
    </div>
      
       <div class="col-md-4">
      <div class="portfolio-item">
        <a href="bilder/bild7.jpg" target="_blank">
          <img src="bilder/bild9.jpg" alt="golf" style="width:100%">

        </a>
      </div>
    </div>
      
       <div class="col-md-4">
      <div class="portfolio-item">
        <a href="bilder/bild10.jpg" target="_blank">
          <img src="bilder/bild11.jpg" alt="golf" style="width:100%"/>

        </a>
      </div>
    </div>
      
       <div class="col-md-4">
      <div class="portfolio-item">
        <a href="bilder/bild11.jpg" target="_blank">
          <img src="bilder/bild10.jpg" alt="golf" style="width:100%"/>

        </a>
      </div>
    </div>         
  </div>   
     <hr class="small">
</div>                           
 
    

</asp:Content>


