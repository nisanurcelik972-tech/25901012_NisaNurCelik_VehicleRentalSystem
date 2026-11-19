
namespace VehicleRentalSystem.Models
{
    // Kiralanabilir araçların uygulaması gereken metotları tanımlar.
    public interface IRentable
    {
        void Rent();
        void ReturnVehicle();
    }
}
