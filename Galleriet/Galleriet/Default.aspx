<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Galleriet.Default" ViewStateMode="Disabled"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Galleriet</title>
    <link href="Main.css" rel="stylesheet" />
    <script src="Main.js"></script>
</head>
<body>
    <h1>Galleriet</h1>
    <form id="form1" runat="server">
        <div>
            <asp:Image ImageUrl="imageurl" ID="Largeimage" runat="server" />
            <div>
                <a href="#"><asp:Image runat="server" /></a>
                <a href="#"><asp:Image runat="server" /></a>
                <a href="#"><asp:Image runat="server" /></a>
                <a href="#"><asp:Image runat="server" /></a>
                <a href="#"><asp:Image runat="server" /></a>
                <a href="#"><asp:Image runat="server" /></a>
                <a href="#"><asp:Image runat="server" /></a>
                <a href="#"><asp:Image runat="server" /></a>
                <a href="#"><asp:Image runat="server" /></a>
                <a href="#"><asp:Image runat="server" /></a>
                <a href="#"><asp:Image runat="server" /></a>
            </div>
        </div>
    <div>
        <asp:TextBox runat="server" ID="QueryStringLabel" TextMode="MultiLine" Width="800" Height="800" />
        <br />
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Button Text="Ladda upp" ID="UploadButton" runat="server" OnClick="UploadButton_Click" />
    </div>
    </form>
</body>
</html>
