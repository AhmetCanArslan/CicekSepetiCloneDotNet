using CicekSepetiCloneDotNet.Pages.AdminPage.Products;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages
{
    public class SearchModel : PageModel
    {
        public List<ProductInfo> SearchResults { get; set; } = new List<ProductInfo>();
        public string user_id = "";
        string connectionString = ConnectionStrings.DefaultConnection;

        public void OnGet()
        {
            String query = Request.Query["query"];
            user_id = Request.Query["user_id"];
            if (!string.IsNullOrEmpty(query))
            {
                try
                {

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "SELECT * FROM TBL_Products WHERE product_name LIKE @query";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@query", "%" + query + "%");
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ProductInfo productInfo = new ProductInfo();
                                    productInfo.product_id = "" + reader.GetInt32(0);
                                    productInfo.product_name = reader.GetString(1);
                                    productInfo.product_description = reader.GetString(2);
                                    productInfo.product_price = "" + reader.GetInt32(3);
                                    productInfo.product_image = reader.GetString(4);
                                    productInfo.product_categoryid = "" + reader.GetInt32(5);

                                    SearchResults.Add(productInfo);

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SQL Error: {ex.Message}");

                }
            }
        }
    }
}
