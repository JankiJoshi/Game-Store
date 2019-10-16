using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using CVGS.Models;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace CVGS.Account
{
    public partial class ForgotPassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Forgot(object sender, EventArgs e)
        {
            String email = email = Email.Text.Trim();
            String code = GetUniqueKey(6);

            UserModel login = null;

            using (var ctx = new CVGSEntities())
            {
                login = ctx.users
                    .Where(s => s.email == email)
                    .Select(s => new UserModel()
                    {
                        username = s.username,
                        firstName = s.firstName,
                        lastname = s.lastName,
                        email = s.email,
                        mailAddress = s.mailAddress,
                        shipAddress = s.shipAddress,
                        age = s.age,
                        employee = s.employee,
                    }).FirstOrDefault<UserModel>();
            }

            passReset resetPass = new passReset();
            resetPass.resetCode = code;
            resetPass.username = login.username;

            using (var ctx = new CVGSEntities())
            {
                if (ModelState.IsValid)
                {
                    ctx.passResets.Add(resetPass);
                    ctx.SaveChanges();
                }
            }


            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(email);
            message.Subject = "CVGS password reset code";
            message.From = new System.Net.Mail.MailAddress("TeamSiXCVGS@outlook.com");
            message.Body = "Please enter the following code on the reset password page of CVGS: "
                + code;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp-mail.outlook.com");
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("TeamSiXCVGS@outlook.com", "CVGSPassword");
            smtp.EnableSsl = true;
            smtp.Send(message);

            Response.Redirect("~/Account/ResetPassword");

        }

        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
    }
}