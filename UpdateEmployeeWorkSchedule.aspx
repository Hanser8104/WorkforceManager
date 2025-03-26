<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="UpdateEmployeeWorkSchedule.aspx.cs" Inherits="WebApplication2.WebForm11" %>
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
        .search-container {
            margin-bottom: 20px;
        }
        .search-container input[type="text"] {
            padding: 8px;
            width: 200px;
        }
        .search-container .btn-primary {
            padding: 8px 16px;
        }
    </style>
    <script type="text/javascript">
    function autoFillTimes() {
        // Get the Shift Start time
        const shiftStart = document.getElementById('<%= txtShiftStart.ClientID %>').value;

        if (shiftStart) {
            // Parse the Shift Start time
            const startTime = new Date(`1970-01-01T${shiftStart}:00`);

            // Calculate Shift End (8 hours later)
            const shiftEnd = new Date(startTime.getTime() + 8 * 60 * 60 * 1000);
            document.getElementById('<%= txtShiftEnd.ClientID %>').value = formatTime(shiftEnd);

            // Calculate Break Start (2 hours after Shift Start)
            const breakStart = new Date(startTime.getTime() + 2 * 60 * 60 * 1000);
            document.getElementById('<%= txtBreakStart.ClientID %>').value = formatTime(breakStart);

            // Calculate Break End (15 minutes after Break Start)
            const breakEnd = new Date(breakStart.getTime() + 15 * 60 * 1000);
            document.getElementById('<%= txtBreakEnd.ClientID %>').value = formatTime(breakEnd);

            // Calculate Lunch Start (1 hour and 30 minutes after Shift Start)
            const lunchStart = new Date(startTime.getTime() + 1.5 * 60 * 60 * 1000);
            document.getElementById('<%= txtLunchStart.ClientID %>').value = formatTime(lunchStart);

            // Calculate Lunch End (1 hour after Lunch Start)
            const lunchEnd = new Date(lunchStart.getTime() + 1 * 60 * 60 * 1000);
            document.getElementById('<%= txtLunchEnd.ClientID %>').value = formatTime(lunchEnd);
        }
    }

    // Helper function to format time as HH:MM
    function formatTime(date) {
        const hours = String(date.getHours()).padStart(2, '0');
        const minutes = String(date.getMinutes()).padStart(2, '0');
        return `${hours}:${minutes}`;
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="dashboard-container">
        <h1 class="welcome-text">Update Employee Work Schedule</h1>
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
        <!-- Section 2: Add a New Work Schedule -->
        <h3>Add a New Work Schedule</h3>
<table class="info-table">
    <tr>
        <th>Workdays</th>
        <td>
            <asp:TextBox ID="txtWorkdays" runat="server" Width="209px" TextMode="MultiLine"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvWorkdays" runat="server" ControlToValidate="txtWorkdays" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <!-- Shift Period -->
    <tr>
        <th>Shift Period</th>
        <td>
            <asp:TextBox ID="txtShiftStart" runat="server" TextMode="Time" Width="90px" onchange="autoFillTimes()"></asp:TextBox>
            &nbsp;-&nbsp;
            <asp:TextBox ID="txtShiftEnd" runat="server" TextMode="Time" Width="90px" onchange="this.value = this.value;" ReadOnly="True"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvShiftStart" runat="server" ControlToValidate="txtShiftStart" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <!-- Break Period -->
    <tr>
        <th>Break Period</th>
        <td>
            <asp:TextBox ID="txtBreakStart" runat="server" TextMode="Time" Width="90px" onchange="this.value = this.value;" ReadOnly="True"></asp:TextBox>
            &nbsp;-&nbsp;
            <asp:TextBox ID="txtBreakEnd" runat="server" TextMode="Time" Width="90px" onchange="this.value = this.value;" ReadOnly="True"></asp:TextBox>
        </td>
    </tr>
    <!-- Lunch Period -->
    <tr>
        <th>Lunch Period</th>
        <td>
            <asp:TextBox ID="txtLunchStart" runat="server" TextMode="Time" Width="90px" onchange="this.value = this.value;" ReadOnly="True"></asp:TextBox>
            &nbsp;-&nbsp;
            <asp:TextBox ID="txtLunchEnd" runat="server" TextMode="Time" Width="90px" ReadOnly="true" onchange="this.value = this.value;"></asp:TextBox>
        </td>
    </tr>
</table>
<div style="text-align: center; margin-top: 20px;">
    <asp:Button ID="btnAddSchedule" runat="server" Text="Add Schedule" CssClass="btn-primary" OnClick="btnAddSchedule_Click" OnClientClick="return confirm('Are you sure you want to save these changes?');" />
    <asp:Button ID="btnModifySchedule" runat="server" Text="Modify an Employee Work Schedule" CssClass="btn-primary" OnClick="btnModifySchedule_Click" style="background-color: #0C0C3B; margin-left: 10px;" Width="328px" OnClientClick="return confirm('Are you sure you want to modify a user work schedule instead?');" />
</div>


        <br />
        <br />
        
    </div>
</asp:Content>