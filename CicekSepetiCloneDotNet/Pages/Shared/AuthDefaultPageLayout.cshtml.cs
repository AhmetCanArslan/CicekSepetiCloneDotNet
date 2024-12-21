using CicekSepetiCloneDotNet.Pages.AdminPage.Users;
using CicekSepetiCloneDotNet.Pages.Categories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.Shared
{
    public class AuthDefaultPageLayout : PageModel
    {
        public List<CategoryInfo> listCategory = new List<CategoryInfo>();
        public UsersInfo userInfo = new UsersInfo();

        public void OnGet(string id)
        {
            if (!string.IsNullOrEmpty(id) && int.TryParse(id, out int userId))
            {
                GetUserById(userId);
            }

            GetCategories();
        }

        private void GetUserById(int userId)
        {
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    String sql = "SELECT * FROM TBL_users WHERE user_id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", userId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userInfo.user_id = reader["user_id"].ToString();
                                userInfo.user_name = reader["user_name"].ToString();
                                userInfo.user_surname = reader["user_surname"].ToString();
                                userInfo.user_mail = reader["user_mail"].ToString();
                                userInfo.user_number = reader["user_number"].ToString();
                                userInfo.user_city = reader["user_city"].ToString();
                                userInfo.user_pass = reader["user_pass"].ToString();
                                userInfo.user_category = reader["user_category"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }
        }

        private void GetCategories()
        {
            try
            {
                using (SqlConnection connection = GetConnection())
                {
                    connection.Open();
                    String sql = "SELECT * FROM TBL_Category";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listCategory.Add(new CategoryInfo
                                {
                                    category_id = reader["category_id"].ToString(),
                                    category_name = reader["category_name"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }
        }

        private SqlConnection GetConnection()
        {
            String connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
            return new SqlConnection(connectionString);
        }
    }
}
