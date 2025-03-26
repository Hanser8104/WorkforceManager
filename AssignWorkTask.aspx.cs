using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class WebForm9 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get EmployeeID from session (stored during login)
            string employeeID = Session["EmployeeID"]?.ToString();

            if (!IsPostBack)
            {
                LoadActiveEmployee();
            }
            
            if (string.IsNullOrEmpty(employeeID))
            {
                Response.Redirect("Login.aspx"); // Redirect if session is lost
                return;
            }
        }

        private void LoadActiveEmployee()
        {
            string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;";
            databaseConnection += "Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

            using (OleDbConnection conn = new OleDbConnection(databaseConnection))
            {
                try
                {

                    conn.Open();

                    // Query to bind data to the GridView
                    string empQuery = @"SELECT 
                                            EU.EmployeeID, EU.GivenName & ' ' & EU.MiddleName & ' ' & EU.LastName AS FullName,
                                            EU.Department, EU.SupervisorID, EU.LeaveBalance
                                            FROM EMPLOYEE EU
                                            LEFT JOIN ACCOUNT AC ON EU.EmployeeID = AC.EmployeeID
                                            WHERE AC.Role = 'Employee' AND EU.EmploymentStatus = 'Active'";
                    using (OleDbCommand empcmd = new OleDbCommand(empQuery, conn))
                    {

                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(empcmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            adapter.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gvEmployee.DataSource = dt;
                                gvEmployee.DataBind();
                            }
                            else
                            {
                                gvEmployee.DataSource = null;
                                gvEmployee.DataBind();
                            }
                        }
                    }

                    // Query to get EmployeeID for active employees with the role "Employee"
                    string drpQuery = @"
                SELECT EU.EmployeeID 
                FROM EMPLOYEE EU 
                LEFT JOIN ACCOUNT AC ON EU.EmployeeID = AC.EmployeeID 
                WHERE AC.Role = 'Employee' AND EU.EmploymentStatus = 'Active'";
                    using (OleDbCommand drpcmd = new OleDbCommand(drpQuery, conn))
                    {
                        using (OleDbDataReader reader = drpcmd.ExecuteReader())
                        {
                            ddlEmployeeID.Items.Clear(); // Clear existing items
                            ddlEmployeeID.Items.Add(new ListItem("Select an Employee Number", "")); // Add default item

                            while (reader.Read())
                            {
                                string employeeID = reader["EmployeeID"].ToString();
                                ddlEmployeeID.Items.Add(new ListItem(employeeID, employeeID));
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

        protected void ddlEmployeeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedEmployeeID = ddlEmployeeID.SelectedValue;

            if (!string.IsNullOrEmpty(selectedEmployeeID))
            {
                FetchEmployee(selectedEmployeeID);
            }
        }

        private void FetchEmployee(string selectedID)
        {
            string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;";
            databaseConnection += "Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

            using (OleDbConnection conn = new OleDbConnection(databaseConnection))
            {
                try
                {
                    conn.Open();

                    // Query to fetch employee details for the selected employee
                    string selectedQuery = @"
                SELECT EU.EmployeeID, EU.GivenName & ' ' & EU.MiddleName & ' ' & EU.LastName AS FullName,
                       EU.Department, EU.LeaveBalance, EU.ScheduleID, WS.WorkDays, WS.ShiftStart, WS.ShiftEnd
                FROM EMPLOYEE EU 
                LEFT JOIN WORK_SCHEDULE WS ON EU.ScheduleID = WS.ScheduleID
                WHERE EU.EmployeeID = ?";
                    using (OleDbCommand selectedcmd = new OleDbCommand(selectedQuery, conn))
                    {
                        selectedcmd.Parameters.AddWithValue("?", selectedID);

                        using (OleDbDataReader selectedReader = selectedcmd.ExecuteReader())
                        {
                            if (selectedReader.Read())
                            {
                                lblEmployeeName.Text = selectedReader["FullName"].ToString();
                                lblScheduleID.Text = selectedReader["ScheduleID"].ToString();
                                lblDepartment.Text = selectedReader["Department"].ToString();
                                lblLeaveBalance.Text = selectedReader["LeaveBalance"].ToString();
                                lblWorkDays.Text = selectedReader["WorkDays"].ToString();
                                lblShiftPeriod.Text = selectedReader["ShiftStart"].ToString() + " - " + selectedReader["ShiftEnd"].ToString();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
           
            string department = lblDepartment.Text.Trim();
            string schedule = lblScheduleID.Text.Trim();
            string details = txtSupervisorRemark.Text.Trim();
            string employeeID = ddlEmployeeID.SelectedValue;
            string dateToday = DateTime.Now.ToString("MM-dd-yyyy"); 
            if (string.IsNullOrEmpty(employeeID))
            {
                Response.Write("<script>alert('Please fill all the required fields before submitting.');</script>");
                return;
            }
            string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

            string query = @"INSERT INTO WORK_TASK (EmployeeID, ScheduleID, TaskDate, TaskDetails, Status)
                     VALUES (@EmployeeID, @ScheduleID, @TaskDate, @TaskDetails, 'Pending')";

            using (OleDbConnection conn = new OleDbConnection(databaseConnection))
            {
                try
                {
                    conn.Open();

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                        cmd.Parameters.AddWithValue("@ScheduleID", schedule);
                        cmd.Parameters.AddWithValue("@TaskDate", dateToday);
                        cmd.Parameters.AddWithValue("@TaskDetails", details);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Response.Write("<script>alert('Task saved successfully!');</script>");
                        }
                        else
                        { 
                            Response.Write("<script>alert('Failed to save the task.');</script>");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write($"<script>alert('Error: {ex.Message}');</script>");
                }
            }

            // Reset form fields
            lblDepartment.Text = "";
            lblEmployeeName.Text = "";
            lblLeaveBalance.Text = "";
            lblShiftPeriod.Text = "";
            lblScheduleID.Text = "";
            lblWorkDays.Text = "";
            txtSupervisorRemark.Text = "";
            ddlEmployeeID.SelectedIndex = 0;
        }
    }
}