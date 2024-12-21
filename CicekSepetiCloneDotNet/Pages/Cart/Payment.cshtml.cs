using CicekSepetiCloneDotNet.Pages.SellerPage.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.Cart
{
    public class PaymentModel : PageModel
    {
        public string connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
        public string buyer_id;
        public List<OrderInfo> listOrder = new List<OrderInfo>();
        public void OnGet()
        {
            buyer_id = Request.Query["id"];
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "GetOrdersForBuyer";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@BuyerId", buyer_id);
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
                                if (orderInfo.isActive == 1)
                                {
                                    listOrder.Add(orderInfo);
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
    }
}
