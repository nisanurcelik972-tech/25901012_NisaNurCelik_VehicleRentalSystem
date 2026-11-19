
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VehicleRentalSystem.Models;
using VehicleRentalSystem.Services;

namespace VehicleRentalSystem.Forms
{
    // Müşteri ekleme, güncelleme, silme ve listeleme ekranı.
    public class CustomerForm : Form
    {
        private readonly RentalManager _manager;

        // Kayıt alanları.
        private readonly TextBox txtFirstName = new TextBox();
        private readonly TextBox txtLastName = new TextBox();
        private readonly TextBox txtPhone = new TextBox();
        private readonly TextBox txtLicense = new TextBox();

        // Müşterilerin gösterileceği tablo.
        private readonly DataGridView dgvCustomers = new DataGridView();

        // İşlem butonları.
        private readonly Button btnAdd = new Button();
        private readonly Button btnUpdate = new Button();
        private readonly Button btnDelete = new Button();
        private readonly Button btnClear = new Button();

        // Seçilen müşterinin ID bilgisi.
        private int selectedCustomerId = 0;

        public CustomerForm(RentalManager manager)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            _manager = manager;

            Text = "Müşteri Yönetimi";
            Size = new Size(950, 650);
            MinimumSize = new Size(850, 600);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Arial", 10);
            KeyPreview = true;

            CreateInterface();
            RefreshCustomerList();

            Shown += (sender, e) => txtFirstName.Focus();
        }

        // Bütün ekran kontrollerini kodla oluşturur.
        private void CreateInterface()
        {
            Label title = new Label
            {
                Text = "MÜŞTERİ YÖNETİMİ",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 45, 65),
                Location = new Point(30, 20),
                AutoSize = true
            };

            Controls.Add(title);

            AddInput("Ad", txtFirstName, 30, 90, "Örn: Ahmet");
            AddInput("Soyad", txtLastName, 490, 90, "Örn: Yılmaz");
            AddInput("Telefon", txtPhone, 30, 165, "Örn: 05551112233");
            AddInput("Ehliyet Numarası", txtLicense, 490, 165,
                "Örn: EHL12345");

            ConfigureButton(btnAdd, "MÜŞTERİ EKLE", 30, 260);
            ConfigureButton(btnUpdate, "GÜNCELLE", 260, 260);
            ConfigureButton(btnDelete, "SİL", 490, 260);
            ConfigureButton(btnClear, "TEMİZLE", 720, 260);

            btnAdd.Click += (sender, e) => AddCustomer();
            btnUpdate.Click += (sender, e) => UpdateCustomer();
            btnDelete.Click += (sender, e) => DeleteCustomer();
            btnClear.Click += (sender, e) => ClearInputs();

            // Enter tuşu, MÜŞTERİ EKLE butonunu çalıştırır.
            AcceptButton = btnAdd;

            dgvCustomers.Location = new Point(30, 350);
            dgvCustomers.Size = new Size(860, 220);

            dgvCustomers.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            dgvCustomers.ReadOnly = true;
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.AllowUserToDeleteRows = false;
            dgvCustomers.MultiSelect = false;
            dgvCustomers.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvCustomers.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvCustomers.BackgroundColor = Color.White;
            dgvCustomers.RowHeadersVisible = false;

            dgvCustomers.SelectionChanged +=
                (sender, e) => FillSelectedCustomer();

