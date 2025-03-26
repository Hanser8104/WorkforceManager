using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class WebForm11 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadWorkSchedules();
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

                    // Query to bind data to the GridView
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

        protected void btnAddSchedule_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtWorkdays.Text) || string.IsNullOrEmpty(txtShiftStart.Text))
            {
                Response.Write("<script>alert('Please fill all the required fields before submitting.');</script>");
                return;
            }

            string workdays = txtWorkdays.Text;
            DateTime shiftStart;

            if (!DateTime.TryParse(txtShiftStart.Text, out shiftStart))
            {
                Response.Write("<script>alert('Invalid Shift Start time.');</script>");
                return;
            }
                                                                    // Don't Remove comment
            DateTime shiftEnd = shiftStart.AddHours(8);            // Shift End = Start + 8 hours
            DateTime breakStart = shiftStart.AddHours(2);          // Break Start = Start + 2 hours
            DateTime breakEnd = breakStart.AddMinutes(15);         // Break End = Break Start + 15 minutes
            DateTime lunchStart = shiftStart.AddHours(4);        // Lunch Start = Start + 4 hours
            DateTime lunchEnd = lunchStart.AddHours(1);           // Lunch End = Lunch Start + 1 hour

            string shiftStartStr = shiftStart.ToString("hh:mmtt");
            string shiftEndStr = shiftEnd.ToString("hh:mmtt");
            string breakStartStr = breakStart.ToString("hh:mmtt");
            string breakEndStr = breakEnd.ToString("hh:mmtt");
            string lunchStartStr = lunchStart.ToString("hh:mmtt");
            string lunchEndStr = lunchEnd.ToString("hh:mmtt");

            string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;";
            databaseConnection += "Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

            using (OleDbConnection conn = new OleDbConnection(databaseConnection))
            {
                try
                {
                    conn.Open();

                    string scheduleID = GenerateScheduleID(conn);

                    string insertQuery = @"
                INSERT INTO WORK_SCHEDULE (ScheduleID, WorkDays, ShiftStart, ShiftEnd, BreakStart, BreakEnd, LunchStart, LunchEnd)
                VALUES (@ScheduleID, @WorkDays, @ShiftStart, @ShiftEnd, @BreakStart, @BreakEnd, @LunchStart, @LunchEnd)";

                    using (OleDbCommand cmd = new OleDbCommand(insertQuery, conn))
                    {
                        // Add parameters
                        cmd.Parameters.AddWithValue("@ScheduleID", scheduleID);
                        cmd.Parameters.AddWithValue("@Workdays", workdays);
                        cmd.Parameters.AddWithValue("@ShiftStart", shiftStartStr);
                        cmd.Parameters.AddWithValue("@ShiftEnd", shiftEndStr);
                        cmd.Parameters.AddWithValue("@BreakStart", breakStartStr);
                        cmd.Parameters.AddWithValue("@BreakEnd", breakEndStr);
                        cmd.Parameters.AddWithValue("@LunchStart", lunchStartStr);
                        cmd.Parameters.AddWithValue("@LunchEnd", lunchEndStr);

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Response.Write("<script>alert('Schedule added successfully!');</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('Failed to add the schedule.');</script>");
                        }
                    }

                    // Refresh the data (if needed)
                    LoadWorkSchedules(); // Assuming this method refreshes the GridView or other UI elements
                    txtBreakEnd.Text = "";
                    txtBreakStart.Text = "";
                    txtLunchEnd.Text = "";
                    txtLunchStart.Text = "";
                    txtShiftEnd.Text = "";
                    txtShiftStart.Text = "";
                    txtWorkdays.Text = "";
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
            }
            LoadWorkSchedules();
        }

        // Helper method to generate the ScheduleID
        private string GenerateScheduleID(OleDbConnection conn)
        {
            string scheduleID = "SCH-00001"; // Default value if no records exist

            // Query to count existing schedules
            string countQuery = "SELECT COUNT(*) FROM WORK_SCHEDULE";

            using (OleDbCommand countCmd = new OleDbCommand(countQuery, conn))
            {
                int count = Convert.ToInt32(countCmd.ExecuteScalar()); // Get the count of existing schedules
                scheduleID = "SCH-" + (count + 1).ToString("D5"); // Format as SCH-00001, SCH-00002, etc.
            }

            return scheduleID;
        }

        protected void btnModifySchedule_Click(object sender, EventArgs e)
        {
            Response.Redirect("ModifyUserSchedule.aspx");
        }
    }
}