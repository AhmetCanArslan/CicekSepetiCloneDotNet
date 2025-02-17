﻿using Azure.Core;
using CicekSepetiCloneDotNet.Pages.AdminPage.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.Login
{
    public class LoginIndexModel : PageModel
    {
        public UsersInfo userInfo = new UsersInfo();
        public string errorMessage = "";
        string logOutStatus = "";
        string connectionString = ConnectionStrings.DefaultConnection;

        public void OnGet()
        {
            logOutStatus = Request.Query["status"];
            if (logOutStatus == "logout")
            {
                HttpContext.Session.Clear();
                System.Threading.Thread.Sleep(1000);
                Response.Redirect("/Index");

            }

            // eğer çıkış yapılmak istenilmişse bu sayfaya gelecek ve contexti silecek ve çıkış yapılmış olacak
        }

        public void OnPost()
        {
            string mail = Request.Form["mail"];
            string pass = Request.Form["password"];
            //mail ve pass alıp database işlemleri yapacak
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT user_id FROM TBL_Users WHERE user_mail = @Email AND user_pass = @Password";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Email", mail);
                        command.Parameters.AddWithValue("@Password", pass);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var id = reader["user_id"].ToString();

                                // Kullanıcı ID'sini Session'a koy
                                HttpContext.Session.SetString("user_id", id);

                                // Kullanıcıyı başka bir sayfaya yönlendir
                                Response.Redirect("/Index?user_id="+id);

                            }
                            else
                            {
                                errorMessage = "Kullanıcı adı veya şifre hatalı.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Bir hata oluştu: " + ex.Message;
            }
        }
    }
}
