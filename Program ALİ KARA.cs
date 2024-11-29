using System;
using System.Collections.Generic;

class Program
{
    // Kitap ve Kiralama bilgilerini tutacak sınıflar
    class Kitap
    {
        public string Ad { get; set; }
        public string Yazar { get; set; }
        public int YayinYili { get; set; }
        public int Adet { get; set; }

        public Kitap(string ad, string yazar, int yayinYili, int adet)
        {
            Ad = ad;
            Yazar = yazar;
            YayinYili = yayinYili;
            Adet = adet;
        }
    }

    class Kiralama
    {
        public string KullaniciAd { get; set; }
        public string KitapAd { get; set; }
        public int KiralamaSuresi { get; set; }
        public DateTime IadeTarihi { get; set; }

        public Kiralama(string kullaniciAd, string kitapAd, int kiralamaSuresi, DateTime iadeTarihi)
        {
            KullaniciAd = kullaniciAd;
            KitapAd = kitapAd;
            KiralamaSuresi = kiralamaSuresi;
            IadeTarihi = iadeTarihi;
        }
    }

    // Kitaplar ve kiralamalar için liste tanımlamaları
    static List<Kitap> kitaplar = new List<Kitap>();
    static List<Kiralama> kiralamalar = new List<Kiralama>();

    // Kitap Ekleme Fonksiyonu
    static void KitapEkle()
    {
        Console.Write("Kitap Adı: ");
        string ad = Console.ReadLine();

        Console.Write("Yazar Adı: ");
        string yazar = Console.ReadLine();

        Console.Write("Yayın Yılı: ");
        int yayinYili = int.Parse(Console.ReadLine());

        Console.Write("Adet (Stok Adedi): ");
        int adet = int.Parse(Console.ReadLine());

        // Kitap zaten varsa, stok artırılır
        var mevcutKitap = kitaplar.Find(k => k.Ad == ad && k.Yazar == yazar);
        if (mevcutKitap != null)
        {
            mevcutKitap.Adet += adet;
            Console.WriteLine($"{adet} adet {ad} kitabı mevcut stok miktarına eklendi.");
        }
        else
        {
            kitaplar.Add(new Kitap(ad, yazar, yayinYili, adet));
            Console.WriteLine($"{ad} kitabı kütüphaneye eklendi.");
        }
    }

    // Kitap Kiralama Fonksiyonu
    static void KitapKirala()
    {
        if (kitaplar.Count == 0)
        {
            Console.WriteLine("Kütüphanede hiç kitap bulunmamaktadır.");
            return;
        }

        Console.WriteLine("Mevcut Kitaplar:");
        for (int i = 0; i < kitaplar.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {kitaplar[i].Ad} - {kitaplar[i].Yazar} - Stok: {kitaplar[i].Adet}");
        }

        Console.Write("Kiralamak istediğiniz kitabı seçin (1, 2, 3, ...): ");
        int secim = int.Parse(Console.ReadLine()) - 1;

        if (secim < 0 || secim >= kitaplar.Count)
        {
            Console.WriteLine("Geçersiz seçim.");
            return;
        }

        var kitap = kitaplar[secim];
        if (kitap.Adet <= 0)
        {
            Console.WriteLine("Stokta yeterli kitap yok.");
            return;
        }

        Console.Write("Kaç gün kiralamak istersiniz? ");
        int kiralamaSuresi = int.Parse(Console.ReadLine());
        int kiraUcreti = kiralamaSuresi * 5;

        Console.Write("Bütçeniz ne kadar? ");
        int butce = int.Parse(Console.ReadLine());

        if (butce >= kiraUcreti)
        {
            Console.Write("Adınızı girin: ");
            string kullaniciAd = Console.ReadLine();

            DateTime iadeTarihi = DateTime.Today.AddDays(kiralamaSuresi);
            kiralamalar.Add(new Kiralama(kullaniciAd, kitap.Ad, kiralamaSuresi, iadeTarihi));

            kitap.Adet -= 1;
            Console.WriteLine($"{kitap.Ad} kitabı {kullaniciAd} tarafından kiralandı. İade tarihi: {iadeTarihi.ToShortDateString()}");
        }
        else
        {
            Console.WriteLine("Bütçeniz yeterli değil.");
        }
    }

