using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace WebApplication2
{
    public partial class WebForm7 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EmployeeLeaveRecords();
            }

            this.UnobtrusiveValidationMode = UnobtrusiveValidationMode.WebForms;

            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
            {
                Path = "~/Scripts/jquery-3.6.0.min.js" 
            });
        }
        

        protected void ddlRequestID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string requestID = ddlRequestID.SelectedValue;

            if (!string.IsNullOrEmpty(requestID))
            {
                // Fetch Employee Details
                FetchEmployeeDetails(requestID);

                // Fetch Leave Details
                FetchLeaveDetails(requestID);

                CalculateAndDisplayWorkforceDemand();
            }
        }

        private void EmployeeLeaveRecords()
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

                    // Query to get RequestID for the logged-in employee's pending leave requests
                    string requestQuery = "SELECT RequestID FROM LEAVE_REQUEST WHERE NOT EmployeeID = ? AND ApprovalStatus = 'Pending'";
                    using (OleDbCommand requestCmd = new OleDbCommand(requestQuery, conn))
                    {
                        requestCmd.Parameters.AddWithValue("?", employeeID);

                        using (OleDbDataReader reader = requestCmd.ExecuteReader())
                        {
                            ddlRequestID.Items.Clear(); // Clear existing items
                            ddlRequestID.Items.Add(new ListItem("Select a Request Number", "")); // Add default item

                            while (reader.Read())
                            {
                                string requestID = reader["RequestID"].ToString();
                                ddlRequestID.Items.Add(new ListItem(requestID, requestID));
                            }
                        }
                    }

                    // Load Employee Leave Records
                    string leaveQuery = @"
                SELECT 
                    EU.EmployeeID, 
                    EU.LastName + ', ' + EU.GivenName + ' ' + EU.MiddleName AS EmployeeName,
                    FORMAT(LR.StartDate, 'MM/dd/yyyy') + ' - ' + FORMAT(LR.EndDate, 'MM/dd/yyyy') AS LeavePeriod,
                    LR.ApprovalStatus, LR.RequestID
                FROM 
                    EMPLOYEE EU
                INNER JOIN 
                    LEAVE_REQUEST LR 
                ON 
                    EU.EmployeeID = LR.EmployeeID
                WHERE 
                    NOT EU.EmployeeID = ? AND LR.ApprovalStatus = 'Pending'";

                    using (OleDbCommand leaveCmd = new OleDbCommand(leaveQuery, conn))
                    {
                        leaveCmd.Parameters.AddWithValue("?", employeeID);

                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(leaveCmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            adapter.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gvLeaveRecords.DataSource = dt;
                                gvLeaveRecords.DataBind();
                            }
                            else
                            {
                                gvLeaveRecords.DataSource = null;
                                gvLeaveRecords.DataBind();
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

        private void FetchEmployeeDetails(string requestID)
        {
            string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

            string query = @"
        SELECT 
            EU.EmployeeID, 
            EU.LastName, 
            EU.GivenName, 
            EU.Department, 
            EU.SupervisorID, 
            EU.LeaveBalance
        FROM 
            EMPLOYEE EU
        INNER JOIN 
            LEAVE_REQUEST LR 
        ON 
            EU.EmployeeID = LR.EmployeeID
        WHERE 
            LR.RequestID = ?";

            using (OleDbConnection conn = new OleDbConnection(databaseConnection))
            {
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", requestID);
                    conn.Open();
                    OleDbDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblEmployeeName.Text = reader["LastName"].ToString() + ", " + reader["GivenName"].ToString();
                        lblEmployeeID.Text = reader["EmployeeID"].ToString();
                        lblDepartment.Text = reader["Department"].ToString();
                        lblSupervisorID.Text = reader["SupervisorID"].ToString();
                        lblLeaveBalance.Text = reader["LeaveBalance"].ToString();
                    }
                }
            }
        }

        private void FetchLeaveDetails(string requestID)
        {
            string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

            string query = "SELECT LeaveType, StartDate, EndDate, Reason FROM LEAVE_REQUEST WHERE RequestID = ?";
            using (OleDbConnection conn = new OleDbConnection(databaseConnection))
            {
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("?", requestID);
                    conn.Open();
                    OleDbDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        lblLeaveType.Text = reader["LeaveType"].ToString();
                        lblLeavePeriod.Text = reader["StartDate"].ToString() + " - " + reader["EndDate"].ToString();
                        lblReason.Text = reader["Reason"].ToString();
                    }
                }
            }
        }

        private void CalculateAndDisplayWorkforceDemand()
        {
            string startDateStr = lblLeavePeriod.Text.Split(new[] { " - " }, StringSplitOptions.None)[0];
            DateTime startDate;

            if (DateTime.TryParse(startDateStr, out startDate))
            {
                string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

                int totalEmployees = 0;
                int employeesWorking = 0;
                int employeesOnLeave = 0;

                using (OleDbConnection conn = new OleDbConnection(databaseConnection))
                {
                    conn.Open();

                    // Get total employees
                    string totalEmployeesQuery = @"
                SELECT COUNT(*) 
                FROM EMPLOYEE E 
                INNER JOIN ACCOUNT A ON E.EmployeeID = A.EmployeeID 
                WHERE A.Role = 'Employee'";
                    using (OleDbCommand cmd = new OleDbCommand(totalEmployeesQuery, conn))
                    {
                        totalEmployees = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Get employees scheduled to work 
                    string workingEmployeesQuery = @"
                SELECT COUNT(*) 
                FROM ((WORK_SCHEDULE WS 
                INNER JOIN EMPLOYEE E ON WS.ScheduleID = E.ScheduleID)
                INNER JOIN ACCOUNT A ON E.EmployeeID = A.EmployeeID)
                WHERE A.Role = 'Employee'
                AND WS.Workdays LIKE '%' & ? & '%'";
                    using (OleDbCommand cmd = new OleDbCommand(workingEmployeesQuery, conn))
                    {
                        string dayOfWeek = startDate.ToString("dddd"); // e.g., "Monday"
                        cmd.Parameters.AddWithValue("?", dayOfWeek);
                        employeesWorking = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Get employees on approved leave (with role "employees")
                    string employeesOnLeaveQuery = @"
                SELECT COUNT(*) 
                FROM (
                    SELECT DISTINCT LR.EmployeeID 
                    FROM LEAVE_REQUEST LR 
                    INNER JOIN ACCOUNT A ON LR.EmployeeID = A.EmployeeID 
                    WHERE A.Role = 'Employee'
                    AND LR.ApprovalStatus = 'Approved' 
                    AND ? BETWEEN LR.StartDate AND LR.EndDate
                )";
                    using (OleDbCommand cmd = new OleDbCommand(employeesOnLeaveQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("?", startDate);
                        employeesOnLeave = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                // Calculate demand level
                int employeesAvailable = employeesWorking - employeesOnLeave;
                double workPercentage = (totalEmployees > 0) ? (employeesAvailable * 100.0 / totalEmployees) : 0;

                string demandLevel;
                string color;
                if (workPercentage >= 70)
                {
                    demandLevel = "Low Demand";
                    color = "green";
                }
                else if (workPercentage > 50)
                {
                    demandLevel = "Medium Demand";
                    color = "orange";
                }
                else
                {
                    demandLevel = "High Demand";
                    color = "red";
                }

                lblWorkforce.Text = demandLevel;
                lblWorkforce.Style["color"] = color;
            }
            else
            {
                lblWorkforce.Text = "Invalid date format";
                lblWorkforce.Style["color"] = "red";
            }
        }




        protected void btnSave_Click(object sender, EventArgs e)
        {
            string requestID = ddlRequestID.SelectedValue;
            string approvalStatus = drpLeaveStatus.SelectedValue;
            string supervisorRemarks = txtSupervisorRemark.Text;
            string EmpID = lblEmployeeID.Text;

            if (string.IsNullOrEmpty(requestID) || string.IsNullOrEmpty(approvalStatus) || string.IsNullOrEmpty(supervisorRemarks) || string.IsNullOrEmpty(EmpID))
            {
                Response.Write("<script>alert('Please fill all the required fields before submitting.');</script>");
                return;
            }

            if (!string.IsNullOrEmpty(requestID) && !string.IsNullOrEmpty(approvalStatus))
            {
                string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

                string query = "UPDATE LEAVE_REQUEST SET ApprovalStatus = ?, SupervisorRemarks = ? WHERE RequestID = ?";
                using (OleDbConnection conn = new OleDbConnection(databaseConnection))
                {
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", approvalStatus);
                        cmd.Parameters.AddWithValue("?", supervisorRemarks);
                        cmd.Parameters.AddWithValue("?", requestID);
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

                if (drpLeaveStatus.SelectedValue == "Accepted")
                {
                    string leaveBalanceQuery = "UPDATE EMPLOYEE SET LeaveBalance = LeaveBalance - 1 WHERE EmployeeID = ?";
                    using (OleDbConnection conn = new OleDbConnection(databaseConnection))
                    {
                        using (OleDbCommand cmd = new OleDbCommand(leaveBalanceQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("?", EmpID);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                requestID = "";
                drpLeaveStatus.SelectedIndex = 0;
                ddlRequestID.SelectedIndex = 0;
                txtSupervisorRemark.Text = "";
                lblDepartment.Text = "";
                lblEmployeeID.Text = "";
                lblEmployeeName.Text = "";
                lblLeaveBalance.Text = "";
                lblLeavePeriod.Text = "";
                lblLeaveType.Text = "";
                lblReason.Text = "";
                lblWorkforce.Text = "";
                lblSupervisorID.Text = "";

                EmployeeLeaveRecords();
            }
        }
    }
}