<%@ Page Title="Employee Dashboard" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="EmployeeDashboard.aspx.cs" Inherits="WebApplication2.WebForm1" %>

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
        .button-container {
            float: right;
            margin-bottom: 20px;
        }
        .button-container .btn {
            margin-left: 10px;
            padding: 8px 16px;
            font-size: 14px;
            cursor: pointer;
            border: none;
            border-radius: 4px;
            background-color: #007bff;
            color: white;
        }
        .button-container .btn-danger {
            background-color: #dc3545;
        }
        .auto-style1 {
            width: 398px;
        }
        .auto-style2 {
            width: 405px;
        }
        .auto-style3 {
            width: 405px;
            height: 41px;
        }
        .auto-style4 {
            height: 41px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="dashboard-container">
        <div class="button-container">
    <asp:Button ID="btnFileLeave" runat="server" Text="File a Leave" CssClass="btn" OnClick="btnFileLeave_Click" style="background-color: #CF182C; margin-left: 10px;" Visible="false" />
    <asp:Button ID="btnViewLeave" runat="server" Text="View Leave Status" CssClass="btn" OnClick="btnViewLeave_Click" style="background-color: #0C0C3B" Visible="false" Width="181px"/>
    <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" CssClass="btn btn-danger" OnClick="btnChangePassword_Click" style="background-color: #CF182C;" Width="196px" />
    <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="btn btn-danger" OnClick="btnLogout_Click" style="background-color: #0C0C3B" Width="102px"/>

        </div>

        <div class="welcome-text">
            Welcome, <asp:Label ID="lblEmployeeName" runat="server" Text="Employee Name"></asp:Label>!
        </div>
        <br />
        <h3>Employee Information</h3>
        <table class="info-table">
            <tr>
                <th class="auto-style1">Employee ID</th>
                <td><asp:Label ID="lblEmployeeID" runat="server" /></td>
            </tr>
            <tr>
                <th class="auto-style1">Full Name</th>
                <td><asp:Label ID="lblFullName" runat="server" /></td>
            </tr>
            <tr>
                <th class="auto-style1">Department</th>
                <td><asp:Label ID="lblDepartment" runat="server" /></td>
            </tr>
            <tr>
                <th class="auto-style1"">Role</th>
                <td><asp:Label ID="lblRole" runat="server" /></td>
            </tr>
            <tr>
                <th class="auto-style1">Supervisor Employee ID</th>
                <td><asp:Label ID="lblSupervisorID" runat="server" /></td>
            </tr>
            <tr>
                <th class="auto-style1">Supervisor Name</th>
                <td><asp:Label ID="lblSupervisorName" runat="server" /></td>
            </tr>
        </table>
        <br />

        <asp:Panel ID="pnlWorkSchedule" runat="server">
        <h3>Work Schedule Overview</h3>
        <table class="info-table">
            <tr>
                <th class="auto-style3">Schedule ID</th>
                <td class="auto-style4"><asp:Label ID="lblScheduleID" runat="server" /></td>
            </tr>
            <tr>
                <th class="auto-style2">Workdays</th>
                <td><asp:Label ID="lblWorkdays" runat="server" /></td>
            </tr>
            <tr>
                <th class="auto-style1">Leave Balance</th>
                <td><asp:Label ID="lblLeaveBalance" runat="server" /></td>
            </tr>
            <tr>
                <th class="auto-style2">Shift Period</th>
                <td><asp:Label ID="lblShiftPeriod" runat="server" /></td>
            </tr>
            <tr>
                <th class="auto-style2">Break Period</th>
                <td><asp:Label ID="lblBreakPeriod" runat="server" /></td>
            </tr>
            <tr>
                <th class="auto-style2">Lunch Period</th>
                <td><asp:Label ID="lblLunchPeriod" runat="server" /></td>
            </tr>
        </table>
        </asp:Panel>
    </div>
</asp:Content>