using CicekSepetiCloneDotNet.Pages.AdminPage.Products;
using CicekSepetiCloneDotNet.Pages.AdminPage.Users;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CicekSepetiCloneDotNet.Pages.AdminPage.Comments
{
    public class CreateModel : PageModel
    {
        string connectionString = ConnectionStrings.DefaultConnection;

        public string errorMessage= " ";
        public string user_id =" ";
        public string product_id=" ";
        public ProductInfo productInfo = new ProductInfo();
        public CommentInfo commentInfo = new CommentInfo();
        public void OnGet()
        {
            user_id  = Request.Query["user_id"];
            product_id = Request.Query["product_id"];

            try
            {
                
                // Ürün bilgilerini getir
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM TBL_products where product_id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", product_id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                productInfo.product_id = "" + reader.GetInt32(0);
                                productInfo.product_name = reader.GetString(1);
                                productInfo.product_description = reader.GetString(2);
                                productInfo.product_price = "" + reader.GetInt32(3);
                                productInfo.product_image = reader.GetString(4);
                                productInfo.product_categoryid = "" + reader.GetInt32(5);
                                productInfo.product_seller_id = "" + reader.GetInt32(6);
                                productInfo.product_quantity = "" + reader.GetInt32(7);

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

        }
        public void OnPost()
        {
            user_id = Request.Query["user_id"];
            product_id = Request.Query["product_id"];
            commentInfo.comment = Request.Form["comment"];
            commentInfo.commentTitle = Request.Form["title"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO TBL_comments (productId,comment,commentTitle,userId,commentDate) VALUES (@product_id,@comment,@comment_title,@user_id,@date)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@product_id", product_id);
                        command.Parameters.AddWithValue("@comment", commentInfo.comment);
                        command.Parameters.AddWithValue("@comment_title", commentInfo.commentTitle);
                        command.Parameters.AddWithValue("@user_id", user_id);
                        command.Parameters.AddWithValue("@date", System.DateTime.Now);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                 Console.WriteLine(ex.ToString());
            }
            Response.Redirect("/Products/Preview?id="+product_id+"&user_id="+user_id);
        }
    }
}
