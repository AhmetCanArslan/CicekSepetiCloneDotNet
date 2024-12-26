using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.AdminPage.Comments
{
    public class IndexModel : PageModel
    {
        //admin tüm yorumları görebilir düzenleyebliir ve silebilir 
        string connectionString = ConnectionStrings.DefaultConnection;

        public List<CommentInfo> listComments = new List<CommentInfo>();
        public void OnGet()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM tbl_comments ORDER BY Id DESC";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CommentInfo commentInfo = new CommentInfo();
                                commentInfo.commentId = "" + reader.GetInt32(0);
                                commentInfo.productId = "" + reader.GetInt32(1);
                                commentInfo.comment = reader.GetString(2);
                                commentInfo.commentTitle = reader.GetString(3);
                                commentInfo.userId = "" + reader.GetInt32(4);

                                listComments.Add(commentInfo);

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
    public class CommentInfo
    {
        public string? commentId;
        public string? productId;
        public string? comment; 
        public string? commentTitle;
        public string? userId;
        public string? user_name;
        public DateTime date;
        public string? productName;
    }
}
