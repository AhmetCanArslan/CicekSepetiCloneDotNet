using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.SellerPage.Products
{
    public class IndexModel : PageModel
    {
        public List<ProductInfo> listProduct = new List<ProductInfo>();
        public string seller_id;
        string connectionString = ConnectionStrings.DefaultConnection;
        public int productCount;

        public void OnGet()
        {
            seller_id = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    string sql = "GetProductsBySellerWithCategory";



                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@seller_id", seller_id);

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
                                productInfo.product_quantity = "" + reader.GetInt32(5);
                                productInfo.product_category_name = "" + reader.GetString(6);
                                productInfo.product_seller_name = reader.GetString(7);


                                listProduct.Add(productInfo);
                            }
                        }
                    }

                    sql = "SELECT dbo.GetTotalProductsBySellerID(@SellerID)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@SellerID", seller_id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                productCount = reader.GetInt32(0); // Fonksiyon sonucu
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

    public class ProductInfo
    {
        public string? product_id;
        public string? product_name;
        public string? product_description;
        public string? product_price;
        public string? product_image;
        public string? product_categoryid;
        public string? product_quantity;
        public string? product_seller_id;
        public string? product_seller_name;
        public string? product_category_name;
        public string? product_category_id;


    }
}
