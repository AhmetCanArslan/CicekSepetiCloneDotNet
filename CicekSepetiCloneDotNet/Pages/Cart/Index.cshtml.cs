using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace CicekSepetiCloneDotNet.Pages.Cart
{
    public class IndexModel : PageModel
    {
        // Sepet verilerinin tutulduğu liste (örnek bir ViewModel)
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();

        // OnGet: Sepet verilerini getirir
        public void OnGet()
        {
            // Örnek: Sepet verilerini veritabanından çekiyoruz (simülasyon)
            CartItems = GetCartItemsFromDatabase();
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
        private List<CartItemViewModel> GetCartItemsFromDatabase()
        {
            // Örnek veriler
            return new List<CartItemViewModel>
            {
                new CartItemViewModel { CartId = 1, ProductName = "Ürün 1", ProductImage = "/images/urun1.jpg", Price = 100, Quantity = 2 },
                new CartItemViewModel { CartId = 2, ProductName = "Ürün 2", ProductImage = "/images/urun2.jpg", Price = 200, Quantity = 1 }
            };
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
        public int CartId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
