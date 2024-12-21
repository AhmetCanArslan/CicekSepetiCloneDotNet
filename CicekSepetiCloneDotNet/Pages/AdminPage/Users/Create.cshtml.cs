using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CicekSepetiCloneDotNet.Pages.AdminPage.Users
{
    public class CreateModel : PageModel
    {
        public UsersInfo userInfo = new UsersInfo();
        public string errorMessage = "";
        public string succesMessage = "";
        public string intMessage = "";
        public string connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";

        public void OnGet()
        {

        }

        public void OnPost()
        {
            userInfo.user_name = Request.Form["name"];
            userInfo.user_surname = Request.Form["surname"];
            userInfo.user_mail = Request.Form["mail"];
            userInfo.user_number = Request.Form["number"];
            userInfo.user_city = Request.Form["city"];
            userInfo.user_pass = Request.Form["pass"];
            userInfo.user_category = Request.Form["category"];

            Regex pattern = new Regex("^-?[0-9]+$", RegexOptions.Singleline);

            if (!pattern.Match(userInfo.user_number).Success)
            {
                intMessage = "Number should be int";
                return;
            }


            if (userInfo.user_name.Length == 0 || userInfo.user_surname.Length == 0 ||
                userInfo.user_mail.Length == 0 || userInfo.user_number.Length == 0 || userInfo.user_city.Length == 0 ||
                userInfo.user_pass.Length == 0 || userInfo.user_category.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

           

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO TBL_users " +
    "(user_name, user_surname , user_mail , user_number , user_city, user_pass, user_category) VALUES" +
    "(@name,@surname,@mail,@number,@city,@pass,@category)";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("name", userInfo.user_name);
                        command.Parameters.AddWithValue("surname", userInfo.user_surname);
                        command.Parameters.AddWithValue("mail", userInfo.user_mail);
                        command.Parameters.AddWithValue("number", userInfo.user_number);
                        command.Parameters.AddWithValue("city", userInfo.user_city);
                        command.Parameters.AddWithValue("pass", userInfo.user_pass);
                        command.Parameters.AddWithValue("category", userInfo.user_category);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception err)
            {
                errorMessage = err.Message;
                return;
            }

            succesMessage = "User created successfully. Redirecting to users page!";

            Response.Redirect("/AdminPage/Users");
        }
    }
}
