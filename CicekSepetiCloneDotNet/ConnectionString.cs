namespace CicekSepetiCloneDotNet
{
    public static class ConnectionStrings
    {
        public static string DefaultConnection =>
            "Data Source=JUANWIN\\SQLEXPRESS;Initial Catalog=DbProjectCicekSepeti;Integrated Security=True;Encrypt=False";
        // erişim ve kullanım kolaylığı olsun diye static bir class oluşturduk ve connection stringi burada tanımladık.
        // daha sonra kuıllanmak istediğimiz yerde ConnectionStrings.DefaultConnection şeklinde kullanabiliriz.
        // başka bir bilgisayar üzerinde çalıştırılacaksa Data Source kısmı değiştirilmelidir.
        // böylece esnek bir yapı oluşturmuş olduk.
    }
}
