using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Reflection.PortableExecutable;
using CicekSepetiCloneDotNet.Pages.AdminPage.Users;
using CicekSepetiCloneDotNet.Pages.AdminPage.Categories;

namespace CicekSepetiCloneDotNet.Pages.AdminPage.Products
{
    public class EditModel : PageModel
    {
        public ProductInfo productInfo = new ProductInfo();

        public List<CategoryInfo> Categories { get; set; } = new List<CategoryInfo>();
        public List<UsersInfo> Users { get; set; } = new List<UsersInfo>();

        public string errorMessage = "";
        public string succesMessage = "";
        public string intMessage = "";


        public void OnGet()
        {
            string id = Request.Query["id"];



            if (string.IsNullOrEmpty(id))
            {
                errorMessage = "Invalid product ID.";
                return;
            }

            try
            {

                string connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Ürün bilgilerini getir
                    string sql = "GetProductByIdWithSellerAndCategory";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@product_id", id);

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
                                productInfo.product_category_name = reader.GetString(8);
                                productInfo.product_seller_name = reader.GetString(9);

                            }
                        }
                    }

                    // Kategorileri getir
                    sql = "SELECT * FROM TBL_CATEGOry";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Categories.Add(new CategoryInfo
                                {
                                    category_id = "" + reader.GetInt32(0),
                                    category_name = reader.GetString(1)

                                });

                            }
                        }
                    }
                    //Get Sellers

                    string sql2 = "SELECT user_id, user_name, user_surname FROM TBL_USERS where user_category='seller' ";
                    using (SqlCommand command2 = new SqlCommand(sql2, connection))
                    {
                        using (SqlDataReader reader2 = command2.ExecuteReader())
                        {
                            while (reader2.Read())
                            {
                                Users.Add(new UsersInfo
                                {
                                    user_id = reader2["user_id"].ToString(),
                                    user_name = reader2["user_name"].ToString(),
                                    user_surname = reader2["user_surname"].ToString()
                                });

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
            productInfo.product_id = Request.Form["id"];
            productInfo.product_name = Request.Form["name"];
            productInfo.product_description = Request.Form["description"];
            productInfo.product_price = Request.Form["price"];
            productInfo.product_image = Request.Form["image"];
            productInfo.product_categoryid = Request.Form["categoryid"];
            productInfo.product_seller_id = Request.Form["seller_id"];
            productInfo.product_quantity = Request.Form["quantity"];

            Regex pattern = new Regex("^-?[0-9]+$", RegexOptions.Singleline);

            if (!pattern.Match(productInfo.product_price).Success)
            {
                intMessage = "Price should be int";
                return;
            }

            if (!pattern.Match(productInfo.product_quantity).Success)
            {
                intMessage = "Quantity should be int";
                return;
            }


            if (productInfo.product_image.Length == 0 || productInfo.product_price.Length == 0 || productInfo.product_name.Length == 0 ||
                productInfo.product_description.Length == 0 || productInfo.product_quantity.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                string connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE TBL_PRODUCTS " +
    "SET product_name = @name, product_description = @description, product_price = @price, product_image = @image,product_category_id=@categoryid, product_seller_id = @seller_id, product_quantity = @quantity " +
    "WHERE product_id = @id";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("name", productInfo.product_name);
                        command.Parameters.AddWithValue("description", productInfo.product_description);
                        command.Parameters.AddWithValue("price", productInfo.product_price);
                        command.Parameters.AddWithValue("image", productInfo.product_image);
                        command.Parameters.AddWithValue("categoryid", productInfo.product_categoryid);
                        command.Parameters.AddWithValue("id", productInfo.product_id);
                        command.Parameters.AddWithValue("quantity", productInfo.product_quantity);
                        command.Parameters.AddWithValue("seller_id", productInfo.product_seller_id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception err)
            {
                errorMessage = err.Message;
                return;
            }

            succesMessage = "Product updated successfully. Redirecting to products page!";

            Response.Redirect("/Products/Index");
        }
    }

}
