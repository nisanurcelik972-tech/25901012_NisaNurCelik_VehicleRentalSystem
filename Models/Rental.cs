using System;

namespace VehicleRentalSystem.Models
{
    public class Rental
    {
        public int RentalId { get; set; }
        public Customer RentedCustomer { get; set; }
        public Vehicle RentedVehicle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        public int TotalDays
        {
            get
            {
                int days = (EndDate - StartDate).Days;
                return days > 0 ? days : 1;
            }
        }

        public decimal TotalPrice
        {
            get { return RentedVehicle.CalculateRentalPrice(TotalDays); }
        }

        public Rental(int id, Customer customer, Vehicle vehicle, DateTime start, DateTime end)
        {
            RentalId = id;
            RentedCustomer = customer;
            RentedVehicle = vehicle;
            StartDate = start;
            EndDate = end;

            RentedVehicle.Rent(); // Araç durumunu Kirada yapar[cite: 13, 14]
        }
    }
}