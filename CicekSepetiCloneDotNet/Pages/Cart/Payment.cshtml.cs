using CicekSepetiCloneDotNet.Pages.AdminPage.Products;
using CicekSepetiCloneDotNet.Pages.AdminPage.Users;
using CicekSepetiCloneDotNet.Pages.SellerPage.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CicekSepetiCloneDotNet.Pages.Cart
{
    public class PaymentModel : PageModel
    {
        public string connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
        public string? buyer_id;
        public List<CartItemViewModel> cartItems = new List<CartItemViewModel>();
        public UsersInfo buyerInfo = new UsersInfo();
        public int totalPrice = 0;
        public void OnGet()
        {
            buyer_id = Request.Query["user_id"];
            cartItems = GetCartItemsFromDatabase(buyer_id);
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
                                buyerInfo.user_id= reader["user_id"].ToString();
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
        }

        public void OnPost()
        {

        }

        private void createOrder()
        {
            //carttan çekilenleri order tablosuna ekle
        }

        public void createPayment()
        {
            //order tablosundan çekilenleri payment tablosuna ekle
        }

        public void createMessage()
        {
            //order ve payment tablosundan çekilenleri message tablosuna ekle
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
    }
}
