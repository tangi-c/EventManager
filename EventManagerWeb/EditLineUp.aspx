<%@ Page Title="Change line up" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="EditLineup.aspx.cs" Inherits="EditLineUp" %>

<asp:Content ID="EditLineUpContent" ContentPlaceHolderID="MainContent" Runat="Server">

    <div class="jumbotron">
        <p class="lead">You can edit the line up for this event here.</p>
    </div>

    <%-- Line up details --%>
    <asp:FormView ID="fvLineUp" DataSourceID="odsLineUp" runat="server">
        <ItemTemplate>
            <div class="row">
                <div class="col-md-12">
                    <table>
                        <tr>
                            <td><b>Headline</b></td>
                            <td>
                                <asp:TextBox ID="tbHeadline" CssClass="form-control" runat="server" Text='<%#Bind("Headline") %>' MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <td><b>Support</b></td>
                            <td>
                                <asp:TextBox ID="tbSupport" CssClass="form-control" runat="server" Text='<%#Bind("Support") %>' MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <td><b>Opener</b></td>
                            <td>
                                <asp:TextBox ID="tbOpener" CssClass="form-control" runat="server" Text='<%#Bind("Opener") %>' MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td><asp:TextBox ID="tbLineUpId" runat="server" Text='<%#Bind("LineUpId") %>' Enabled="false" Visible="false" /></td>
                        </tr>
                        <tr>
                            <td><asp:LinkButton ID="bUpdate" runat="server" Text="Update" OnClick="bUpdate_Click" CssClass="btn btn-default" /></td>
                            <td><asp:LinkButton ID="bCancel" runat="server" Text="Cancel" OnClick="bCancel_Click" CssClass="btn btn-default" /></td>
                        </tr>
                    </table>
                </div>
            </div>
        </ItemTemplate>
    </asp:FormView>

    <%-- Lineup datasource --%>
    <asp:ObjectDataSource ID="odsLineUp" runat="server" SelectMethod="GetLineUpByEventId" TypeName="EventManager.Business.LineUp">
        <SelectParameters>
            <asp:SessionParameter Name="eventId" SessionField="EventId_LineUp" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>

</asp:Content>