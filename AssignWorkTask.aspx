<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="AssignWorkTask.aspx.cs" Inherits="WebApplication2.WebForm9" %>
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
    <div class="dashboard-container">
        <h1 class="welcome-text">Assign Work Task</h1>
        <br />
        <h3>Active Employees</h3>
        <asp:GridView ID="gvEmployee" runat="server" AutoGenerateColumns="False" CssClass="info-table">
            <Columns>
                <asp:BoundField DataField="EmployeeID" HeaderText="Employee ID"  />
                    <asp:BoundField DataField="FullName" HeaderText="Full Name"  />
                    <asp:BoundField DataField="Department" HeaderText="Department"/>
                    <asp:BoundField DataField="SupervisorID" HeaderText="SupervisorID" />
                    <asp:BoundField DataField="LeaveBalance" HeaderText="Leave Balance" />
            </Columns>
        <EmptyDataTemplate>
            <div style="text-align: center; padding: 20px;">
                    <asp:Label ID="lblEmptyMessage" runat="server" Text="Oooo, so empty~" Font-Italic="true" ForeColor="Gray"></asp:Label>
            </div>
        </EmptyDataTemplate>
     </asp:GridView>
        <br />
        <h3>Assign Task to an Employee</h3>
        <table class="info-table">
    <!-- Your existing table structure -->
    <tr>
        <th class="auto-style2">EmployeeID</th>
        <td class="auto-style3">
            <asp:DropDownList ID="ddlEmployeeID" runat="server" Height="29px" Width="191px" 
                AutoPostBack="True" OnSelectedIndexChanged="ddlEmployeeID_SelectedIndexChanged">
                <asp:ListItem Text="Select an Employee Number" Value="" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvrequestid" runat="server" 
                ControlToValidate="ddlEmployeeID" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <!-- Rest of your labels -->
    <tr><th class="auto-style1">Employee Name</th><td><asp:Label ID="lblEmployeeName" runat="server" /></td></tr>
    <tr><th class="auto-style1">Schedule ID</th><td><asp:Label ID="lblScheduleID" runat="server" /></td></tr>
    <tr><th class="auto-style1">Department</th><td><asp:Label ID="lblDepartment" runat="server" /></td></tr>
    <tr><th class="auto-style1">Leave Balance</th><td><asp:Label ID="lblLeaveBalance" runat="server" /></td></tr>
    <tr><th class="auto-style1">Work Days</th><td><asp:Label ID="lblWorkDays" runat="server" /></td></tr>
    <tr><th class="auto-style1">Shift Period</th><td><asp:Label ID="lblShiftPeriod" runat="server" /></td></tr>
    <tr>
        <th class="auto-style1">Task Details</th>
        <td>
            <asp:TextBox ID="txtSupervisorRemark" TextMode="MultiLine" Rows="4" Columns="50" runat="server" Height="98px" Width="554px"></asp:TextBox>
        </td>
    </tr>
</table>
        <div style="text-align: center; margin-top: 20px;">
    <asp:Button ID="btnAssign" runat="server" Text="Assign" CssClass="btn-primary" OnClick="btnSave_Click" OnClientClick="return confirm('Are you sure you want to save these changes?');" />


    </div>       
      </div>

</asp:Content>
