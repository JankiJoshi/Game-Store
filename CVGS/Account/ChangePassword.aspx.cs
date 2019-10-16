using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using CVGS.Models;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;

namespace CVGS.Account
{
    public partial class ChangePassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ChangePass_Click(object sender, EventArgs e)
        {
            String oldPassword = "";
            String newPassword = "";
            String user = Session["User"].ToString();

            LoginModel logins = null;

            using (var ctx = new CVGSEntities())
            {
                logins = ctx.logins
                    .Where(s => s.username == user)
                    .Select(s => new LoginModel()
                {
                    user = s.username,
                    pword = s.password
                }).FirstOrDefault<LoginModel>();
            }

            if (OldPass.Text.Trim() != null || OldPass.Text.Trim() != "")
            {
                oldPassword = OldPass.Text.Trim();
            }
            if (NewPassword.Text.Trim() != null || NewPassword.Text.Trim() != "")
            {
                newPassword = NewPassword.Text.Trim();
            }
            if (logins.pword == oldPassword)
            {
                login log = new login();
                log.username = user;
                log.password = newPassword;

                
                using (var ctx = new CVGSEntities())
                {
                    if (ModelState.IsValid)
                    {
                        ctx.Entry(log).State = EntityState.Modified;
                        ctx.SaveChanges();
                    }

                }
                Response.Redirect("/Default");
                
            }
            else
            {
                ErrorMessage.Text = "Current password does not match entered value.";
            }
        }
    }
}