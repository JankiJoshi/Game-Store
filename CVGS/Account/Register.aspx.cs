/*
 * Sign Up Use Case
 * Team SiX - Jankiben Joshi
 * 
 * Last Updated: 10/10/18
 * 
 */



using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using CVGS.Models;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace CVGS.Account
{
    public partial class Register : Page
    {

        protected void CreateUser_Click(object sender, EventArgs e)
        {
          
            String email = "";
            String uName = "";
            String password = "";
            String fName = "";
            String lName = "";
            Int16 age = 10;
            Boolean isEmployee = false;
            Boolean invalidUserName = false;

            IList<LoginModel> logins = null;
            bool result = false;

            using (var ctx = new CVGSEntities())
            {
                logins = ctx.logins.Select(s => new LoginModel()
                {
                    user = s.username,
                    pword = s.password
                }).ToList<LoginModel>();
            }

            if (Email.Text.Trim() != null || Email.Text.Trim() != "")
            {
                email = Email.Text.Trim();
            }
            if (userName.Text.Trim() != null || userName.Text.Trim() != "")
            {
                foreach (LoginModel row in logins)
                {
                    if (userName.Text.Trim() == row.user.ToString())
                    {
                        ErrorMessage.Text = "Invalid user name";
                        invalidUserName = true;
                    }
                }
                uName = userName.Text.Trim();
            }
            if (firstName.Text.Trim() != null || firstName.Text.Trim() != "")
            {
                fName = firstName.Text.Trim();
            }
            if (lastName.Text.Trim() != null || lastName.Text.Trim() != "")
            {
                lName = lastName.Text.Trim();
            }
            if (Password.Text.Trim() != null || Password.Text.Trim() != "")
            {
                password = Password.Text.Trim();
            }

            age = Convert.ToInt16(Age.Text.Trim());
            isEmployee = Employee.Checked;

            user usr = new user();
            usr.username = uName;
            usr.email = email;
            usr.firstName = fName;
            usr.lastName = lName;
            usr.mailAddress = null;
            usr.shipAddress = null;
            usr.age = age;
            usr.employee = isEmployee;
            usr.favGenre = null;
            usr.favGenre2 = null;
            usr.favPlatform = null;
            usr.favPlatform2 = null;
            usr.promoEmails = true;
            usr.publicWishlist = true;

            login log = new login();
            log.username = uName;
            log.password = password;

            if (!invalidUserName)
            {
                using (var ctx = new CVGSEntities())
                {
                    if (ModelState.IsValid)
                    {
                        ctx.users.Add(usr);
                        ctx.logins.Add(log);
                        ctx.SaveChanges();
                    }
                
                }

                userName.Text = "";
                firstName.Text = "";
                lastName.Text = "";
                Age.Text = "";
                Email.Text = "";
                Response.Redirect("/Account/Login");
            }
        }
    }
}