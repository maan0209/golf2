<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="printpage.aspx.cs" Inherits="Golf2.printpage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Skriva ut sida</title>
    
    <!-- Script som hämtar info från panelen med ID Scorecard och sedan skriver ut detta -->
    <script>
        function printpage() {
            var getpanel = document.getElementById("<%= Scorecard.ClientID%>");
            var MainWindow = window.open('', '', 'height=500,width=800');
            MainWindow.document.write('<html><head><title>Print Page</title>');
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
</head>
<body>
    <form id="form1" runat="server">
    
        <!-- Panel med ID Scorecard som innehåller HTML tabell. -->
        <asp:Panel ID="Scorecard" runat="server">
            <table style="width: 100%;">
                <tr>
                    <td>Namn:</td>
                    <td>Hp:</td>
                    <td>Datum:</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
    
        </asp:Panel>
        
        <!-- Knapp som anropar funktionen printpage() i scriptet för att skriva ut det som finns i panelen. -->
        <asp:Button ID="printScorecard" runat="server" OnClientClick="return printpage();" Text="Skriv ut scorekort"/>
    <br />

        <!--    Sätter öppettider för banan. I Page Load så läser sidan in från databasens tabell bookingtime till dropdownlist.
                Vid knapptryck så hämtas selectedValue och skickas med som en update till databasen.
                -->
        <asp:Label ID="open" runat="server" Text="Välj tid då banan öppnar"></asp:Label><br />
        <asp:DropDownList ID="openhour" runat="server"></asp:DropDownList>
    <br />
        <asp:Label ID="close" runat="server" Text="Välj tid då banan stänger"></asp:Label><br />
        <asp:DropDownList ID="closehour" runat="server"></asp:DropDownList>
        <p>

        </p>
        <asp:Button ID="setOpenClose" runat="server" Text="Ange öppettider" OnClick="setOpenClose_Click" />




    </form>
</body>
</html>
