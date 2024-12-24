using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;


namespace CicekSepetiCloneDotNet.Pages.Cart
{
    public class IndexModel : PageModel
    {
        // Sepet verilerinin tutulduğu liste (örnek bir ViewModel)
        public List<CartItemViewModel> cartItems = new List<CartItemViewModel>();
        public string user_id = "";
        string connectionString = ConnectionStrings.DefaultConnection;


        // OnGet: Sepet verilerini getirir
        public void OnGet()
        {
            user_id = Request.Query["user_id"];

            // Örnek: Sepet verilerini veritabanından çekiyoruz (simülasyon)
            cartItems = GetCartItemsFromDatabase(user_id);
        }

        // Ürün miktarını güncellemek için POST işlemi
        public void OnPostUpdateQuantity(int cartId, int quantity ,int user_id)
        {
            if (quantity <= 0)
            {
                ModelState.AddModelError("", "Miktar en az 1 olmalıdır.");
                Page();
            }

            // Veritabanında miktarı güncelle
            UpdateCartQuantity(cartId, quantity);

            // Tekrar sepete yönlendir
            Response.Redirect("/Cart/Index?user_id="+ user_id);
        
        }

    // Ürünü sepetten kaldırmak için POST işlemi
    public void OnPostRemoveFromCart(int cartId, int user_id)
        {
            // Veritabanından ürünü sil
            RemoveCartItem(cartId);

            // Tekrar sepete yönlendir
            Response.Redirect("/Cart/Index?user_id=" + user_id);
        }

        // Alışverişi tamamlamak için POST işlemi
      

        // Aşağıdaki metotlar örnek olarak veritabanı işlemlerini simüle eder:
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
                                cartProducts.ProductId = ""+reader.GetInt32(5);
                                cartProducts.cartId = ""+reader.GetInt32(6);
                                cartProducts.userId = user_id;


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

        private void UpdateCartQuantity(int cartId, int quantity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    string sql = "UPDATE tbl_cart SET quantity = @quantity WHERE CartId = @CartId";



                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CartId", cartId);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.ExecuteNonQuery();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private void RemoveCartItem(int cartId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM TBL_Cart WHERE CartId = @cartId";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@cartId", cartId);
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                // Hata mesajını loglamak için
                Console.WriteLine(ex.Message); // Hata mesajını yazdırma (veya uygun loglama yapılabilir)
                Response.Redirect("/Error"); // Hata sayfasına yönlendirme
            }
        }

     
    }

    // Örnek ViewModel
    public class CartItemViewModel
    {
        public string userId;
        public string cartId;
        public string SellerName;
        public string ProductName;
        public int ProductQuantity;
        public string ProductImage;
        public int ProductPrice;
        public string ProductId;
    }
}
