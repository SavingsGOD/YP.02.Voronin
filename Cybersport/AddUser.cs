using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Cybersport
{
    public partial class AddUser : Form
    {
        string conString = data.conStr;
        public AddUser()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            this.Visible = false;
            users.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            string email = emailTextBox.Text + comboBox1.Text.Trim();
            string fio = fioTextBox.Text;
            string phoneNumber = maskedTextBox1.Text;
            string role = roleComboBox.SelectedItem?.ToString();

            // Проверка на заполнение всех полей
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(fio) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(phoneNumber) || string.IsNullOrWhiteSpace(role) ||
                !maskedTextBox1.MaskFull || string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(conString))
            {
                connection.Open();

                // Проверка существования логина
                if (IsLoginTaken(connection, username))
                {
                    MessageBox.Show("Этот логин уже занят. Пожалуйста, выберите другой.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Проверка существования email
                if (IsEmailTaken(connection, email))
                {
                    MessageBox.Show("Этот email уже зарегистрирован. Пожалуйста, используйте другой.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Проверка существования номера телефона
                if (IsPhoneTaken(connection, phoneNumber))
                {
                    MessageBox.Show("Этот номер телефона уже зарегистрирован. Пожалуйста, используйте другой.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string hashedPassword = HashPassword(password);
                string query = "INSERT INTO Users (Username, Password, FIO, Email, PhoneNumber, Role) VALUES (@login, @password, @fullName, @Email, @Phone, @Role)";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@login", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@fullName", fio);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Phone", phoneNumber);
                    cmd.Parameters.AddWithValue("@Role", role);
                    try
                    {
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Регистрация прошла успешно!", "Регистрация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при регистрации. Попробуйте еще раз.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка: " + ex.Message);
                    }
                }
            }
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            roleComboBox.Items.Add("Администратор");
            roleComboBox.Items.Add("Менеджер");

            comboBox1.Items.Add("@mail.ru");
            comboBox1.Items.Add("@inbox.ru");
            comboBox1.Items.Add("@bk.ru");
            comboBox1.Items.Add("@list.ru");
            comboBox1.Items.Add("@internet.ru");
            comboBox1.Items.Add("@yandex.ru");
            comboBox1.Items.Add("@xmail.ru");
            comboBox1.Items.Add("@gmail.com");
            comboBox1.Items.Add("@yahoo.com");
            comboBox1.Items.Add("@hotmail.com");
            comboBox1.Items.Add("@outlook.com");
        }

        private void AddUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        // Метод для проверки наличия логина
        private bool IsLoginTaken(MySqlConnection connection, string login)
        {
            using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @login", connection))
            {
                cmd.Parameters.AddWithValue("@login", login);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0; // Возвращает true, если логин уже занят
            }
        }

        // Метод для проверки наличия email
        private bool IsEmailTaken(MySqlConnection connection, string email)
        {
            using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM Users WHERE Email = @Email", connection))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0; // Возвращает true, если email уже занят
            }
        }

        // Метод для проверки наличия номера телефона
        private bool IsPhoneTaken(MySqlConnection connection, string phone)
        {
            using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM Users WHERE PhoneNumber = @Phone", connection))
            {
                cmd.Parameters.AddWithValue("@Phone", phone);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0; // Возвращает true, если номер телефона уже занят
            }
        }

        // Метод для очистки полей
        private void ClearFields()
        {
            usernameTextBox.Clear();
            passwordTextBox.Clear();
            fioTextBox.Clear();
            emailTextBox.Clear();
            maskedTextBox1.Clear();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void usernameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidLoginCharacter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        bool IsValidLoginCharacter(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   (c >= '0' && c <= '9');
        }

        private void passwordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidPassCharacter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        bool IsValidPhoneCharacter(char c)
        {
            return
                   (c >= '0' && c <= '9');
        }
        bool IsValidPassCharacter(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   "!@#$%^&*()_=-+,./{}<>?".Contains(c) ||
                   (c >= '0' && c <= '9');
        }

        private void fioTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidFIOCharacter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        bool IsValidFIOCharacter(char c)
        {
            return (c >= 'а' && c <= 'я') ||
                   (c >= 'А' && c <= 'Я') ||
                   (c == 32) ||
                   (c == '-');

        }

        private void emailTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidEmailCharacter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        bool IsValidEmailCharacter(char c)
        {
            return (c >= 'a' && c <= 'z') ||

                   (c >= '0' && c <= '9');
        }

        private void fioTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(fioTextBox.Text))
            {
                var words = fioTextBox.Text.Split(' ');

                for (int i = 0; i < words.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(words[i]))
                    {
                        words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                    }
                }

                fioTextBox.Text = string.Join(" ", words);

                fioTextBox.SelectionStart = fioTextBox.Text.Length;

                fioTextBox.SelectionStart = fioTextBox.Text.Length;
                fioTextBox.SelectionLength = 0;
                fioTextBox.TextChanged -= fioTextBox_TextChanged;
                fioTextBox.Text = fioTextBox.Text;
                fioTextBox.TextChanged += fioTextBox_TextChanged;
            }
        }
    }
}
