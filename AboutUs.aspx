<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Employee.Master" AutoEventWireup="true" CodeBehind="AboutUs.aspx.cs" Inherits="WebApplication2.WebForm5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .dashboard-container {
            padding: 20px;
            font-family: Arial, sans-serif;
            text-align: center;
        }
        .welcome-text {
            font-size: xx-large;
            font-weight: bold;
            margin-bottom: 40px;
        }
        .team-container {
            display: flex;
            justify-content: center;
            gap: 20px; /* Space between images */
            margin-top: 20px;
        }
        .team-member {
            text-align: center;
        }
        .team-member img {
            width: 250px; /* Adjust image size as needed */
            height: 250px;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }
        .team-member h3 {
            font-size: 20px;
            font-weight: bold;
            margin: 15px 0 5px;
        }
        .team-member p {
            font-size: 16px;
            color: #555;
        }
        .company-details {
            max-width: 800px;
            margin: 40px auto 0;
            font-size: 16px;
            color: #555;
            line-height: 1.6;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="dashboard-container">
        <br />
        <div class="team-container">
            <div class="team-member">
                <asp:Image ID ="Image3" runat ="server" AlternateText="WorkSync Solutions" ImageUrl ="~/Logo.png" Height="174px" Width="864px" />
                <h3>COMPANY NAME</h3>
            </div>
        </div>
        <br />
        <br />
        <br />
        <div class="welcome-text">
            MEET THE FOUNDERS
        </div>

        <div class="team-container">
            <div class="team-member">
                <asp:Image ID="Image1" runat="server" AlternateText="Charles Louie C. Sabandal" ImageUrl="~/SABANDAL, CHARLES LOUIE C..png" Width="223px" />
                <h3>SABANDAL, Charles Louie C.</h3>
                <p>Developer & System Administrator</p>
            </div>

            <div class="team-member">
                <asp:Image ID="Image2" runat="server" AlternateText="Kathleen Mae S. Pascual" ImageUrl="~/PASCUAL, KATHLEEN MAE S..png" />
                <h3>PASCUAL, Kathleen Mae S.</h3>
                <p>Front-End Developer</p>
            </div>
        </div>
        <br />

        <div class="company-details">
            <h3>About WorkSync Solutions</h3>
            <p>WorkSync Solutions was founded in 2025 by Charles Louie C. Sabandal and Kathleen Me S. Pascual, two passionate second-year Computer Science students with a vision to streamline workforce management. The idea stemmed form their academic projects, where they recognized the challenges businesses face in handling employee leave requests, work scheduling, and workload balancing.

	</p>
            <p>&nbsp;Driven by a problem-solving mindset, they transformed their coursework into a fully functional system designed for Business Process Outsourcing (BPO) and customer service industries. Despite being students, their commitment to innovation and efficiency led to the development of a system that enhances leave request automation, workforce availability tracking and task assignments.
</p>
            <p>
                &nbsp;</p> 
            <br />
            <h3>WorkSync Operations</h3>
            <p>WorkSync Solutions operates as an independent software development team, focusing on workforce management systems. With Charles overseeing full-stack development and system administration, and Kathleen specializing in front-end design, the company ensures a seamless user experience while maintaining a robust database and backend functionality.</p>
               
            <p></p>
            <br />
            <h3>Goal for the Users</h3>
            <p>Provide a Workforce Leave Manager system that helps employees efficiently request leave while providing real-time insights into team availability. It also assists managers in making informed leave approval decisions by indicating high-demand workdays.</p>

            <br />
            <p></p>
            <h3>Type of Information System</h3>
            <p>The Workforce Leave Manager is a Workforce Management Information System (WMIS) designed to help businesses, particularly in the BPO and customer service industries, manage employee leave requests efficiently.

This system ensures that:
Employees can check workforce availability before submitting a leave request.
Managers can approve or reject leave based on team demand and availability.
The company can track leave balances and prevent scheduling conflicts.

	By integrating real-time workforce indicators, the system helps maintain optimal staffing levels while employees have a transparent and structured leave approval process.
</p>
        </div>

    </div>
</asp:Content>