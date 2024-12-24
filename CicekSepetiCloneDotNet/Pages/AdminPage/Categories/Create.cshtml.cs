using CicekSepetiCloneDotNet.Pages.AdminPage.Users;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CicekSepetiCloneDotNet.Pages.AdminPage.Categories
{
    public class CreateModel : PageModel
    {
        public CategoryInfo categoryInfo = new CategoryInfo();
        public List<UsersInfo> users = new List<UsersInfo>();
        public string errorMessage = "";
        public string succesMessage = "";
        public string intMessage = "";
        string connectionString = ConnectionStrings.DefaultConnection;

        public void OnGet()
        {
            // Satıcıları getir
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open(); // Bağlantıyı açmayı unutmayın!
                string sql2 = "SELECT user_id, user_name, user_surname FROM TBL_USERS WHERE user_category = 'seller'";
                using (SqlCommand command2 = new SqlCommand(sql2, connection))
                {
                    using (SqlDataReader reader2 = command2.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            users.Add(new UsersInfo
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
        public void OnPost()
        {
            categoryInfo.category_name = Request.Form["name"];
            categoryInfo.creator_id = Request.Form["seller_id"];


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
                        "(@name, @seller_id);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", categoryInfo.category_name);
                        command.Parameters.AddWithValue("@seller_id", categoryInfo.creator_id);


                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

           

            Response.Redirect("/AdminPage/Categories/Index");
        }
    }
}
