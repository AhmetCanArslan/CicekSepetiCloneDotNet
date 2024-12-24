using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using CicekSepetiCloneDotNet.Pages.AdminPage.Users;
using CicekSepetiCloneDotNet.Pages.AdminPage.Categories;



namespace CicekSepetiCloneDotNet.Pages.SellerPage.Products
{
    public class CreateModel : PageModel
    {
        string connectionString = ConnectionStrings.DefaultConnection;

        public ProductInfo productInfo = new ProductInfo();
        public string errorMessage = "";
        public string succesMessage = "";
        public string intMessage = "";
        public List<CategoryInfo> Categories { get; set; } = new List<CategoryInfo>();
        public UsersInfo Users { get; set; } = new UsersInfo();
        public string? seller_id;

        public void OnGet()
        {
            seller_id = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM TBL_Category";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Categories.Add(new CategoryInfo
                            {
                                category_id = reader["category_id"].ToString(),
                                category_name = reader["category_name"].ToString()
                            });
                        }
                    }
                    string sql2 = "SELECT user_id, user_name, user_surname FROM TBL_USERS where user_id=@seller_id ";
                    using (SqlCommand command2 = new SqlCommand(sql2, connection))
                    {
                        command2.Parameters.AddWithValue("seller_id", seller_id);
                        using (SqlDataReader reader2 = command2.ExecuteReader())
                        {
                            if (reader2.Read())
                            {
                                Users.user_id = reader2["user_id"].ToString();
                                Users.user_name = reader2["user_name"].ToString();
                                Users.user_surname = reader2["user_surname"].ToString();
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
            seller_id = Request.Query["id"];
            productInfo.product_name = Request.Form["name"];
            productInfo.product_description = Request.Form["description"];
            productInfo.product_price = Request.Form["price"];
            productInfo.product_image = Request.Form["image"];
            productInfo.product_categoryid = Request.Form["categoryid"];
            productInfo.product_seller_id = Request.Query["id"];
            productInfo.product_quantity = Request.Form["quantity"];

            Regex pattern = new Regex("^-?[0-9]+$", RegexOptions.Singleline);

            if (!pattern.Match(productInfo.product_price).Success)
            {
                intMessage = "Price should be int";
                return;
            }



            if (productInfo.product_image.Length == 0 || productInfo.product_price.Length == 0 || productInfo.product_name.Length == 0 ||
                productInfo.product_description.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            //save the product to the data
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO TBL_PRODUCTS" +
                        "(product_name, product_description, product_price, product_image, product_category_id, product_seller_id , product_quantity) VALUES" +
                        "(@name, @description, @price, @image ,@category ,@seller_id, @quantity);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("name", productInfo.product_name);
                        command.Parameters.AddWithValue("description", productInfo.product_description);
                        command.Parameters.AddWithValue("price", productInfo.product_price);
                        command.Parameters.AddWithValue("image", productInfo.product_image);
                        command.Parameters.AddWithValue("category", productInfo.product_categoryid);
                        command.Parameters.AddWithValue("seller_id", productInfo.product_seller_id);
                        command.Parameters.AddWithValue("quantity", productInfo.product_quantity);


                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            


            Response.Redirect("/SellerPage/Products/Index?id=" + seller_id);
        }
    }
}
