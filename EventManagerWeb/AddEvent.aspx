<%@ Page Title="Add a new event" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AddEvent.aspx.cs" Inherits="AddEvent" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <p class="lead">Create a new event here.</p>
    </div>

    <div class="row">
    <%-- EVENT details --%>
        <div class="col-md-4">
            <table>
                <tr><td><b>Event details</b></td></tr>
                <tr>
                    <td><b>Event date</b></td>
                    <td>
                        <div class="input-group date" id="dtpEventDate">
                            <asp:TextBox ID="dtpBoxEventDate" CssClass="form-control" runat="server" ClientIDMode="Static" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td><b>Door time</b></td>
                    <td>
                        <div class="input-group date" id="dtpDoorTime">
                            <asp:TextBox ID="dtpBoxDoorTime" CssClass="form-control" runat="server" ClientIDMode="Static" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td><b>Door time</b></td>
                    <td>
                        <div class="input-group date" id="dtpCurfewTime">
                            <asp:TextBox ID="dtpBoxCurfewTime" CssClass="form-control" runat="server" ClientIDMode="Static" />
                            <span class="input-group-addon">
                                <span class="glyphicon glyphicon-time"></span>
                            </span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td><b>Promoter Charge</b></td>
                    <td>
                        <asp:TextBox ID="tbPromoterCharge" type="number" CssClass="form-control" runat="server" MaxLength="100" />
                    </td>
                </tr>
                <tr>
                    <td><b>Security Cost</b></td>
                    <td>
                        <asp:TextBox ID="tbSecurityCost" type="number" CssClass="form-control" runat="server" MaxLength="100" />
                    </td>
                </tr>
                <tr>
                    <td><b>Sound Cost</b></td>
                    <td>
                        <asp:TextBox ID="tbSoundCost" type="number" CssClass="form-control" runat="server" MaxLength="100" />
                    </td>
                </tr>
                <tr>
                    <td><b>Light Cost</b></td>
                    <td>
                        <asp:TextBox ID="tbLightCost" type="number" CssClass="form-control" runat="server" MaxLength="100" />
                    </td>
                </tr>
            </table>
        </div>

        <%-- LINE UP details --%>
        <div class="col-md-4">
            <table>
                <tr><td><b>Line up</b></td></tr>
                <tr>
                    <td><b>Headline</b></td>
                    <td>
                        <asp:TextBox ID="tbHeadline" type="text" CssClass="form-control" runat="server" MaxLength="100" />
                    </td>
                </tr>
                <tr>
                    <td><b>Support</b></td>
                    <td>
                        <asp:TextBox ID="tbSupport" type="text" CssClass="form-control" runat="server" MaxLength="100" />
                    </td>
                </tr>
                <tr>
                    <td><b>Opener</b></td>
                    <td>
                        <asp:TextBox ID="tbOpener" type="text" CssClass="form-control" runat="server" MaxLength="100" />
                    </td>
                </tr>
            </table>
        </div>

        <%-- PROMOTER details --%>
        <div class="col-md-4">
            <table>
                <tr><td><b>Select/add promoter.</b></td></tr>
                <tr>
                    <td><b>Select a promoter</b></td>
                    <td>
                        <asp:DropDownList ID="ddlPromoters" AutoPostBack="true" OnSelectedIndexChanged="ddlPromoters_SelectedIndexChanged" CssClass="form-control" DataSourceID="odsPromoters" AppendDataBoundItems="true" DataTextField="ContactName" DataValueField="PromoterId" runat="server">
                            <asp:ListItem Selected="True" Value="-1">Create new promoter</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td><b>Promoter Name</b></td>
                    <td>
                        <asp:TextBox ID="tbContactName" type="text" CssClass="form-control" runat="server" MaxLength="100" />
                    </td>
                </tr>
                <tr>
                    <td><b>Promoter Company</b></td>
                    <td>
                        <asp:TextBox ID="tbCompanyName" type="text" CssClass="form-control" runat="server" MaxLength="100" />
                    </td>
                </tr>
                <tr>
                    <td><b>Promoter Phone</b></td>
                    <td>
                        <asp:TextBox ID="tbPhone" type="text" CssClass="form-control" runat="server" MaxLength="100" />
                    </td>
                </tr>
                <tr>
                    <td><b>Promoter Email</b></td>
                    <td>
                        <asp:TextBox ID="tbEmail" type="email" CssClass="form-control" runat="server" MaxLength="100" />
                    </td>
                </tr>
            </table>
        </div>

        <%-- BUTTONS create + cancel --%>
        <div class="col-md-12">
            <br />
            <br />
        </div>
        <div class="col-md-12">
            <asp:LinkButton ID="bCreate" Text="Create new event" CssClass="btn btn-default" OnClick="bCreate_Click" runat="server" />
            <asp:LinkButton ID="bCancel" Text="Cancel" CssClass="btn btn-default" OnClick="bCancel_Click" runat="server" />
        </div>
    </div>
    <br />

    <%-- Promoter drop downlist data source --%>
    <asp:ObjectDataSource ID="odsPromoters" SelectMethod="GetAllPromotersSorted" TypeName="EventManager.Business.Promoter" runat="server" />

    <%-- DateTimePicker javascript --%>
    <script type="text/javascript">
        $(function () {
            $('#dtpEventDate').datetimepicker({ format: 'DD/MM/YYYY'});
            $('#dtpDoorTime').datetimepicker({ format: 'HH:mm', useCurrent: false, defaultDate: "01/01/1900 19:30:00" });
            $('#dtpCurfewTime').datetimepicker({ format: 'HH:mm', useCurrent: false, defaultDate: "01/01/1900 23:00:00" });
            });
    </script>

</asp:Content>