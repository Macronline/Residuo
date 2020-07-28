<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestPlugin.aspx.cs" Inherits="TestPlugin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <table style="width: 80%;">
            <tr>
                <td>Fecha Inicial</td>
                <td>Fecha Final</td>
                <td>Camión</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtFecIni" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtFecIniHora" runat="server"></asp:TextBox>
                </td>
                <td><asp:TextBox ID="txtFecFin" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtFecFinHora" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCamiones" runat="server" Height="16px" Width="159px">
                        <asp:ListItem Value="21174714">GV300W</asp:ListItem>
                        <asp:ListItem Value="21191347">HSZC-85</asp:ListItem>
                        <asp:ListItem Value="21193017">HSZC-86</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <asp:Button ID="Button1" runat="server" Text="Consultar Wialon" OnClick="Button1_Click" />
        <table>
            <tr>
                <td><asp:Image ID="Image2" runat="server" ImageUrl="~/images/LogoMyResiduo.png" Height="793px" Width="1107px" /></td>
                
            </tr>
            <tr>
                <td><asp:Image ID="Image1" runat="server" ImageUrl="~/images/LogoMyResiduo.png" /></td>
            </tr>
        </table>
        
    </form>
</body>
</html>