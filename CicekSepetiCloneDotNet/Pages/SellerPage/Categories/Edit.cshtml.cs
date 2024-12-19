using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CicekSepetiCloneDotNet.Pages.SellerPage.Categories
{
    public class EditModel : PageModel
    {
        public CategoryInfo categoryInfo = new CategoryInfo();
        public string errorMessage = "";
        public string succesMessage = "";
        public string intMessage = "";
        public string seller_id;
        public void OnGet()
        {
            string id = Request.Query["id"];
            if (string.IsNullOrEmpty(id))
            {
                errorMessage = "Invalid category ID.";
                return;
            }
            try
            {
                string connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "Select * from TBL_category where category_id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                categoryInfo.category_id = "" + reader.GetInt32(0);
                                categoryInfo.category_name = reader.GetString(1);
                                categoryInfo.seller_id = ""+reader.GetInt32(2);
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

            categoryInfo.category_id = Request.Form["id"];
            categoryInfo.category_name = Request.Form["name"];


            if (categoryInfo.category_name.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                string connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE TBL_Category SET category_name = @name WHERE category_id = @id";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("name", categoryInfo.category_name);
                        command.Parameters.AddWithValue("id", categoryInfo.category_id);


                        command.ExecuteNonQuery();
                    }

                    sql = "Select seller_id from TBL_Category where category_id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", categoryInfo.category_id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                seller_id = ""+reader.GetInt32(0);

                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return;
            }

            Response.Redirect("/SellerPage/Categories/Index?id=" + seller_id);
        }
    }
}
