
using System;

namespace VehicleRentalSystem.Models
{
    // Araç durumlarını belirlemek için enum kullanıyoruz.
    public enum VehicleStatus
    {
        Available,
        Rented
    }

    // Abstract sınıf: Doğrudan Vehicle nesnesi oluşturulamaz.
    // Bütün araçların ortak özelliklerini burada topluyoruz.
    public abstract class Vehicle : IRentable
    {
        // KAPSÜLLEME: Alanlar dışarıdan doğrudan erişime kapalıdır.
        private int _id;
        private string _plate;
        private string _brand;
        private string _model;
        private int _year;
        private decimal _dailyRentalPrice;
        private VehicleStatus _status;

        // Property kullanarak kontrollü erişim sağlıyoruz.
        public int Id
        {
            get { return _id; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Araç ID pozitif olmalıdır.");

                _id = value;
            }
        }

        public string Plate
        {
            get { return _plate; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Plaka boş bırakılamaz.");

                _plate = value.Trim().ToUpper();
            }
        }

        public string Brand
        {
            get { return _brand; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Marka boş bırakılamaz.");

                _brand = value.Trim();
            }
        }

        public string Model
        {
            get { return _model; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Model boş bırakılamaz.");

                _model = value.Trim();
            }
        }

        public int Year
        {
            get { return _year; }
            set
            {
                if (value < 1950 || value > DateTime.Now.Year + 1)
                    throw new ArgumentException("Geçersiz araç yılı.");

                _year = value;
            }
        }

        public decimal DailyRentalPrice
        {
            get { return _dailyRentalPrice; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Günlük kira ücreti sıfırdan büyük olmalıdır.");

                _dailyRentalPrice = value;
            }
        }

        public VehicleStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        // Constructor: Araç oluşturulurken bilgileri alır.
        protected Vehicle(int id, string plate, string brand,
            string model, int year, decimal dailyRentalPrice)
        {
            Id = id;
            Plate = plate;
            Brand = brand;
            Model = model;
            Year = year;
            DailyRentalPrice = dailyRentalPrice;
            Status = VehicleStatus.Available;
        }

        // SOYUTLAMA: Alt sınıflar bu metodu kendilerine göre uygulayacak.
        public abstract decimal CalculateRentalPrice(int days);

        // Aracın kiralanmaya uygun olup olmadığını kontrol eder.
        public bool IsAvailable
        {
            get { return Status == VehicleStatus.Available; }
        }

        // Araç kiralama işlemi.
        public void Rent()
        {
            if (!IsAvailable)
            {
                throw new InvalidOperationException(
                    "Bu araç zaten kiralanmış.");
            }

            Status = VehicleStatus.Rented;
        }

        // Araç iade işlemi.
        public void ReturnVehicle()
        {
            if (IsAvailable)
            {
                throw new InvalidOperationException(
                    "Bu araç zaten müsait.");
            }

            Status = VehicleStatus.Available;
        }

    }
}
