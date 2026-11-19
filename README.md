# Araç Kiralama Yönetim Sistemi

**Ders:** BGY203 – Nesne Tabanlı Programlama  
**Üniversite:** Kapadokya Üniversitesi  
**Geliştirici:** Nisa Nur Çelik  
**Öğrenci Numarası:** 25901012  
**Öğretim Elemanı:** Dr. Öğr. Üyesi Tohid YOUSEFİ

## Proje Hakkında

Araç Kiralama Yönetim Sistemi, C# ve Windows Forms kullanılarak geliştirilen bir masaüstü uygulamasıdır. Araçların ve müşterilerin kaydedilmesini, uygun araçların kiralanmasını, kiralama ücretlerinin hesaplanmasını ve kiralanan araçların iade edilmesini sağlar. Uygulamanın ana ekranında araç, müşteri ve aktif kiralama bilgileri özetlenir.

## Temel Özellikler

- **Ana sayfa (Dashboard):** Toplam araç, müsait araç, kiradaki araç, müşteri ve aktif kiralama sayılarının görüntülenmesi.
- **Araç yönetimi:** Araç ekleme, listeleme, güncelleme ve silme; plaka tekrarı ve geçersiz alanlar için doğrulama.
- **Müşteri yönetimi:** Müşteri ekleme, listeleme ve kayıt yönetimi.
- **Araç kiralama:** Müsait araç ve müşteri seçimi, tarih belirleme, kiralama süresine göre toplam ücret hesaplama.
- **Araç iade:** Aktif kiralamaları görüntüleme, iade onayı ve aracın yeniden müsait duruma getirilmesi.
- **Hata kontrolleri:** Boş alanlar, geçersiz üretim yılı, telefon bilgisi ve kiradaki araç üzerinde uygun olmayan işlemler için uyarılar.

## Kullanılan Teknolojiler

- C#
- .NET / Windows Forms (WinForms)
- Microsoft Visual Studio
- Nesne tabanlı programlama (OOP)
- Git ve GitHub

## Nesne Tabanlı Programlama İlkeleri

- **Kapsülleme (Encapsulation):** `Vehicle` sınıfındaki özel alanlara özellikler (`properties`) üzerinden kontrollü erişim.
- **Kalıtım (Inheritance):** `Car` sınıfının soyut `Vehicle` sınıfından türetilmesi.
- **Çok biçimlilik (Polymorphism):** `CalculateRentalPrice(int days)` metodunun `Car` sınıfında `override` edilmesi.
- **Soyutlama (Abstraction):** Soyut `Vehicle` sınıfı ve `IRentable` arayüzü.

## Projeyi Çalıştırma

### Visual Studio ile

1. Windows bilgisayarında Microsoft Visual Studio'yu açın.
2. `01_SourceCode` klasöründeki `25901012_NisaNurCelik_VehicleRentalSystem.slnx` çözüm dosyasını açın. Alternatif olarak `.csproj` proje dosyasını açabilirsiniz.
3. Projenin gerektirdiği .NET geliştirme bileşenlerinin yüklü olduğundan emin olun.
4. Visual Studio'da projeyi derleyin ve **Başlat (F5)** seçeneğiyle çalıştırın.

### Hazır uygulama ile

1. Teslim paketindeki `02_Application` klasörünü açın.
2. Araç Kiralama Yönetim Sistemi uygulama dosyasını (`.exe`) çalıştırın.
3. Uygulama açılmazsa gerekli .NET bileşenlerinin ve uygulamaya ait bağımlılık dosyalarının bulunduğunu kontrol edin.

> **Not:** Uygulamadaki kayıtlar bellekte yönetilmektedir; uygulama kapatıldığında kayıtların kalıcı olarak saklanması garanti edilmez.

## Proje Dosya Yapısı

```text
25901012_NisaNurCelik_VehicleRentalSystem/
├── 01_SourceCode/
│   ├── Forms/          # Windows Forms ekranları
│   ├── Models/         # Vehicle, Car, Customer, Rental, IRentable
│   ├── Services/       # RentalManager ve iş kuralları
│   ├── Properties/     # Proje kaynakları ve ayarları
│   ├── Program.cs      # Uygulamanın başlangıç noktası
│   ├── App.config      # Yapılandırma
│   ├── *.csproj        # Visual Studio proje dosyası
│   └── *.slnx          # Visual Studio çözüm dosyası
├── 02_Application/     # Çalıştırılabilir uygulama dosyaları
├── 03_Report/          # PDF proje raporu ve ekran görüntüleri
├── 04_GitHub/          # GitHub bağlantısı ve teslim kanıtları
└── README.md
```

## Ekran Görüntüleri ve Rapor

Ana sayfa, araç yönetimi, müşteri yönetimi, kiralama, iade ve hata kontrolü ekranlarının görüntüleri, açıklamalarıyla birlikte **`03_Report/25901012_NisaNurCelik_Proje_Raporu.pdf`** dosyasında yer almaktadır. Raporda sınıf tasarımı, OOP ilkeleri ve test senaryoları da açıklanmıştır.

## GitHub Deposu
**Proje bağlantısı:** https://github.com/nisanurcelik972-tech/25901012_NisaNurCelik_VehicleRentalSystem

Depo **Private (Özel)** olarak oluşturulmuştur. Erişim için depo sahibinin daveti ve GitHub yetkilendirmesi gerekebilir.

## Geliştirici

**Nisa Nur Çelik** — **25901012**  
Kapadokya Üniversitesi, Bilgi Güvenliği Teknolojileri Programı  
BGY203 Nesne Tabanlı Programlama — 2026 Güz Dönemi
