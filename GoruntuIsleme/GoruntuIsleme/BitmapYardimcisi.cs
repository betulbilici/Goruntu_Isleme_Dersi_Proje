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
        /// Beyaz (R > 128) pikselleri 8-komşulukla etiketler, bölgeleri listeler ve renklendirir.
        /// </summary>
        public static Bitmap BolgeBulVeRenklendir(Bitmap siyahBeyazResim, int minPikselAlani, out List<BolgeBilgisi> bolgeler, out int[,] etiketNumarasi)
        {
            int genislik = siyahBeyazResim.Width;
            int yukseklik = siyahBeyazResim.Height;
            etiketNumarasi = new int[genislik, yukseklik];
            bool[,] beyazMi = new bool[genislik, yukseklik];

            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < yukseklik; y++)
                {
                    beyazMi[x, y] = siyahBeyazResim.GetPixel(x, y).R > 128;
                }
            }

            int etiketNo = 0;
            bool degisimVar;

            // 1. Aşama: İleri + geri taramalı iteratif bölge etiketleme
            do
            {
                degisimVar = false;

                // İleri tarama
                for (int y = 1; y < yukseklik - 1; y++)
                {
                    for (int x = 1; x < genislik - 1; x++)
                    {
                        if (!beyazMi[x, y]) continue;

                        int ilkDeger = etiketNumarasi[x, y];
                        int komsuEnKucuk = KomsuEnKucukEtiketBul(etiketNumarasi, x, y);

                        if (komsuEnKucuk != 0)
                            etiketNumarasi[x, y] = komsuEnKucuk;
                        else
                        {
                            etiketNo++;
                            etiketNumarasi[x, y] = etiketNo;
                        }

                        if (ilkDeger != etiketNumarasi[x, y])
                            degisimVar = true;
                    }
                }

                // Geri tarama (yakınsamayı hızlandırır)
                if (degisimVar)
                {
                    for (int y = yukseklik - 2; y >= 1; y--)
                    {
                        for (int x = genislik - 2; x >= 1; x--)
                        {
                            if (!beyazMi[x, y]) continue;

                            int ilkDeger = etiketNumarasi[x, y];
                            int komsuEnKucuk = KomsuEnKucukEtiketBul(etiketNumarasi, x, y);
                            if (komsuEnKucuk != 0 && komsuEnKucuk < ilkDeger)
                            {
                                etiketNumarasi[x, y] = komsuEnKucuk;
                                degisimVar = true;
                            }
                        }
                    }
                }
            } while (degisimVar);

            return EtiketlerdenBolgeVeRenkliResimOlustur(etiketNumarasi, genislik, yukseklik, minPikselAlani, out bolgeler);
        }

        private static int KomsuEnKucukEtiketBul(int[,] etiketNumarasi, int x, int y)
        {
            int enKucuk = 0;
            for (int j = -1; j <= 1; j++)
            {
                for (int i = -1; i <= 1; i++)
                {
                    int komsuEtiket = etiketNumarasi[x + i, y + j];
                    if (komsuEtiket != 0 && (enKucuk == 0 || komsuEtiket < enKucuk))
                    {
                        enKucuk = komsuEtiket;
                    }
                }
            }
            return enKucuk;
        }

        private static Bitmap EtiketlerdenBolgeVeRenkliResimOlustur(
            int[,] etiketNumarasi,
            int genislik,
            int yukseklik,
            int minPikselAlani,
            out List<BolgeBilgisi> bolgeler)
        {
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

            // Küçük gürültü etiketlerini matristen sıfırla
            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < yukseklik; y++)
                {
                    int et = etiketNumarasi[x, y];
                    if (et != 0 && !renkHaritasi.ContainsKey(et))
                        etiketNumarasi[x, y] = 0;
                }
            }

            Bitmap cikisResmi = new Bitmap(genislik, yukseklik);
            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < yukseklik; y++)
                {
                    int et = etiketNumarasi[x, y];
                    cikisResmi.SetPixel(x, y, et != 0 ? renkHaritasi[et] : Color.Black);
                }
            }

            return cikisResmi;
        }

        /// <summary>
        /// 5x5 Gaussian çekirdeği ile gri tonlu görüntüdeki küçük gürültü ve gölge pürüzlerini azaltır.
        /// </summary>
        public static Bitmap GaussianBlurUygula(Bitmap girisResmi)
        {
            int w = girisResmi.Width;
            int h = girisResmi.Height;
            int[,] gri = GriDiziOku(girisResmi);
            Bitmap cikis = new Bitmap(w, h);

            int[,] cekirdek =
            {
                { 1,  4,  7,  4, 1 },
                { 4, 16, 26, 16, 4 },
                { 7, 26, 41, 26, 7 },
                { 4, 16, 26, 16, 4 },
                { 1,  4,  7,  4, 1 }
            };

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    int toplam = 0;
                    for (int i = -2; i <= 2; i++)
                    {
                        int nx = Math.Clamp(x + i, 0, w - 1);
                        for (int j = -2; j <= 2; j++)
                        {
                            int ny = Math.Clamp(y + j, 0, h - 1);
                            toplam += gri[nx, ny] * cekirdek[i + 2, j + 2];
                        }
                    }
                    int deger = Math.Clamp(toplam / 273, 0, 255);
                    cikis.SetPixel(x, y, Color.FromArgb(deger, deger, deger));
                }
            }
            return cikis;
        }

        /// <summary>
        /// Gri tonlu resimde sınıflar arası varyansı maksimize eden Otsu otomatik eşik değerini bulur.
        /// </summary>
        public static int OtsuEsikHesapla(Bitmap griResim)
        {
            int w = griResim.Width;
            int h = griResim.Height;
            int[] hist = new int[256];

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Color c = griResim.GetPixel(x, y);
                    int gri = (c.R + c.G + c.B) / 3;
                    hist[gri]++;
                }
            }

            int toplamPiksel = w * h;
            double toplamYogunluk = 0;
            for (int i = 0; i < 256; i++)
                toplamYogunluk += i * hist[i];

            double arkaToplam = 0;
            int arkaSayac = 0;
            double enYuksekVaryans = -1;
            int enIyiEsik = 128;

            for (int t = 0; t < 256; t++)
            {
                arkaSayac += hist[t];
                if (arkaSayac == 0) continue;

                int onSayac = toplamPiksel - arkaSayac;
                if (onSayac == 0) break;

                arkaToplam += t * hist[t];
                double mArka = arkaToplam / arkaSayac;
                double mOn = (toplamYogunluk - arkaToplam) / onSayac;

                double fark = mArka - mOn;
                double varyans = (double)arkaSayac * onSayac * fark * fark;

                if (varyans > enYuksekVaryans)
                {
                    enYuksekVaryans = varyans;
                    enIyiEsik = t;
                }
            }

            return enIyiEsik;
        }

        /// <summary>
        /// İkili maskeye 3x3 kare şablonla Açma (gürültü temizleme) ve Kapama (delik kapatma) uygular.
        /// </summary>
        public static Bitmap MorfolojiAcmaKapama(Bitmap ikiliResim, int acmaTekrar = 1, int kapamaTekrar = 1)
        {
            int w = ikiliResim.Width;
            int h = ikiliResim.Height;
            bool[,] maske = new bool[w, h];

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    maske[x, y] = ikiliResim.GetPixel(x, y).R > 128;
                }
            }

            maske = MaskeAcmaKapama(maske, w, h, acmaTekrar, kapamaTekrar);

            Bitmap cikis = new Bitmap(w, h);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    cikis.SetPixel(x, y, maske[x, y] ? Color.White : Color.Black);
                }
            }
            return cikis;
        }

        /// <summary>
        /// Boolean maske dizisine 3x3 kare şablonla Açma ve Kapama uygular.
        /// </summary>
        public static bool[,] MaskeAcmaKapama(bool[,] maske, int w, int h, int acmaTekrar = 1, int kapamaTekrar = 1)
        {
            bool[,] sonuc = (bool[,])maske.Clone();

            // Açma: Aşındır -> Genişlet (küçük nokta ve gölge kalıntılarını siler)
            for (int k = 0; k < acmaTekrar; k++)
            {
                sonuc = IkiliSablonUygula(sonuc, w, h, asindir: true);
                sonuc = IkiliSablonUygula(sonuc, w, h, asindir: false);
            }

            // Kapama: Genişlet -> Aşındır (nesne içindeki küçük delikleri doldurur)
            for (int k = 0; k < kapamaTekrar; k++)
            {
                sonuc = IkiliSablonUygula(sonuc, w, h, asindir: false);
                sonuc = IkiliSablonUygula(sonuc, w, h, asindir: true);
            }

            return sonuc;
        }

        private static bool[,] IkiliSablonUygula(bool[,] giris, int w, int h, bool asindir)
        {
            bool[,] cikis = new bool[w, h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    bool durum = asindir;
                    for (int i = -1; i <= 1; i++)
                    {
                        int nx = Math.Clamp(x + i, 0, w - 1);
                        for (int j = -1; j <= 1; j++)
                        {
                            int ny = Math.Clamp(y + j, 0, h - 1);
                            if (asindir && !giris[nx, ny])
                            {
                                durum = false;
                                break;
                            }
                            if (!asindir && giris[nx, ny])
                            {
                                durum = true;
                                break;
                            }
                        }
                        if (durum != asindir) break;
                    }
                    cikis[x, y] = durum;
                }
            }
            return cikis;
        }

        /// <summary>
        /// Saf C# Canny Kenar Bulma: Sobel gradyan + Non-Maximum Suppression + Çift Eşikleme & Histerezis.
        /// </summary>
        public static Bitmap CannyKenarBul(Bitmap griBulanikResim, int dusukEsik = 30, int yuksekEsik = 80)
        {
            int w = griBulanikResim.Width;
            int h = griBulanikResim.Height;
            int[,] gri = GriDiziOku(griBulanikResim);

            double[,] mag = new double[w, h];
            int[,] yon = new int[w, h]; // 0: yatay, 1: 45, 2: dikey, 3: 135

            int[,] sx = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
            int[,] sy = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };

            for (int x = 1; x < w - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    double gx = 0, gy = 0;
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int val = gri[x + i, y + j];
                            gx += val * sx[j + 1, i + 1];
                            gy += val * sy[j + 1, i + 1];
                        }
                    }

                    mag[x, y] = Math.Sqrt(gx * gx + gy * gy);
                    double aci = Math.Atan2(gy, gx) * (180.0 / Math.PI);
                    if (aci < 0) aci += 180.0;

                    if (aci < 22.5 || aci >= 157.5) yon[x, y] = 0;
                    else if (aci < 67.5) yon[x, y] = 1;
                    else if (aci < 112.5) yon[x, y] = 2;
                    else yon[x, y] = 3;
                }
            }

            // Non-Maximum Suppression (NMS)
            double[,] nms = new double[w, h];
            for (int x = 1; x < w - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    double q = 0, r = 0;
                    switch (yon[x, y])
                    {
                        case 0: q = mag[x + 1, y]; r = mag[x - 1, y]; break;
                        case 1: q = mag[x + 1, y - 1]; r = mag[x - 1, y + 1]; break;
                        case 2: q = mag[x, y + 1]; r = mag[x, y - 1]; break;
                        default: q = mag[x - 1, y - 1]; r = mag[x + 1, y + 1]; break;
                    }
                    nms[x, y] = (mag[x, y] >= q && mag[x, y] >= r) ? mag[x, y] : 0;
                }
            }

            // Çift Eşikleme + Histerezis
            int alt = Math.Min(dusukEsik, yuksekEsik);
            int ust = Math.Max(dusukEsik, yuksekEsik);
            byte[,] durum = new byte[w, h]; // 0: yok, 128: zayıf, 255: güçlü
            Queue<(int x, int y)> kuyruk = new Queue<(int x, int y)>();

            for (int x = 1; x < w - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    if (nms[x, y] >= ust)
                    {
                        durum[x, y] = 255;
                        kuyruk.Enqueue((x, y));
                    }
                    else if (nms[x, y] >= alt)
                    {
                        durum[x, y] = 128;
                    }
                }
            }

            while (kuyruk.Count > 0)
            {
                var p = kuyruk.Dequeue();
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        int nx = p.x + i;
                        int ny = p.y + j;
                        if (nx > 0 && nx < w - 1 && ny > 0 && ny < h - 1 && durum[nx, ny] == 128)
                        {
                            durum[nx, ny] = 255;
                            kuyruk.Enqueue((nx, ny));
                        }
                    }
                }
            }

            Bitmap kenarBmp = new Bitmap(w, h);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    kenarBmp.SetPixel(x, y, durum[x, y] == 255 ? Color.White : Color.Black);
                }
            }
            return kenarBmp;
        }

        /// <summary>
        /// Mesafe dönüşümü ve çekirdek büyütme (Watershed benzeri BFS) ile
        /// birbirine temas eden ceviz ve fındıkları ayrı etiketlere böler.
        /// </summary>
        public static Bitmap TemasEdenBolgeleriAyir(
            Bitmap ikiliMaske,
            int minPikselAlani,
            out List<BolgeBilgisi> bolgeler,
            out int[,] etiketNumarasi)
        {
            int w = ikiliMaske.Width;
            int h = ikiliMaske.Height;

            // İlk olarak normal bağlantılı bileşenleri bul
            BolgeBulVeRenklendir(ikiliMaske, minPikselAlani, out var hamBolgeler, out int[,] hamEtiket);

            // 2 geçişli Manhattan mesafe dönüşümü
            int[,] mesafe = new int[w, h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (hamEtiket[x, y] == 0)
                    {
                        mesafe[x, y] = 0;
                    }
                    else if (x == 0 || y == 0 || x == w - 1 || y == h - 1)
                    {
                        mesafe[x, y] = 1;
                    }
                    else
                    {
                        mesafe[x, y] = Math.Min(mesafe[x - 1, y], mesafe[x, y - 1]) + 1;
                    }
                }
            }

            for (int x = w - 2; x >= 1; x--)
            {
                for (int y = h - 2; y >= 1; y--)
                {
                    if (mesafe[x, y] > 0)
                    {
                        int komsu = Math.Min(mesafe[x + 1, y], mesafe[x, y + 1]) + 1;
                        if (komsu < mesafe[x, y])
                            mesafe[x, y] = komsu;
                    }
                }
            }

            etiketNumarasi = new int[w, h];
            int sonrakiEtiketNo = 1;

            foreach (var bolge in hamBolgeler)
            {
                int maxMesafe = 0;
                for (int x = bolge.MinX; x <= bolge.MaxX; x++)
                {
                    for (int y = bolge.MinY; y <= bolge.MaxY; y++)
                    {
                        if (hamEtiket[x, y] == bolge.EtiketNo && mesafe[x, y] > maxMesafe)
                            maxMesafe = mesafe[x, y];
                    }
                }

                // Küçük/ince nesneleri bölme
                if (maxMesafe < 6)
                {
                    HamBolgeyiKopyala(hamEtiket, etiketNumarasi, bolge, sonrakiEtiketNo++);
                    continue;
                }

                int cekirdekEsik = (int)(maxMesafe * 0.62);
                int[,] cekirdekEtiket = new int[w, h];
                List<(int id, double cx, double cy, int alan)> cekirdekler = CekirdekBilesenleriniBul(
                    hamEtiket, mesafe, bolge, cekirdekEsik, cekirdekEtiket);

                // Yakın alt tepeleri (tek oval ceviz içindeki pürüzleri) birleştir
                double minMerkezUzaklik = maxMesafe * 1.15;
                var birlesikCekirdekler = YakinCekirdekleriBirlestir(
                    cekirdekler, cekirdekEtiket, bolge, minMerkezUzaklik);

                if (birlesikCekirdekler.Count <= 1)
                {
                    HamBolgeyiKopyala(hamEtiket, etiketNumarasi, bolge, sonrakiEtiketNo++);
                    continue;
                }

                // Çok-kaynaklı BFS ile temas eden nesneleri ayır
                Dictionary<int, int> cekirdektenFinalEtikete = new Dictionary<int, int>();
                Queue<(int x, int y)> kuyruk = new Queue<(int x, int y)>();

                foreach (var ck in birlesikCekirdekler)
                    cekirdektenFinalEtikete[ck.id] = sonrakiEtiketNo++;

                for (int x = bolge.MinX; x <= bolge.MaxX; x++)
                {
                    for (int y = bolge.MinY; y <= bolge.MaxY; y++)
                    {
                        int ckId = cekirdekEtiket[x, y];
                        if (ckId != 0 && cekirdektenFinalEtikete.ContainsKey(ckId))
                        {
                            etiketNumarasi[x, y] = cekirdektenFinalEtikete[ckId];
                            kuyruk.Enqueue((x, y));
                        }
                    }
                }

                while (kuyruk.Count > 0)
                {
                    var p = kuyruk.Dequeue();
                    int mevcutEtiket = etiketNumarasi[p.x, p.y];

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int nx = p.x + i;
                            int ny = p.y + j;
                            if (nx >= bolge.MinX && nx <= bolge.MaxX &&
                                ny >= bolge.MinY && ny <= bolge.MaxY &&
                                hamEtiket[nx, ny] == bolge.EtiketNo &&
                                etiketNumarasi[nx, ny] == 0)
                            {
                                etiketNumarasi[nx, ny] = mevcutEtiket;
                                kuyruk.Enqueue((nx, ny));
                            }
                        }
                    }
                }
            }

            return EtiketlerdenBolgeVeRenkliResimOlustur(etiketNumarasi, w, h, minPikselAlani, out bolgeler);
        }

        private static void HamBolgeyiKopyala(int[,] kaynak, int[,] hedef, BolgeBilgisi bolge, int yeniEtiket)
        {
            for (int x = bolge.MinX; x <= bolge.MaxX; x++)
            {
                for (int y = bolge.MinY; y <= bolge.MaxY; y++)
                {
                    if (kaynak[x, y] == bolge.EtiketNo)
                        hedef[x, y] = yeniEtiket;
                }
            }
        }

        private static List<(int id, double cx, double cy, int alan)> CekirdekBilesenleriniBul(
            int[,] hamEtiket,
            int[,] mesafe,
            BolgeBilgisi bolge,
            int cekirdekEsik,
            int[,] cekirdekEtiket)
        {
            var sonuc = new List<(int id, double cx, double cy, int alan)>();
            int sonrakiId = 1;
            Queue<(int x, int y)> q = new Queue<(int x, int y)>();

            for (int x = bolge.MinX; x <= bolge.MaxX; x++)
            {
                for (int y = bolge.MinY; y <= bolge.MaxY; y++)
                {
                    if (hamEtiket[x, y] == bolge.EtiketNo && mesafe[x, y] >= cekirdekEsik && cekirdekEtiket[x, y] == 0)
                    {
                        int id = sonrakiId++;
                        cekirdekEtiket[x, y] = id;
                        q.Enqueue((x, y));

                        long topX = 0, topY = 0;
                        int alan = 0;

                        while (q.Count > 0)
                        {
                            var p = q.Dequeue();
                            topX += p.x;
                            topY += p.y;
                            alan++;

                            for (int i = -1; i <= 1; i++)
                            {
                                for (int j = -1; j <= 1; j++)
                                {
                                    int nx = p.x + i;
                                    int ny = p.y + j;
                                    if (nx >= bolge.MinX && nx <= bolge.MaxX &&
                                        ny >= bolge.MinY && ny <= bolge.MaxY &&
                                        hamEtiket[nx, ny] == bolge.EtiketNo &&
                                        mesafe[nx, ny] >= cekirdekEsik &&
                                        cekirdekEtiket[nx, ny] == 0)
                                    {
                                        cekirdekEtiket[nx, ny] = id;
                                        q.Enqueue((nx, ny));
                                    }
                                }
                            }
                        }

                        if (alan >= 25)
                            sonuc.Add((id, (double)topX / alan, (double)topY / alan, alan));
                        else
                        {
                            for (int rx = bolge.MinX; rx <= bolge.MaxX; rx++)
                                for (int ry = bolge.MinY; ry <= bolge.MaxY; ry++)
                                    if (cekirdekEtiket[rx, ry] == id) cekirdekEtiket[rx, ry] = 0;
                        }
                    }
                }
            }
            return sonuc;
        }

        private static List<(int id, double cx, double cy, int alan)> YakinCekirdekleriBirlestir(
            List<(int id, double cx, double cy, int alan)> cekirdekler,
            int[,] cekirdekEtiket,
            BolgeBilgisi bolge,
            double minUzaklik)
        {
            if (cekirdekler.Count <= 1) return cekirdekler;

            Dictionary<int, int> harita = new Dictionary<int, int>();
            foreach (var c in cekirdekler) harita[c.id] = c.id;

            for (int i = 0; i < cekirdekler.Count; i++)
            {
                for (int j = i + 1; j < cekirdekler.Count; j++)
                {
                    double dx = cekirdekler[i].cx - cekirdekler[j].cx;
                    double dy = cekirdekler[i].cy - cekirdekler[j].cy;
                    double dist = Math.Sqrt(dx * dx + dy * dy);
                    if (dist < minUzaklik)
                    {
                        int ana = harita[cekirdekler[i].id];
                        int eski = harita[cekirdekler[j].id];
                        foreach (var c in cekirdekler)
                        {
                            if (harita[c.id] == eski)
                                harita[c.id] = ana;
                        }
                    }
                }
            }

            List<(int id, double cx, double cy, int alan)> birlesik = new List<(int id, double cx, double cy, int alan)>();
            HashSet<int> eklenen = new HashSet<int>();
            foreach (var c in cekirdekler)
            {
                int hedefId = harita[c.id];
                if (eklenen.Add(hedefId))
                    birlesik.Add((hedefId, c.cx, c.cy, c.alan));
            }

            if (birlesik.Count < cekirdekler.Count)
            {
                for (int x = bolge.MinX; x <= bolge.MaxX; x++)
                {
                    for (int y = bolge.MinY; y <= bolge.MaxY; y++)
                    {
                        int ck = cekirdekEtiket[x, y];
                        if (ck != 0 && harita.ContainsKey(ck))
                            cekirdekEtiket[x, y] = harita[ck];
                    }
                }
            }

            return birlesik;
        }

        private static int[,] GriDiziOku(Bitmap resim)
        {
            int w = resim.Width;
            int h = resim.Height;
            int[,] gri = new int[w, h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Color c = resim.GetPixel(x, y);
                    gri[x, y] = (c.R + c.G + c.B) / 3;
                }
            }
            return gri;
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

