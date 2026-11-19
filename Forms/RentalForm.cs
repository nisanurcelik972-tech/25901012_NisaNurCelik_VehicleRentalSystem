
using System;
using System.Drawing;
using System.Windows.Forms;
using VehicleRentalSystem.Models;
using VehicleRentalSystem.Services;

namespace VehicleRentalSystem.Forms
{
    // Araç kiralama işlemlerinin yapılacağı pencere.
    public class RentalForm : Form
    {
        // Ana ekranla aynı kayıtları kullanır.
        private readonly RentalManager _manager;

        // Müşteri ve araç seçim kutuları.
        private readonly ComboBox cmbCustomers = new ComboBox();
        private readonly ComboBox cmbVehicles = new ComboBox();

        // Kiralama tarihleri.
        private readonly DateTimePicker dtpStart = new DateTimePicker();
        private readonly DateTimePicker dtpEnd = new DateTimePicker();

        // Hesaplanan süre ve ücret bilgileri.
        private readonly Label lblDays = new Label();
        private readonly Label lblTotalPrice = new Label();

        // İşlem butonları.
        private readonly Button btnCalculate = new Button();
        private readonly Button btnRent = new Button();

        // Constructor: Pencere oluşturulurken çalışır.
        public RentalForm(RentalManager manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            _manager = manager;

            Text = "Araç Kiralama";
            Size = new Size(950, 650);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Arial", 10);
            KeyPreview = true;

            CreateInterface();

            LoadCustomers();
            LoadAvailableVehicles();

        }

        // Pencerenin ilk görünümünü oluşturur.
        private void CreateInterface()
        {
            Label title = new Label();

            title.Text = "ARAÇ KİRALAMA";
            title.Font = new Font(
                "Arial", 20, FontStyle.Bold);

            title.ForeColor = Color.FromArgb(30, 45, 65);
            title.Location = new Point(30, 25);
            title.AutoSize = true;

            Controls.Add(title);

            Label description = new Label();

            description.Text =
                "Müşteri ve araç seçerek kiralama işlemi yapabilirsiniz.";

            description.Location = new Point(30, 85);
            description.AutoSize = true;
            description.ForeColor = Color.DimGray;

            Controls.Add(description);

            // Müşteri seçim alanı.
            Label lblCustomer = new Label();
            lblCustomer.Text = "MÜŞTERİ SEÇİN";

            lblCustomer.Location = new Point(30, 140);
            lblCustomer.AutoSize = true;
            Controls.Add(lblCustomer);

            cmbCustomers.Location = new Point(30, 170);
            cmbCustomers.Size = new Size(400, 30);
            cmbCustomers.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(cmbCustomers);

            // Araç seçim alanı.
            Label lblVehicle = new Label();
            lblVehicle.Text = "MÜSAİT ARAÇ SEÇİN";
            lblVehicle.Location = new Point(490, 140);
            lblVehicle.AutoSize = true;
            Controls.Add(lblVehicle);

            cmbVehicles.Location = new Point(490, 170);
            cmbVehicles.Size = new Size(400, 30);
            cmbVehicles.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(cmbVehicles);

            // Başlangıç tarihi.
            Label lblStart = new Label();
            lblStart.Text = "BAŞLANGIÇ TARİHİ";
            lblStart.Location = new Point(30, 230);
            lblStart.AutoSize = true;
            Controls.Add(lblStart);

            dtpStart.Location = new Point(30, 260);
            dtpStart.Size = new Size(400, 30);
            dtpStart.Format = DateTimePickerFormat.Short;
            Controls.Add(dtpStart);

            // Bitiş tarihi.
            Label lblEnd = new Label();
            lblEnd.Text = "BİTİŞ TARİHİ";
            lblEnd.Location = new Point(490, 230);
            lblEnd.AutoSize = true;
            Controls.Add(lblEnd);

            dtpEnd.Location = new Point(490, 260);
            dtpEnd.Size = new Size(400, 30);
            dtpEnd.Format = DateTimePickerFormat.Short;
            dtpEnd.Value = DateTime.Today.AddDays(1);
            Controls.Add(dtpEnd);

            // Kiralama süresi.
            lblDays.Text = "Kiralama Süresi: 0 gün";
            lblDays.Location = new Point(30, 330);
            lblDays.Size = new Size(400, 35);
            lblDays.Font = new Font("Arial", 12, FontStyle.Bold);
            Controls.Add(lblDays);

            // Toplam ücret.
            lblTotalPrice.Text = "Toplam Ücret: 0 TL";
            lblTotalPrice.Location = new Point(490, 330);
            lblTotalPrice.Size = new Size(400, 35);
            lblTotalPrice.Font = new Font("Arial", 12, FontStyle.Bold);
            Controls.Add(lblTotalPrice);

            // Ücret hesaplama butonu.
            btnCalculate.Text = "ÜCRET HESAPLA";
            btnCalculate.Location = new Point(30, 410);
            btnCalculate.Size = new Size(400, 55);
            btnCalculate.BackColor = Color.FromArgb(30, 45, 65);
            btnCalculate.ForeColor = Color.White;
            btnCalculate.FlatStyle = FlatStyle.Flat;
            btnCalculate.Cursor = Cursors.Hand;

            // ÜCRET HESAPLA butonuna basıldığında hesaplama yapar.
            btnCalculate.Click += (sender, e) =>
            {
                CalculatePrice();
            };

            Controls.Add(btnCalculate);

            // Araç kiralama butonu.
            btnRent.Text = "ARACI KİRALA";
            btnRent.Location = new Point(490, 410);
            btnRent.Size = new Size(400, 55);
            btnRent.BackColor = Color.FromArgb(30, 100, 75);
            btnRent.ForeColor = Color.White;
            btnRent.FlatStyle = FlatStyle.Flat;
            btnRent.Cursor = Cursors.Hand;

            // ARACI KİRALA butonuna basıldığında kiralama işlemini başlatır.
            btnRent.Click += (sender, e) =>
            {
                RentSelectedVehicle();
            };

            Controls.Add(btnRent);

        }

