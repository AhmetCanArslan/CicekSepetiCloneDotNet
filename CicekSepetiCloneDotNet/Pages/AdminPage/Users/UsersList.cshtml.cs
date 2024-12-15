using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.AdminPage.Users
{
    public class ListUsersModel : PageModel
    {
        public List<UsersInfo> listUsers = new List<UsersInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM TBL_users";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UsersInfo userInfo = new UsersInfo();
                                userInfo.user_id = "" + reader.GetInt32(0);
                                userInfo.user_name = reader.GetString(1);
                                userInfo.user_surname = reader.GetString(2);
                                userInfo.user_mail = reader.GetString(3);
                                userInfo.user_number = "" + reader.GetInt64(4);
                                userInfo.user_city = reader.GetString(5);
                                userInfo.user_pass = reader.GetString(6);
                                userInfo.user_category = reader.GetString(7);

                                listUsers.Add(userInfo);

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
    public class UsersInfo
    {
        public string user_id;
        public string user_name;
        public string user_surname;
        public string user_mail;
        public string user_number;
        public string user_city;
        public string user_pass;
        public string user_category;
    }
}
