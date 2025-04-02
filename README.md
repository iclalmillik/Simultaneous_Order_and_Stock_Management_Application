#  Eş Zamanlı Sipariş ve Stok Yönetimi Uygulaması

 Amaç, **multithreading** ve **senkronizasyon mekanizmaları** kullanarak eş zamanlı sipariş ve stok yönetimi yapabilen bir sistem geliştirmektir.

##  Proje Amacı

- Müşteri ve stok işlemlerinin **eş zamanlı** (concurrent) olarak yürütülmesini sağlamak
- Aynı kaynağa çoklu erişimde oluşabilecek hataları önlemek
- **Thread**, **mutex**, **semaphore** ve **önceliklendirme** gibi kavramların kullanımıyla pratik geliştirmek

##  Kullanılan Teknolojiler

- C# ASP.NET MVC
- MSSQL

##  Müşteri Özellikleri

- Müşteriler random oluşturulur (5-10 adet)
- Budget (500-3000 TL arasında)
- 2 tür müşteri:
  - **Premium** (yüksek öncelik)
  - **Standard** (normal öncelik)
- Her müşteri birden fazla üründen max 5 adet sipariş verebilir
- Dinamik öncelik skoru:

##  Admin Paneli

- Ürün ekleme, silme, stok güncelleme
- Admin işlemleri müşteri işlemleriyle **eş zamanlı** gerçekleşir
- Kaynaklara erişirken diğer işlemleri kilitler

##  Stok Yönetimi
- Stok yetersizse işlem reddedilir
- Stoklar işlem sırasında **anlık güncellenir**

##  Bütçe Yönetimi

- Sipariş tutarı müşteri bütçesinden düşer
- Yetersiz bakiye varsa sipariş reddedilir


## Dinamik Önceliklendirme

- Premium: Başlangıç öncelik skoru 15
- Standard: Başlangıç öncelik skoru 10
- Bekleme süresi arttıkça Standard müşterilerin önceliği dinamik olarak artar
- Hesaplama formülü:
ÖncelikSkoru = TemelÖncelikSkoru + (BeklemeSüresi × 0.5)

##  Kullanıcı Arayüzleri

###  Müşteri Paneli

- Müşteri listesi: ID, Ad, Tür, Bütçe, Bekleme süresi, Öncelik skoru
- Sipariş formu: Ürün seçimi, adet girişi
- Sipariş durumu: Renklendirilmiş durum göstergesi (Bekliyor, İşleniyor, Tamamlandı)

###  Stok Durumu Paneli

- Ürün listesi: Ad, Stok, Fiyat
- Grafik gösterimi: Stoklar bar/dairesel grafik ile görselleştirilir

###  Log Paneli

- Gerçek zamanlı log akışı
- Başarı/hata mesajları anlık listelenir

###  Dinamik Öncelik Paneli

- Tabloda bekleme süresi ve öncelik skoru gösterimi
- Müşteri sırası animasyonla güncellenir

##  Sipariş İşleme Animasyonu

- Aktif siparişler için animasyon: "Müşteri X’in siparişi işleniyor..." gibi görseller

#  Real-Time Order and Inventory Management System

The goal of this project is to develop a system that can manage real-time orders and inventory updates using **multithreading** and **synchronization mechanisms**.

##  Project Objectives

- Enable **concurrent** customer and inventory operations
- Prevent errors caused by multiple access to the same resources
- Gain hands-on experience with concepts like **thread**, **mutex**, **semaphore**, and **priority scheduling**

## 🛠 Technologies Used

- C# ASP.NET MVC
- MSSQL

##  Customer Features

- Customers are generated randomly (5–10 customers)
- Budget ranges between 500–3000 TL
- Two types of customers:
  - **Premium** (high priority)
  - **Standard** (normal priority)
- Each customer can order up to 5 units of each product
- Dynamic priority score is calculated based on customer type and wait time

##  Admin Panel

- Add, delete, and update product stock
- Admin operations run **concurrently** with customer operations
- Locks access to shared resources during updates

##  Stock Management

- If stock is insufficient, the transaction is rejected
- Stock levels are **instantly updated** during transactions

##  Budget Management

- The order total is deducted from the customer's budget
- If the balance is insufficient, the order is rejected

## ⏱ Dynamic Priority System

- Premium: Initial priority score = 15  
- Standard: Initial priority score = 10  
- As wait time increases, Standard customers’ priority increases dynamically  
- Calculation formula:  
PriorityScore = BasePriorityScore + (WaitTime × 0.5)


##  User Interfaces

###  Customer Panel

- Customer list: ID, Name, Type, Budget, Wait Time, Priority Score
- Order form: Product selection and quantity input
- Order status: Colored indicators (Waiting, Processing, Completed)

###  Stock Panel

- Product list: Name, Stock, Price
- Visual display: Bar or pie chart for stock levels

###  Log Panel

- Real-time log stream
- Success/error messages are listed instantly

###  Dynamic Priority Panel

- Table showing wait time and priority score
- Customer queue updates dynamically with animations

##  Order Processing Animation

- Active orders are visualized with animations like:
> "Processing order for Customer X.."
