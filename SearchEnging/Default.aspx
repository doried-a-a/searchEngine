<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="SearchEnging._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to Super Search Engine
    </h2>
    Enter your text to search for here:<br />
    <asp:Panel ID="Panel1" runat="server" Height="30px">
        <asp:TextBox ID="txtSearch" runat="server" Height="29px" Style="margin-bottom: 16px"
            Width="394px">
        </asp:TextBox>
    </asp:Panel>
    <br />
    &nbsp;&nbsp;
    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
    <br />
    <br />
    <asp:Label ID="lblResultsCount" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
    <br />
   <div style="text-align:right" dir="rtl">
        <asp:Label  id="lblResults" runat="server" Font-Names="Arial"  >
        
        </asp:Label>
    </div>
    <br />
    <br />
</asp:Content>
