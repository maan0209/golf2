<%@ Page Title="" Language="C#" MasterPageFile="~/Master2_SignedIn.Master" AutoEventWireup="true" CodeBehind="BookingSchedule.aspx.cs" Inherits="Golf2.BookingSchedule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="container">
        <h1>Bokningsschema</h1><br />
        <div id="DisplayChangeDay" runat="server">

            <asp:Button ID="Button1" runat="server" Text="<" OnClick="Button1_Click"/>

            <asp:Button ID="Button2" runat="server" Text=">" OnClick="Button2_Click"/>
            
        </div>

        <!-- Bokningsschemat printas ut här-->
        <div id="DisplayBookingSchedule" runat="server">       
        
        </div>
        <!-- Bokningsschemat printas ut här-->

    </div>
    <div id ="hej" runat="server"></div>

    <script type="text/javascript">
        function reservera() {
            
        }

    </script>
    
</asp:Content>


