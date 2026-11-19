using System;
using System.Drawing;
using System.Windows.Forms;
using VehicleRentalSystem.Services;

namespace VehicleRentalSystem.Forms
{
    public partial class MainForm : Form
    {
        private readonly RentalManager _manager = new RentalManager();

        private Label lblTotalVehicles;
        private Label lblAvailableVehicles;
        private Label lblRentedVehicles;
        private Label lblCustomers;
        private Label lblActiveRentals;

        public MainForm()
        {
            // Ana pencere ayarları.
            Text = "Araç Kiralama Yönetim Sistemi";
            Size = new Size(1100, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 247, 250);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            CreateInterface();
            UpdateDashboard();
        }

        // Bütün arayüz elemanlarını kodla oluşturur.
        private void CreateInterface()
        {
            Panel header = new Panel();
            header.Size = new Size(1100, 90);
            header.Location = new Point(0, 0);
            header.BackColor = Color.FromArgb(30, 45, 65);
            Controls.Add(header);

            Label title = new Label();
            title.Text = "ARAÇ KİRALAMA YÖNETİM SİSTEMİ";
            title.Font = new Font("Arial", 19, FontStyle.Bold);
            title.ForeColor = Color.White;
            title.AutoSize = true;
            title.Location = new Point(30, 28);
            header.Controls.Add(title);

            Label dashboardTitle = new Label();
            dashboardTitle.Text = "GENEL BAKIŞ";
            dashboardTitle.Font = new Font("Arial", 16, FontStyle.Bold);
            dashboardTitle.ForeColor = Color.FromArgb(30, 45, 65);
            dashboardTitle.AutoSize = true;
            dashboardTitle.Location = new Point(35, 120);
            Controls.Add(dashboardTitle);

            // Dashboard bilgi kartları.
            lblTotalVehicles = CreateCard(
                "TOPLAM ARAÇ", 35, 175);

            lblAvailableVehicles = CreateCard(
                "MÜSAİT ARAÇ", 370, 175);

            lblRentedVehicles = CreateCard(
                "KİRADAKİ ARAÇ", 705, 175);

            lblCustomers = CreateCard(
                "TOPLAM MÜŞTERİ", 35, 340);

            lblActiveRentals = CreateCard(
                "AKTİF KİRALAMA", 370, 340);

            // Menü butonları.
            CreateMenuButton("Araç Yönetimi", 35, 520);
            CreateMenuButton("Müşteri Yönetimi", 285, 520);
            CreateMenuButton("Araç Kiralama", 535, 520);
            CreateMenuButton("Araç İade", 785, 520);
        }

        // Dashboard kartı oluşturur.
        private Label CreateCard(string heading, int x, int y)
        {
            Panel card = new Panel();
            card.Size = new Size(300, 125);
            card.Location = new Point(x, y);
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(card);

            Label title = new Label();
            title.Text = heading;
            title.Font = new Font("Arial", 11, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(90, 100, 115);
            title.AutoSize = true;
            title.Location = new Point(20, 18);
            card.Controls.Add(title);

            Label value = new Label();
            value.Text = "0";
            value.Font = new Font("Arial", 25, FontStyle.Bold);
            value.ForeColor = Color.FromArgb(30, 45, 65);
            value.AutoSize = true;
            value.Location = new Point(20, 55);
            card.Controls.Add(value);

            return value;
        }

        // Ana menü butonu oluşturur.
        private void CreateMenuButton(string text, int x, int y)
        {
            Button button = new Button();
            button.Text = text;
            button.Size = new Size(220, 55);
            button.Location = new Point(x, y);
            button.Font = new Font("Arial", 11, FontStyle.Bold);
            button.BackColor = Color.FromArgb(30, 45, 65);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.Cursor = Cursors.Hand;


            button.Click += (sender, e) =>
            {
                if (text == "Araç Yönetimi")
                {
                    // Araç yönetimi penceresini aç.
                    using (VehicleForm vehicleForm = new VehicleForm(_manager))
                    {
                        vehicleForm.ShowDialog(this);
                    }

                    // Ana ekrandaki araç sayılarını güncelle.
                    UpdateDashboard();
                }

                else if (text == "Müşteri Yönetimi")
                {
                    // Müşteri yönetimi penceresini açar.
                    using (CustomerForm customerForm = new CustomerForm(_manager))
                    {
                        customerForm.ShowDialog(this);
                    }

                    // Ana ekrandaki müşteri sayısını günceller.
                    UpdateDashboard();
                }

                else if (text == "Araç Kiralama")
                {
                    // Araç kiralama penceresini açar.
                    using (RentalForm rentalForm = new RentalForm(_manager))
                    {
                        rentalForm.ShowDialog(this);
                    }

                    // Kiralama sonrasında ana ekrandaki sayıları günceller.
                    UpdateDashboard();
                }

                else if (text == "Araç İade")
                {
                    // Araç iade penceresini açar.
                    using (ReturnForm returnForm = new ReturnForm(_manager))
                    {
                        returnForm.ShowDialog(this);
                    }

                    // İade sonrasında ana ekrandaki sayıları günceller.
                    UpdateDashboard();
                }

                else
                {
                    MessageBox.Show(
                        text + " ekranı henüz hazırlanıyor.",
                        "Bilgilendirme",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }

            };




            Controls.Add(button);
        }

        // Dashboard verilerini günceller.
        private void UpdateDashboard()
        {
            lblTotalVehicles.Text =
                _manager.GetTotalVehicleCount().ToString();

            lblAvailableVehicles.Text =
                _manager.GetAvailableVehicleCount().ToString();

            lblRentedVehicles.Text =
                _manager.GetRentedVehicleCount().ToString();

            lblCustomers.Text =
                _manager.GetCustomerCount().ToString();

            lblActiveRentals.Text =
                _manager.GetActiveRentalCount().ToString();
        }
    }
}
