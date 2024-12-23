using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.SellerPage
{
    public class IndexModel : PageModel
    {
        public string connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";

        string? seller_id;
        public List<MessageInfo> ReadListMessage = new List<MessageInfo>();
        public List<MessageInfo> UnreadListMessage = new List<MessageInfo>();
        public void OnGet()
        {
            seller_id = Request.Query["user_id"];
            getMessages(seller_id);
        }
        public void OnPostReadMessage(int message_id, int seller_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "update tbl_messages set isRead=1 where id=" + message_id;
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }

            Response.Redirect("/SellerPage/Index?user_id=" + seller_id);

        }

        private void getMessages(string? seller_id)
        {
            try
            {
                string connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "select * from tbl_messages where sellerId = @sellerId";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@sellerId", seller_id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MessageInfo messageInfo = new MessageInfo();
                                messageInfo.Id = "" + reader.GetInt32(0);
                                messageInfo.Title = reader.GetString(1);
                                messageInfo.isRead = "" + reader.GetInt32(2);
                                messageInfo.SellerId = "" + reader.GetInt32(3);
                                messageInfo.productId = "" + reader.GetInt32(4);

                                if(messageInfo.isRead == "1")
                                {
                                    ReadListMessage.Add(messageInfo);
                                }
                                else
                                {
                                    UnreadListMessage.Add(messageInfo);
                                }

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
        public class MessageInfo
        {
            public string Id;
            public string Title;
            public string isRead;
            public string SellerId;
            public string productId;
        }
    }
}
