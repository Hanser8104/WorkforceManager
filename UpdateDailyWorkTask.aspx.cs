using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadWorkTaskData();
            }
            this.UnobtrusiveValidationMode = UnobtrusiveValidationMode.WebForms;

            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
            {
                Path = "~/Scripts/jquery-3.6.0.min.js" // Replace with the actual path to your jQuery file
            });
        }

        private void LoadWorkTaskData()
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

                    // Query to get TaskID for the DropDownList
                    string taskQuery = "SELECT TaskID FROM WORK_TASK WHERE EmployeeID = ? AND Status ='In Progress' OR Status ='Pending'";
                    using (OleDbCommand taskCmd = new OleDbCommand(taskQuery, conn))
                    {
                        taskCmd.Parameters.AddWithValue("?", employeeID);

                        using (OleDbDataReader reader = taskCmd.ExecuteReader())
                        {
                            ddlTaskNumber.Items.Clear(); // Clear existing items
                            ddlTaskNumber.Items.Add(new ListItem("Select Task to Update", "")); // Add default item

                            while (reader.Read())
                            {
                                string taskID = reader["TaskID"].ToString();
                                ddlTaskNumber.Items.Add(new ListItem(taskID, taskID));
                            }
                        }
                    }

                    // Query to bind data to the GridView
                    string workforceQuery = "SELECT TaskID, TaskDate, TaskDetails, Status FROM WORK_TASK WHERE EmployeeID = ? AND Status = 'Pending' OR Status = 'In Progress'";
                    using (OleDbCommand workforcecmd = new OleDbCommand(workforceQuery, conn))
                    {
                        workforcecmd.Parameters.AddWithValue("?", employeeID);

                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(workforcecmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            adapter.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gvWorkforce.DataSource = dt;
                                gvWorkforce.DataBind();
                            }
                            else
                            {
                                gvWorkforce.DataSource = null;
                                gvWorkforce.DataBind();
                            }
                        }
                    }

                    // Query to bind data to the GridView
                    string historyQuery = "SELECT TaskID, TaskDate, TaskDetails, Status FROM WORK_TASK WHERE EmployeeID = ? AND Status = 'Done'";
                    using (OleDbCommand historycmd = new OleDbCommand(historyQuery, conn))
                    {
                        historycmd.Parameters.AddWithValue("?", employeeID);

                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(historycmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            adapter.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                GvTaskHistory.DataSource = dt;
                                GvTaskHistory.DataBind();
                            }
                            else
                            {
                                GvTaskHistory.DataSource = null;
                                GvTaskHistory.DataBind();
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

            string selectedTaskID = ddlTaskNumber.SelectedValue;
            string chosenStatus = ddlTaskStatus.SelectedValue;

            if (string.IsNullOrEmpty(selectedTaskID) || string.IsNullOrEmpty(chosenStatus))
            {
                Response.Write("<script>alert('Please fill all the required fields before submitting.');</script>");
                return;
            }

            if (!string.IsNullOrEmpty(selectedTaskID) && !string.IsNullOrEmpty(chosenStatus))
            {

                string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;";
                databaseConnection += "Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

                using (OleDbConnection conn = new OleDbConnection(databaseConnection))
                {
                    try
                    {
                        conn.Open();

                        // Update query with parameters
                        string taskUpdate = @"UPDATE WORK_TASK SET Status = @NewStatus WHERE TaskID = @NewTaskID AND EmployeeID = @EmployeeID";
                        using (OleDbCommand taskCmd = new OleDbCommand(taskUpdate, conn))
                        {
                            // Add parameters
                            taskCmd.Parameters.AddWithValue("@NewStatus", chosenStatus);
                            taskCmd.Parameters.AddWithValue("@NewTaskID", selectedTaskID);
                            taskCmd.Parameters.AddWithValue("@EmployeeID", Session["EmployeeID"]?.ToString());

                            // Execute the update
                            int rowsAffected = taskCmd.ExecuteNonQuery();
                        }

                        // Refresh the data
                        LoadWorkTaskData();

                        ddlTaskStatus.SelectedIndex = 0;
                        ddlTaskNumber.SelectedIndex = 0;
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                    }
                }
            }
            LoadWorkTaskData();
        }


    }
}