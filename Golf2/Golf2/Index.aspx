<%@ Page Title="" Language="C#" MasterPageFile="~/Master1.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Golf2.Home1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   
    <div id="myCarousel" class="carousel slide" data-ride="carousel">
    <!-- Indicators -->
    <ol class="carousel-indicators">
      <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
      <li data-target="#myCarousel" data-slide-to="1"></li>
      <li data-target="#myCarousel" data-slide-to="2"></li>
        
    </ol>

    <!-- Wrapper for slides -->
    <div class="carousel-inner" role="listbox">
      <div class="item active">
        <img src="Bilder/bild1.jpg" alt="Banan" width="100%" height="100%">
        <div class="carousel-caption">
          <h3>Banan</h3>
          <p>Banan är nu öppen och i toppenskick!</p>
        </div>      
      </div>

      <div class="item">
        <img src="Bilder/bild3.jpg" alt="Klubbhuset" width="100%" height="100%">
        <div class="carousel-caption">
          <h3>Utslag hål 9</h3>
          <p>Vår bana har jättemånga hål.</p>
        </div>      
      </div>
        
        <div class="item">
        <img src="Bilder/bild2.jpg" alt="Klubbhuset" width="100%" height="100%">
        <div class="carousel-caption">
          <h3>Hålslagets klubbhus</h3>
          <p>Välkommen in och ta en matbit. Öppet från 11.00 - 18.00</p>
        </div>      
      </div>
        
    </div>

    <!-- Left and right controls -->
    <a class="left carousel-control" href="#myCarousel" role="button" data-slide="prev">
      <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
      <span class="sr-only">Previous</span>
    </a>
    <a class="right carousel-control" href="#myCarousel" role="button" data-slide="next">
      <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
      <span class="sr-only">Next</span>
    </a>
</div>    
    
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
                    
        <div class="row" id="banan">
            <div class="col-lg-10 col-lg-offset-1 text-center">
                    <h2>Upplev vår bana i världsklass</h2>
                    <hr class="small">
                    <p>En massa hål att försöka träffa med så få slag som möjligt, roligt har vi här!</p>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="portfolio-item">
                                    <img class="img-portfolio img-responsive" src="Bilder/bild1.jpg">
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="portfolio-item">
                                    <img class="img-portfolio img-responsive" src="Bilder/bild1.jpg">
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="portfolio-item">
                                    <img class="img-portfolio img-responsive" src="Bilder/bild1.jpg">
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="portfolio-item">
                                    <img class="img-portfolio img-responsive" src="Bilder/bild1.jpg">
                            </div>
                        </div>
                    </div>
                </div>
            </div>

</asp:Content>


