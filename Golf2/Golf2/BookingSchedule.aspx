<%@ Page Title="" Language="C#" MasterPageFile="~/Master2_SignedIn.Master" EnableEventValidation="true" AutoEventWireup="true" CodeBehind="BookingSchedule.aspx.cs" Inherits="Golf2.BookingSchedule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

            <!-- Bokningsschemat printas ut här-->
            <div id="DisplayBookingSchedule" runat="server">       

            </div>
            <!-- Bokningsschemat printas ut här-->
    </div>
    <!-- Denna knapp används för att trigga postback då man tryckt på bekräfta bokning                                              -->
    <asp:Button ID="fakeSenderButton" runat="server" OnCommand="fakeSenderButton_Command" CommandArgument="" text="" reservation0="" reservation1="" reservation2="" reservation3="" currBookingTime="" style="visibility:hidden"/>
    
    <!-- Vi har haft problem med att skicka dynamiska värden via JS till serversidan, nedan HiddenField-tag är en lösning för detta -->
    <!-- HiddenField-ID't kan läsas av ifrån Code Behind                                                                            -->
    <!-- Källa: https://www.codeproject.com/Questions/708697/Pass-javascript-variables-value-to-Csharp-code-beh                     -->
    <asp:HiddenField ID="hdnfldVariable" runat="server" />
    <asp:HiddenField ID="HiddenChangeDateVariable" runat="server" />
</asp:Content>


