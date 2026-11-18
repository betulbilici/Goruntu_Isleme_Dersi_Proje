using System;
using System.Collections.Generic;
using System.Drawing;

namespace GoruntuIsleme
{
    /// <summary>
    /// Dr. Engin ŞAHİN'in ders slaytlarındaki saf Bitmap (GetPixel/SetPixel)
    /// yaklaşımına uygun ortak görüntü işleme yardımcı metotları.
    /// </summary>
    public static class BitmapYardimcisi
    {
        /// <summary>
        /// OrnekGorseller klasöründeki bir örnek fotoğrafı dosyayı kilitlemeden belleğe yükler.
        /// </summary>
        public static Bitmap OrnekGorselYukle(string dosyaAdi)
        {
            string[] olasiYollar = new[]
            {
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OrnekGorseller", dosyaAdi),
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "OrnekGorseller", dosyaAdi),
                System.IO.Path.Combine(@"C:\Users\LENOVO\Desktop\GoruntuIsleme\GoruntuIsleme\GoruntuIsleme\OrnekGorseller", dosyaAdi)
            };

            foreach (string yol in olasiYollar)
            {
                if (System.IO.File.Exists(yol))
                {
                    using Bitmap gecici = new Bitmap(yol);
                    return new Bitmap(gecici);
                }
            }
            return null;
        }

        public class BolgeBilgisi
        {
            public int EtiketNo { get; set; }
            public int PikselSayisi { get; set; }
            public int MinX { get; set; }
            public int MaxX { get; set; }
            public int MinY { get; set; }
            public int MaxY { get; set; }
            public double MerkezX { get; set; }
            public double MerkezY { get; set; }
            public Color Renk { get; set; }

            public int Genislik => MaxX - MinX + 1;
            public int Yukseklik => MaxY - MinY + 1;
            public int Cap => Math.Max(Genislik, Yukseklik);
        }

