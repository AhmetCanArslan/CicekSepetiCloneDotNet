using CicekSepetiCloneDotNet.Pages.AdminPage.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CicekSepetiCloneDotNet.Pages.AdminPage.Categories
{
    public class EditModel : PageModel
    {
        public CategoryInfo categoryInfo = new CategoryInfo();
        public List<UsersInfo> users = new List<UsersInfo>();
        public string errorMessage = "";
        public string succesMessage = "";
        string connectionString = ConnectionStrings.DefaultConnection;

        public void OnGet()
        {
            string id = Request.Query["id"];
            if (string.IsNullOrEmpty(id))
            {
                errorMessage = "Invalid category ID.";
                return;
            }

            try
            {
                // Kategoriyi getir
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "Select * from TBL_category where category_id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                categoryInfo.category_id = "" + reader.GetInt32(0);
                                categoryInfo.category_name = reader.GetString(1);
                                categoryInfo.creator_id = "" + reader.GetInt32(2);
                            }
                        }
                    }
                }

                // Satıcıları getir
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
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
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            categoryInfo.category_id = Request.Form["id"];
            categoryInfo.category_name = Request.Form["name"];
            categoryInfo.creator_id = Request.Form["seller_id"];

            if (string.IsNullOrEmpty(categoryInfo.category_name))
            {
                errorMessage = "All the fields are required.";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE TBL_Category " +
                                 "SET category_name = @name, seller_id = @seller_id " +
                                 "WHERE category_id = @id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", categoryInfo.category_name);
                        command.Parameters.AddWithValue("@seller_id", categoryInfo.creator_id);
                        command.Parameters.AddWithValue("@id", categoryInfo.category_id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                errorMessage = "An error occurred while updating the category.";
                return;
            }

            succesMessage = "Category updated successfully! Redirecting...";
            Thread.Sleep(2000);
            Response.Redirect("/AdminPage/Categories/Index");
        }
    }
}
