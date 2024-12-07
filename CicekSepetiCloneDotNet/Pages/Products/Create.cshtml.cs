using CicekSepetiCloneDotNet.Pages.Index;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CicekSepetiCloneDotNet.Pages.Products
{
    public class CreateModel : PageModel
    {
        public ProductInfo productInfo = new ProductInfo();
        public string errorMessage="";
        public string succesMessage="";
        public string intMessage = "";

        public void OnGet()
        {
        }
        public void OnPost() {
            productInfo.product_name = Request.Form["name"];
            productInfo.product_description = Request.Form["description"];
            productInfo.product_price = Request.Form["price"];
            productInfo.product_image = Request.Form["image"];
            productInfo.product_categoryid = Request.Form["categoryid"];

            Regex pattern = new Regex("^-?[0-9]+$", RegexOptions.Singleline);

            if (!pattern.Match(productInfo.product_price).Success)
            {
                intMessage = "Price should be int";
                return;
            }
            if (!pattern.Match(productInfo.product_categoryid).Success)
            {
                intMessage = "Category ID should be int";
                return;
            }


            if (productInfo.product_image.Length == 0 || productInfo.product_price.Length == 0 || productInfo.product_name.Length == 0 ||
                productInfo.product_description.Length == 0 || productInfo.product_categoryid.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            //save the product to the data
            try
            {
                String connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO TBL_PRODUCTS"+
                        "(product_name, product_description, product_price, product_image, product_category_id, product_seller_id , product_quantity) VALUES"+
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

            succesMessage = "New Product Added, Redirecting to Products Page";

            productInfo.product_name = "";
            productInfo.product_description = "";
            productInfo.product_price = "";
            productInfo.product_image = "";
            productInfo.product_categoryid = "";
            productInfo.product_seller_id = "";
            productInfo.product_quantity = "";

            
            Response.Redirect("/Products/Index");
        }
    }
}
