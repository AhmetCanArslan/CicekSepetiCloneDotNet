using CicekSepetiCloneDotNet.Pages.AdminPage.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.Login
{
    public class PasswordResetAuthModel : PageModel
    {
        public string errorMessage = "";
        public string connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            string id = Request.Query["id"];
            string password1 = Request.Form["password1"];
            string password2 = Request.Form["password2"];

            if (password1 != password2)
            {
                errorMessage = "Şifreler Uyuşmuyor";
                Response.Redirect("/Login/PasswordResetAuth");
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "update tbl_users set user_pass = @password1 where user_id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@password1", password1);
                        command.Parameters.AddWithValue("@id", id);
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
