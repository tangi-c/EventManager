<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" %>

<!DOCTYPE html>

<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%: Page.Title %> - Event Manager</title>

    <script type="text/javascript" src="/scripts/jquery.min.js"></script>
    <script type="text/javascript" src="/scripts/moment.min.js"></script>
    <script type="text/javascript" src="/scripts/bootstrap.min.js"></script>
    <script type="text/javascript" src="/scripts/bootstrap-datetimepicker.min.js"></script>
    <link rel="stylesheet/less" type="text/css" href="~/Content/bootstrap-datetimepicker-build.less" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/less.js/2.7.1/less.min.js" ></script>

</head>
<body>
    <form id="form1" runat="server">
    <div>

    <table>
        <tr><b>Event details</b></tr>
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
    </table>

    <script type="text/javascript">
                $(function () {
                    $('#dtpEventDate').datetimepicker({ format: 'DD/MM/YYYY'});
                    $('#dtpDoorTime').datetimepicker({ format: 'HH:mm', useCurrent: false });
                    $('#dtpCurfewTime').datetimepicker({ format: 'HH:mm', useCurrent: false });
                    });
    </script>

    
    </div>
    </form>
</body>
</html>
