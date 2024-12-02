using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cybersport
{
    public partial class Users : Form
    {
        public Users()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
        }

        private void Users_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void Users_Load(object sender, EventArgs e)
        {
            dataGridView1.RowHeadersVisible = false;
            string connectionString = data.conStr;
            string query = @"
        SELECT 
            UserID AS 'ID', 
            Username AS 'Логин', 
            Password AS 'Пароль', 
            Email AS 'Email', 
            FIO AS 'ФИО', 
            PhoneNumber AS 'Телефон', 
            Role AS 'Роль' 
        FROM Users";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable usersTable = new DataTable();
                    adapter.Fill(usersTable);

                    dataGridView1.DataSource = usersTable;
                    dataGridView1.ClearSelection();

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                int r = e.RowIndex;
                dataGridView1.Rows[r].Selected = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Admin admin = new Admin();
            this.Visible = false;
            admin.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddUser addUser = new AddUser();
            this.Visible = false;
            addUser.ShowDialog();
            this.Close();
        }
    }
}



