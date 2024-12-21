using CicekSepetiCloneDotNet.Pages.AdminPage.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.Login
{
    public class RegistrationModel : PageModel
    {
        public string errorMessage = "";
        public string successMessage = "";
        public UsersInfo userInfo = new UsersInfo();
        public string connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";

        public void OnPost()
        {
            string pass = Request.Form["pass"];
            string pass2 = Request.Form["pass2"];


            if (pass != pass2)
            {
                errorMessage = "Şifreler Uyuşmuyor";
                Response.Redirect("/Login/Regsitration");
            }
            userInfo.user_pass = pass;
            userInfo.user_mail = Request.Form["mail"];
            userInfo.user_name = Request.Form["name"];
            userInfo.user_surname = Request.Form["surname"];
            userInfo.user_number = Request.Form["phone"];
            userInfo.user_city = Request.Form["city"];

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO tbl_users (user_name, user_surname, user_mail, user_number, user_city, user_pass, user_category) " +
             "VALUES (@name, @surname, @mail, @user_number, @city, @pass, 'User')";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", userInfo.user_name);
                        command.Parameters.AddWithValue("@surname", userInfo.user_surname);
                        command.Parameters.AddWithValue("@mail", userInfo.user_mail);
                        command.Parameters.AddWithValue("@user_number", userInfo.user_number);
                        command.Parameters.AddWithValue("@city", userInfo.user_city);
                        command.Parameters.AddWithValue("@pass", userInfo.user_pass);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
            Response.Redirect("/Login/Index");
        }
    }
}
