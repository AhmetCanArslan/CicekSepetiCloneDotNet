using CicekSepetiCloneDotNet.Pages.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.Users
{
    public class LoginIndexModel : PageModel
    {
        public UsersInfo userInfo = new UsersInfo();
        public string errorMessage = "";
        public void OnPost()
        {

            userInfo.user_mail = Request.Form["mail"];
            userInfo.user_pass = Request.Form["pass"];


            if (userInfo.user_pass.Length == 0 || userInfo.user_mail.Length == 0)
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
                    string sql = "UPDATE TBL_Category SET category_name = @name WHERE category_id = @id";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        //command.Parameters.AddWithValue("name", categoryInfo.category_name);
                        //command.Parameters.AddWithValue("id", categoryInfo.category_id);


                        //command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return;
            }

            //succesMessage = "Category updated successfully. Redirecting to categories page!";
            int milliseconds = 2000;
            Thread.Sleep(milliseconds);

            Response.Redirect("/Categories/Index");
        }
    }
}
