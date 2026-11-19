using System;
using System.Collections.Generic;
using System.Linq;
using VehicleRentalSystem.Models;

namespace VehicleRentalSystem.Services
{
    public class RentalManager
    {
        // GENERIC KOLEKSİYONLAR (List<T>): Bellekte veri yönetimi[cite: 12, 13, 15]
        public List<Vehicle> Vehicles { get; private set; }
        public List<Customer> Customers { get; private set; }
        public List<Rental> Rentals { get; private set; }

        public RentalManager()
        {
            Vehicles = new List<Vehicle>();
            Customers = new List<Customer>();
            Rentals = new List<Rental>();
        }

        // 1. Araç Ekleme (Plaka tekrarı kontrolü)[cite: 13, 14]
        public void AddVehicle(Vehicle vehicle)
        {
            if (Vehicles.Any(v => v.Plate.Equals(vehicle.Plate, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("Hata: Bu plakaya sahip bir araç zaten sistemde kayıtlı!");

            Vehicles.Add(vehicle);
        }

        // 2. Müşteri Ekleme[cite: 14]
        public void AddCustomer(Customer customer)
        {
            Customers.Add(customer);
        }

        // 3. Araç Kiralama (Tarih ve müsaitlik kontrolü)[cite: 14]
        public void RentVehicle(Customer customer, Vehicle vehicle, DateTime startDate, DateTime endDate)
        {
            if (!vehicle.IsAvailable)
                throw new Exception("Seçilen araç şu anda kiralık (müsait değil)!");

            if (endDate < startDate)
                throw new Exception("Bitiş tarihi, başlangıç tarihinden önce olamaz!");

            Rental newRental = new Rental(Rentals.Count + 1, customer, vehicle, startDate, endDate);
            Rentals.Add(newRental);
        }

        // 4. Araç İade Etme[cite: 14]
        public void ReturnVehicle(int rentalId)
        {
            Rental activeRental = Rentals.FirstOrDefault(r => r.RentalId == rentalId && r.IsActive);
            if (activeRental != null)
            {
                activeRental.IsActive = false;
                activeRental.RentedVehicle.ReturnVehicle(); // Araç yeniden Müsait[cite: 14]
            }
            else
            {
                throw new Exception("Aktif kiralama kaydı bulunamadı!");
            }
        }

        // Dashboard: Sistemdeki toplam araç sayısı.
        public int GetTotalVehicleCount()
        {
            return Vehicles.Count;
        }

        // Dashboard: Kiralanmaya uygun araç sayısı.
        public int GetAvailableVehicleCount()
        {
            return Vehicles.Count(v => v.IsAvailable);
        }

        // Dashboard: Şu anda kirada olan araç sayısı.
        public int GetRentedVehicleCount()
        {
            return Vehicles.Count(v => !v.IsAvailable);
        }

        // Dashboard: Sistemdeki toplam müşteri sayısı.
        public int GetCustomerCount()
        {
            return Customers.Count;
        }

        // Dashboard: Henüz tamamlanmamış kiralama sayısı.
        public int GetActiveRentalCount()
        {
            return Rentals.Count(r => r.IsActive);
        }

    }
}