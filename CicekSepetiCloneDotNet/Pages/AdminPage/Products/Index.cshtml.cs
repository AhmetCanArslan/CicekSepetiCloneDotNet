using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.AdminPage.Products
{
    public class IndexModel : PageModel
    {
        string connectionString = ConnectionStrings.DefaultConnection;
        public string productCount ="";

        public List<ProductInfo> listProduct = new List<ProductInfo>();
        public void OnGet(string categoryId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    string sql = "GetProductsWithSellerAndCategory";



                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {


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
                                productInfo.product_seller_id = "" + reader.GetInt32(6);
                                productInfo.product_quantity = "" + reader.GetInt32(7);
                                productInfo.product_category_name = reader.GetString(8);
                                productInfo.product_seller_name = reader.GetString(9);


                                listProduct.Add(productInfo);
                            }
                        }
                    }

                    sql = "SELECT dbo.GetTotalProductCount()";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                productCount = ""+reader.GetInt32(0);
                                
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
        public string product_id;
        public string product_name;
        public string product_description;
        public string product_price;
        public string product_image;
        public string product_categoryid;
        public string product_quantity;
        public string product_seller_id;
        public string product_seller_name;
        public string product_category_name;
        public string product_category_id;


    }
}
