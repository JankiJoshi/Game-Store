using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CVGS.Models;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace CVGS
{
    public partial class Reports : System.Web.UI.Page
    {
        private static SqlDataReader dr;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void DownloadReport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable QueryList = getTable(DdlReports.SelectedIndex);
                String reportName = DdlReports.SelectedItem.Value;
                ExportDataTableToPdf(QueryList, @"D:\" + reportName + ".pdf", reportName, this);
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }

        private DataTable getTable(int selectedIndex)
        {
            DataTable dt = new DataTable();
            // Game List
            if (selectedIndex == 1)
            {
                IList<GameModel> games = null;
                using (var context = new CVGSEntities())
                {
                    games = context.games.Select(s => new GameModel()
                    {
                        name = s.name
                    }).ToList<GameModel>();
                }
                dt.Columns.Add("Name", typeof(string));
                dt.Rows.Add("Name");
                foreach (GameModel row in games)
                {
                    dt.Rows.Add(row.name.ToString());
                }
            }
            // Game Detail
            else if (selectedIndex == 2)
            {
                IList<GameModel> games = null;
                using (var context = new CVGSEntities())
                {
                    games = context.games.Select(s => new GameModel()
                    {
                        name = s.name,
                        description = s.description,
                        publisher = s.publisher,
                        publishDate = s.publishDate,
                        genre = s.genre,
                        rating = s.rating,
                        price = s.price
                    }).ToList<GameModel>();
                }
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Description", typeof(string));
                dt.Columns.Add("Publisher", typeof(string));
                dt.Columns.Add("PublishDate", typeof(DateTime));
                dt.Columns.Add("Genre", typeof(string));
                dt.Columns.Add("Rating", typeof(string));
                dt.Columns.Add("Price", typeof(decimal));

                foreach (GameModel row in games)
                {
                    dt.Rows.Add(row.name.ToString(), row.description.ToString(), row.publisher.ToString(),
                        row.publishDate.ToString(), row.genre.ToString(), row.rating.ToString(), row.price.ToString());
                }
            }
            // Member List
            else if (selectedIndex == 3)
            {
                IList<UserModel> users = null;
                using (var context = new CVGSEntities())
                {
                    users = context.users.Select(s => new UserModel()
                    {
                        firstName = s.firstName,
                        lastname = s.lastName
                    }).ToList<UserModel>();
                }
                dt.Columns.Add("Name", typeof(string));
                dt.Rows.Add("Name");
                foreach (UserModel user in users)
                {
                    dt.Rows.Add(user.firstName.ToString()+' ' + user.lastname.ToString());
                }
            }
            // Member Detail
            else if (selectedIndex == 4)
            {
                IList<UserModel> users = null;
                using (var context = new CVGSEntities())
                {
                    users = context.users.Select(s => new UserModel()
                    {
                        username = s.username,
                        firstName = s.firstName,
                        lastname = s.lastName,
                        email = s.email,
                        age = s.age
                    }).ToList<UserModel>();
                }
                dt.Columns.Add("username", typeof(string));
                dt.Columns.Add("firstName", typeof(string));
                dt.Columns.Add("lastname", typeof(string));
                dt.Columns.Add("email", typeof(string));
                dt.Columns.Add("age");
                dt.Rows.Add("Name", "Firest Name", "Last Name", "Email", "Age");

                foreach (UserModel row in users)
                {
                    dt.Rows.Add(row.username.ToString(), row.firstName.ToString(), row.lastname.ToString(),
                        row.email.ToString(), row.age.ToString());
                }
            }
            // wishList
            else if (selectedIndex == 5)
            {
                IList<WishListModel> wish = null;
                using (var context = new CVGSEntities())
                {
                    wish = context.wishLists.Select(s => new WishListModel()
                    {
                        username = s.username,
                    }).ToList<WishListModel>();

                }
                dt.Columns.Add("Name", typeof(string));
                dt.Rows.Add("Name");
                foreach (WishListModel row in wish)
                {
                    dt.Rows.Add(row.username.ToString());
                }
            }
            // Sales Report
            else if (selectedIndex == 6)
            {
                IList<SalesModel> sales = null;
                using (var context = new CVGSEntities())
                {
                    sales = context.orders.Select(s => new SalesModel()
                    {
                        username = s.username,
                        gameId = s.gameId,
                        orderDate = s.orderDate
                        
                    }).ToList<SalesModel>();
                }
                dt.Columns.Add("Name");
                dt.Columns.Add("Game Id");
                dt.Columns.Add("Order Date");
                dt.Rows.Add("Name", "Game Id", "Order Date");

                foreach (SalesModel row in sales)
                {
                    dt.Rows.Add(row.username.ToString(), row.gameId.ToString(), row.orderDate.ToString());
                }
            }
            return dt;
        }

        private void ExportDataTableToPdf(DataTable queryList, String url, String v2, Control control)
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition",
                "attachment;filename=" + v2 + ".pdf");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            PdfWriter.GetInstance(document, Response.OutputStream);
            document.Open();

            //Table Data
            PdfPTable table = new PdfPTable(queryList.Columns.Count);
            for (int i = 0; i < queryList.Rows.Count; i++)
            {
                for (int j = 0; j < queryList.Columns.Count; j++)
                {
                    table.AddCell(queryList.Rows[i][j].ToString());
                }
            }

            document.Add(table);

            document.Close();

            Response.Write(document);

            Response.End();
        }
    }
}
