<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="WebApplication2.login" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>WORKFORCE</title>
    <style type="text/css">
        html, body {
            height: 100%; 
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            background-color: #fcf7f9; /* Background color */
            display: flex;
            flex-direction: column;
        }

        #form1 {
            flex: 1;
            display: flex;
            flex-direction: column;
        }

        .header {
            text-align: center;
            font-size: 28px;
            font-weight: bold;
            padding: 20px 0;
            background-color: #0c0c3b; /* Dark Blue */
            color: white;
        }

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
            background-color: #d6182c;
            color: #FFFFFF;
            font-weight: bold;
            font-size: medium;
        }

        .newStyle2 {
            background-color: #D0CCD0;
        }

        .footer {
            text-align: center;
            padding: 15px 0;
            color: black;
            font-size: 14px;
            width: 100%;
            border-top: 2px solid #333;
            background-color: #fcf7f9; 
            margin-top: auto; 
        }

        .content {
            flex: 1; 
            display: flex;
            flex-direction: column;
            justify-content: center; 
            align-items: center; 
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
        .auto-style5 {
            position: relative;
            width: 401px;
            left: -1px;
            top: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            WorkSync Solutions</div>
        <div class="content">
            <center><strong><span class="auto-style1">LOGIN TO WORKFORCE <br /> LEAVE MANAGER
            </span></strong></center>
            <br />
            <center>
                <table class="newStyle2">
                    <tr>
                        <td class="auto-style4">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Employee ID</td>
                    </tr>
                    <tr>
                        <td class="auto-style4">
                            <center>
                                <asp:TextBox ID="usernametxt" runat="server" Width="381px"></asp:TextBox>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style4">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Password</td>
                    </tr>
                    <tr>
                        <td class="auto-style4">
                            <center>
                                <div class="auto-style5">
                                    <asp:TextBox ID="passwordtxt" runat="server" Width="383px" TextMode="Password"></asp:TextBox>
                                    <span class="password-toggle" onclick="togglePasswordVisibility()">👁️</span>
                                </div>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="auto-style4">
                            <center>
                                <asp:Button ID="Button1" runat="server" Text="Sign In" CssClass="newStyle1" Height="50px" Width="200px" OnClick="Button1_Click" />
                                <br />
                            </center>
                        </td>
                    </tr>
                </table>
            </center>
        </div>
        <footer class="footer">
            Copyright 2025 | WorkSync Solutions | All Rights Reserved
        </footer>
    </form>

    <script type="text/javascript">
        function togglePasswordVisibility() {
            var passwordField = document.getElementById('<%= passwordtxt.ClientID %>');
            var toggleIcon = document.querySelector('.password-toggle');

            if (passwordField.type === "password") {
                passwordField.type = "text";
                toggleIcon.textContent = "🙈";
            } else {
                passwordField.type = "password";
                toggleIcon.textContent = "👁️"; 
            }
        }
    </script>
</body>
</html>