<%@ Page Title="" Language="C#" MasterPageFile="~/Master2_SignedIn.Master" EnableEventValidation="true" AutoEventWireup="true" CodeBehind="BookingSchedule.aspx.cs" Inherits="Golf2.BookingSchedule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        #div{
        float:left;
    }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    

    <div class="container">
            <h1>Bokningsschema</h1><br />
            <div id="DisplayChangeDay" runat="server">

                <div class="changeDayButton">
                    <asp:Button ID="Button1"  runat="server" Text="<" OnClick="Button1_Click"/>
                </div>
                <div class="changeDayButton">
                    <h2 id="cdate" ></h2>
                </div>
                <div class="changeDayButton">
                    <asp:Button ID="Button2"  runat="server" Text=">" OnClick="Button2_Click"/>
                </div>
            
            </div>
            
        <!-- Osynliga Alertfält som visas som Fail/Success efter man tryckt på "Bekräfta" -->
        <div class="alert alert-danger text-center" id="bookingAlertFail" runat="server" visible="false" role="alert"></div>
        <div class="alert alert-success text-center" id="bookingAlertsuccess" runat="server" visible="false" role="alert"></div>

            <!-- Bokningsschemat printas ut här-->
            <div id="DisplayBookingSchedule" runat="server">       
                
                

            </div>
            <!-- Bokningsschemat printas ut här-->
         
        </div>
        
        
    <!-- Denna tag behövs för att aktivera PageMethods så att man kan nå metoder i code behind från javascript-->    
    <!-- källa: http://stackoverflow.com/questions/2257631/how-to-create-a-session-using-javascript           -->
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True"></asp:ScriptManager>
    <!-- Denna knapp används för att trigga postback då man tryckt på bekräfta bokning                                              -->
    <asp:Button ID="fakeSenderButton" runat="server" OnCommand="fakeSenderButton_Command" CommandArgument="" text="" reservation0="" reservation1="" reservation2="" reservation3="" currBookingTime="" style="visibility:hidden"/>
    
    <!-- Vi har haft problem med att skicka dynamiska värden via JS till serversidan, nedan HiddenField-tag är en lösning för detta -->
    <!-- HiddenField-ID't kan läsas av ifrån Code Behind                                                                            -->
    <!-- Källa: https://www.codeproject.com/Questions/708697/Pass-javascript-variables-value-to-Csharp-code-beh                     -->
    <asp:HiddenField ID="hdnfldVariable" runat="server" />
    <asp:HiddenField ID="HiddenChangeDateVariable" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

<script>
        function printpage() {
            var getpanel = document.getElementById("<%= ScorecardWithInfo.ClientID%>");
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

     <div class="container">
        <div id="dropdownScorecard">
            <asp:DropDownList ID="dropdownscorecard" runat="server" OnSelectedIndexChanged="dropdownscorecard_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            
            <asp:Label ID="golfID" runat="server" Text="GolfID: "></asp:Label><asp:Label ID="aktuelltgolfID" runat="server"></asp:Label>

             <asp:Label ID="name" runat="server" Text="Namn: "></asp:Label><asp:Label ID="aktuelltNamn" runat="server"></asp:Label>
             <asp:Label ID="date" runat="server" Text="Datum: "></asp:Label><asp:Label ID="aktuelltDatum" runat="server"></asp:Label>
             <asp:Button ID="printScorecard" runat="server" OnClientClick="return printpage();" Text="Skriv ut scorekort"/><br />
  <!--Här börjar scorekortet och alla tabeller-->
<asp:Panel ID="ScorecardWithInfo" runat="server"> 
   <div id="div">

      <!-- Tabell Huvet-->  
<table id="Table1" width="1100" border="1">
	<tr>
	<td rowspan="2" id="l">Spelare: <asp:Label ID="scorecardName" runat="server"></asp:Label></td>
		<td id="r">GolfId: <asp:Label ID="scorecardGolfId" runat="server"></asp:Label></td>
		<td id="t">Datum: <asp:Label ID="scorecardDate" runat="server"></asp:Label></td>
	</tr>
    
<tr>
<td id="r">Klubb</td>
<td id="t">Starttid: <asp:Label ID="scorecardTime" runat="server"></asp:Label></td>
</tr>

	<tr>
	<td rowspan="2" id="l">Tävling</td>
		<td id="r">ExaktHcp: <asp:Label ID="scorecardHcp" runat="server"></asp:Label></td>
		<td id="t">Startordning</td>
	</tr>
    
<tr>
<td id="r">SpelHcp <asp:Label ID="scorecardSpelHcp" runat="server"></asp:Label>  </td>
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
<td id="Erh"> <asp:Label ID="hole1" runat="server"></asp:Label></td>
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
<td id="Erh"><asp:Label ID="hole2" runat="server"></asp:Label></td>
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



        </div>  

    </div>


</asp:Content>


