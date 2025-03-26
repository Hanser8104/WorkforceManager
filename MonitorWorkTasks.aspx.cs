using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Collections;

namespace WebApplication2
{
    public partial class WebForm8 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get EmployeeID from session (stored during login)
            string employeeID = Session["EmployeeID"]?.ToString();
            if (string.IsNullOrEmpty(employeeID))
            {
                Response.Redirect("Login.aspx"); // Redirect if session is lost
                return;
            }
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            string department = ddlDepartment.SelectedValue;
            LoadEmployeeTables(department);
        }

        private void LoadEmployeeTables(string chosenDepartment)
        {
            string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;";
            databaseConnection += "Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");


            using (OleDbConnection conn = new OleDbConnection(databaseConnection))
            {
                try
                {
                    conn.Open();

                    // Query to bind data to the GridView
                    string historyQuery = @"SELECT 
                                          WT.TaskID, 
                                          EU.EmployeeID, 
                                          EU.GivenName & ' ' & EU.MiddleName & ' ' & EU.LastName AS FullName,
                                          WT.TaskDate, 
                                          WT.TaskDetails
                                          FROM WORK_TASK WT 
                                          LEFT JOIN EMPLOYEE EU ON WT.EmployeeID = EU.EmployeeID
                                          WHERE EU.Department = ? AND WT.Status = 'Pending'";
                    using (OleDbCommand historycmd = new OleDbCommand(historyQuery, conn))
                    {
                        historycmd.Parameters.AddWithValue("?", chosenDepartment);

                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(historycmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            adapter.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gvPending.DataSource = dt;
                                gvPending.DataBind();
                            }
                            else
                            {
                                gvPending.DataSource = null;
                                gvPending.DataBind();
                            }
                        }
                    }

                    // Query to bind data to the GridView
                    string progressQuery = @"SELECT 
                                          WT.TaskID, 
                                          EU.EmployeeID, 
                                          EU.GivenName & ' ' & EU.MiddleName & ' ' & EU.LastName AS FullName,
                                          WT.TaskDate, 
                                          WT.TaskDetails
                                          FROM WORK_TASK WT 
                                          LEFT JOIN EMPLOYEE EU ON WT.EmployeeID = EU.EmployeeID
                                          WHERE EU.Department = ? AND WT.Status = 'In Progress'";
                    using (OleDbCommand progresscmd = new OleDbCommand(progressQuery, conn))
                    {
                        progresscmd.Parameters.AddWithValue("?", chosenDepartment);

                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(progresscmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            adapter.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gvInProgress.DataSource = dt;
                                gvInProgress.DataBind();
                            }
                            else
                            {
                                gvInProgress.DataSource = null;
                                gvInProgress.DataBind();
                            }
                        }
                    }

                    // Query to bind data to the GridView
                    string doneQuery = @"SELECT 
                                          WT.TaskID, 
                                          EU.EmployeeID, 
                                          EU.GivenName & ' ' & EU.MiddleName & ' ' & EU.LastName AS FullName, 
                                          WT.TaskDate, 
                                          WT.TaskDetails
                                          FROM WORK_TASK WT 
                                          LEFT JOIN EMPLOYEE EU ON WT.EmployeeID = EU.EmployeeID
                                          WHERE EU.Department = ? AND WT.Status = 'Done'";
                    using (OleDbCommand donecmd = new OleDbCommand(doneQuery, conn))
                    {
                        donecmd.Parameters.AddWithValue("?", chosenDepartment);

                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(donecmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            adapter.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gvDone.DataSource = dt;
                                gvDone.DataBind();
                            }
                            else
                            {
                                gvDone.DataSource = null;
                                gvDone.DataBind();
                            }
                        }
                    }

                    lblcompleted.Visible = true;
                    lblinprogress.Visible = true;
                    lblpending.Visible = true;

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
            }
        }
    }
}