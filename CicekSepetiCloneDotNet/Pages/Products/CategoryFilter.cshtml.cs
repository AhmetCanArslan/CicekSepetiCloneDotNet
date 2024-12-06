using CicekSepetiCloneDotNet.Pages.Categories;
using CicekSepetiCloneDotNet.Pages.Index;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.Products
{
    public class CategoryFilterModel : PageModel
    {
        string errorMessage = "";
        public List<ProductInfo> productList = new List<ProductInfo>();
        public string Categoryid = "";
        public string CategoryName = "";
        public void OnGet()
        {
            String id = Request.Query["id"];
            Categoryid = id;
            


            try
            {
                String connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "Select * from TBL_PRODUCTS where product_category_id = @id";
                    String sqlCategoryName = "Select * from TBL_Category where category_id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
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
                                productList.Add(productInfo);

                            }

                        }
                    }
                    using (SqlCommand command1 = new SqlCommand(sqlCategoryName, connection))
                    {
                        command1.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CategoryName =reader.GetString(1);
                                
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
    }
}
