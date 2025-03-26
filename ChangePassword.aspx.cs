using System;
using System.Data.OleDb;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace WebApplication2
{
    public partial class WebForm6 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.UnobtrusiveValidationMode = UnobtrusiveValidationMode.WebForms;

            // Ensure jQuery mapping if needed
            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
            {
                Path = "~/Scripts/jquery-3.6.0.min.js"
            });

            if (!IsPostBack)
            {
                // Optionally, load user details or clear messages.
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string employeeID = Session["EmployeeID"]?.ToString();
            string currentPassword = txtCurrentPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmNewPassword = txtConfirmNewPassword.Text.Trim();

            if (string.IsNullOrEmpty(employeeID))
            {
                lblMessage.Text = "You must be logged in to change your password.";
                return;
            }

            if (newPassword != confirmNewPassword)
            {
                lblMessage.Text = "New Password and Confirm New Password do not match.";
                return;
            }

            string databaseConnection = "Provider=Microsoft.Ace.OLEDB.12.0;";
            databaseConnection += "Data Source=" + Server.MapPath("~/App_Data/WorkforceLeaveManagerDb.MDB");

            using (OleDbConnection conn = new OleDbConnection(databaseConnection))
            {
                try
                {
                    conn.Open();

                    // Retrieve stored password for the employee
                    string checkPasswordQuery = "SELECT Password FROM ACCOUNT WHERE EmployeeID = ?";
                    using (OleDbCommand checkPasswordCmd = new OleDbCommand(checkPasswordQuery, conn))
                    {
                        checkPasswordCmd.Parameters.AddWithValue("?", employeeID);
                        string storedPassword = checkPasswordCmd.ExecuteScalar()?.ToString();

                        // Hash the entered current password using SHA-1 (uppercase)
                        string hashedCurrentPassword = HashPassword(currentPassword);

                        if (storedPassword != hashedCurrentPassword)
                        {
                            lblMessage.Text = "Current Password is incorrect.";
                            return;
                        }
                    }

                    // Hash the new password before storing
                    string hashedNewPassword = HashPassword(newPassword);

                    string updatePasswordQuery = "UPDATE ACCOUNT SET [Password] = ? WHERE EmployeeID = ?";
                    using (OleDbCommand updatePasswordCmd = new OleDbCommand(updatePasswordQuery, conn))
                    {
                        updatePasswordCmd.Parameters.AddWithValue("?", hashedNewPassword);
                        updatePasswordCmd.Parameters.AddWithValue("?", employeeID);

                        int rowsAffected = updatePasswordCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            lblMessage.Text = "Password changed successfully!";
                            lblMessage.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            lblMessage.Text = "Failed to update password.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }
        }

        // SHA-1 Hashing function, outputs uppercase hex string
        private string HashPassword(string password)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder hashString = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashString.Append(b.ToString("X2")); // "X2" outputs uppercase hex
                }
                return hashString.ToString();
            }
        }
    }
}
