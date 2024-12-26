using CicekSepetiCloneDotNet.Pages.AdminPage.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.Login
{
    public class PasswordResetModel : PageModel
    {
        string connectionString = ConnectionStrings.DefaultConnection;

        public string errorMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            string mail = Request.Form["email"];
            string phone = Request.Form["phone"];
            //eğer email ve phone doğruysa kullanıcıyı bulup id'sini alıp PasswordResetAuth sayfasına yönlendiriyoruz
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM TBL_users where user_mail=@mail and user_number=@phone";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@mail", mail);
                        command.Parameters.AddWithValue("@phone", phone);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UsersInfo userInfo = new UsersInfo();
                                userInfo.user_id = "" + reader.GetInt32(0);

                                Response.Redirect("/Login/PasswordResetAuth?id="+userInfo.user_id);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            errorMessage = "Kullanıcı Bulunamadı";
            RedirectToPage("/Login/PasswordReset");
        }
    }
}
