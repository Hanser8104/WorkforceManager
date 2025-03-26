using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployeeData();
            }
        }
       


        private void LoadEmployeeData()
        {
            // Get EmployeeID from session (stored during login)
            string employeeID = Session["EmployeeID"]?.ToString();

            if (string.IsNullOrEmpty(employeeID))
            {
                Response.Redirect("Login.aspx"); // Redirect if session is lost
                return;
            }

            string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;";
            databaseConnection += "Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

            using (OleDbConnection conn = new OleDbConnection(databaseConnection))
            {
                try
                {
                    conn.Open();

                    // Load Employee Data
                    string employeeQuery = "SELECT EmployeeID, GivenName, LastName, Department, SupervisorID, LeaveBalance FROM EMPLOYEE WHERE EmployeeID = ?";
                    using (OleDbCommand empCmd = new OleDbCommand(employeeQuery, conn))
                    {
                        empCmd.Parameters.AddWithValue("?", employeeID);

                        using (OleDbDataReader reader = empCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblEmployeeID.Text = reader["EmployeeID"].ToString();
                                lblFullName.Text = reader["GivenName"] + " " + reader["LastName"];
                                lblDepartment.Text = reader["Department"].ToString();
                                lblSupervisorID.Text = reader["SupervisorID"].ToString();
                                lblLeaveBalance.Text = reader["LeaveBalance"].ToString();
                            }
                        }
                    }

                    // Load Leave Status Data
                    string leaveQuery = "SELECT RequestID, LeaveType, StartDate, EndDate, ApprovalStatus, SupervisorRemarks FROM LEAVE_REQUEST WHERE EmployeeID = ?";
                    using (OleDbCommand leaveCmd = new OleDbCommand(leaveQuery, conn))
                    {
                        leaveCmd.Parameters.AddWithValue("?", employeeID);

                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(leaveCmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            adapter.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gvLeaveStatus.DataSource = dt;
                                gvLeaveStatus.DataBind();
                            }
                            else
                            {
                                gvLeaveStatus.DataSource = null;
                                gvLeaveStatus.DataBind();
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

    }
}