<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="about-title"><%: Title %> Event Manager</h2>
    <p class="about-p">Thanks for checking out this app. To find out about me or my other applications, please visit <asp:HyperLink ID=tangic NavigateUrl="http://www.tangic.co.uk" runat="server">www.tangic.co.uk</asp:HyperLink></p>
</asp:Content>
