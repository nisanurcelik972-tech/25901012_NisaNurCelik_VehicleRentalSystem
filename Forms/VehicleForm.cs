
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VehicleRentalSystem.Models;
using VehicleRentalSystem.Services;

namespace VehicleRentalSystem.Forms
{
    // Araç ekleme, listeleme, güncelleme ve silme ekranı.
    public class VehicleForm : Form
    {
        private readonly RentalManager _manager;

        // Kullanıcıdan bilgi alacağımız kutular.
        private readonly TextBox txtPlate = new TextBox();
        private readonly TextBox txtBrand = new TextBox();
        private readonly TextBox txtModel = new TextBox();
        private readonly TextBox txtYear = new TextBox();
        private readonly TextBox txtPrice = new TextBox();
        private readonly TextBox txtSeats = new TextBox();

        // Araçların gösterileceği tablo.
        private readonly DataGridView dgvVehicles = new DataGridView();

        // İşlem butonları.
        private readonly Button btnAdd = new Button();
        private readonly Button btnUpdate = new Button();
        private readonly Button btnDelete = new Button();
        private readonly Button btnClear = new Button();

        // Seçilen aracın ID bilgisini saklar.
        private int selectedVehicleId = 0;

        public VehicleForm(RentalManager manager)
        {
            _manager = manager ??
                throw new ArgumentNullException(nameof(manager));

            Text = "Araç Yönetimi";
            Size = new Size(1000, 750);
            MinimumSize = new Size(850, 650);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Arial", 10);
            KeyPreview = true;

            CreateInterface();
            RefreshVehicleList();

            Shown += (sender, e) => txtPlate.Focus();
        }

        // Arayüzü sürükle-bırak kullanmadan oluşturur.
        private void CreateInterface()
        {
            Label title = new Label
            {
                Text = "ARAÇ YÖNETİMİ",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 45, 65),
                Location = new Point(30, 20),
                AutoSize = true
            };

            Controls.Add(title);

            // Giriş alanlarını iki sütun hâlinde oluşturur.
            AddInput("Plaka", txtPlate, 30, 90, "Örn: 34ABC123");
            AddInput("Marka", txtBrand, 510, 90, "Örn: Toyota");

            AddInput("Model", txtModel, 30, 165, "Örn: Corolla");
            AddInput("Yıl", txtYear, 510, 165, "Örn: 2023");

            AddInput("Günlük Ücret (TL)", txtPrice, 30, 240, "Örn: 1500");
            AddInput("Koltuk Sayısı", txtSeats, 510, 240, "Örn: 5");

            ConfigureButton(btnAdd, "ARAÇ EKLE", 30, 330);
            ConfigureButton(btnUpdate, "GÜNCELLE", 270, 330);
            ConfigureButton(btnDelete, "SİL", 510, 330);
            ConfigureButton(btnClear, "TEMİZLE", 750, 330);

            btnAdd.Click += (sender, e) => AddVehicle();
            btnUpdate.Click += (sender, e) => UpdateVehicle();
            btnDelete.Click += (sender, e) => DeleteVehicle();
            btnClear.Click += (sender, e) => ClearInputs();

            // Enter tuşu ile araç ekleme işlemini başlatır.
            AcceptButton = btnAdd;

            // Araç listesinin görünümü.
            dgvVehicles.Location = new Point(30, 415);
            dgvVehicles.Size = new Size(920, 245);
            dgvVehicles.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            dgvVehicles.ReadOnly = true;
            dgvVehicles.AllowUserToAddRows = false;
            dgvVehicles.AllowUserToDeleteRows = false;
            dgvVehicles.MultiSelect = false;
            dgvVehicles.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvVehicles.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvVehicles.BackgroundColor = Color.White;
            dgvVehicles.RowHeadersVisible = false;

            dgvVehicles.SelectionChanged +=
                (sender, e) => FillSelectedVehicle();

            Controls.Add(dgvVehicles);
        }

