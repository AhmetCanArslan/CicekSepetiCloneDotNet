using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.SellerPage.Orders
{
    public class IndexModel : PageModel
    {
        string connectionString = ConnectionStrings.DefaultConnection;
        public List<OrderInfo> NewlistOrder = new List<OrderInfo>();
        public List<OrderInfo> OldListOrder = new List<OrderInfo>();
        public string seller_id;
        public void OnGet()
        {
            // burada satıcıya ait siparişler listelenir accept işelmmeri buradan yapılır
            seller_id = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "GetOrdersForSeller";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@SellerId", seller_id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OrderInfo orderInfo = new OrderInfo();
                                orderInfo.order_id = "" + reader.GetInt32(0);
                                orderInfo.product_name = reader.GetString(1);
                                orderInfo.product_id = "" + reader.GetInt32(2);
                                orderInfo.buyer_name = reader.GetString(3);
                                orderInfo.productQuantity = reader.GetInt32(4);
                                orderInfo.orderDate = reader.GetDateTime(5);
                                orderInfo.isActive = reader.GetInt32(6);
                                orderInfo.isSent = reader.GetInt32(7);
                                orderInfo.product_image = reader.GetString(11);
                                if (orderInfo.isActive == 1)
                                {
                                    NewlistOrder.Add(orderInfo);
                                }
                                else
                                {
                                    OldListOrder.Add(orderInfo);
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
        public void OnPostAcceptOrder(int orderId, int seller_id)
        {
            try
            {// eğer sipariş onaylanırsa isSent=1 yapılır ve isActive=0 yapılır ve Siparişler farazi olarak yola çıkar
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "update tbl_orders set isSent=1, isActive=0 where id=" + orderId;
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

            Response.Redirect("/SellerPage/Orders/Index?id=" + seller_id);

        }
    }

    public class OrderInfo
    {
        public string order_id;
        public string product_name; 
        public string product_id;
        public string buyer_name;
        public int productQuantity;
        public DateTime orderDate;
        public int isActive;
        public int isSent;
        public string payment_method;
        public string address;
        public int price;
        public string product_image;
    }
}
