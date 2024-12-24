using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.Categories
{
    public class IndexModel : PageModel
    {
        string connectionString = ConnectionStrings.DefaultConnection;

        public List<CategoryInfo> listCategory = new List<CategoryInfo>();
        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM TBL_Category";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CategoryInfo categoryInfo = new CategoryInfo();
                                categoryInfo.category_id = "" + reader.GetInt32(0);
                                categoryInfo.category_name = reader.GetString(1);

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
    }
}
