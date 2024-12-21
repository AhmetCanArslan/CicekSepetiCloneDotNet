using CicekSepetiCloneDotNet.Pages.AdminPage.Comments;
using CicekSepetiCloneDotNet.Pages.AdminPage.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.Products
{
    public class PreviewModel : PageModel
    {
        String connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";

        public ProductInfo productInfo = new ProductInfo();
        public List<CommentInfo> commentInfos = new List<CommentInfo>();
        public int commentCounter = 0;
        public string? user_id;
        public string? product_id;
        public void OnGet()
        {
            product_id = Request.Query["id"];
            user_id = Request.Query["user_id"];
            

            // Ürün bilgilerini getir
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "GetProductByIdWithSellerAndCategory";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@product_id", product_id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                productInfo.product_id = "" + reader.GetInt32(0);
                                productInfo.product_name = reader.GetString(1);
                                productInfo.product_description = reader.GetString(2);
                                productInfo.product_price = "" + reader.GetInt32(3);
                                productInfo.product_image = reader.GetString(4);
                                productInfo.product_category_id = "" + reader.GetInt32(5);
                                productInfo.product_seller_id = "" + reader.GetInt32(6);
                                productInfo.product_quantity = "" + reader.GetInt32(7);
                                productInfo.product_category_name = reader.GetString(8);
                                productInfo.product_seller_name = reader.GetString(9);
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            // Yorumları getir
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "GetCommentsByProductID";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductID", product_id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CommentInfo commentInfo = new CommentInfo();
                                commentInfo.commentId = "" + reader.GetInt32(0);
                                commentInfo.productId = "" + reader.GetInt32(2);
                                commentInfo.comment = reader.GetString(1);
                                commentInfo.commentTitle = reader.GetString(3);
                                commentInfo.date =reader.GetDateTime(4);
                                commentInfo.user_name = reader.GetString(5);
                                commentInfo.productName = reader.GetString(6);
                                commentCounter++;
                                commentInfos.Add(commentInfo);
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
