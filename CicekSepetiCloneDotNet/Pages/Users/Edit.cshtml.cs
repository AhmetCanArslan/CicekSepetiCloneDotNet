using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CicekSepetiCloneDotNet.Pages.Users
{
    public class EditModel : PageModel
    {
        string connectionString = ConnectionStrings.DefaultConnection;

        public UsersInfo userInfo = new UsersInfo();
        public string errorMessage = "";
        public string succesMessage = "";
        public string intMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];
            if (string.IsNullOrEmpty(id))
            {
                errorMessage = "Invalid user ID.";
                return;
            }


            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "Select * from TBL_users where user_id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
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
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            userInfo.user_id = Request.Form["id"];
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
                    String sql = "UPDATE TBL_users " +
    "SET user_name = @name, user_surname = @surname, user_mail = @mail, user_number = @number, user_city=@city, user_pass=@pass, user_category=@category " +
    "WHERE user_id = @id";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("name", userInfo.user_name);
                        command.Parameters.AddWithValue("surname", userInfo.user_surname);
                        command.Parameters.AddWithValue("mail", userInfo.user_mail);
                        command.Parameters.AddWithValue("number", userInfo.user_number);
                        command.Parameters.AddWithValue("city", userInfo.user_city);
                        command.Parameters.AddWithValue("pass", userInfo.user_pass);
                        command.Parameters.AddWithValue("category", userInfo.user_category);
                        command.Parameters.AddWithValue("id", userInfo.user_id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception err)
            {
                errorMessage = err.Message;
                return;
            }


            Response.Redirect("/Users");
        }
    }
}
