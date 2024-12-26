using CicekSepetiCloneDotNet.Pages.AdminPage.Products;
using CicekSepetiCloneDotNet.Pages.AdminPage.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CicekSepetiCloneDotNet.Pages.AdminPage.Comments
{
    //yorumda hakaret varsa veya yazım yanlışı varsa admin bunu düzeltebelir
    public class EditModel : PageModel
    {
        string connectionString = ConnectionStrings.DefaultConnection;
        public CommentInfo commentInfo = new CommentInfo();
        public List<ProductInfo> listProducts = new List<ProductInfo>();
        public string errorMessage = "";
        public string succesMessage = "";

        public void OnGet()
        {
            string id = Request.Query["id"];
            try
            {
                // Yorumları getir
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT comment,commentTitle, productId FROM TBL_comments WHERE Id =@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
    
                                commentInfo.comment = reader.GetString(0);
                                commentInfo.commentTitle = reader.GetString(1);
                                commentInfo.productId = reader.GetInt32(2).ToString();
                                commentInfo.commentId = id;
                            }
                        }
                    }
                }
                // Ürünleri getir
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM TBL_products";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductInfo prodcutInfo = new ProductInfo();
                                prodcutInfo.product_id = reader.GetInt32(0).ToString();
                                prodcutInfo.product_name = reader.GetString(1);
                                listProducts.Add(prodcutInfo);
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
            commentInfo.comment = Request.Form["comment"];
            commentInfo.commentTitle = Request.Form["commentTitle"];
            commentInfo.productId = Request.Form["productId"];
            commentInfo.commentId = Request.Form["id"];

            if (string.IsNullOrEmpty(commentInfo.comment) || string.IsNullOrEmpty(commentInfo.commentTitle) || string.IsNullOrEmpty(commentInfo.productId))
            {
                errorMessage = "All the fields are required.";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE TBL_Comments " +
                                 "SET comment = @comment, commentTitle = @commentTitle, productId=@productId " +
                                 "WHERE Id = @commentId";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@comment", commentInfo.comment);
                        command.Parameters.AddWithValue("@commentTitle", commentInfo.commentTitle);
                        command.Parameters.AddWithValue("@productId", commentInfo.productId);
                        command.Parameters.AddWithValue("@commentId", commentInfo.commentId);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                errorMessage = "An error occurred while updating the category.";
                return;
            }


            Response.Redirect("/AdminPage/Comments/Index");
        }
    }
}
