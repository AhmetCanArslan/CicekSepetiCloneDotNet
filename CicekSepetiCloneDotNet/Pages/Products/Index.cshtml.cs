using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.Index
{
    public class IndexModel : PageModel
    {
        public List<ProductInfo> listProduct = new List<ProductInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM TBL_Products";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) 
                            {
                                ProductInfo productInfo = new ProductInfo();
                                productInfo.product_id = ""+reader.GetInt32(0);
                                productInfo.product_name = reader.GetString(1);
                                productInfo.product_description = reader.GetString(2);
                                productInfo.product_price = ""+reader.GetInt32(3);
                                productInfo.product_image = reader.GetString(4);

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
    }
}
