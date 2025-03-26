<%@ Page Title="" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="UpdateDailyWorkTask.aspx.cs" Inherits="WebApplication2.WebForm2" %>
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
    </style>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validation/1.19.3/jquery.validate.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validation-unobtrusive/3.2.12/jquery.validate.unobtrusive.min.js"></script>
<script type="text/javascript">
    function validateAndConfirm() {
        // Trigger jQuery validation
        var isValid = $("#form1").valid(); // Replace "form1" with your form's ID
        if (isValid) {
            // If validation passes, show confirmation dialog
            return confirm("Are you sure you want to Save Changes?");
        }
        // If validation fails, cancel the postback
        return false;
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="dashboard-container">
        <h1 class="welcome-text">Update Daily Work Tasks</h1>
        <br />
        <!-- Employee Information -->
        <h3>Current Work Tasks</h3>
        <br />
        <asp:GridView ID="gvWorkforce" runat="server" AutoGenerateColumns="False" CssClass="info-table">
            <Columns>
            <asp:BoundField DataField="TaskID" HeaderText="Task Number"  />
            <asp:BoundField DataField="TaskDate" HeaderText="Date of Task Assigned" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="TaskDetails" HeaderText="Details" />
            <asp:BoundField DataField="Status" HeaderText="Progress Status"  />
            </Columns>
            <EmptyDataTemplate>
                <div style="text-align: center; padding: 20px;">
            <asp:Label ID="lblEmptyMessage" runat="server" Text="Oooo, so empty~" Font-Italic="true" ForeColor="Gray"></asp:Label>
        </div>
    </EmptyDataTemplate>
        </asp:GridView>
        <br />
        <h3>Update a Work Task</h3>
            <table class="info-table">
                <tr>
                    <th>Task Number</th>
                        <td>
                            <asp:DropDownList ID="ddlTaskNumber" runat="server" Height="40px" Width="163px">
                                <asp:ListItem Text="Select Task to Update" Value="" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="tasknumRFV" runat="server" ControlToValidate="ddlTaskNumber" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th>Task Status</th>
                    <td>
                         <asp:DropDownList ID="ddlTaskStatus" runat="server" Height="40px" Width="163px">
                             <asp:ListItem Text="Select Status" Value="" />
                             <asp:ListItem Text="In Progress" Value="In Progress" />
                             <asp:ListItem Text="Done" Value="Done" />
                         </asp:DropDownList>
                         <asp:RequiredFieldValidator ID="taskstatusRFV" runat="server" ControlToValidate="ddlTaskStatus" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
         <br />                           

        <div style="text-align: center; margin-top: 20px;">
            <asp:Button ID="btnSave" runat="server" Text="Save Changes" 
    CssClass="btn-primary" 
    OnClick="btnSave_Click" 
    OnClientClick="return validateAndConfirm();" />

        </div>
        <br />
        <br />
        <h3>Work Tasks History</h3>
        <br />
        <asp:GridView ID="GvTaskHistory" runat="server" AutoGenerateColumns="False" CssClass="info-table">
         <Columns>
            <asp:BoundField DataField="TaskID" HeaderText="Task Number"  />
            <asp:BoundField DataField="TaskDate" HeaderText="Date of Task Assigned" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:BoundField DataField="TaskDetails" HeaderText="Details" />
            <asp:BoundField DataField="Status" HeaderText="Progress Status"  />
         </Columns>
</asp:GridView>
    </div>
</asp:Content>
