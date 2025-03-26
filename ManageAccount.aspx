<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="ManageAccount.aspx.cs" Inherits="WebApplication2.WebForm12" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <style>
    /* Consistent Styles for All Pages */
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
            .auto-style1 {
                width: 400px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="info-table">
    <br />
    <!-- Employee Information -->
    <h3>User Account Records</h3>
    <br />
    <asp:GridView ID="gvAccounts" runat="server" AutoGenerateColumns="False" CssClass="info-table">
        <Columns>
        <asp:BoundField DataField="EmployeeID" HeaderText="Employee ID" />
        <asp:BoundField DataField="FullName" HeaderText="Name" />
        <asp:BoundField DataField="Department" HeaderText="Department" />
            <asp:BoundField DataField="Role" HeaderText="Role" />
        <asp:BoundField DataField="AccountStatus" HeaderText="Account Status"  />
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
    <tr><th class="auto-style1">Schedule ID</th><td><asp:Label ID="lblScheduleID" runat="server" /></td></tr>
    <tr><th class="auto-style1">Department</th><td><asp:Label ID="lblDepartment" runat="server" /></td></tr>
    <tr><th class="auto-style1">Role</th><td><asp:Label ID="lblRole" runat="server" /></td></tr>
    <tr><th class="auto-style1">Modify Account Status </th><td><asp:DropDownList ID="ddlTaskStatus" runat="server" Height="41px" Width="142px">
    <asp:ListItem Text="Select Status" Value="" />
    <asp:ListItem Text="Activate" Value="Activated" />
    <asp:ListItem Text="Deactivate" Value="Deactivated" />
</asp:DropDownList>
        <asp:RequiredFieldValidator ID="rfvaccountstatus" runat="server" ControlToValidate="ddlTaskStatus" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
        </td></tr>
</table>

            <div style="text-align: center; margin-top: 20px;">
<asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn-primary" OnClick="btnSave_Click" OnClientClick="return confirm('Are you sure you want to save these changes?');" />


</div> 
</div >

</asp:Content>
