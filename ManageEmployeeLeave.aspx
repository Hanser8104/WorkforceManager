<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="ManageEmployeeLeave.aspx.cs" Inherits="WebApplication2.WebForm7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Your existing CSS remains untouched */
        .dashboard-container { padding: 20px; font-family: Arial, sans-serif; }
        .welcome-text { font-size: x-large; font-weight: bold; margin-bottom: 20px; }
        .info-table { width: 100%; border-collapse: collapse; margin-bottom: 20px; }
        .info-table th, .info-table td { border: 1px solid #ddd; padding: 10px; text-align: left; }
        .info-table th { background-color: #f2f2f2; }
        .btn-primary { background-color: #CF182C; color: #FFFFFF; border: none; padding: 10px 20px; cursor: pointer; font-size: 16px; }
        .btn-primary:hover { background-color: #A81424; }
        .auto-style1 { width: 366px; }
        .auto-style2 { width: 366px; height: 47px; }
        .auto-style3 { height: 47px; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="dashboard-container">
        <h1 class="welcome-text">Manage Employee Leave</h1>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        
        <br />
        <h3>Employee Leave Records</h3>
        <asp:GridView ID="gvLeaveRecords" runat="server" AutoGenerateColumns="False" CssClass="info-table">
            <Columns>
                <asp:BoundField DataField="RequestID" HeaderText="Request Number" />
                <asp:BoundField DataField="EmployeeName" HeaderText="Employee Name" />
                <asp:BoundField DataField="LeavePeriod" HeaderText="Leave Period" />
                <asp:BoundField DataField="ApprovalStatus" HeaderText="Approval Status"/>
            </Columns>
                    <EmptyDataTemplate>
            <div style="text-align: center; padding: 20px;">
        <asp:Label ID="lblEmptyMessage" runat="server" Text="Oooo, so empty~" Font-Italic="true" ForeColor="Gray"></asp:Label>
    </div>
</EmptyDataTemplate>
        </asp:GridView>
        <br />

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <h3>Manage An Employee Leave</h3>
                <table class="info-table">
                    <tr>
                        <th class="auto-style2">Request ID</th>
                        <td class="auto-style3">
                            <asp:DropDownList ID="ddlRequestID" runat="server" Height="29px" Width="178px" 
                                AutoPostBack="True" OnSelectedIndexChanged="ddlRequestID_SelectedIndexChanged">
                                <asp:ListItem Text="Select a Request Number" Value="" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvrequestid" runat="server" 
                                ControlToValidate="ddlRequestID" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr><th class="auto-style1">Employee Name</th><td><asp:Label ID="lblEmployeeName" runat="server" /></td></tr>
                    <tr><th class="auto-style1">Employee ID</th><td><asp:Label ID="lblEmployeeID" runat="server" /></td></tr>
                    <tr><th class="auto-style1">Department</th><td><asp:Label ID="lblDepartment" runat="server" /></td></tr>
                    <tr><th class="auto-style1">Supervisor ID</th><td><asp:Label ID="lblSupervisorID" runat="server" /></td></tr>
                    <tr><th class="auto-style1">Leave Balance</th><td><asp:Label ID="lblLeaveBalance" runat="server" /></td></tr>
                    <tr><th class="auto-style1">Leave Type</th><td><asp:Label ID="lblLeaveType" runat="server" /></td></tr>
                    <tr><th class="auto-style1">Leave Period</th><td><asp:Label ID="lblLeavePeriod" runat="server" /></td></tr>
                    <tr><th class="auto-style1">Workforce Demand</th><td><asp:Label ID="lblWorkforce" runat="server" /></td></tr>
                    <tr><th class="auto-style1">Reason</th><td><asp:Label ID="lblReason" runat="server" /></td></tr>
                    <tr>
                        <th class="auto-style1">Manage Leave</th>
                        <td>
                            <asp:DropDownList ID="drpLeaveStatus" runat="server" Height="29px" Width="157px">
                                <asp:ListItem Text="Select an option" Value="" />
                                <asp:ListItem Text="Accept" Value="Accepted" />
                                <asp:ListItem Text="Reject" Value="Rejected" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvmanageleave" runat="server" 
                                ControlToValidate="drpLeaveStatus" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th class="auto-style1">Remarks</th>
                        <td>
                            <asp:TextBox ID="txtSupervisorRemark" TextMode="MultiLine" Rows="4" Columns="50" runat="server" Height="98px" Width="554px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlRequestID" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>

        <div style="text-align: center; margin-top: 20px;">
            <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="btn-primary" OnClick="btnSave_Click" OnClientClick="return confirm('Are you sure you want to save these changes?');"/>
        </div>
    </div>
</asp:Content>