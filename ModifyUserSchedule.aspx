<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="ModifyUserSchedule.aspx.cs" Inherits="WebApplication2.WebForm13" %>
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="info-table">
        <br />
             <h3>Available Work Schedules</h3>
<asp:GridView ID="gvWorkSchedules" runat="server" AutoGenerateColumns="False" CssClass="info-table">
    <Columns>
        <asp:BoundField DataField="ScheduleID" HeaderText="Schedule ID" />
        <asp:BoundField DataField="Workdays" HeaderText="Work Days" />
        <asp:BoundField DataField="ShiftPeriod" HeaderText="Shift Period" />
        <asp:BoundField DataField="BreakPeriod" HeaderText="Break Period" />
        <asp:BoundField DataField="LunchPeriod" HeaderText="Lunch Period" />
    </Columns>
    <EmptyDataTemplate>
        <div style="text-align: center; padding: 20px;">
            <asp:Label ID="lblEmptyMessage" runat="server" Text="No work schedules available." Font-Italic="true" ForeColor="Gray"></asp:Label>
        </div>
    </EmptyDataTemplate>
</asp:GridView>
        <br />
<h3>Employee Records</h3>
<br />
<asp:GridView ID="gvAccounts" runat="server" AutoGenerateColumns="False" CssClass="info-table">
    <Columns>
    <asp:BoundField DataField="EmployeeID" HeaderText="Employee ID" />
    <asp:BoundField DataField="FullName" HeaderText="Name" />
    <asp:BoundField DataField="Department" HeaderText="Department" />
        <asp:BoundField DataField="Role" HeaderText="Role" />
    <asp:BoundField DataField="ScheduleID" HeaderText="ScheduleID"  />
    </Columns>
    <EmptyDataTemplate>
        <div style="text-align: center; padding: 20px;">
    <asp:Label ID="lblEmptyMessage" runat="server" Text="Oooo, so empty~" Font-Italic="true" ForeColor="Gray"></asp:Label>
</div>
    </EmptyDataTemplate>
</asp:GridView>
<br />
                <h3>Manage a User Account</h3>
                <table class="info-table">
    <!-- Your existing table structure -->
    <tr>
        <th class="auto-style1">EmployeeID</th>
        <td class="auto-style3">
            <asp:DropDownList ID="ddlEmployeeID" runat="server" Height="29px" Width="200px" 
                AutoPostBack="True" OnSelectedIndexChanged="ddlEmployeeID_SelectedIndexChanged">
                <asp:ListItem Text="Select an Employee Number" Value="" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvemployeeid" runat="server" 
                ControlToValidate="ddlEmployeeID" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <!-- Rest of your labels -->
    <tr><th class="auto-style1">Employee Name</th><td><asp:Label ID="lblEmployeeName" runat="server" /></td></tr>
    <tr><th class="auto-style1">Department</th><td><asp:Label ID="lblDepartment" runat="server" /></td></tr>
    <tr><th class="auto-style1">Role</th><td><asp:Label ID="lblRole" runat="server" /></td></tr>
    <tr><td class="auto-style1">Schedule ID</td><td><asp:TextBox ID="txtScheduleID" runat="server" /></td></tr>
        
</table>
        <div style="text-align: center; margin-top: 20px;">
    <asp:Button ID="btnCreate" runat="server" CssClass="btn-primary" Text="Update Employee Schedule" OnClick="btnCreate_Click"  OnClientClick="return confirm('Are you sure you want to save the changes?');" />
</div>


    </div>
</asp:Content>
