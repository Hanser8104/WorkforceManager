<%@ Page Title="File Employee Leave Request" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="FileEmployeeLeave.aspx.cs" Inherits="WebApplication2.WebForm3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .dashboard-container {
            padding: 20px;
            font-family: Arial, sans-serif;
        }
        .welcome-text {
            font-size: x-large;
            font-weight: bold;
            margin-bottom: 20px;
        }
        .info-table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
        }
        .info-table th, .info-table td {
            border: 1px solid #ddd;
            padding: 10px;
            text-align: left;
        }
        .info-table th {
            background-color: #f2f2f2;
        }
        .form-table {
            width: 100%;
            margin-bottom: 20px;
        }
        .form-table td {
            padding: 10px;
            text-align: left;
        }
        .form-table th {
            padding: 10px;
            text-align: left;
            background-color: #f2f2f2;
        }
        .btn-primary {
            background-color: #CF182C;
            color: #FFFFFF;
            border: none;
            padding: 10px 20px;
            cursor: pointer;
            font-size: 16px;
        }
        .btn-primary:hover {
            background-color: #A81424;
        }
        .auto-style4 {
        width: 273px;
    }
        .auto-style5 {
            width: 200px;
        }
    </style>
   <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $("#<%= txtStartDate.ClientID %>").change(function () {
            var selectedDate = $(this).val();
            if (selectedDate) {
                $.ajax({
                    type: "POST",
                    url: "FileEmployeeLeave.aspx/GetWorkforceLoad",
                    data: JSON.stringify({ date: selectedDate }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        // Parse the JSON response
                        var result = JSON.parse(response.d);
                        
                        // Update the label with the demand level and color
                        $("#<%= lblWorkforceLoad.ClientID %>")
                            .text(result.DemandLevel)
                            .css("color", result.Color);
                    },
                    error: function () {
                        alert("Error fetching workforce demand.");
                    }
                });
            }
        });
    });
</script>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="dashboard-container">
        

        <!-- Leave Balance -->
        <table class="info-table">
            <tr>
                <th class="auto-style4">Leave Balance</th>
                <td><asp:Label ID="lblLeaveBalance" runat="server" Text="Loading..."></asp:Label></td>
            </tr>
        </table>
        <br />                    
        <!-- Employee Information -->
        <h3>Employee Information</h3>
        <table class="info-table">
            <tr>
                <th class="auto-style1">Employee ID</th>
                <td class="auto-style1">
                    <asp:Label ID="lblEmployeeID" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <th class="auto-style5">Department</th>
                <td><asp:Label ID="lblDepartment" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <th class="auto-style5">Supervisor ID</th>
                <td><asp:Label ID="lblSupervisor" runat="server"></asp:Label></td>
            </tr>
        </table>
        <br />
        <h3>Workforce Load</h3>
        <table class="info-table">
            <tr>
                <th class="auto-style4">Selected Date Demand Level</th>
                <td><asp:Label ID="lblWorkforceLoad" runat="server" Text="Select a date"></asp:Label></td>
            </tr>
        </table>
        <br />
        <!-- Leave Request Details -->
        <h3>Leave Request Details</h3>
<table class="info-table">
    <tr>
        <th>Type of Leave</th>
        <td>
            <asp:DropDownList ID="ddlLeaveType" runat="server">
                <asp:ListItem Text="Select Leave Type" Value="" />
                <asp:ListItem Text="Sick Leave" Value="Sick" />
                <asp:ListItem Text="Vacation Leave" Value="Vacation" />
                <asp:ListItem Text="Maternity Leave" Value="Maternity" />
                <asp:ListItem Text="Casual Leave" Value="Casual" />
                <asp:ListItem Text="Solo Parent Leave" Value="SPL" />
                <asp:ListItem Text="Compassionate Leave" Value="Casual" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="ddlLeaveTypeRFV" runat="server" ControlToValidate="ddlLeaveType" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>Start Date & Time</th>
        <td>
            <asp:TextBox ID="txtStartDate" runat="server" TextMode="Date"></asp:TextBox>
            <asp:TextBox ID="txtStartTime" runat="server" TextMode="Time"></asp:TextBox>
            <asp:RequiredFieldValidator ID="startdateRFV" runat="server" ControlToValidate="txtStartDate" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="starttimeRFV" runat="server" ControlToValidate="txtStartTime" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>End Date & Time</th>
        <td>
            <asp:TextBox ID="txtEndDate" runat="server" TextMode="Date"></asp:TextBox>
            <asp:TextBox ID="txtEndTime" runat="server" TextMode="Time"></asp:TextBox>
            <asp:RequiredFieldValidator ID="enddateRFV" runat="server" ControlToValidate="txtEndDate" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="endtimeRFV" runat="server" ControlToValidate="txtEndTime" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>Reason</th>
        <td>
            <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Rows="4" Columns="50"></asp:TextBox>
        </td>
    </tr>
</table>


        <!-- Submit Button -->
        <div style="text-align: center; margin-top: 20px;">
            <asp:Button ID="Button1" runat="server" Text="Submit Leave Request" OnClick="btnSubmit_Click" CssClass="btn-primary" Height="36px" Width="204px" OnClientClick="return confirm('Are you sure you want to submit this request?');"/>
        </div>
    </div>
</asp:Content>