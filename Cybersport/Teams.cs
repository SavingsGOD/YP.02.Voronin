using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Cybersport
{
    public partial class Teams : Form
    {
        public Teams()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
            InitializeComboBox(); // Инициализация ComboBox

        }

        private void InitializeComboBox()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("По возрастанию");
            comboBox1.Items.Add("По убыванию");
        }

        private void Teams_Load(object sender, EventArgs e)
        {
            LoadTeamsForTournaments();
        }
        void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        public void LoadTeamsForTournaments(string searchTerm = "")
        {
            comboBox1.KeyPress += new KeyPressEventHandler(OnKeyPress);

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ClearSelection();
            string connectionString = data.conStr;

            string query = $@"
    SELECT 
        Te.TeamID, 
        Te.TeamName AS 'Название команды',
        T.TournamentName AS 'Название турнира', 
        T.StartDate AS 'Дата начала',
        T.EndDate AS 'Дата завершения',
        U.Username AS 'Капитан',
        GROUP_CONCAT(PU.Username SEPARATOR ', ') AS 'Игроки'
    FROM 
        TournamentRegistrations TR
    JOIN 
        Tournaments T ON TR.TournamentID = T.TournamentID
    JOIN 
        Teams Te ON TR.TeamID = Te.TeamID
    JOIN 
        Users U ON Te.CaptainID = U.UserID
    LEFT JOIN 
        Participants P ON Te.TeamID = P.TeamID
    LEFT JOIN 
        Users PU ON P.UserID = PU.UserID
    WHERE 
        Te.TeamName LIKE @searchTerm
    GROUP BY 
        Te.TeamID, T.TournamentName, Te.TeamName, U.Username, T.StartDate, T.EndDate
    ";

            // Добавление сортировки
            if (comboBox1.SelectedIndex == 0) // По возрастанию
            {
                query += " ORDER BY T.StartDate ASC"; // Сортировка по дате начала
            }
            else if (comboBox1.SelectedIndex == 1) // По убыванию
            {
                query += " ORDER BY T.StartDate DESC"; // Сортировка по дате начала
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                // Добавляем параметр для фильтрации
                adapter.SelectCommand.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                try
                {
                    connection.Open();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;

                    // Убираем выделение у всех строк
                    dataGridView1.ClearSelection();

                    if (dataGridView1.Rows.Count > 0)
                    {
                        dataGridView1.Rows[0].Selected = false;
                    }

                    HighlightUserTeamRows();

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void HighlightUserTeamRows()
        {
            List<int> userTeamIds = GetUserTeamIds();  // Получаем список идентификаторов команд пользователя
            DateTime currentDate = DateTime.Now; // Получаем текущее время

            if (dataGridView1.Rows.Count > 0 && userTeamIds != null && userTeamIds.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Убираем выделение
                    row.Selected = false;

                    if (row.Cells[0].Value != null && userTeamIds.Contains(Convert.ToInt32(row.Cells[0].Value))) // 0 - индекс столбца с TeamID
                    {
                        // Получаем дату начала и окончания турнира из ячеек "Дата начала" и "Дата завершения"
                        DateTime tournamentStartDate = Convert.ToDateTime(row.Cells["Дата начала"].Value);
                        DateTime tournamentEndDate = Convert.ToDateTime(row.Cells["Дата завершения"].Value);

                        // Проверяем, идет ли турнир в настоящий момент
                        if (tournamentStartDate <= currentDate && tournamentEndDate >= currentDate)
                        {
                            row.DefaultCellStyle.BackColor = Color.LightBlue; // Если турнир идет
                            row.DefaultCellStyle.ForeColor = Color.Black; // Установка цвета текста, чтобы он был виден на голубом фоне
                        }
                        else if (tournamentStartDate > currentDate)
                        {
                            row.DefaultCellStyle.BackColor = Color.LightGreen; // Если турнир в будущем
                            row.DefaultCellStyle.ForeColor = Color.Black; // Установка цвета текста, чтобы он был виден на зеленом фоне
                        }
                        else
                        {
                            row.DefaultCellStyle.BackColor = Color.LightGray; // Если турнир уже прошёл
                            row.DefaultCellStyle.ForeColor = Color.Black; // Установка цвета текста, чтобы он был виден на сером фоне
                        }
                    }
                }
            }
        }

        private List<int> GetUserTeamIds()
        {
            List<int> teamIds = new List<int>();
            string userName = data.Login; // Получаем логин текущего пользователя

            string query = @"
                SELECT DISTINCT TR.TeamID
                FROM TournamentRegistrations TR
                JOIN Teams Te ON TR.TeamID = Te.TeamID
                JOIN Users U ON U.UserID = Te.CaptainID OR U.UserID IN (SELECT UserID FROM Participants WHERE TeamID = Te.TeamID)
                WHERE U.Username = @username;";

            using (MySqlConnection connection = new MySqlConnection(data.conStr))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", userName);

                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            teamIds.Add(reader.GetInt32(0)); // Добавляем идентификаторы команд в список
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
            return teamIds;
        }

        private void Teams_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Player player = new Player();
            this.Visible = false;
            player.ShowDialog();
            this.Close();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Скрытие столбца TeamID
            dataGridView1.Columns[0].Visible = false;  // Скрываем TeamID

            // Убираем выделение у всех строк
            dataGridView1.ClearSelection();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadTeamsForTournaments(textBox1.Text);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTeamsForTournaments(textBox1.Text);
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
                   (c == 32) ||
                   (c >= '0' && c <= '9');
        }
    }
}
