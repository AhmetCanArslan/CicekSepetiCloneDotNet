using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.Index
{
    public class IndexModel : PageModel
    {
        public List<ProductInfo> listProduct = new List<ProductInfo>();
        public void OnGet(string categoryId)
        {
            try
            {
                string connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
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
                                productInfo.product_category_name = reader.GetString(5);
                                productInfo.product_seller_name = reader.GetString(6);
                                productInfo.product_quantity = "" + reader.GetInt32(7);


                                listProduct.Add(productInfo);
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

    }
}
