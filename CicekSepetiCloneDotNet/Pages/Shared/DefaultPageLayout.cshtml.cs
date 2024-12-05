using CicekSepetiCloneDotNet.Pages.Categories;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.Shared
{
    public class DefaultPageLayout : PageModel
    {
        public Dictionary<int, List<CategoryInfo>> CategoriesByParentId { get; set; } = new Dictionary<int, List<CategoryInfo>>();

        public void LoadCategories()
        {
            try
            {
                string connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT category_id, category_name, category_parent_id FROM TBL_Category";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var category = new CategoryInfo
                                {
                                    category_id = ""+reader.GetInt32(0),
                                    category_name = reader.GetString(1),
                                    category_parent_id =""+ reader.GetInt32(2) 
                                };

                                int parentId =  Convert.ToInt32(category.category_parent_id);

                                if (!CategoriesByParentId.ContainsKey(parentId))
                                {
                                    CategoriesByParentId[parentId] = new List<CategoryInfo>();
                                }

                                CategoriesByParentId[parentId].Add(category);
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
}
