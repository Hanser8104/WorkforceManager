<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="ViewLeaveStatus.aspx.cs" Inherits="WebApplication2.WebForm4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Consistent Styles for All Pages */
        .dashboard-container {
            padding: 20px;
            font-family: Arial, sans-serif;
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
            width: 370px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="dashboard-container">

        <!-- Employee Information -->
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
        <th class="auto-style1">Supervisor ID</th>
        <td><asp:Label ID="lblSupervisorID" runat="server" /></td>
    </tr>
    <tr>
        <th class="auto-style1">Leave Balance</th>
        <td><asp:Label ID="lblLeaveBalance" runat="server" /></td>
    </tr>
</table>
        <br />                          
        <h3>Leave Status</h3>
        <asp:GridView ID="gvLeaveStatus" runat="server" AutoGenerateColumns="False" CssClass="info-table">
    <Columns>
        <asp:BoundField DataField="RequestID" HeaderText="Request Number" />
        <asp:BoundField DataField="LeaveType" HeaderText="Leave Type" />
        <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:yyyy-MM-dd}" />
        <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:yyyy-MM-dd}" />
        <asp:BoundField DataField="ApprovalStatus" HeaderText="Approval Status" />
        <asp:BoundField DataField="SupervisorRemarks" HeaderText="Supervisor Remarks" />
    </Columns>
                    <EmptyDataTemplate>
            <div style="text-align: center; padding: 20px;">
        <asp:Label ID="lblEmptyMessage" runat="server" Text="Oooo, so empty~" Font-Italic="true" ForeColor="Gray"></asp:Label>
    </div>
</EmptyDataTemplate>
</asp:GridView>
    </div>

</asp:Content>