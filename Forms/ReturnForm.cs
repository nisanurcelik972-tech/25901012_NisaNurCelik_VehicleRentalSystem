
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VehicleRentalSystem.Services;

namespace VehicleRentalSystem.Forms
{
    // Kiralanan araçların iade işlemlerinin yapılacağı pencere.
    public class ReturnForm : Form
    {
        // Diğer ekranlarla aynı kiralama verilerini kullanır.
        private readonly RentalManager _manager;

        // Aktif kiralamaların gösterileceği tablo.
        private readonly DataGridView dgvRentals = new DataGridView();

        // İade işlemini başlatacak buton.
        private readonly Button btnReturn = new Button();

        // Constructor: Pencere oluşturulurken çalışır.
        public ReturnForm(RentalManager manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            _manager = manager;

            Text = "Araç İade Yönetimi";
            Size = new Size(1000, 650);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Arial", 10);
            KeyPreview = true;

            CreateInterface();
            LoadActiveRentals();
        }

        // Araç iade ekranının temel görünümünü oluşturur.
        private void CreateInterface()
        {
            Label title = new Label();
            title.Text = "ARAÇ İADE YÖNETİMİ";
            title.Font = new Font("Arial", 20, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(30, 45, 65);
            title.Location = new Point(30, 25);
            title.AutoSize = true;
            Controls.Add(title);

            Label description = new Label();
            description.Text =
                "Aktif kiralamaları görüntüleyip araç iade işlemi yapabilirsiniz.";
            description.Location = new Point(30, 85);
            description.AutoSize = true;
            description.ForeColor = Color.DimGray;
            Controls.Add(description);

            // Kiralama tablosunun görünümü.
            dgvRentals.Location = new Point(30, 145);
            dgvRentals.Size = new Size(920, 350);
            dgvRentals.ReadOnly = true;
            dgvRentals.AllowUserToAddRows = false;
            dgvRentals.AllowUserToDeleteRows = false;
            dgvRentals.MultiSelect = false;
            dgvRentals.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvRentals.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            Controls.Add(dgvRentals);

            // Araç iade butonunun görünümü.
            btnReturn.Text = "SEÇİLİ ARACI İADE ET";
            btnReturn.Location = new Point(30, 525);
            btnReturn.Size = new Size(920, 55);
            btnReturn.BackColor = Color.FromArgb(30, 100, 75);
            btnReturn.ForeColor = Color.White;
            btnReturn.FlatStyle = FlatStyle.Flat;
            btnReturn.Cursor = Cursors.Hand;
            // İade butonuna basıldığında iade işlemini başlatır.
            btnReturn.Click += (sender, e) =>
            {
                ReturnSelectedVehicle();
            };
            Controls.Add(btnReturn);
        }

        // Aktif kiralamaları tabloya yükler.
        private void LoadActiveRentals()
        {
            // Önce tablonun eski verilerini temizler.
            dgvRentals.DataSource = null;

            // Yalnızca aktif kiralamaları gösterir.
            var activeRentals = _manager.Rentals
                .Where(r => r.IsActive)
                .Select(r => new
                {
                    KiralamaNo = r.RentalId,
                    Musteri = r.RentedCustomer.FirstName + " " + r.RentedCustomer.LastName,
                    Plaka = r.RentedVehicle.Plate,
                    Baslangic = r.StartDate.ToShortDateString(),
                    Bitis = r.EndDate.ToShortDateString(),
                    ToplamUcret = r.TotalPrice
                })
                .ToList();

            dgvRentals.DataSource = activeRentals;
        }
        private void ReturnSelectedVehicle()

        {
            // Kullanıcı bir kiralama seçmiş mi kontrol eder.
            if (dgvRentals.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Lütfen iade edilecek kiralamayı seçin.",
                    "Uyarı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                dgvRentals.Focus();
                return;
            }

            // Seçilen kiralamanın numarasını alır.
            int rentalId = Convert.ToInt32(
                dgvRentals.SelectedRows[0].Cells["KiralamaNo"].Value);

            // İade işlemi için onay ister.
            DialogResult result = MessageBox.Show(
                "Seçilen aracı iade etmek istiyor musunuz?",
                "İade Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                dgvRentals.Focus();
                return;
            }

            try
            {
                // Aracı iade eder.
                _manager.ReturnVehicle(rentalId);

                MessageBox.Show(
                    "Araç başarıyla iade edildi.",
                    "Başarılı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Aktif kiralamalar tablosunu yeniler.
                LoadActiveRentals();
                dgvRentals.ClearSelection();
                dgvRentals.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "İade Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                dgvRentals.Focus();
            }
        }

    }
}
