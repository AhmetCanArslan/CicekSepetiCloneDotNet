using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using CicekSepetiCloneDotNet.Pages.Users;
using CicekSepetiCloneDotNet.Pages.SellerPage.Categories;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.Shared
{
    public class _Layout : PageModel
    {

        public UsersInfo userInfo = new UsersInfo();
        string connectionString = ConnectionStrings.DefaultConnection;


        public void OnGet(string id)
        {

            if (id != null)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        String sql = "SELECT * FROM TBL_users WHERE user_id = " + id;
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    userInfo.user_id = "" + reader.GetInt32(0);
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
            
        }
        
    }
}
