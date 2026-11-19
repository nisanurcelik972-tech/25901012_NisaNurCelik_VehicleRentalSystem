
using System;

namespace VehicleRentalSystem.Models
{
    // Customer sınıfı müşteri bilgilerini temsil eder.
    public class Customer
    {
        // KAPSÜLLEME: Müşteri bilgilerini private alanlarda saklıyoruz.
        private int _id;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _phone = string.Empty;
        private string _licenseNumber = string.Empty;

        public int Id
        {
            get { return _id; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentException(
                        "Müşteri ID pozitif olmalıdır.");

                _id = value;
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "Müşteri adı boş bırakılamaz.");

                _firstName = value.Trim();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "Müşteri soyadı boş bırakılamaz.");

                _lastName = value.Trim();
            }
        }

        public string Phone
        {
            get { return _phone; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "Telefon numarası boş bırakılamaz.");

                _phone = value.Trim();
            }
        }

        public string LicenseNumber
        {
            get { return _licenseNumber; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "Ehliyet numarası boş bırakılamaz.");

                _licenseNumber = value.Trim().ToUpper();
            }
        }

        // Constructor: Yeni müşteri oluşturulurken çalışır.
        public Customer(int id, string firstName,
            string lastName, string phone, string licenseNumber)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            LicenseNumber = licenseNumber;
        }

        // Müşterinin adını ve soyadını birlikte döndürür.
        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
