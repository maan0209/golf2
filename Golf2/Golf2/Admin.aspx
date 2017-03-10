<%@ Page Title="" Language="C#" MasterPageFile="~/Master3_Admin.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Golf2.Admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



<div class="container" id="season">
  <h2>Säsongsinställningar</h2>
  <p>Välj när säsongen startar och slutar.</p>      
  <div class="row">

    <div class="col-sm-4" id="kalendersektion">
        <p>Säsongsstart:</p>
     <asp:Calendar ID="Calendar1" runat="server" OnSelectionChanged="Calendar1_SelectionChanged" Height="100%" Width="100%"></asp:Calendar>
        <p>Valt datum:</p>
        <asp:Label ID="lblStartdatum" runat="server" Text="Label"></asp:Label>

    </div>

    <div class="col-sm-4" id="kalendersektion">
        <p>Säsongsslut:</p>
        <asp:Calendar ID="Calendar2" runat="server" OnSelectionChanged="Calendar2_SelectionChanged" Height="100%" Width="100%"></asp:Calendar>
        <p>Valt datum:</p>
        <asp:Label ID="lblSlutdatum" runat="server" Text="Label"></asp:Label>

    </div>  
  </div>

    <asp:LinkButton ID="saveSeason" CssClass="btnadmin" Font-Underline="false" OnClick="saveSeason_Click" runat="server">Spara säsongsdatum</asp:LinkButton>

         <hr class="small">

</div>


    <div class="container" id="openhours">
         <h2>Dagliga öppetider</h2>
          <p>Välj när banan öppnar och stänger.</p> 
  
  <div class="row">

    <div class="col-sm-3" id="kalendersektion">

        <asp:Label ID="open" runat="server" Text="Välj tid då banan öppnar"></asp:Label>
        <asp:DropDownList ID="openhour" runat="server"></asp:DropDownList>

    </div>

    <div class="col-sm-3" id="kalendersektion">

        <asp:Label ID="close" runat="server" Text="Välj tid då banan stänger"></asp:Label>
        <asp:DropDownList ID="closehour" runat="server"></asp:DropDownList>

    </div>  
  </div>

    <asp:LinkButton ID="setOpenClose" OnClick="setOpenClose_Click" CssClass="btnadmin" Font-Underline="false" runat="server">Spara öppetider</asp:LinkButton>
        
        

    
<%--        //Din knapp Martin--%>
<%--    <asp:Button ID="setOpenClose" runat="server" Text="Ange öppettider" OnClick="setOpenClose_Click" />--%>

         <hr class="small">

</div>















    </div>



</asp:Content>