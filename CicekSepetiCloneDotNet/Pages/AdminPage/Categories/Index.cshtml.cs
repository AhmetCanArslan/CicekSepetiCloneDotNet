using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.AdminPage.Categories
{
    public class IndexModel : PageModel
    {
        public List<CategoryInfo> listCategory = new List<CategoryInfo>();
        string connectionString = ConnectionStrings.DefaultConnection;

        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "GetCategoriesWithCreatorName";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CategoryInfo categoryInfo = new CategoryInfo();
                                categoryInfo.category_id = "" + reader.GetInt32(0);
                                categoryInfo.category_name = reader.GetString(1);
                                categoryInfo.creator_id = "" + reader.GetInt32(2);
                                categoryInfo.creator_name = reader.GetString(3);


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
        public string? category_id;
        public string? category_name; 
        public string? creator_id;
        public string? creator_name;

    }
}
