<%@ Page Title="Create Employee" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="CreateEmployee.aspx.cs" Inherits="WebApplication2.WebForm10" %>

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
        .optional {
            font-size: 12px;
            color: #666;
        }
        .hidden {
            display: none;
        }
        .auto-style1 {
            width: 400px;
        }
    </style>
    <script type="text/javascript">
        // Function to toggle optional fields based on selected Role
        function toggleFields() {
            var role = document.getElementById('<%= ddlRole.ClientID %>').value;
            var supervisorRow = document.getElementById("supervisorRow");
            var scheduleRow = document.getElementById("scheduleRow");
            var leaveRow = document.getElementById("leaveRow");

            if (role === "Admin") {
                supervisorRow.style.display = "none";
                scheduleRow.style.display = "none";
                leaveRow.style.display = "none";
            }
            else if (role === "Supervisor") {
                supervisorRow.style.display = "none";
                scheduleRow.style.display = "table-row";
                leaveRow.style.display = "table-row";
            }
            else if (role === "Employee") {
                supervisorRow.style.display = "table-row";
                scheduleRow.style.display = "table-row";
                leaveRow.style.display = "table-row";
            }
            else {
                supervisorRow.style.display = "table-row";
                scheduleRow.style.display = "table-row";
                leaveRow.style.display = "table-row";
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="dashboard-container">
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
        <h3>Available Supervisor Users</h3>
<asp:GridView ID="gvAccounts" runat="server" AutoGenerateColumns="False" CssClass="info-table">
    <Columns>
    <asp:BoundField DataField="EmployeeID" HeaderText="Supervisor ID" />
    <asp:BoundField DataField="FullName" HeaderText="Name" />
    <asp:BoundField DataField="Department" HeaderText="Department" />
    <asp:BoundField DataField="EmploymentStatus" HeaderText="Employment Status"  />
    </Columns>
    <EmptyDataTemplate>
        <div style="text-align: center; padding: 20px;">
    <asp:Label ID="lblEmptyMessage" runat="server" Text="Oooo, so empty~" Font-Italic="true" ForeColor="Gray"></asp:Label>
</div>
    </EmptyDataTemplate>
</asp:GridView>
        <br />
        <h1 class="welcome-text">Create Employee</h1>

        <!-- Manual Employee Creation Form -->
        <h3>Employee Information</h3>
        <table class="info-table">
            <!-- EmployeeID is auto-generated; no textbox needed -->
            <tr>
                <th class="auto-style1">Last Name</th>
                <td>
                    <asp:TextBox ID="txtLastName" runat="server" Width="243px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th class="auto-style1">First Name</th>
                <td>
                    <asp:TextBox ID="txtFirstName" runat="server" Width="243px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th class="auto-style1">Middle Name</th>
                <td>
                    <asp:TextBox ID="txtMiddleName" runat="server" Width="243px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="auto-style1">Department</th>
                <td>
                    <asp:DropDownList ID="ddlDepartment" runat="server" Width="243px">
                        <asp:ListItem Value="" Text="Select Department"></asp:ListItem>
                        <asp:ListItem Value="Customer Service">Customer Service</asp:ListItem>
                        <asp:ListItem Value="IT Support">IT Support</asp:ListItem>
                        <asp:ListItem Value="Finance">Finance</asp:ListItem>
                        <asp:ListItem Value="Human Resources">Human Resources</asp:ListItem>
                        <asp:ListItem Value="Accounting">Accounting</asp:ListItem>
                        <asp:ListItem Value="Back Office">Back Office</asp:ListItem>
                        <asp:ListItem Value="Marketing">Marketing</asp:ListItem>
                        <asp:ListItem Value="Sales">Sales</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" ControlToValidate="ddlDepartment" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th class="auto-style1">Employment Status</th>
                <td>
                    <asp:DropDownList ID="ddlEmploymentStatus" runat="server" Width="243px">
                        <asp:ListItem Value="" Text="Select a Status"></asp:ListItem>
                        <asp:ListItem Value="Active">Active</asp:ListItem>
                        <asp:ListItem Value="Inactive">Inactive</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvEmploymentStatus" runat="server" ControlToValidate="ddlEmploymentStatus" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th class="auto-style1">Role</th>
                <td>
                    <asp:DropDownList ID="ddlRole" runat="server" Width="243px" AutoPostBack="true" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
                        <asp:ListItem Value="" Text="Select a Role"></asp:ListItem>
                        <asp:ListItem Value="Employee">Employee</asp:ListItem>
                        <asp:ListItem Value="Supervisor">Supervisor</asp:ListItem>
                        <asp:ListItem Value="Admin">Admin</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvRole" runat="server" ControlToValidate="ddlRole" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="supervisorRow" runat="server">
    <td class="auto-style1">Supervisor ID</td>
    <td><asp:TextBox ID="txtSupervisorID" runat="server" /></td>
</tr>
<tr id="scheduleRow" runat="server">
    <td class="auto-style1">Schedule ID</td>
    <td><asp:TextBox ID="txtScheduleID" runat="server" /></td>
</tr>
<tr id="leaveRow" runat="server">
    <td class="auto-style1">Leave Balance</td>
    <td><asp:TextBox ID="txtLeaveBalance" runat="server" /></td>
</tr>

            <tr>
                <th class="auto-style1">Auto-Generated Employee ID</th>
                <td>
                    <asp:Label ID="lblGeneratedEmployeeID" runat="server" Text="Will be generated"></asp:Label>
                </td>
            </tr>
            <tr>
                <th class="auto-style1">Auto-Generated Password</th>
                <td>
                    <asp:Label ID="lblGeneratedPassword" runat="server" Text="Will be generated"></asp:Label>
                </td>
            </tr>

        </table>

        <div style="text-align: center; margin-top: 20px;">
            <asp:Button ID="btnCreate" runat="server" CssClass="btn-primary" Text="Create Employee" OnClick="btnCreate_Click"  OnClientClick="return confirm('This will download the new employee information. Continue?');" />
        </div>
    </div>
</asp:Content>