        // Kayıtlı müşterileri seçim kutusuna yükler.
        private void LoadCustomers()
        {
            cmbCustomers.Items.Clear();

            foreach (Customer customer in _manager.Customers)
            {
                cmbCustomers.Items.Add(customer);
            }

            cmbCustomers.DisplayMember = "FirstName";
            cmbCustomers.SelectedIndex = -1;
        }

        // Yalnızca müsait araçları seçim kutusuna yükler.
        private void LoadAvailableVehicles()
        {
            cmbVehicles.Items.Clear();

            foreach (Vehicle vehicle in _manager.Vehicles)
            {
                if (vehicle.IsAvailable)
                {
                    cmbVehicles.Items.Add(vehicle);
                }
            }

            cmbVehicles.DisplayMember = "Plate";
            cmbVehicles.SelectedIndex = -1;
        }

        // Seçilen aracın kiralama ücretini hesaplar.
        private void CalculatePrice()
        {
            // Araç seçilip seçilmediğini kontrol eder.

            Vehicle vehicle = cmbVehicles.SelectedItem as Vehicle;

            if (vehicle == null)

            {
                MessageBox.Show(
                    "Lütfen bir araç seçin.",
                    "Uyarı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbVehicles.Focus();
                return;
            }

            // Başlangıç ve bitiş tarihleri arasındaki gün sayısı.
            int days = (dtpEnd.Value.Date - dtpStart.Value.Date).Days;

            // Geçersiz tarihleri engeller.
            if (days <= 0)
            {
                MessageBox.Show(
                    "Bitiş tarihi başlangıç tarihinden sonra olmalıdır.",
                    "Uyarı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                dtpEnd.Focus();
                return;
            }

            // Araç sınıfının ücret hesaplama metodunu kullanır.
            decimal totalPrice = vehicle.CalculateRentalPrice(days);

            // Hesaplanan sonuçları ekranda gösterir.
            lblDays.Text = "Kiralama Süresi: " + days + " gün";
            lblTotalPrice.Text = "Toplam Ücret: " +
                                 totalPrice.ToString("N2") + " TL";
        }

        private void RentSelectedVehicle()
        {

            // Seçilen müşteriyi kontrol eder.
            Customer customer = cmbCustomers.SelectedItem as Customer;

            if (customer == null)
            {
                MessageBox.Show(
                    "Lütfen bir müşteri seçin.",
                    "Uyarı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbCustomers.Focus();
                return;
            }

            // Seçilen aracı kontrol eder.
            Vehicle vehicle = cmbVehicles.SelectedItem as Vehicle;

            if (vehicle == null)
            {
                MessageBox.Show(
                    "Lütfen bir araç seçin.",
                    "Uyarı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbVehicles.Focus();
                return;
            }

            // Kiralama tarihlerini alır.
            DateTime startDate = dtpStart.Value.Date;
            DateTime endDate = dtpEnd.Value.Date;

            // Geçersiz tarihleri engeller.
            if (endDate <= startDate)
            {
                MessageBox.Show(
                    "Bitiş tarihi başlangıç tarihinden sonra olmalıdır.",
                    "Uyarı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                dtpEnd.Focus();
                return;
            }

            try
            {
                // Kiralama kaydını oluşturur.
                _manager.RentVehicle(customer, vehicle, startDate, endDate);

                MessageBox.Show(
                    "Araç başarıyla kiralandı.",
                    "Başarılı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Kiralanan aracı müsait araç listesinden çıkarır.
                LoadAvailableVehicles();

                // Hesaplama sonuçlarını temizler.
                lblDays.Text = "Kiralama Süresi: 0 gün";
                lblTotalPrice.Text = "Toplam Ücret: 0 TL";

                // Müşteri seçimini temizler.
                cmbCustomers.SelectedIndex = -1;
                cmbCustomers.Focus();
            }
            catch (Exception ex)
            {
                // Hata oluşursa kullanıcıyı bilgilendirir.
                MessageBox.Show(
                    ex.Message,
                    "Kiralama Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
