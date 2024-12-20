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
        string user_id = "";

        // OnGet: Sepet verilerini getirir
        public void OnGet()
        {
            user_id = Request.Query["user_id"];

            // Örnek: Sepet verilerini veritabanından çekiyoruz (simülasyon)
            cartItems = GetCartItemsFromDatabase(user_id);
        }

        // Ürün miktarını güncellemek için POST işlemi
        public IActionResult OnPostUpdateQuantity(int cartId, int quantity)
        {
            if (quantity <= 0)
            {
                ModelState.AddModelError("", "Miktar en az 1 olmalıdır.");
                return Page();
            }

            // Veritabanında miktarı güncelle
            UpdateCartQuantity(cartId, quantity);

            // Tekrar sepete yönlendir
            return RedirectToPage();
        }

        // Ürünü sepetten kaldırmak için POST işlemi
        public IActionResult OnPostRemoveFromCart(int cartId)
        {
            // Veritabanından ürünü sil
            RemoveCartItem(cartId);

            // Tekrar sepete yönlendir
            return RedirectToPage();
        }

        // Alışverişi tamamlamak için POST işlemi
        public IActionResult OnPostCheckout()
        {
            // Ödeme işlemi veya sipariş oluşturma işlemi (simülasyon)
            CheckoutCart();

            // Sepet başarıyla tamamlandı sayfasına yönlendirme
            return RedirectToPage("/Checkout/Success");
        }

        // Aşağıdaki metotlar örnek olarak veritabanı işlemlerini simüle eder:
        private List<CartItemViewModel> GetCartItemsFromDatabase(string user_id)
        {
            try
            {
                string connectionString = "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
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
            // Burada veritabanındaki miktar güncellenebilir.
            // Örnek: "UPDATE Cart SET Quantity = @quantity WHERE CartId = @cartId"
        }

        private void RemoveCartItem(int cartId)
        {
            // Burada ürünü sepetten kaldırmak için veritabanından silinebilir.
            // Örnek: "DELETE FROM Cart WHERE CartId = @cartId"
        }

        private void CheckoutCart()
        {
            // Sepeti tamamla ve siparişi kaydet.
            // Örnek: Sipariş tablosuna ekleme işlemi yapılabilir.
        }
    }

    // Örnek ViewModel
    public class CartItemViewModel
    {
        public string SellerName;
        public string ProductName;
        public int ProductQuantity;
        public string ProductImage;
        public int ProductPrice;
        public string ProductId;
    }
}