    // Kitap İade Etme Fonksiyonu
    static void KitapIadeEt()
    {
        if (kiralamalar.Count == 0)
        {
            Console.WriteLine("Hiçbir kitap kiralanmamış.");
            return;
        }

        Console.WriteLine("Kiralanan Kitaplar:");
        for (int i = 0; i < kiralamalar.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {kiralamalar[i].KitapAd} - Kiralayan: {kiralamalar[i].KullaniciAd} - İade Tarihi: {kiralamalar[i].IadeTarihi.ToShortDateString()}");
        }

        Console.Write("İade etmek istediğiniz kitabı seçin (1, 2, 3, ...): ");
        int secim = int.Parse(Console.ReadLine()) - 1;

        if (secim < 0 || secim >= kiralamalar.Count)
        {
            Console.WriteLine("Geçersiz seçim.");
            return;
        }

        var kiralama = kiralamalar[secim];
        var kitap = kitaplar.Find(k => k.Ad == kiralama.KitapAd);

        if (kitap != null)
        {
            kitap.Adet += 1;
            kiralamalar.RemoveAt(secim);
            Console.WriteLine($"{kiralama.KitapAd} kitabı iade edildi ve stok artırıldı.");
        }
    }

    // Kitap Arama Fonksiyonu
    static void KitapArama()
    {
        Console.Write("Kitap adıyla mı yoksa yazar adıyla mı arama yapmak istersiniz? (kitap/yazar): ");
        string secim = Console.ReadLine().ToLower();

        if (secim == "kitap")
        {
            Console.Write("Kitap adı girin: ");
            string arama = Console.ReadLine();
            foreach (var kitap in kitaplar)
            {
                if (kitap.Ad.Contains(arama, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"{kitap.Ad} - {kitap.Yazar} - {kitap.YayinYili} - Stok: {kitap.Adet}");
                }
            }
        }
        else if (secim == "yazar")
        {
            Console.Write("Yazar adı girin: ");
            string arama = Console.ReadLine();
            foreach (var kitap in kitaplar)
            {
                if (kitap.Yazar.Contains(arama, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"{kitap.Ad} - {kitap.Yazar} - {kitap.YayinYili} - Stok: {kitap.Adet}");
                }
            }
        }
        else
        {
            Console.WriteLine("Geçersiz seçim.");
        }
    }

    // Raporlama Fonksiyonu
    static void Raporlama()
    {
        Console.WriteLine("Raporlama Seçenekleri:");
        Console.WriteLine("1. Tüm kitapları listele");
        Console.WriteLine("2. Belirli bir yazara ait kitapları listele");
        Console.WriteLine("3. Belirli bir yayın yılına ait kitapları listele");
        Console.WriteLine("4. Kirada olan kitapları listele");
        Console.Write("Seçiminizi yapın: ");
        int secim = int.Parse(Console.ReadLine());

        if (secim == 1)
        {
            foreach (var kitap in kitaplar)
            {
                Console.WriteLine($"{kitap.Ad} - {kitap.Yazar} - {kitap.YayinYili} - Stok: {kitap.Adet}");
            }
        }
        else if (secim == 2)
        {
            Console.Write("Yazar adı girin: ");
            string yazar = Console.ReadLine();
            foreach (var kitap in kitaplar)
            {
                if (kitap.Yazar.Contains(yazar, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"{kitap.Ad} - {kitap.Yazar} - {kitap.YayinYili} - Stok: {kitap.Adet}");
                }
            }
        }
        else if (secim == 3)
        {
            Console.Write("Yayın yılı girin: ");
            int yayinYili = int.Parse(Console.ReadLine());
            foreach (var kitap in kitaplar)
            {
                if (kitap.YayinYili == yayinYili)
                {
                    Console.WriteLine($"{kitap.Ad} - {kitap.Yazar} - {kitap.YayinYili} - Stok: {kitap.Adet}");
                }
            }
        }
        else if (secim == 4)
        {
            foreach (var kiralama in kiralamalar)
            {
                Console.WriteLine($"{kiralama.KitapAd} - Kiralayan: {kiralama.KullaniciAd} - İade Tarihi: {kiralama.IadeTarihi.ToShortDateString()}");
            }
        }
    }

    // Menü Sistemi
    static void AnaMenu()
    {
        while (true)
        {
            Console.WriteLine("\nKütüphane Yönetim Sistemi");
            Console.WriteLine("1. Kitap Ekle");
            Console.WriteLine("2. Kitap Kirala");
            Console.WriteLine("3. Kitap İade Et");
            Console.WriteLine("4. Kitap Arama");
            Console.WriteLine("5. Raporlama");
            Console.WriteLine("6. Çıkış");
            Console.Write("Seçiminizi yapın: ");
            int secim = int.Parse(Console.ReadLine());

            switch (secim)
            {
                case 1:
                    KitapEkle();
                    break;
                case 2:
                    KitapKirala();
                    break;
                case 3:
                    KitapIadeEt();
                    break;
                case 4:
                    KitapArama();
                    break;
                case 5:
                    Raporlama();
                    break;
                case 6:
                    Console.WriteLine("Çıkılıyor...");
                    return;
                default:
                    Console.WriteLine("Geçersiz seçim.");
                    break;
            }
        }
    }

    // Uygulama Başlatma
    static void Main()
    {
        AnaMenu();
    }
}
