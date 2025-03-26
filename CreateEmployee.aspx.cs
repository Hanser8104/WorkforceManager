using System;
using System.Data;
using System.Data.OleDb;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace WebApplication2
{
    public partial class WebForm10 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadWorkSchedules();
                LoadUserAccounts();
                lblGeneratedEmployeeID.Text = GenerateEmployeeID();
                string randomPassword = GenerateRandomPassword();
                Session["GeneratedPassword"] = randomPassword; // Store the password in session
                lblGeneratedPassword.Text = randomPassword;
            }

            if (Session["DownloadFlag"] != null && (bool)Session["DownloadFlag"])
            {
                Session["DownloadFlag"] = null;
                ClientScript.RegisterStartupScript(this.GetType(), "DownloadFile",
                    $@"<script>
                window.open('DownloadHandler.ashx', '_blank');
                setTimeout(function() {{ window.location = window.location.href; }}, 1000);
               </script>");
            }
        }
        public class DownloadHandler : IHttpHandler
        {
            public void ProcessRequest(HttpContext context)
            {
                string filePath = context.Server.MapPath("~/App_Data/EmployeeCredentials.txt");

                context.Response.ContentType = "text/plain";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=EmployeeCredentials.txt");
                context.Response.TransmitFile(filePath);
                context.Response.End();
            }

            public bool IsReusable => false;
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

                    string accQuery = @"SELECT 
                                            EU.EmployeeID, EU.GivenName & ' ' & EU.MiddleName & ' ' & EU.LastName AS FullName,
                                            AC.AccountStatus, EU.EmploymentStatus, EU.Department
                                            FROM EMPLOYEE EU LEFT JOIN ACCOUNT AC ON
                                            EU.EmployeeID = AC.EmployeeID
                                            WHERE AC.Role = 'Supervisor' AND EU.EmploymentStatus = 'Active'";
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

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
            }
        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            string role = ddlRole.SelectedValue;
            if (role == "Admin")
            {
                supervisorRow.Style["display"] = "none";
                scheduleRow.Style["display"] = "none";
                leaveRow.Style["display"] = "none";
            }
            else if (role == "Supervisor")
            {
                supervisorRow.Style["display"] = "none";
                scheduleRow.Style["display"] = "table-row";
                leaveRow.Style["display"] = "table-row";
            }
            else if (role == "Employee")
            {
                supervisorRow.Style["display"] = "table-row";
                scheduleRow.Style["display"] = "table-row";
                leaveRow.Style["display"] = "table-row";
            }
            else
            {
                supervisorRow.Style["display"] = "table-row";
                scheduleRow.Style["display"] = "table-row";
                leaveRow.Style["display"] = "table-row";
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtMiddleName.Text) ||
                string.IsNullOrWhiteSpace(ddlDepartment.SelectedValue) ||
                string.IsNullOrWhiteSpace(ddlEmploymentStatus.SelectedValue) ||
                string.IsNullOrWhiteSpace(ddlRole.SelectedValue))
            {
                Response.Write("<script>alert('Error: All fields are required!');</script>");
                return;
            }

            string newEmployeeID = GenerateEmployeeID();
            string role = ddlRole.SelectedValue;
            string password = Session["GeneratedPassword"] != null ? Session["GeneratedPassword"].ToString() : GenerateRandomPassword();
            string hashedPassword = HashSHA1(password);

            string scheduleID = string.IsNullOrWhiteSpace(txtScheduleID.Text) ? null : txtScheduleID.Text;
            string supervisorID = string.IsNullOrWhiteSpace(txtSupervisorID.Text) ? null : txtSupervisorID.Text;
            string leaveBalance = string.IsNullOrWhiteSpace(txtLeaveBalance.Text) ? null : txtLeaveBalance.Text;

            if (role == "Admin")
            {
                scheduleID = null;
                supervisorID = null;
                leaveBalance = null;
            }
            else if (role == "Supervisor")
            {
                if (string.IsNullOrWhiteSpace(scheduleID) || string.IsNullOrWhiteSpace(leaveBalance))
                {
                    Response.Write("<script>alert('Error: Supervisor must have Schedule ID and Leave Balance!');</script>");
                    return;
                }
                supervisorID = null; // No supervisor for a Supervisor role
            }
            else if (role == "Employee")
            {
                if (string.IsNullOrWhiteSpace(scheduleID) || string.IsNullOrWhiteSpace(supervisorID) || string.IsNullOrWhiteSpace(leaveBalance))
                {
                    Response.Write("<script>alert('Error: Employee must have Schedule ID, Supervisor ID, and Leave Balance!');</script>");
                    return;
                }
            }


            string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;";
            databaseConnection += "Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

            using (OleDbConnection conn = new OleDbConnection(databaseConnection))
            {
                conn.Open();

                string insertEmployeeQuery = "INSERT INTO EMPLOYEE (EmployeeID, ScheduleID, LastName, GivenName, MiddleName, Department, SupervisorID, EmploymentStatus, LeaveBalance) VALUES (@EmployeeID, @ScheduleID, @LastName, @GivenName, @MiddleName, @Department, @SupervisorID, @EmploymentStatus, @LeaveBalance)";
                using (OleDbCommand cmd = new OleDbCommand(insertEmployeeQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", newEmployeeID);
                    cmd.Parameters.AddWithValue("@ScheduleID", string.IsNullOrEmpty(scheduleID) ? (object)DBNull.Value : scheduleID);
                    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@GivenName", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@MiddleName", txtMiddleName.Text);
                    cmd.Parameters.AddWithValue("@Department", ddlDepartment.SelectedValue);
                    cmd.Parameters.AddWithValue("@SupervisorID", string.IsNullOrEmpty(supervisorID) ? (object)DBNull.Value : supervisorID);
                    cmd.Parameters.AddWithValue("@EmploymentStatus", ddlEmploymentStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@LeaveBalance", string.IsNullOrEmpty(leaveBalance) ? (object)DBNull.Value : leaveBalance);
                    cmd.ExecuteNonQuery();
                }

                string insertAccountQuery = "INSERT INTO ACCOUNT (EmployeeID, Username, [Password], Role, AccountStatus) VALUES (@EmployeeID, @Username, @Password, @Role, 'Activated')";
                using (OleDbCommand cmd = new OleDbCommand(insertAccountQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", newEmployeeID);
                    cmd.Parameters.AddWithValue("@Username", newEmployeeID);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.ExecuteNonQuery();
                }
            }


            string csvLine = $"{newEmployeeID}," +
                             $"{txtFirstName.Text}," +
                             $"{txtMiddleName.Text}," +
                             $"{txtLastName.Text}," +
                             $"{ddlDepartment.SelectedValue}," +
                             $"{supervisorID ?? "NULL"}," +
                             $"{ddlEmploymentStatus.SelectedValue}," +
                             $"{scheduleID ?? "NULL"}," +
                             $"{newEmployeeID}," +
                             $"{password}," +
                             $"{role}";

            txtLastName.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            ddlDepartment.SelectedIndex = 0;
            ddlEmploymentStatus.SelectedIndex = 0;
            ddlRole.SelectedIndex = 0;
            txtSupervisorID.Text = string.Empty;
            txtScheduleID.Text = string.Empty;
            txtLeaveBalance.Text = string.Empty;


            lblGeneratedEmployeeID.Text = GenerateEmployeeID();
            lblGeneratedPassword.Text = GenerateRandomPassword();

            string fileName = "EmployeeCredentials.txt";
            string filePath = Server.MapPath($"~/App_Data/{fileName}");

            if (!File.Exists(filePath))
            {
                string header = @"EMPLOYEE INFORMATION FOR ACCOUNT DISSEMINATION

EmployeeID,FirstName,MiddleName,LastName,Department,SupervisorID,EmploymentStatus,ScheduleID,Username,Password,Role
";
                File.WriteAllText(filePath, header + Environment.NewLine);
            }

            File.AppendAllText(filePath, csvLine + Environment.NewLine);

            Session["DownloadFlag"] = true;
            Response.Redirect(Request.Url.AbsoluteUri);
        }



        private string GenerateEmployeeID()
        {
            string year = DateTime.Now.Year.ToString();
            string newID = year + "00001";
            string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

            using (OleDbConnection conn = new OleDbConnection(databaseConnection))
            {
                conn.Open();
                string query = "SELECT MAX(EmployeeID) FROM EMPLOYEE WHERE EmployeeID LIKE @YearPattern";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@YearPattern", year + "%");
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        string latestID = result.ToString();
                        int counter = int.Parse(latestID.Substring(4)) + 1;
                        newID = year + counter.ToString("D5");
                    }
                }
            }
            return newID;
        }

        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            char[] password = new char[12];
            for (int i = 0; i < 12; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }
            return new string(password);
        }

        private string HashSHA1(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            }
        }


    }
}