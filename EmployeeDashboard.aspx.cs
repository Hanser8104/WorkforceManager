using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Role"] != null && Session["Role"].ToString() == "Supervisor")
                {
                    btnFileLeave.Visible = true;
                    btnViewLeave.Visible = true;
                }
                else
                {
                    btnFileLeave.Visible = false;
                    btnViewLeave.Visible = false;
                }

                if (Session["Role"] != null && Session["Role"].ToString() == "Admin")
                {
                    pnlWorkSchedule.Visible = false;
                }
                else
                {
                    pnlWorkSchedule.Visible = true;
                }
                LoadEmployeeData();
            }
        }


        private void LoadEmployeeData()
        {
            string employeeID = Session["EmployeeID"]?.ToString();

            if (string.IsNullOrEmpty(employeeID))
            {
                Response.Redirect("Login.aspx"); 
                return;
            }

            string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;";
            databaseConnection += "Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

            using (OleDbConnection conn = new OleDbConnection(databaseConnection))
            {
                try
                {
                    conn.Open();

                    string employeeQuery = "SELECT EmployeeID, GivenName, MiddleName, LastName, Department, SupervisorID, LeaveBalance FROM EMPLOYEE WHERE EmployeeID = ?";
                    using (OleDbCommand empCmd = new OleDbCommand(employeeQuery, conn))
                    {
                        empCmd.Parameters.AddWithValue("?", employeeID);

                        using (OleDbDataReader reader = empCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string middleName = reader["MiddleName"] as string ?? string.Empty;
                                lblEmployeeName.Text = reader["GivenName"].ToString();
                                lblEmployeeID.Text = reader["EmployeeID"].ToString();
                                lblFullName.Text = $"{reader["GivenName"]} {middleName} {reader["LastName"]}".Trim();
                                lblDepartment.Text = reader["Department"].ToString();
                                lblSupervisorID.Text = reader["SupervisorID"].ToString();
                                lblLeaveBalance.Text = reader["LeaveBalance"].ToString();
                            }
                        }
                    }

                    string roleQuery = @"SELECT Role FROM ACCOUNT WHERE EmployeeID = ?";
                    using (OleDbCommand rolecmd = new OleDbCommand(roleQuery, conn))
                    {
                        rolecmd.Parameters.AddWithValue("?", employeeID);
                        using (OleDbDataReader reader = rolecmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblRole.Text = reader["Role"].ToString();
                            }
                        }
                    }

                    string supervisorID = lblSupervisorID.Text;

                    if (string.IsNullOrEmpty(supervisorID))
                    {
                        lblSupervisorName.Text = "- -";
                    }
                    else
                    {
                        string supervisorName = @"SELECT GivenName, MiddleName, LastName, Department FROM EMPLOYEE WHERE EmployeeID = ?";

                        using (OleDbCommand supervisorcmd = new OleDbCommand(supervisorName, conn))
                        {
                            supervisorcmd.Parameters.AddWithValue("?", supervisorID);
                            using (OleDbDataReader reader = supervisorcmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // Assuming MiddleName is optional and might be null
                                    string middleName = reader["MiddleName"] as string ?? string.Empty;
                                    lblSupervisorName.Text = $"{reader["GivenName"]} {middleName} {reader["LastName"]}".Trim();
                                }
                                else
                                {
                                    lblSupervisorName.Text = "- -";
                                }
                            }
                        }
                    }


                    string scheduleQuery =  @"SELECT WS.ScheduleID, WS.Workdays, WS.ShiftStart, WS.ShiftEnd, WS.BreakStart, WS.BreakEnd, 
                            WS.LunchStart, WS.LunchEnd
                            FROM WORK_SCHEDULE WS LEFT JOIN EMPLOYEE ED ON WS.ScheduleID = ED.ScheduleID
                            WHERE ED.EmployeeID = ?";

                    using (OleDbCommand scheduleCmd = new OleDbCommand(scheduleQuery, conn))
                    {
                        scheduleCmd.Parameters.AddWithValue("?", employeeID);

                        using (OleDbDataReader scheduleReader = scheduleCmd.ExecuteReader())
                        {
                            if (scheduleReader.Read())
                            {
                                lblScheduleID.Text = scheduleReader["ScheduleID"].ToString();
                                lblWorkdays.Text = scheduleReader["Workdays"].ToString();
                                lblShiftPeriod.Text = scheduleReader["ShiftStart"].ToString() + " - " + scheduleReader["ShiftEnd"].ToString();
                                lblBreakPeriod.Text = scheduleReader["BreakStart"].ToString() + " - " + scheduleReader["BreakEnd"].ToString();
                                lblLunchPeriod.Text = scheduleReader["LunchStart"].ToString() + " - " + scheduleReader["LunchEnd"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePassword.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            string confirmScript = "if (confirm('Are you sure you want to log out?')) { window.location.href = 'login.aspx'; }";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmLogout", confirmScript, true);
        }

        protected void btnFileLeave_Click(object sender, EventArgs e)
        {
            Response.Redirect("FileEmployeeLeave.aspx");
        }

        protected void btnViewLeave_Click(object sender, EventArgs e)
        {
            Response.Redirect("ViewLeaveStatus.aspx");
        }
    }
}