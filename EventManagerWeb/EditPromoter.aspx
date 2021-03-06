﻿<%@ Page Title="Change promoter" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="EditPromoter.aspx.cs" Inherits="EditPromoter" %>

<asp:Content ID="EditPromoterContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <p class="lead">ATTENTION, if you modify this promoter, all the events with this promoter will be affected.</p>
    </div>

    <%--promoter details--%>
    <div class="row">
        <div class="col-md-12">
            <table>
                <tr><td><b>Change the promoter or make changes to the promoter.</b></td></tr>
                <tr><td><br /></td></tr>
                <tr>
                    <td><b>Select a promoter</b></td>
                    <td>
                        <asp:DropDownList ID="ddlPromoters" AutoPostBack="true" OnInit="ddlPromoters_Init" OnSelectedIndexChanged="ddlPromoters_SelectedIndexChanged" CssClass="form-control" DataSourceID="odsPromoterList" AppendDataBoundItems="true" DataTextField="ContactName" DataValueField="PromoterId" runat="server">
                            <asp:ListItem Selected="True" Value="-1">Edit current promoter</asp:ListItem>
                            <asp:ListItem Value ="-2">Add a new promoter</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td><b>Promoter Name</b></td>
                    <td>
                        <asp:TextBox ID="tbContactName" CssClass="form-control" runat="server" MaxLength="100" />
                    </td>
                </tr>
                <tr>
                    <td><b>Promoter Company</b></td>
                    <td>
                        <asp:TextBox ID="tbCompanyName" CssClass="form-control" runat="server" MaxLength="100" />
                    </td>
                </tr>
                <tr>
                    <td><b>Promoter Phone</b></td>
                    <td>
                        <asp:TextBox ID="tbPhone" CssClass="form-control" runat="server" MaxLength="100" />
                    </td>
                </tr>
                <tr>
                    <td><b>Promoter Email</b></td>
                    <td>
                        <asp:TextBox ID="tbEmail" CssClass="form-control" runat="server" MaxLength="100" />
                    </td>
                </tr>
                <tr>
                    <td><asp:LinkButton ID="bUpdate" runat="server" Text="Update" OnClick="bUpdate_Click" CssClass="btn btn-default" /></td>
                    <td><asp:LinkButton ID="bCancel" runat="server" Text="Cancel" OnClick="bCancel_Click" CssClass="btn btn-default" /></td>
                </tr>
            </table>
        </div>
    </div>

    <%-- Promoter datasource --%>
    <asp:ObjectDataSource ID="odsPromoterList" SelectMethod="GetAllPromotersSorted" TypeName="EventManager.Business.Promoter" runat="server" />

</asp:Content>