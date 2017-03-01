<%@ Page Title="" Language="C#" MasterPageFile="~/Master1.Master" AutoEventWireup="true" CodeBehind="Home1.aspx.cs" Inherits="Golf2.Home1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container" id="main" >    
<h1>Välkommen till Hålslaget GK!</h1>    

    <div id="myCarousel" class="carousel slide" data-ride="carousel">
    <!-- Indicators -->
    <ol class="carousel-indicators">
      <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
      <li data-target="#myCarousel" data-slide-to="1"></li>
    </ol>

    <!-- Wrapper for slides -->
    <div class="carousel-inner" role="listbox">
      <div class="item active">
        <img src="bilder/bild2.jpg" alt="Banan" width="1200" height="700">
        <div class="carousel-caption">
          <h3>Banan.</h3>
          <p>Utslag hål 9.</p>
        </div>      
      </div>

      <div class="item">
        <img src="bilder/bild1.jpg" alt="Klubbhuset" width="1200" height="700">
        <div class="carousel-caption">
          <h3>Hålslagets klubbhus</h3>
          <p>Ta en lunch eller bärs.</p>
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

    
 <section class="bg-primary" id="about">
        <div class="container">
            <div class="row">
                <div class="col-lg-8 col-lg-offset-2 text-center">
                    <h2 class="section-heading">Kom och spela golf hos oss!</h2>

                    <p class="text-faded">Kom och spela golf hos oss, Kom och spela golf hos oss, Kom och spela golf hos oss, Kom och spela golf hos oss.</p>
                    
                    <a href="#services" class="btn btn-default">Boka tid!</a>
                </div>
            </div>
        </div>
    </section>    
    
   </div>  

</asp:Content>
