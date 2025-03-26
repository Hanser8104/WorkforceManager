<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="WebApplication2.WebForm6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            font-size: xx-large;
            font-family: 'Century Gothic';
            text-align: center;
        }
        .style1 {
            width: 300px;
        }
        .style2 {
            width: 600px;
            font-family: "Century Gothic";
        }
        .style3 {
            width: auto;
        }
        .auto-style4 {
            width: 500px;
            font-family: "Century Gothic";
            height: 27px;
            text-align: left;
        }
        .newStyle1 {
            background-color: #E50019;
            color: #FFFFFF;
            font-weight: bold;
            font-size: medium;
        }
        .newStyle2 {
            background-color: #D0CCD0;
        }
        .error-message {
            color: red;
            font-family: "Century Gothic";
            font-size: small;
        }
        .auto-style5 {
            background-color: #D6182C;
            color: #FFFFFF;
            font-weight: bold;
            font-size: medium;
        }
        .password-container {
            position: relative;
            width: 400px; /* Match the width of the textbox */
        }
        .password-toggle {
            position: absolute;
            right: 10px;
            top: 50%;
            transform: translateY(-50%);
            cursor: pointer;
            color: #0c0c3b; /* Dark Blue */
            font-size: 14px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <br />
        <br />
        <center><strong><span class="auto-style1">CHANGE PASSWORD</span></strong></center>
        <br />
        <center>
            <table class="newStyle2">
                <tr>
                    <td class="auto-style4">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Current Password</td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <center>
                            <div class="password-container">
                                <asp:TextBox ID="txtCurrentPassword" runat="server" Width="382px" TextMode="Password"></asp:TextBox>
                                <span class="password-toggle" onclick="togglePasswordVisibility('<%= txtCurrentPassword.ClientID %>')">👁️</span>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvCurrentPassword" runat="server" ControlToValidate="txtCurrentPassword" ErrorMessage="Current Password is required." CssClass="error-message" Display="Dynamic"></asp:RequiredFieldValidator>
                        </center>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; New Password</td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <center>
                            <div class="password-container">
                                <asp:TextBox ID="txtNewPassword" runat="server" Width="382px" TextMode="Password"></asp:TextBox>
                                <span class="password-toggle" onclick="togglePasswordVisibility('<%= txtNewPassword.ClientID %>')">👁️</span>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword" ErrorMessage="New Password is required." CssClass="error-message" Display="Dynamic"></asp:RequiredFieldValidator>
                        </center>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Confirm New Password</td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <center>
                            <div class="password-container">
                                <asp:TextBox ID="txtConfirmNewPassword" runat="server" Width="382px" TextMode="Password"></asp:TextBox>
                                <span class="password-toggle" onclick="togglePasswordVisibility('<%= txtConfirmNewPassword.ClientID %>')">👁️</span>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvConfirmNewPassword" runat="server" ControlToValidate="txtConfirmNewPassword" ErrorMessage="Confirm New Password is required." CssClass="error-message" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvConfirmNewPassword" runat="server" ControlToValidate="txtConfirmNewPassword" ControlToCompare="txtNewPassword" ErrorMessage="Passwords do not match." CssClass="error-message" Display="Dynamic"></asp:CompareValidator>
                        </center>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <center>
                            <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" CssClass="auto-style5" Height="50px" Width="200px" OnClick="btnChangePassword_Click" OnClientClick="return confirm('This will download the new employee information. Continue?');" />
                        </center>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <center>
                            <asp:Label ID="lblMessage" runat="server" CssClass="error-message"></asp:Label>
                        </center>
                    </td>
                </tr>
            </table>
        </center>
    </div>

    <script type="text/javascript">
        function togglePasswordVisibility(passwordFieldId) {
            var passwordField = document.getElementById(passwordFieldId); // Use the passed ID directly
            var toggleIcon = document.querySelector('[onclick="togglePasswordVisibility(\'' + passwordFieldId + '\')"]');

            if (passwordField.type === "password") {
                passwordField.type = "text";
                toggleIcon.textContent = "🙈"; // Change icon to "hide"
            } else {
                passwordField.type = "password";
                toggleIcon.textContent = "👁️"; // Change icon to "show"
            }
        }
    </script>
</asp:Content>