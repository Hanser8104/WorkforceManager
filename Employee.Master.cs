using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class homepage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null)
            {
                // Redirect to login if role is not set
                Response.Redirect("Login.aspx");
            }

            string role = Session["Role"].ToString();

            // Reset visibility of all hyperlinks
            homehyperlink.Visible = true;
            worktaskhyperlink.Visible = false;
            leavehyperlink.Visible = false;
            leavestatushyperlink.Visible = false;
            manageLeaveHyperlink.Visible = false;
            assignWorkTaskHyperlink.Visible = false;
            monitorWorkTasksHyperlink.Visible = false;
            createEmployeeHyperlink.Visible = false;
            updateScheduleHyperlink.Visible = false;
            manageAccountHyperlink.Visible = false;
            aboutushyperlink.Visible = true;

            // Show links based on role
            if (role == "Employee")
            {
                worktaskhyperlink.Visible = true;
                leavehyperlink.Visible = true;
                leavestatushyperlink.Visible = true;
            }
            else if (role == "Supervisor")
            {
                manageLeaveHyperlink.Visible = true;
                assignWorkTaskHyperlink.Visible = true;
                monitorWorkTasksHyperlink.Visible = true;
            }
            else if (role == "Admin")
            {
                createEmployeeHyperlink.Visible = true;
                updateScheduleHyperlink.Visible = true;
                manageAccountHyperlink.Visible = true;
            }
        }
    }
}