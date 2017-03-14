<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mail.aspx.cs" Inherits="Golf2.Mail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br /> <br /> <br /> <br /> <br /> <br />
        <table align="center" width="60%">
            
            <tr>
                <td>From:</td>
                <td><asp:TextBox ID="fromTbx" runat="server" Width="99%"></asp:TextBox></td>
            </tr>
            <tr>
                <td>To:</td>
                <td><asp:TextBox ID="toTbx" runat="server" Width="99%"></asp:TextBox></td>
            </tr>         
             <tr>
                <td>Subject:</td>
                <td><asp:TextBox ID="subjectTbx" Text="Test" runat="server" Width="99%"></asp:TextBox></td>
            </tr>
             <tr>
                <td>Body:</td>
                <td><asp:TextBox ID="bodyTbx" Text="Body text" runat="server" Height="150px" TextMode="MultiLine" Width="99%"></asp:TextBox></td>
            </tr>
            
              <tr>
                <td>User:</td>
                <td><asp:TextBox ID="User" Text="User" runat="server" Width="30%"></asp:TextBox></td>
              </tr>
            <tr>
                 <td>Password:</td>
                <td><asp:TextBox ID="Password" Text="Password" runat="server" Width="30%"></asp:TextBox></td>
            </tr>
           <tr>



           </tr>
             <tr>
                <td></td>
                <td><asp:Button ID="sendBtn" onclick="sendBtn_Click" runat="server" Text="Send!" /></td>
            </tr>
             <tr>
                <td></td>
                <td><asp:Label ID="Status" runat="server" Text="Label"></asp:Label></td>
            </tr>
            


        </table>
    
    </div>
    </form>
</body>
</html>
