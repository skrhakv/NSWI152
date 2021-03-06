﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NSWI152.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:FileUpload ID="MyFileUpload" runat="server" />
                <asp:Button ID="GoButton" Text="GO!" OnClick="GoButton_Click" runat="server" />
        </div>
         <h1>Files</h1>
         <asp:Repeater ID="FilesRepeater" ItemType="Microsoft.Azure.Storage.Blob.IListBlobItem" runat="server">
 	        <ItemTemplate>
 		        <asp:LinkButton ID="FileLink" CommandArgument="<%# Item.Uri %>" Text="<%# Item.Uri %>" OnCommand="FileLink_Command" runat="server" /><br />
 	        </ItemTemplate>
         </asp:Repeater>
        <asp:TextBox ID="QueueMessageTB" runat="server" />
        <asp:Button ID="SendToQueueButton" Text="Send to Queue" OnClick="SendToQueueButton_Click" runat="server" />

        <asp:Label ID="QueueMessageLb" runat="server" />
        <asp:Button ID="GetMessageButton" Text="Get from Queue" OnClick="GetMessageButton_Click" runat="server" />
    </form>
</body>
</html>
