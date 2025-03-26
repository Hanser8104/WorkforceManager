using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;

namespace WebApplication2
{
    public partial class WebForm13 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get EmployeeID from session (stored during login)
            string employeeID = Session["EmployeeID"]?.ToString();

            if (!IsPostBack)
            {
                LoadWorkSchedules();
                LoadUserAccounts();
            }

            if (string.IsNullOrEmpty(employeeID))
            {
                Response.Redirect("Login.aspx"); // Redirect if session is lost
                return;
            }
        }


        private void LoadWorkSchedules()
        {
            string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;";
            databaseConnection += "Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

            using (OleDbConnection conn = new OleDbConnection(databaseConnection))
            {
                try
                {

                    conn.Open();

                    string accQuery = @"SELECT 
                                            ScheduleID, Workdays,
                                            ShiftStart & '-' & ShiftEnd AS ShiftPeriod,
                                            BreakStart & '-' & BreakEnd AS BreakPeriod,
                                            LunchStart & '-' & LunchEnd AS LunchPeriod
                                            FROM WORK_SCHEDULE";
                    using (OleDbCommand Accountcmd = new OleDbCommand(accQuery, conn))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(Accountcmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            adapter.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gvWorkSchedules.DataSource = dt;
                                gvWorkSchedules.DataBind();
                            }
                            else
                            {
                                gvWorkSchedules.DataSource = null;
                                gvWorkSchedules.DataBind();
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

        private void LoadUserAccounts()
        {
            string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;";
            databaseConnection += "Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

            using (OleDbConnection conn = new OleDbConnection(databaseConnection))
            {
                try
                {

                    conn.Open();

                    // Query to bind data to the GridView
                    string accQuery = @"SELECT 
                                            EU.EmployeeID, EU.GivenName & ' ' & EU.MiddleName & ' ' & EU.LastName AS FullName, EU.Department, EU.ScheduleID,
                                            AC.Role
                                            FROM EMPLOYEE EU LEFT JOIN ACCOUNT AC ON
                                            EU.EmployeeID = AC.EmployeeID
                                            WHERE AC.Role = 'Employee' OR AC.Role = 'Supervisor'";
                    using (OleDbCommand Accountcmd = new OleDbCommand(accQuery, conn))
                    {

                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(Accountcmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            adapter.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gvAccounts.DataSource = dt;
                                gvAccounts.DataBind();
                            }
                            else
                            {
                                gvAccounts.DataSource = null;
                                gvAccounts.DataBind();
                            }
                        }
                    }

                    // Query to get EmployeeID 
                    string drpQuery = @"
                SELECT EU.EmployeeID 
                FROM EMPLOYEE EU 
                LEFT JOIN ACCOUNT AC ON EU.EmployeeID = AC.EmployeeID 
                WHERE AC.Role = 'Employee' OR AC.Role = 'Supervisor'";
                    using (OleDbCommand drpcmd = new OleDbCommand(drpQuery, conn))
                    {
                        using (OleDbDataReader reader = drpcmd.ExecuteReader())
                        {
                            ddlEmployeeID.Items.Clear(); // Clear existing items
                            ddlEmployeeID.Items.Add(new ListItem("Select an Employee Number", "")); // Add default item

                            while (reader.Read())
                            {
                                string employee = reader["EmployeeID"].ToString();
                                ddlEmployeeID.Items.Add(new ListItem(employee, employee));
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
                       EU.Department, EU.ScheduleID, AC.Role
                FROM EMPLOYEE EU 
                LEFT JOIN ACCOUNT AC ON EU.EmployeeID = AC.EmployeeID
                WHERE EU.EmployeeID = ?";
                    using (OleDbCommand selectedcmd = new OleDbCommand(selectedQuery, conn))
                    {
                        selectedcmd.Parameters.AddWithValue("?", selectedID);

                        using (OleDbDataReader selectedReader = selectedcmd.ExecuteReader())
                        {
                            if (selectedReader.Read())
                            {
                                lblEmployeeName.Text = selectedReader["FullName"].ToString();
                                lblDepartment.Text = selectedReader["Department"].ToString();
                                lblRole.Text = selectedReader["Role"].ToString();

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

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            string empID = ddlEmployeeID.SelectedValue;
            string schedID = txtScheduleID.Text;

            if (string.IsNullOrWhiteSpace(txtScheduleID.Text) ||
                string.IsNullOrWhiteSpace(ddlEmployeeID.SelectedValue))
            {
                Response.Write("<script>alert('Error: All fields are required!');</script>");
                return;
            }

            if (!string.IsNullOrEmpty(schedID) && !string.IsNullOrEmpty(empID))
            {
                string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

                string query = "UPDATE EMPLOYEE SET ScheduleID = ? WHERE EmployeeID = ?";
                using (OleDbConnection conn = new OleDbConnection(databaseConnection))
                {
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", schedID);
                        cmd.Parameters.AddWithValue("?", empID);
                        conn.Open();

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

                ddlEmployeeID.SelectedIndex = 0;
                lblDepartment.Text = "";
                lblEmployeeName.Text = "";
                lblRole.Text = "";
                txtScheduleID.Text = "";

                LoadUserAccounts();
            }

        }
    }
}