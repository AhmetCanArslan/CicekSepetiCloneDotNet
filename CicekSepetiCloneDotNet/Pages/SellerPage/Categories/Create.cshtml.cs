using Azure.Core;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace CicekSepetiCloneDotNet.Pages.SellerPage.Categories
{
    public class CreateModel : PageModel
    {
        public CategoryInfo categoryInfo = new CategoryInfo();
        public string errorMessage = "";
        public string succesMessage = "";
        public string intMessage = "";
        public string seller_id;
        public void OnGet()
        {
            categoryInfo.seller_id = Request.Query["seller_id"];

        }
        public void OnPost()
        {
            categoryInfo.seller_id = Request.Query["seller_id"];
            categoryInfo.category_name = Request.Form["name"];



            if (categoryInfo.category_name.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }
            Regex pattern = new Regex("^-?[0-9]+$", RegexOptions.Singleline);

            if (pattern.Match(categoryInfo.category_name).Success)
            {
                intMessage = "Category should be string";
                return;
            }

            //save the product to the data
            try
            {
                string connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO TBL_Category " +
                        "(category_name, seller_id) VALUES " +
                        "(@name ,@seller_id);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("name", categoryInfo.category_name);
                        command.Parameters.AddWithValue("seller_id", categoryInfo.seller_id);


                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            

            Response.Redirect("/SellerPage/Categories/Index?id="+ categoryInfo.seller_id);
        }
    }
}
