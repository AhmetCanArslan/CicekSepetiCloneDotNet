using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace CicekSepetiCloneDotNet.Pages.Categories
{
    public class CreateModel : PageModel
    {
        string connectionString = ConnectionStrings.DefaultConnection;

        public CategoryInfo categoryInfo = new CategoryInfo();
        public string errorMessage = "";
        public string succesMessage = "";
        public string intMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
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
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO TBL_Category " +
                        "(category_name) VALUES " +
                        "(@name);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("name", categoryInfo.category_name);


                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }



            Response.Redirect("/Categories/Index");
        }
    }
}
