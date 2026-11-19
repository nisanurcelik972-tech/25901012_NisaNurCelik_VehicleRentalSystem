
using System;

namespace VehicleRentalSystem.Models
{
    // Car sınıfı Vehicle sınıfından kalıtım alır.
    public class Car : Vehicle
    {
        // Kapsülleme: Koltuk sayısını private alanda saklıyoruz.
        private int _seatCount;

        public int SeatCount
        {
            get { return _seatCount; }
            set
            {
                if (value < 2 || value > 9)
                    throw new ArgumentException(
                        "Koltuk sayısı 2 ile 9 arasında olmalıdır.");

                _seatCount = value;
            }
        }

        // Constructor: Önce temel sınıfın constructor'ı çalışır.
        public Car(int id, string plate, string brand,
            string model, int year, decimal dailyRentalPrice,
            int seatCount)
            : base(id, plate, brand, model, year, dailyRentalPrice)
        {
            SeatCount = seatCount;
        }

        // Çok biçimlilik: Vehicle sınıfındaki abstract metodu uyguluyoruz.
        public override decimal CalculateRentalPrice(int days)
        {
            if (days <= 0)
                throw new ArgumentException(
                    "Kiralama günü sıfırdan büyük olmalıdır.");

            // Otomobilin toplam kiralama ücretini hesaplar.
            return DailyRentalPrice * days;
        }
    }
}