        /// <summary>
        /// Resmi ortalama yöntemle gri tona dönüştürür.
        /// </summary>
        public static Bitmap GriTonaDonustur(Bitmap girisResmi)
        {
            int genislik = girisResmi.Width;
            int yukseklik = girisResmi.Height;
            Bitmap cikisResmi = new Bitmap(genislik, yukseklik);

            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < yukseklik; y++)
                {
                    Color okunan = girisResmi.GetPixel(x, y);
                    int gri = (okunan.R + okunan.G + okunan.B) / 3;
                    cikisResmi.SetPixel(x, y, Color.FromArgb(gri, gri, gri));
                }
            }
            return cikisResmi;
        }

        /// <summary>
        /// Gri veya renkli resmi eşik değerine göre Siyah-Beyaz (0 / 255) yapar.
        /// </summary>
        public static Bitmap EsiklemeYap(Bitmap girisResmi, int esikDegeri, bool tersle = false)
        {
            int genislik = girisResmi.Width;
            int yukseklik = girisResmi.Height;
            Bitmap cikisResmi = new Bitmap(genislik, yukseklik);

            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < yukseklik; y++)
                {
                    Color okunan = girisResmi.GetPixel(x, y);
                    int gri = (okunan.R + okunan.G + okunan.B) / 3;
                    bool beyazMi = tersle ? (gri < esikDegeri) : (gri >= esikDegeri);
                    int deger = beyazMi ? 255 : 0;
                    cikisResmi.SetPixel(x, y, Color.FromArgb(deger, deger, deger));
                }
            }
            return cikisResmi;
        }

        /// <summary>
        /// Hafta 8 slaytındaki (s. 13-19) iteratif BOLGE_BULMA algoritması.
        /// Beyaz (R > esik) pikselleri 8-komşulukla etiketler, bölgeleri listeler ve renklendirir.
        /// </summary>
        public static Bitmap BolgeBulVeRenklendir(Bitmap siyahBeyazResim, int minPikselAlani, out List<BolgeBilgisi> bolgeler, out int[,] etiketNumarasi)
        {
            int genislik = siyahBeyazResim.Width;
            int yukseklik = siyahBeyazResim.Height;
            etiketNumarasi = new int[genislik, yukseklik];

            int etiketNo = 0;
            bool degisimVar;

            // 1. Aşama: İteratif bölge etiketleme (Slayt W8 s.14-16 mantığı)
            do
            {
                degisimVar = false;
                for (int y = 1; y < yukseklik - 1; y++)
                {
                    for (int x = 1; x < genislik - 1; x++)
                    {
                        if (siyahBeyazResim.GetPixel(x, y).R > 128)
                        {
                            int ilkDeger = etiketNumarasi[x, y];
                            int komsuEnKucuk = 0;

                            for (int j = -1; j <= 1; j++)
                            {
                                for (int i = -1; i <= 1; i++)
                                {
                                    int komsuEtiket = etiketNumarasi[x + i, y + j];
                                    if (komsuEtiket != 0 && (komsuEnKucuk == 0 || komsuEtiket < komsuEnKucuk))
                                    {
                                        komsuEnKucuk = komsuEtiket;
                                    }
                                }
                            }

                            if (komsuEnKucuk != 0)
                            {
                                etiketNumarasi[x, y] = komsuEnKucuk;
                            }
                            else
                            {
                                etiketNo++;
                                etiketNumarasi[x, y] = etiketNo;
                            }

                            if (ilkDeger != etiketNumarasi[x, y])
                                degisimVar = true;
                        }
                    }
                }
            } while (degisimVar);

            // 2. Aşama: Her tekil etiketin istatistiklerini (alan, sınırlar, merkez) topla
            Dictionary<int, BolgeBilgisi> sozluk = new Dictionary<int, BolgeBilgisi>();
            Dictionary<int, long> toplamX = new Dictionary<int, long>();
            Dictionary<int, long> toplamY = new Dictionary<int, long>();

            for (int x = 1; x < genislik - 1; x++)
            {
                for (int y = 1; y < yukseklik - 1; y++)
                {
                    int et = etiketNumarasi[x, y];
                    if (et == 0) continue;

                    if (!sozluk.ContainsKey(et))
                    {
                        sozluk[et] = new BolgeBilgisi
                        {
                            EtiketNo = et,
                            PikselSayisi = 0,
                            MinX = x,
                            MaxX = x,
                            MinY = y,
                            MaxY = y
                        };
                        toplamX[et] = 0;
                        toplamY[et] = 0;
                    }

                    BolgeBilgisi b = sozluk[et];
                    b.PikselSayisi++;
                    if (x < b.MinX) b.MinX = x;
                    if (x > b.MaxX) b.MaxX = x;
                    if (y < b.MinY) b.MinY = y;
                    if (y > b.MaxY) b.MaxY = y;
                    toplamX[et] += x;
                    toplamY[et] += y;
                }
            }

            // Gürültü olan küçük bölgeleri ele ve geçerli bölgelere belirgin renkler ata
            Random rnd = new Random(42);
            bolgeler = new List<BolgeBilgisi>();
            Dictionary<int, Color> renkHaritasi = new Dictionary<int, Color>();

            foreach (var b in sozluk.Values)
            {
                if (b.PikselSayisi >= minPikselAlani)
                {
                    b.MerkezX = (double)toplamX[b.EtiketNo] / b.PikselSayisi;
                    b.MerkezY = (double)toplamY[b.EtiketNo] / b.PikselSayisi;
                    b.Renk = Color.FromArgb(rnd.Next(60, 255), rnd.Next(60, 255), rnd.Next(60, 255));
                    renkHaritasi[b.EtiketNo] = b.Renk;
                    bolgeler.Add(b);
                }
            }

            // 3. Aşama: Çıkış resmini etiket renkleriyle boya
            Bitmap cikisResmi = new Bitmap(genislik, yukseklik);
            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < yukseklik; y++)
                {
                    int et = etiketNumarasi[x, y];
                    if (et != 0 && renkHaritasi.ContainsKey(et))
                    {
                        cikisResmi.SetPixel(x, y, renkHaritasi[et]);
                    }
                    else
                    {
                        cikisResmi.SetPixel(x, y, Color.Black);
                    }
                }
            }

            return cikisResmi;
        }

        /// <summary>
        /// Çok büyük telefon fotoğraflarının GetPixel/SetPixel ile çok uzun sürmemesi için
        /// en-boy oranını koruyarak makul bir boyuta ölçekler.
        /// </summary>
        public static Bitmap BoyutSinirla(Bitmap kaynak, int maxBoyut = 500)
        {
            if (kaynak.Width <= maxBoyut && kaynak.Height <= maxBoyut)
                return new Bitmap(kaynak);

            double oran = Math.Min((double)maxBoyut / kaynak.Width, (double)maxBoyut / kaynak.Height);
            int yeniW = Math.Max(1, (int)(kaynak.Width * oran));
            int yeniH = Math.Max(1, (int)(kaynak.Height * oran));
            return new Bitmap(kaynak, yeniW, yeniH);
        }

        /// <summary>
        /// Basit 3x3 Sobel kenar bulma (Madeni para Yazı/Tura kenar deseni çıkarımı için).
        /// </summary>
        public static Bitmap SobelKenarBul(Bitmap girisResmi)
        {
            int genislik = girisResmi.Width;
            int yukseklik = girisResmi.Height;
            Bitmap cikis = new Bitmap(genislik, yukseklik);

            int[,] gx = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] gy = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

            for (int x = 1; x < genislik - 1; x++)
            {
                for (int y = 1; y < yukseklik - 1; y++)
                {
                    int toplamX = 0, toplamY = 0;
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            Color c = girisResmi.GetPixel(x + i, y + j);
                            int gri = (c.R + c.G + c.B) / 3;
                            toplamX += gri * gx[i + 1, j + 1];
                            toplamY += gri * gy[i + 1, j + 1];
                        }
                    }
                    int kenar = Math.Min(255, Math.Abs(toplamX) + Math.Abs(toplamY));
                    cikis.SetPixel(x, y, Color.FromArgb(kenar, kenar, kenar));
                }
            }
            return cikis;
        }
    }
}

