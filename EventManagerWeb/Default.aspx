<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <%-- Add event and search box --%>
    <div class="jumbotron" style="background-image:url(../P1010769.jpg)">
        <div class="row align-items-center">
            <div class="col-md-6">
                <p style="color:white; font-size:large">Add an event to your calendar.</p>
                <p><a href="AddEvent.aspx" class="btn btn-primary btn-lg">New event &raquo;</a></p>
            </div>
            <div class="col-md-6">
                <asp:Panel ID="pSearch" DefaultButton="bSearch" runat="server">
                    <p style="color:white; font-size:large">Search through the event list.</p>
                    <p><asp:TextBox ID="tbSearch" CssClass="form-control" runat="server" MaxLength="100" OnPreRender="tbSearch_PreRender" /></p>
                    <p><asp:LinkButton ID="bSearch" runat="server" OnClick="bSearch_Click" CssClass="btn btn-primary btn-lg">Search events &raquo;</asp:LinkButton></p>
                </asp:Panel>
            </div>
        </div>
    </div>

    <%-- displaying the events --%>
    <div>
        <asp:ListView
            ID="lvEvents"
            DataSourceID="odsSearchEvents"
            DataKeyNames="EventId"
            runat="server">

            <%-- list layout --%>
            <LayoutTemplate>
                <div class="row">
                    <asp:PlaceHolder ID="itemPlaceHolder" runat="server"/>
                </div>
            <%-- Pager. ALWAYS PLACE INSIDE LAYOUT TEMPLATE TO AVOID PROBLEMS --%>
                <asp:DataPager OnLoad="dpEvents_Load" runat="server" ID="dpEvents" PageSize="9">
                    <Fields>
                        <asp:NextPreviousPagerField
                            ButtonType="Button"
                            ShowFirstPageButton="true"
                            ShowLastPageButton="true"
                            ShowNextPageButton="true"
                            ShowPreviousPageButton="true"
                            ButtonCssClass="btn btn-default"
                            FirstPageText="<<"
                            PreviousPageText="<"
                            NextPageText=">"
                            LastPageText=">>" />
                        <asp:NumericPagerField
                            NextPreviousButtonCssClass="btn btn-default"
                            ButtonType="Button"
                            NumericButtonCssClass="btn btn-default"
                            CurrentPageLabelCssClass="btn btn-default"
                            ButtonCount="5" 
                            PreviousPageText="<"
                            NextPageText=">" />
                    </Fields>
                </asp:DataPager>
            </LayoutTemplate>

            <%-- item layout --%>
            <ItemTemplate>
                <div class="col-md-4">
                    <table>
                        <tr>
                            <td><b>Date</b></td>
                            <td><asp:Label id="lEventDate" runat="server" Text='<%# Eval("EventDate", "{0:dd/MM/yyyy}") %>' /></td>
                        </tr>
                        <tr>
                            <td><b>Door time</b></td>
                            <td><asp:Label id="lDoorTime" runat="server" Text='<%# Eval("DoorTime", "{0:hh\\:mm}") %>' /></td>
                        </tr>
                        <tr>
                            <td><b>Curfew time</b></td>
                            <td><asp:Label id="lCurfewTime" runat="server" Text='<%# Eval("CurfewTime", "{0:hh\\:mm}") %>' /></td>
                        </tr>
                        <tr>
                            <td><b>Promoter's fee</b></td>
                            <td><asp:Label id="lPromoterCharge" runat="server" Text='<%# Eval("PromoterCharge") %>' /></td>
                        </tr>
                        <tr>
                            <td><b>Security cost</b></td>
                            <td><asp:Label id="lSecurityCost" runat="server" Text='<%# Eval("SecurityCost") %>' /></td>
                        </tr>
                        <tr>
                            <td><b>Soundman's charge</b></td>
                            <td><asp:Label id="lSoundCost" runat="server" Text='<%# Eval("SoundCost") %>' /></td>
                        </tr>
                        <tr>
                            <td><b>Lighting Cost</b></td>
                            <td><asp:Label id="lLightCost" runat="server" Text='<%# Eval("LightCost") %>' /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Label ID="lEventId" runat="server" Text='<%# Eval("EventId") %>' Enabled="false" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td><b><asp:Label id="lPromoterDesc" runat="server">Promoter</asp:Label></b></td>
                            <td><asp:Label id="lPromoterName" runat="server" Text='<%# Eval("EventPromoter.ContactName") %>' OnPreRender="lPromoterName_PreRender" /></td>
                        </tr>
                        <tr>
                            <td><b><asp:Label id="lPromoterEmailDesc" runat="server">Promoter's email</asp:Label></b></td>
                            <td><asp:HyperLink id="hlPromoterEmail" runat="server" Text='<%# Eval("EventPromoter.Email") %>' OnPreRender="hlPromoterEmail_PreRender" /></td>
                        </tr>
                        <tr>
                            <td><b><asp:Label id="lHeadlineDesc" runat="server">Headline</asp:Label></b></td>
                            <td><asp:Label id="lHeadline" runat="server" Text='<%# Eval("EventLineUp.Headline") %>' OnPreRender="lHeadline_PreRender" /></td>
                        </tr>
                    </table>
                    <asp:LinkButton ID="bEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-default" />
                    <hr />
                </div>
            </ItemTemplate>

            <%-- layout for editing item --%>
            <EditItemTemplate>
                <div class="col-md-4">
                    <table>
                        <tr>
                            <td><b>Event date</b></td>
                            <td>
                                <div class="input-group date" id="dtpEventDate">
                                    <asp:TextBox ID="dtpBoxEventDate" CssClass="form-control" runat="server" Text='<%#Bind("EventDate", "{0:dd/MM/yyyy}")%>' ClientIDMode="Static" />
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
                                    <asp:TextBox ID="dtpBoxDootTime" CssClass="form-control" runat="server" Text='<%#Bind("DoorTime")%>' ClientIDMode="Static" />
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
                                    <asp:TextBox ID="dtpBoxCurfewTime" CssClass="form-control" runat="server" Text='<%#Bind("CurfewTime")%>' ClientIDMode="Static" />
                                    <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-time"></span>
                                    </span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td><b>Promoter Charge</b></td>
                            <td>
                                <asp:TextBox ID="tbPromoterCharge" type="number" CssClass="form-control" runat="server" Text='<%#Bind("PromoterCharge") %>' MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <td><b>Security Cost</b></td>
                            <td>
                                <asp:TextBox ID="tbSecurityCost" type="number" CssClass="form-control" runat="server" Text='<%#Bind("SecurityCost") %>' MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <td><b>Sound Cost</b></td>
                            <td>
                                <asp:TextBox ID="tbSoundCost" type="number" CssClass="form-control" runat="server" Text='<%#Bind("SoundCost") %>' MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <td><b>Light Cost</b></td>
                            <td>
                                <asp:TextBox ID="tbLightCost" type="number" CssClass="form-control" runat="server" Text='<%#Bind("LightCost") %>' MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:TextBox ID="tbEventId" runat="server" Text='<%#Bind("EventId") %>' Enabled="false" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td><asp:LinkButton ID="bUpdate" runat="server" CommandName="Update" Text="Update" CssClass="btn btn-default" /></td>
                            <td>
                                <asp:LinkButton ID="bCancel" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-default" />&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="dDelete" runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-default" />
                            </td>
                        </tr>
                        <tr>
                            <td><asp:LinkButton ID="bLineUp" runat="server" OnClick="bLineUp_Click" Text="Edit line up" CssClass="btn btn-default" /></td>
                            <td><asp:LinkButton ID="bPromoter" runat="server" OnClick="bPromoter_Click" Text="Edit promoter" CssClass="btn btn-default" /></td>
                        </tr>
                    </table>
                    <hr />

                    <%-- DateTimePicker javascript --%>
                    <script type="text/javascript">
                        $(function () {
                            $('#dtpEventDate').datetimepicker({ format: 'DD/MM/YYYY'});
                            $('#dtpDoorTime').datetimepicker({ format: 'HH:mm', useCurrent: false });
                            $('#dtpCurfewTime').datetimepicker({ format: 'HH:mm', useCurrent: false });
                            });
                    </script>
                </div>
            </EditItemTemplate>

            <ItemSeparatorTemplate>
            </ItemSeparatorTemplate>
        </asp:ListView>

        <%-- Event datasource --%>
        <asp:ObjectDataSource ID="odsAllEvents" runat="server" 
                              SelectMethod="GetAllEventsSorted" 
                              UpdateMethod="UpdateEvent"
                              TypeName="EventManager.Business.Event">
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsSearchEvents" runat="server" 
                              SelectMethod="SearchEvents" 
                              UpdateMethod="UpdateEvent"
                              DeleteMethod="DeleteEvent"
                              TypeName="EventManager.Business.Event">
            <SelectParameters>
                <asp:QueryStringParameter Name="query" QueryStringField="Search" />
            </SelectParameters>
            <DeleteParameters>
                <asp:FormParameter Name="EventId" Type="Int32" />
            </DeleteParameters>
        </asp:ObjectDataSource>


    </div>

</asp:Content>
