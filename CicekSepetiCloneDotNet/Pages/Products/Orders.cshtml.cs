using CicekSepetiCloneDotNet.Pages.Cart;
using CicekSepetiCloneDotNet.Pages.SellerPage.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.Products
{
    public class OrdersModel : PageModel
    {
        public List<OrderInfo> orders = new List<OrderInfo>();
        public List<OrderInfo> OldOrders = new List<OrderInfo>();
        public string user_id = "";
        string connectionString = ConnectionStrings.DefaultConnection;
        public void OnGet()
        {
            user_id = Request.Query["user_id"];

            // Örnek: Sepet verilerini veritabanından çekiyoruz (simülasyon)
            orders = GetOrders(user_id);
        }

        private List<OrderInfo> GetOrders(string user_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    string sql = "GetOrdersForBuyer";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@BuyerId", user_id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                OrderInfo orderInfo = new OrderInfo();
                                orderInfo.order_id = "" + reader.GetInt32(0);
                                orderInfo.product_name = reader.GetString(1);
                                orderInfo.product_id = "" + reader.GetInt32(2);
                                orderInfo.product_image = reader.GetString(3);

                                orderInfo.buyer_name = reader.GetString(4);
                                orderInfo.productQuantity = reader.GetInt32(5);
                                orderInfo.orderDate = reader.GetDateTime(6);
                                orderInfo.isActive = reader.GetInt32(7);
                                orderInfo.isSent = reader.GetInt32(8);
                                if (orderInfo.isActive == 1)
                                {
                                    orders.Add(orderInfo);
                                }
                                else
                                {
                                    OldOrders.Add(orderInfo);
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
            return orders;

        }


    }
}
