using CicekSepetiCloneDotNet.Pages.Index;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace CicekSepetiCloneDotNet.Pages.Products
{
    public class EditModel : PageModel
    {
        public ProductInfo productInfo = new ProductInfo();
        public string errorMessage = "";
        public string succesMessage = "";
        public string intMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];
            if (string.IsNullOrEmpty(id))
            {
                errorMessage = "Invalid product ID.";
                return;
            }


            try
            {
                String connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "Select * from TBL_PRODUCTS where product_id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {   
                                productInfo.product_id = ""+ reader.GetInt32(0);
                                productInfo.product_name = reader.GetString(1);
                                productInfo.product_description = reader.GetString(2);
                                productInfo.product_price = "" + reader.GetInt32(3);
                                productInfo.product_image = reader.GetString(4);
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

            try
            {
                String connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE TBL_PRODUCTS " +
    "SET product_name = @name, product_description = @description, product_price = @price, product_image = @image " +
    "WHERE product_id = @id";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("name", productInfo.product_name);
                        command.Parameters.AddWithValue("description", productInfo.product_description);
                        command.Parameters.AddWithValue("price", productInfo.product_price);
                        command.Parameters.AddWithValue("image", productInfo.product_image);
                        command.Parameters.AddWithValue("id", productInfo.product_id);

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
            int milliseconds = 2000;
            Thread.Sleep(milliseconds);

            Response.Redirect("/Products/Index");
        }
    }
}