            Controls.Add(dgvCustomers);
        }

        // Etiket ve giriş kutusu oluşturur.
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
                Size = new Size(400, 25),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            textBox.Location = new Point(x, y + 28);
            textBox.Size = new Size(400, 30);

            // .NET Framework ile uyumlu açıklama.
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

        // Giriş bilgilerini kontrol ederek Customer oluşturur.
        private Customer ReadCustomerFromInputs(int id)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtLicense.Text))
            {
                throw new ArgumentException(
                    "Lütfen bütün alanlara bir değer girin.");
            }

            string phone = txtPhone.Text.Trim();

            // Telefon alanı yalnızca rakam içermelidir.
            if (phone.Length != 11 || !phone.All(char.IsDigit))
            {
                txtPhone.Focus();

                throw new ArgumentException(
                    "Telefon numarası 11 rakamdan oluşmalıdır.");
            }

            return new Customer(
                id,
                txtFirstName.Text,
                txtLastName.Text,
                phone,
                txtLicense.Text
            );
        }

        // Yeni müşteri kaydeder.
        private void AddCustomer()
        {
            try
            {
                int newId = _manager.Customers.Count == 0
                    ? 1
                    : _manager.Customers.Max(c => c.Id) + 1;

                Customer customer = ReadCustomerFromInputs(newId);

                bool duplicateLicense = _manager.Customers.Any(
                    c => c.LicenseNumber.Equals(
                        customer.LicenseNumber,
                        StringComparison.OrdinalIgnoreCase));

                if (duplicateLicense)
                {
                    throw new ArgumentException(
                        "Bu ehliyet numarası zaten kayıtlı.");
                }

                _manager.AddCustomer(customer);

                RefreshCustomerList();
                ClearInputs();

                MessageBox.Show(
                    "Müşteri başarıyla eklendi.",
                    "Başarılı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Müşteri Ekleme Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                txtFirstName.Focus();
            }
        }

        // Seçilen müşteriyi günceller.
        private void UpdateCustomer()
        {
            try
            {
                if (selectedCustomerId == 0)
                {
                    throw new ArgumentException(
                        "Güncellemek için listeden bir müşteri seçin.");
                }

                Customer updated =
                    ReadCustomerFromInputs(selectedCustomerId);

                bool duplicateLicense = _manager.Customers.Any(
                    c => c.Id != selectedCustomerId &&
                         c.LicenseNumber.Equals(
                             updated.LicenseNumber,
                             StringComparison.OrdinalIgnoreCase));

                if (duplicateLicense)
                {
                    throw new ArgumentException(
                        "Bu ehliyet numarası başka müşteriye ait.");
                }

                Customer existing = _manager.Customers.First(
                    c => c.Id == selectedCustomerId);

                existing.FirstName = updated.FirstName;
                existing.LastName = updated.LastName;
                existing.Phone = updated.Phone;
                existing.LicenseNumber = updated.LicenseNumber;

                RefreshCustomerList();
                ClearInputs();

                MessageBox.Show(
                    "Müşteri başarıyla güncellendi.",
                    "Başarılı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Güncelleme Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                txtFirstName.Focus();
            }
        }

        // Müşteri kaydını siler.
        private void DeleteCustomer()
        {
            try
            {
                if (selectedCustomerId == 0)
                {
                    throw new ArgumentException(
                        "Silmek için listeden bir müşteri seçin.");
                }

                Customer customer = _manager.Customers.First(
                    c => c.Id == selectedCustomerId);

                // Aktif kiralaması bulunan müşteri silinemez.
                bool hasActiveRental = _manager.Rentals.Any(
                    r => r.RentedCustomer.Id == selectedCustomerId &&
                         r.IsActive);

                if (hasActiveRental)
                {
                    throw new InvalidOperationException(
                        "Aktif kiralaması olan müşteri silinemez.");
                }

                DialogResult answer = MessageBox.Show(
                    "Seçili müşteriyi silmek istiyor musunuz?",
                    "Silme Onayı",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (answer != DialogResult.Yes)
                    return;

                _manager.Customers.Remove(customer);

                RefreshCustomerList();
                ClearInputs();

                MessageBox.Show(
                    "Müşteri başarıyla silindi.",
                    "Başarılı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Silme Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                txtFirstName.Focus();
            }
        }

        // Müşterileri tabloda gösterir.
        private void RefreshCustomerList()
        {
            dgvCustomers.DataSource = null;

            dgvCustomers.DataSource = _manager.Customers
                .Select(c => new
                {
                    c.Id,
                    Ad = c.FirstName,
                    Soyad = c.LastName,
                    Telefon = c.Phone,
                    EhliyetNo = c.LicenseNumber
                })
                .ToList();

            dgvCustomers.ClearSelection();
            selectedCustomerId = 0;
        }

        // Tabloda seçilen müşteriyi kutulara getirir.
        private void FillSelectedCustomer()
        {
            if (dgvCustomers.SelectedRows.Count == 0)
                return;

            DataGridViewRow row = dgvCustomers.SelectedRows[0];

            if (row.Cells["Id"].Value == null)
                return;

            selectedCustomerId =
                Convert.ToInt32(row.Cells["Id"].Value);

            Customer customer = _manager.Customers.First(
                c => c.Id == selectedCustomerId);

            txtFirstName.Text = customer.FirstName;
            txtLastName.Text = customer.LastName;
            txtPhone.Text = customer.Phone;
            txtLicense.Text = customer.LicenseNumber;
        }

        // Kutuları temizler, seçimi kaldırır.
        private void ClearInputs()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtPhone.Clear();
            txtLicense.Clear();

            selectedCustomerId = 0;
            dgvCustomers.ClearSelection();

            txtFirstName.Focus();
        }
    }
}
