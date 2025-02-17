﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.SellerPage.Categories
{
    public class IndexModel : PageModel
    {
        public List<CategoryInfo> listCategory = new List<CategoryInfo>();
        public string seller_id;
        string connectionString = ConnectionStrings.DefaultConnection;

        public void OnGet()
        {
            // satıcı sadece kendi oluşturduğu kategorileri görebilir ve düzenleyebilir
            seller_id = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "GetCategoriesByUserId";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", seller_id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CategoryInfo categoryInfo = new CategoryInfo();
                                categoryInfo.category_id = "" + reader.GetInt32(0);
                                categoryInfo.category_name = reader.GetString(1);
                                categoryInfo.seller_id = "" + reader.GetInt32(2);

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
    public class CategoryInfo
    {
        public string category_id;
        public string category_name;
        public string seller_id;
    }
}
