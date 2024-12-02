using Cybersport.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Cybersport
{
    //Admin
    public partial class Authorization : Form
    {
        public Authorization()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
        }
        string conString = data.conStr;
        public int m1 = 0;
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Registration registration = new Registration();
            this.Visible = false;
            registration.ShowDialog();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Player player = new Player();
            this.Visible = false;
            player.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string login = textBox1.Text.ToString();
                string hashPassword = GetHashPass(textBox2.Text.ToString());
                MySqlConnection con = new MySqlConnection(conString);
                con.Open();

                MySqlCommand cmd = new MySqlCommand($"Select * From Users Where Username = @username", con);
                cmd.Parameters.AddWithValue("@username", login);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Проверяем, существует ли пользователь
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Пользователь не существует", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Получаем данные пользователя
                string userName = dt.Rows[0].ItemArray.GetValue(4).ToString();
                string storedPasswordHash = dt.Rows[0].ItemArray.GetValue(2).ToString();
                string role = dt.Rows[0].ItemArray.GetValue(6).ToString();

                data.Login = login;
                data.usrName = userName;

                // Сравниваем хэш пароля
                if (hashPassword == storedPasswordHash)
                {
                    data.role = role;
                    MessageBox.Show("Вы успешно авторизовались", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Переход к соответствующему интерфейсу
                    OpenRoleBasedForm(role);
                }
                else
                {
                    MessageBox.Show("Неверный пароль", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox2.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка авторизации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static string GetHashPass(string password)
        {
            byte[] bytesPass = Encoding.UTF8.GetBytes(password);

            SHA256Managed hashstring = new SHA256Managed();

            byte[] hash = hashstring.ComputeHash(bytesPass);
            string hashPasswd = string.Empty;

            foreach (byte x in hash)
            {
                hashPasswd += string.Format("{0:x2}", x);
            }
            hashstring.Dispose();

            return hashPasswd;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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
        bool IsValidPassCharacter(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   "!@#$%^&*()_=-+,./{}<>?".Contains(c) ||
                   (c >= '0' && c <= '9');
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidPassCharacter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

            if (m1 == 0)
            {
                textBox2.PasswordChar = default;
                pictureBox1.Image = Properties.Resources.free_icon_hide_2767146; 
                m1 = 1;
            }
            else if (m1 == 1)
            {
                textBox2.PasswordChar = '*'; 
                pictureBox1.Image = Properties.Resources.free_icon_eye_158746; 
                m1 = 0;
            }
        }

            private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void OpenRoleBasedForm(string role)
        {
            if (role == "Администратор")
            {
                Admin admin = new Admin();
                this.Visible = false;
                admin.ShowDialog();
                this.Close();
            }
            else if (role == "Менеджер")
            {
                Manager manager = new Manager();
                this.Visible = false;
                manager.ShowDialog();
                this.Close();
            }
            else if (role == "Участник")
            {
                Player player = new Player();
                this.Visible = false;
                player.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Некорректный пользователь", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
