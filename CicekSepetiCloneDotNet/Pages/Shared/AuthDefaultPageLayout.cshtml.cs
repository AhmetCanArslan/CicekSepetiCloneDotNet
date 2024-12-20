using CicekSepetiCloneDotNet.Pages.AdminPage.Users;
using CicekSepetiCloneDotNet.Pages.AdminPage.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.Shared
{
    
    public class AuthDefaultPageLayout : PageModel
    {

        public List<CategoryInfo> listCategory = new List<CategoryInfo>();
        public UsersInfo userInfo = new UsersInfo();


        public void OnGet(string id)
        {

            if (id != null)
            {
                try
                {
                    String connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        String sql = "SELECT * FROM TBL_users WHERE user_id = " + id;
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    userInfo.user_id = "" + reader.GetInt32(0);
                                    userInfo.user_name = reader.GetString(1);
                                    userInfo.user_surname = reader.GetString(2);
                                    userInfo.user_mail = reader.GetString(3);
                                    userInfo.user_number = "" + reader.GetInt64(4);
                                    userInfo.user_city = reader.GetString(5);
                                    userInfo.user_pass = reader.GetString(6);
                                    userInfo.user_category = reader.GetString(7);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.ToString());
                }
            }
            try
            {
                String connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM TBL_Category";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CategoryInfo categoryInfo = new CategoryInfo();
                                categoryInfo.category_id= "" + reader.GetInt32(0);
                                categoryInfo.category_name = reader.GetString(1);

                                listCategory.Add(categoryInfo);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
        }
       
    }
}
