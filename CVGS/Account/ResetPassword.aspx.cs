using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using CVGS.Models;

namespace CVGS.Account
{
    public partial class ResetPassword : Page
    {

        protected void ResetPass_Click(object sender, EventArgs e)
        {
            string code = Verification.Text.ToString();
            string pass = Password.Text.ToString();


            passReset reset = null;

            using (var ctx = new CVGSEntities())
            {
                reset = ctx.passResets
                    .Where(s => s.resetCode == code).FirstOrDefault();

                if (reset != null)
                {
                    login log = new login();
                    log.username = reset.username;
                    log.password = pass;

                    if (ModelState.IsValid)
                    {
                        ctx.Entry(log).State = System.Data.Entity.EntityState.Modified;
                        ctx.SaveChanges();
                        ctx.passResets.Remove(reset);
                        ctx.SaveChanges();
                    }

                    Response.Redirect("~/Account/Login");
                }
                else
                {
                    ErrorMessage.Text = "Invalid Verification Code";
                }
            } 
        }
    }
}