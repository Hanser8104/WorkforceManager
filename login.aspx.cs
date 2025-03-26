using System;
using System.Security.Cryptography;
using System.Text;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication2
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.UnobtrusiveValidationMode = UnobtrusiveValidationMode.WebForms;

            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
            {
                Path = "~/Scripts/jquery-3.6.0.min.js" // Replace with the actual path to your jQuery file
            });
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string username = usernametxt.Text.Trim();
            string password = passwordtxt.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Response.Write("<script>alert('Please enter username and password.');</script>");
                return;
            }

            // ✅ Hash the entered password before comparison
            string hashedPassword = HashPassword(password);

            string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;";
            databaseConnection += "Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

            string query = "SELECT EmployeeID, Role, AccountStatus FROM ACCOUNT WHERE Username = ? AND Password = ?";

            using (OleDbConnection conn = new OleDbConnection(databaseConnection))
            {
                try
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", username);
                        cmd.Parameters.AddWithValue("?", hashedPassword); // hashed

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string role = reader["Role"].ToString();
                                string accountStatus = reader["AccountStatus"].ToString();

                                if (accountStatus == "Deactivated")
                                {
                                    Response.Write("<script>alert('Your account has been deactivated. Contact Admin.');</script>");
                                    return;
                                }
                                // Method ni semi-pogi, wag galawin
                                Session["EmployeeID"] = reader["EmployeeID"].ToString();
                                Session["Role"] = role;

                                Response.Write("<script>alert('Redirecting to User Dashboard');</script>");
                                Response.Redirect("EmployeeDashboard.aspx");

                            }
                            else
                            {
                                Response.Write("<script>alert('Invalid username or password.');</script>");
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

        // ✅ Hashing function (SHA-1 for compatibility)
        private string HashPassword(string password)
        {
            using (SHA1Managed sha1 = new SHA1Managed()) // If using SHA-1
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder hashString = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashString.Append(b.ToString("x2"));
                }
                return hashString.ToString();
            }
        }
    }
}