        // Açıklama etiketi ve TextBox oluşturur.
        private void AddInput(
            string labelText,
            TextBox textBox,
            int x,
            int y,
            string hint)
        {
            Label label = new Label
            {
                Text = labelText,
                Location = new Point(x, y),
                Size = new Size(430, 25),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            textBox.Location = new Point(x, y + 28);
            textBox.Size = new Size(430, 30);
            textBox.AccessibleDescription = hint;

            Controls.Add(label);
            Controls.Add(textBox);
        }

        // Butonların ortak görünümünü ayarlar.
        private void ConfigureButton(
            Button button,
            string text,
            int x,
            int y)
        {
            button.Text = text;
            button.Location = new Point(x, y);
            button.Size = new Size(200, 55);
            button.BackColor = Color.FromArgb(30, 45, 65);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.Cursor = Cursors.Hand;

            Controls.Add(button);
        }

        // Girilen bilgileri kontrol eder ve araç oluşturur.
        private Car ReadVehicleFromInputs(int id)
        {
            if (string.IsNullOrWhiteSpace(txtPlate.Text) ||
                string.IsNullOrWhiteSpace(txtBrand.Text) ||
                string.IsNullOrWhiteSpace(txtModel.Text) ||
                string.IsNullOrWhiteSpace(txtYear.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text) ||
                string.IsNullOrWhiteSpace(txtSeats.Text))
            {
                throw new ArgumentException(
                    "Lütfen bütün alanlara bir değer girin.");
            }

            if (!int.TryParse(txtYear.Text, out int year))
            {
                txtYear.Focus();
                throw new ArgumentException(
                    "Yıl alanına yalnızca sayı girin.");
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                txtPrice.Focus();
                throw new ArgumentException(
                    "Günlük ücret alanına geçerli bir sayı girin.");
            }

            if (!int.TryParse(txtSeats.Text, out int seats))
            {
                txtSeats.Focus();
                throw new ArgumentException(
                    "Koltuk sayısı alanına yalnızca sayı girin.");
            }

            // Car sınıfının constructor'ı ile nesne oluşturulur.
            return new Car(
                id,
                txtPlate.Text,
                txtBrand.Text,
                txtModel.Text,
                year,
                price,
                seats
            );
        }

        // Yeni araç ekler.
        private void AddVehicle()
        {
            try
            {
                int newId = _manager.Vehicles.Count == 0
                    ? 1
                    : _manager.Vehicles.Max(v => v.Id) + 1;

                Car car = ReadVehicleFromInputs(newId);

                // Aynı plakanın tekrar kaydedilmesini önler.
                bool duplicatePlate = _manager.Vehicles.Any(
                    v => v.Plate.Equals(
                        car.Plate,
                        StringComparison.OrdinalIgnoreCase));

                if (duplicatePlate)
                {
                    throw new ArgumentException(
                        "Bu plakaya sahip bir araç zaten kayıtlı.");
                }

                _manager.AddVehicle(car);

                RefreshVehicleList();
                ClearInputs();

                MessageBox.Show(
                    "Araç başarıyla eklendi.",
                    "Başarılı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                txtPlate.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Araç Ekleme Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // Seçili aracın bilgilerini günceller.
        private void UpdateVehicle()
        {
            try
            {
                if (selectedVehicleId == 0)
                {
                    throw new ArgumentException(
                        "Güncellemek için listeden bir araç seçin.");
                }

                Car updatedCar =
                    ReadVehicleFromInputs(selectedVehicleId);

                bool duplicatePlate = _manager.Vehicles.Any(
                    v => v.Id != selectedVehicleId &&
                         v.Plate.Equals(
                             updatedCar.Plate,
                             StringComparison.OrdinalIgnoreCase));

                if (duplicatePlate)
                {
                    throw new ArgumentException(
                        "Bu plaka başka bir araca ait.");
                }

                // Mevcut araç nesnesini bulur.
                Vehicle existingVehicle =
                    _manager.Vehicles.First(
                        v => v.Id == selectedVehicleId);

                // Kiradaki araçların değiştirilmesini önler.
                if (!existingVehicle.IsAvailable)
                {
                    throw new InvalidOperationException(
                        "Kiradaki araç güncellenemez.");
                }

                existingVehicle.Plate = updatedCar.Plate;
                existingVehicle.Brand = updatedCar.Brand;
                existingVehicle.Model = updatedCar.Model;
                existingVehicle.Year = updatedCar.Year;
                existingVehicle.DailyRentalPrice =
                    updatedCar.DailyRentalPrice;

                if (existingVehicle is Car existingCar)
                {
                    existingCar.SeatCount = updatedCar.SeatCount;
                }

                RefreshVehicleList();
                ClearInputs();

                MessageBox.Show(
                    "Araç başarıyla güncellendi.",
                    "Başarılı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                txtPlate.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Güncelleme Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // Seçili aracı siler.
        private void DeleteVehicle()
        {
            try
            {
                if (selectedVehicleId == 0)
                {
                    throw new ArgumentException(
                        "Silmek için listeden bir araç seçin.");
                }

                Vehicle vehicle = _manager.Vehicles.First(
                    v => v.Id == selectedVehicleId);

                if (!vehicle.IsAvailable)
                {
                    throw new InvalidOperationException(
                        "Kiradaki araç silinemez.");
                }

                // Kiralama geçmişi bulunan araç silinemez.
                if (_manager.Rentals.Any(
                    r => r.RentedVehicle.Id == selectedVehicleId))
                {
                    throw new InvalidOperationException(
                        "Kiralama kaydı bulunan araç silinemez.");
                }

                DialogResult answer = MessageBox.Show(
                    "Seçili aracı silmek istiyor musunuz?",
                    "Silme Onayı",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (answer != DialogResult.Yes)
                {
                    txtPlate.Focus();
                    return;
                }

                _manager.Vehicles.Remove(vehicle);

                RefreshVehicleList();
                ClearInputs();

                MessageBox.Show(
                    "Araç başarıyla silindi.",
                    "Başarılı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                txtPlate.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Silme Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // Araçları tabloda gösterir.
        private void RefreshVehicleList()
        {
            dgvVehicles.DataSource = null;

            dgvVehicles.DataSource = _manager.Vehicles
                .Select(v => new
                {
                    v.Id,
                    Plaka = v.Plate,
                    Marka = v.Brand,
                    Model = v.Model,
                    Yil = v.Year,
                    GunlukUcret = v.DailyRentalPrice,
                    Durum = v.IsAvailable
                        ? "Müsait"
                        : "Kirada",
                    Koltuk = v is Car car
                        ? car.SeatCount
                        : 0
                })
                .ToList();

            dgvVehicles.ClearSelection();
            selectedVehicleId = 0;
        }

        // Tablodan seçilen aracın bilgilerini kutulara getirir.
        private void FillSelectedVehicle()
        {
            if (dgvVehicles.SelectedRows.Count == 0)
            {
                return;
            }

            DataGridViewRow row =
                dgvVehicles.SelectedRows[0];

            if (row.Cells["Id"].Value == null)
            {
                return;
            }

            selectedVehicleId =
                Convert.ToInt32(row.Cells["Id"].Value);

            Vehicle vehicle = _manager.Vehicles.First(
                v => v.Id == selectedVehicleId);

            txtPlate.Text = vehicle.Plate;
            txtBrand.Text = vehicle.Brand;
            txtModel.Text = vehicle.Model;
            txtYear.Text = vehicle.Year.ToString();
            txtPrice.Text = vehicle.DailyRentalPrice.ToString();

            txtSeats.Text = vehicle is Car car
                ? car.SeatCount.ToString()
                : "";
        }

        // Giriş kutularını temizler ve odağı plakaya taşır.
        private void ClearInputs()
        {
            txtPlate.Clear();
            txtBrand.Clear();
            txtModel.Clear();
            txtYear.Clear();
            txtPrice.Clear();
            txtSeats.Clear();

            selectedVehicleId = 0;
            dgvVehicles.ClearSelection();

            txtPlate.Focus();
        }
    }
}
