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
            <asp:Panel runat="server">
                <asp:HyperLink NavigateUrl="navigateurl" runat="server">
                    <asp:Image ImageUrl="imageurl" runat="server" />
                </asp:HyperLink>
                <asp:HyperLink NavigateUrl="navigateurl" runat="server">
                    <asp:Image ImageUrl="imageurl" runat="server" />
                </asp:HyperLink>
                <asp:HyperLink NavigateUrl="navigateurl" runat="server">
                    <asp:Image ImageUrl="imageurl" runat="server" />
                </asp:HyperLink>
                <asp:HyperLink NavigateUrl="navigateurl" runat="server">
                    <asp:Image ImageUrl="imageurl" runat="server" />
                </asp:HyperLink>
                <asp:HyperLink NavigateUrl="navigateurl" runat="server">
                    <asp:Image ImageUrl="imageurl" runat="server" />
                </asp:HyperLink>
            </asp:Panel>
        </div>
    <div>
        <asp:TextBox runat="server" ID="QueryStringLabel" TextMode="MultiLine" Width="800" Height="800" />
        <asp:RequiredFieldValidator ErrorMessage="Du måste välja en fil först!" ControlToValidate="FileUpload1" runat="server" />
        <asp:RegularExpressionValidator ErrorMessage="errormessage" ControlToValidate="FileUpload1" runat="server" ValidationExpression="^.*\.(jpg|png|gif)$" />
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Button Text="Ladda upp" ID="UploadButton" runat="server" OnClick="UploadButton_Click" />
    </div>
    </form>
</body>
</html>
