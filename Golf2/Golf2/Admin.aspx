 <%@ Page Title="" Language="C#" MasterPageFile="~/Master3_Admin.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Golf2.Admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<%--STÄNG ÖPPNA SÄSONG--%>
<div class="container" id="season">
  <h2>Säsongsinställningar</h2>
  <p>Välj när säsongen startar och slutar.</p>      
  <div class="row">

    <div class="col-sm-4 kalendersektion">
        <p>Säsongsstart:</p>
     <asp:Calendar ID="Calendar1" runat="server" OnSelectionChanged="Calendar1_SelectionChanged" Height="162px" Width="100%"></asp:Calendar>
        <p>Valt datum:</p>
        <asp:Label ID="lblStartdatum" runat="server" Text="Label"></asp:Label>
    </div>

    <div class="col-sm-4 kalendersektion">
        <p>Säsongsslut:</p>
        <asp:Calendar ID="Calendar2" runat="server" OnSelectionChanged="Calendar2_SelectionChanged" Height="162px" Width="100%"></asp:Calendar>
        <p>Valt datum:</p>
        <asp:Label ID="lblSlutdatum" runat="server" Text="Label"></asp:Label>
    </div>  
  </div>

    <asp:LinkButton ID="saveSeason" CssClass="btnadmin" Font-Underline="false" OnClick="saveSeason_Click" runat="server">Spara säsongsdatum</asp:LinkButton>
    <div id="tmp" runat="server"></div>   
    <hr class="small">
</div>


<%--STÄNG BANAN--%>
  <div class="container" id="closecourse">
  <h2>Stäng eller öppna banan</h2>
  <p>Välj dagar & tider att stänga eller öppna.</p>

  <div class="row">
    <div class="col-sm-4 kalendersektion">
        <p>Välj första dagen banan ska vara stängd:</p>
        <asp:Calendar ID="Calendar3" runat="server" OnSelectionChanged="Calendar3_SelectionChanged" Height="162px" Width="100%"> </asp:Calendar>
        <p>Valda datum:</p>
        <asp:Label ID="lblfirstclosedday" runat="server" Text=""></asp:Label>
    </div>

        <div class="col-sm-4 kalendersektion">
        <p>Välj sista dagen banan ska vara stängd:</p>
        <asp:Calendar ID="Calendar4" runat="server" OnSelectionChanged="Calendar4_SelectionChanged" Height="162px" Width="100%"></asp:Calendar>
        <p>Valt datum:</p>
        <asp:Label ID="lbllastclosedday" runat="server" Text=""></asp:Label>
    </div>       
  </div>

    <div class="row">

    <div class="col-sm-3 kalendersektion">
        <asp:Label ID="Label1" runat="server" Text="Välj tid då banan öppnar"></asp:Label>
        <asp:DropDownList ID="dropfirstclose" runat="server"></asp:DropDownList>
    </div>

    <div class="col-sm-3 kalendersektion">
        <asp:Label ID="Label2" runat="server" Text="Välj tid då banan stänger"></asp:Label>
        <asp:DropDownList ID="droplastclose" runat="server"></asp:DropDownList>
    </div>  
  </div>

    <asp:LinkButton ID="LinkButton1" CssClass="btnadmin" Font-Underline="false" runat="server" OnClick="LinkButton1_Click">Stäng</asp:LinkButton>

    <div class="row">
    <div class="col-sm-8 kalendersektion">
        <p>Välj de dagar du vill öppna:</p>
        <asp:ListBox ID="listcloseddays" CssClass="listcloseddays" Height="162px" Width="100%" runat="server"></asp:ListBox>
    </div>  
 </div>  
    <asp:LinkButton ID="LinkButton2" CssClass="btnadmin" OnClick="LinkButton2_Click" Font-Underline="false" runat="server">Öppna</asp:LinkButton>
    <hr class="small">
</div>


<%--VÄLJ BANANS ÖPPETIDER--%>
    <div class="container" id="openhours">
         <h2>Dagliga öppetider</h2>
          <p>Välj när banan öppnar och stänger.</p> 
  
  <div class="row">
    <div class="col-sm-3 kalendersektion">
        <asp:Label ID="open" runat="server" Text="Välj tid då banan öppnar"></asp:Label>
        <asp:DropDownList ID="openhour" runat="server"></asp:DropDownList>
    </div>
    <div class="col-sm-3 kalendersektion">
        <asp:Label ID="close" runat="server" Text="Välj tid då banan stänger"></asp:Label>
        <asp:DropDownList ID="closehour" runat="server"></asp:DropDownList>
    </div>        
  </div>
    <asp:LinkButton ID="setOpenClose" OnClick="setOpenClose_Click" CssClass="btnadmin" Font-Underline="false" runat="server">Spara öppetider</asp:LinkButton>       
    <hr class="small">
</div>



</asp:Content>