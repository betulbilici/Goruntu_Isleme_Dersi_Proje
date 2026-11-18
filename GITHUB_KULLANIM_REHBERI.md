# Görüntü İşleme Ekibi — Git & GitHub Kullanım Rehberi

Bu rehber, daha önce Git/GitHub kullanmamış ekip arkadaşlarımızın **birbirlerinin kodunu bozmadan**, **çakışma (conflict) yaşamadan** ve **komut ezberlemeden** eş zamanlı çalışabilmesi için hazırlanmıştır.

---

## 1. İlk Kurulum (Sadece 1 Kez Yapılacak)

1. **GitHub Davetini Kabul Et:** Proje sahibinin (repo yöneticisinin) gönderdiği **Collaborator** davetini e-postandan veya GitHub bildirimlerinden **`Accept invitation`** diyerek onayla.
2. **GitHub Desktop İndir (En Kolay Yöntem):** [desktop.github.com](https://desktop.github.com/) adresinden **GitHub Desktop** programını indir, kur ve kendi GitHub hesabınla giriş yap.
   > *Alternatif:* İstersen doğrudan **Visual Studio** içindeki sağ taraftaki **`Git Changes`** (Git Değişiklikleri) penceresini de kullanabilirsin; mantık tamamen aynıdır.
3. **Projeyi Bilgisayarına İndir (Clone):**
   - GitHub Desktop'ta **`File → Clone repository...`** tıkla.
   - Listeden `GoruntuIsleme` reposunu seç ve nereye kaydedileceğini belirleyip **`Clone`** butonuna bas.
   - İndirme bitince klasör içindeki `GoruntuIsleme/GoruntuIsleme.slnx` dosyasına çift tıklayarak Visual Studio'da aç.

---

## 2. Günlük Çalışma Akışı (Her Kod Yazacağında Bu 4 Adımı İzle)

### Adım 1: Başlamadan Önce Güncel Kodu Çek (`Pull`)
Kod yazmaya başlamadan **mutlaka**:
1. GitHub Desktop'ı aç.
2. Sol üstte `Current Branch` kısmının **`main`** olduğundan emin ol.
3. Üstteki **`Fetch origin`** butonuna bas; eğer **`Pull origin`** çıkarsa tıklayıp arkadaşlarının eklediği son kodları bilgisayarına al.

### Adım 2: Kendi Dalında (Branch) Çalış
Herkesin aynı anda `main` üzerinde çalışıp birbirini ezmesini önlemek için kendi görevine özel bir dal aç:
1. GitHub Desktop'ta **`Current Branch`** → **`New Branch`** butonuna tıkla.
2. Dalına anlaşılır bir isim ver (örnek: `hafta4-5-ali`, `hafta6-ayse`, `arayuz-betul`).
3. **`Create Branch`** de. Artık güvenle Visual Studio'da kodunu yazabilirsin!

### Adım 3: Kodunu Kaydet ve GitHub'a Gönder (`Commit` & `Push`)
Ödevini/modülünü yazıp Visual Studio'da çalıştırarak test ettikten sonra:
1. GitHub Desktop'a dön. Sol tarafta değiştirdiğin ve yeni eklediğin dosyaları göreceksin.
   > ⚠️ **Kontrol:** Listede `bin/`, `obj/` veya `.vs/` dosyaları **olmamalıdır** (`.gitignore` bunları otomatik gizler).
2. Sol alt köşedeki **`Summary (required)`** kutusuna ne yaptığını kısaca yaz *(örnek: `Hafta 5 Mean ve Median filtreleri eklendi`)*.
3. **`Commit to <senin-branch-adin>`** butonuna tıkla.
4. Üstte çıkan **`Publish branch`** (veya **`Push origin`**) butonuna tıkla.

### Adım 4: Ana Projeye Birleştirme (`Pull Request`)
Görevin bittiğinde:
1. GitHub Desktop'ta **`Preview Pull Request`** veya GitHub web sayfasında **`Compare & pull request`** butonuna bas.
2. **`Create pull request`** butonuna tıkla.
3. Çakışma yoksa **`Merge pull request`** ile kodunu `main` dalına güvenle birleştir.

---

## 3. Çakışma (Conflict) Yaşamamak İçin 4 Altın Kural

1. **Ortak Dosyalara (`Form1.cs`) Aynı Anda Dokunma:** Kendi ödevin için yeni `Form` / `UserControl` / `Class` dosyası oluştur. `Form1` menü butonlarını en son tek kişi veya sırayla bağlasın.
2. **Çalışmayan Kodu `main`'e Atma:** Kodu GitHub'a göndermeden önce Visual Studio'da mutlaka **Build (`Ctrl+Shift+B`)** yapıp hatasız çalıştığından emin ol.
3. **Küçük ve Sık Commit At:** 5 ödevi tek seferde atmak yerine her bitirdiğin ödevden sonra commit + push yap.
4. **Başlamadan Önce `Pull` Yapmayı Unutma!**

---

## 4. Terminal Kullanmak İsteyenler İçin Özet Komutlar

```bash
# 1. Projeyi ilk kez indirme
git clone <REPO_URL>

# 2. Güncel main dalını çekme
git checkout main
git pull origin main

# 3. Kendi dalını (branch) oluşturup geçme
git checkout -b hafta5-mehmet

# 4. Değişiklikleri kaydetme ve gönderme
git add .
git commit -m "Hafta 5 blur filtreleri eklendi"
git push -u origin hafta5-mehmet
```

---

## 5. Sık Karşılaşılan Durumlar

* **"Push yapamıyorum, hata veriyor (rejected)":** Sendeki kod güncel değildir. Önce **`Pull origin`** yap, sonra tekrar `Push` et.
* **"Yanlışlıkla bir dosyayı bozdum, geri almak istiyorum":** GitHub Desktop'ta sol listeden o dosyaya sağ tıkla → **`Discard changes...`** seç.
* **"Merge Conflict (Çakışma) uyarısı çıktı":** Panik yapma! İki kişi aynı dosyanın aynı satırını değiştirmiş demektir. Hangi kodun kalacağını seçmek için takım arkadaşınla ekran açıp Visual Studio'da tek tıkla birleştirebilirsiniz.
