using CicekSepetiCloneDotNet.Pages.AdminPage.Products;
using CicekSepetiCloneDotNet.Pages.AdminPage.Users;
using CicekSepetiCloneDotNet.Pages.SellerPage.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CicekSepetiCloneDotNet.Pages.Cart
{
    public class PaymentModel : PageModel
    {
        string connectionString = ConnectionStrings.DefaultConnection;
        public string? buyer_id;
        public List<CartItemViewModel> cartItems = new List<CartItemViewModel>();
        public List<string> productIds = new List<string>();
        public UsersInfo buyerInfo = new UsersInfo();
        public int totalPrice = 0;
        public void OnGet()
        {
            buyer_id = Request.Query["user_id"];
            cartItems = GetCartItemsFromDatabase(buyer_id);// sepetteki ürünleri getirir. kullanıcı id'si ile
            getBuyerInfo(buyer_id);// kullanıcı bilgilerini getirir. kullanıcı id'si ile


        }
        public string paymentMethod;
        public string address;
        public void OnPost()
        {
            buyer_id = Request.Query["user_id"];

            paymentMethod = Request.Form["paymentMethod"];
            address = Request.Form["address"];

            List<CartItemViewModel> cartItems2 = GetCartItemsFromDatabase(buyer_id);
            createOrder(cartItems2, address);//ürünleri order tablosuna ekler

            List<string> productIds = GetProductIds(buyer_id);

            UsersInfo buyerInfo = getBuyerInfo(buyer_id);//kullanıcı bilgilerini getirir


            deactiveCartItems(cartItems2);//satın alınan ürünleri cart tablosundan siler

            Response.Redirect("/Cart/Successful");
        }

       

        private UsersInfo getBuyerInfo(string buyer_id)
        {
            try
            {
                //get user addres from database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "Select * from tbl_users where user_id=@buyer_id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@buyer_id", buyer_id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                buyerInfo.user_id = reader["user_id"].ToString();
                                buyerInfo.user_name = reader["user_name"].ToString();
                                buyerInfo.user_surname = reader["user_surname"].ToString();
                                buyerInfo.user_mail = reader["user_mail"].ToString();
                                buyerInfo.user_number = reader["user_number"].ToString();
                                buyerInfo.user_city = reader["user_city"].ToString();
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
            return buyerInfo;
        }

        private void deactiveCartItems(List<CartItemViewModel> cartItems)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "update TBL_cart set isActive=0 where cartId=@cartId";
                        

                    foreach (var item in cartItems)
                    {
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@cartId", item.cartId);


                            command.ExecuteNonQuery();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void createOrder(List<CartItemViewModel> cartItems, string address)
        {
            //carttan çekilenleri order tablosuna ekle
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO TBL_orders " +
                        "(productId, userId,productQuantity,orderDate,isActive, isSent, PaymentMethod, Adress,Price) VALUES " +
                        "(@productId, @userId, @productQuantity, @orderDate, 1, 0,@PaymentMethod,@Adress, @price);";

                    foreach (var item in cartItems)
                    {
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@productId", item.ProductId);
                            command.Parameters.AddWithValue("@userId", item.userId);
                            command.Parameters.AddWithValue("@productQuantity", item.ProductQuantity);
                            command.Parameters.AddWithValue("@orderDate", DateTime.Now);
                            command.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                            command.Parameters.AddWithValue("@Adress", address);
                            command.Parameters.AddWithValue("@price", item.ProductPrice * item.ProductQuantity);

                            command.ExecuteNonQuery();
                        }
                        
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
      


        private List<CartItemViewModel> GetCartItemsFromDatabase(string user_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    string sql = "GetCartDetailsByUser";



                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", user_id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CartItemViewModel cartProducts = new CartItemViewModel();
                                cartProducts.SellerName = reader.GetString(0);
                                cartProducts.ProductName = reader.GetString(1);
                                cartProducts.ProductQuantity = reader.GetInt32(2);
                                cartProducts.ProductImage = reader.GetString(3);
                                cartProducts.ProductPrice = reader.GetInt32(4);
                                cartProducts.ProductId = "" + reader.GetInt32(5);
                                cartProducts.cartId = "" + reader.GetInt32(6);
                                cartProducts.userId = user_id;
                                totalPrice += cartProducts.ProductPrice * cartProducts.ProductQuantity;
                                cartItems.Add(cartProducts);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return cartItems;

        }
        private List<string> GetProductIds(string user_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    string sql = "GetCartDetailsByUser";



                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", user_id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CartItemViewModel cartProducts = new CartItemViewModel();
                                cartProducts.ProductId = "" + reader.GetInt32(5);
                                productIds.Add(cartProducts.ProductId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return productIds;

        }
    }
}
