<%@ Page Title="" Language="C#" MasterPageFile="~/Master1.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Golf2.Home1" %>   

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   
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
                    <a href="#services" class="btn btn-default">Boka tid</a>
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


