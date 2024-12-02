using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cybersport
{
    public partial class Tournaments : Form
    {
        private ContextMenuStrip contextMenuStrip;
        public Tournaments()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации

            this.FormClosing += new FormClosingEventHandler(Tournaments_FormClosing);
            InitializeComponent();
            contextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem viewResultsItem = new ToolStripMenuItem("Просмотреть результаты турнира");
            viewResultsItem.Click += ViewResultsItem_Click;
            contextMenuStrip.Items.Add(viewResultsItem);
            dataGridView1.MouseDown += dataGridView1_MouseDown;
        }
        string connect = data.conStr;

        private void button4_Click(object sender, EventArgs e)
        {
            Player player = new Player();
            this.Visible = false;
            player.ShowDialog();
            this.Close();
        }

        private void ViewResultsItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int tournamentId = GetTournamentIDFromRow(selectedRow);

                if (HasTournamentResults(tournamentId))
                {
                    this.Visible = false;
                    TournamentResults tournamentResults = new TournamentResults(tournamentId);
                    tournamentResults.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("У данного турнира результаты ещё не сформированы.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private int GetTournamentIDFromRow(DataGridViewRow row)
        {
            return Convert.ToInt32(row.Cells["TournamentID"].Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Registration_on_tournament registration_On_Tournament = new Registration_on_tournament();
            this.Visible = false;
            registration_On_Tournament.ShowDialog();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void Tournaments_Load(object sender, EventArgs e)
        {
            if (data.role == "Администратор")
            {
                button2.Visible = false;
            }
            if (data.role == "Участник")
            {
                button2.Visible = true;
            }


            comboBox1.KeyPress += new KeyPressEventHandler(OnKeyPress);
            comboBox2.KeyPress += new KeyPressEventHandler(OnKeyPress);

            comboBox1.Items.Add("По возрастанию");
            comboBox1.Items.Add("По убыванию");

            dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);

            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = connect;

                con.Open();

                MySqlCommand cmd = new MySqlCommand(@"
        SELECT 
            Tournaments.TournamentID,
            Tournaments.TournamentName AS Название, 
            Tournaments.StartDate AS 'Дата начала', 
            Tournaments.EndDate AS 'Дата окончания',
            Tournaments.GameType AS 'Жанр игр', 
            CASE 
                WHEN Tournaments.StartDate > NOW() THEN 'Предстоящий'
                WHEN Tournaments.EndDate < NOW() THEN 'Завершённый'
                ELSE 'Текущий'
            END AS 'Статус'
        FROM 
            Tournaments
    ", con);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                // Убрать выделение первой строки
                dataGridView1.ClearSelection();

                if (dataGridView1.Columns.Contains("StatusOrder"))
                {
                    dataGridView1.Columns["StatusOrder"].Visible = false;
                }

                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                LoadGameGenres();
                LoadFilteredData();
            }
        }


        void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Player player = new Player();
            this.Visible = false;
            player.ShowDialog();
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadFilteredData();
        }
        string FillDataGridView(string search = "", string genreFilter = "")
        {
            string query = @"
    SELECT 
        Tournaments.TournamentID,
        Tournaments.TournamentName AS Название,
        Tournaments.StartDate AS 'Дата начала',
        Tournaments.EndDate AS 'Дата окончания',
        Tournaments.GameType AS 'Жанр игр',
        CASE 
            WHEN Tournaments.StartDate > NOW() THEN 'Предстоящий'
            WHEN Tournaments.EndDate < NOW() THEN 'Завершенный'
            ELSE 'Текущий'
        END AS 'Статус',
        CASE 
            WHEN Tournaments.StartDate > NOW() THEN 1 -- Предстоящий
            WHEN Tournaments.EndDate < NOW() THEN 3 -- Завершённый
            ELSE 2 -- Текущий
        END AS StatusOrder
    FROM 
        Tournaments";

            List<string> conditions = new List<string>();

            if (!string.IsNullOrEmpty(search))
            {
                conditions.Add($"Tournaments.TournamentName LIKE '%{search}%'");
            }

            if (!string.IsNullOrEmpty(genreFilter))
            {
                conditions.Add($"Tournaments.GameType = '{genreFilter}'");
            }

            if (conditions.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", conditions);
            }

            if (comboBox1.SelectedIndex != -1)
            {
                string sortOrder = comboBox1.SelectedItem.ToString() == "По возрастанию" ? "ASC" : "DESC";
                query += $" ORDER BY StatusOrder {sortOrder}, Tournaments.StartDate {sortOrder}"; // Два уровня сортировки
            }

            return query;
        }
        void LoadData(string query)
        {
            using (MySqlConnection con = new MySqlConnection(data.conStr))
            {
                con.Open();
                MySqlCommand command = new MySqlCommand(query, con);
                DataTable dt = new DataTable();

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                }

                dataGridView1.DataSource = dt;
                dataGridView1.AllowUserToAddRows = false;

                if (dataGridView1.Columns.Contains("StatusOrder"))
                {
                    dataGridView1.Columns["StatusOrder"].Visible = false;
                }

                // Поведение выбора строк в DataGridView
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows[0].Selected = false; // Убираем выделение с первой строки
                }
            }
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
        private void UpdateButtonState()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];

                string tournamentStatus = selectedRow.Cells["Статус"].Value.ToString();

                if (tournamentStatus == "Предстоящий")
                {
                    button2.Enabled = true;
                }
                else
                {
                    button2.Enabled = false;
                }
            }
            else
            {
                button2.Enabled = false;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                string tournamentStatus = selectedRow.Cells["Статус"].Value.ToString();
                if (tournamentStatus == "Предстоящий")
                {
                    string tournamentName = selectedRow.Cells["Название"].Value.ToString();
                    DateTime startDate = Convert.ToDateTime(selectedRow.Cells["Дата начала"].Value);
                    DateTime endDate = Convert.ToDateTime(selectedRow.Cells["Дата окончания"].Value);
                    string gameType = selectedRow.Cells["Жанр игр"].Value.ToString();
                    int tournamentId = GetTournamentIDFromRow(selectedRow); // Получаем ID тут

                    // Передаем TournamentID в Registration_on_tournament
                    Registration_on_tournament registrationForm = new Registration_on_tournament();
                    registrationForm.TournamentID = tournamentId; // Устанавливаем TournamentID
                    registrationForm.SetTournamentDetails(tournamentName, startDate, endDate, gameType);
                    this.Visible = false;
                    registrationForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Вы не можете подать заявку на турнир, так как его статус - " + tournamentStatus + ".");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите турнир для регистрации.");
            }
        }

        private void LoadFilteredData()
        {
            string search = textBox1.Text.Trim();
            string genreFilter = string.Empty;

            if (comboBox2.SelectedIndex != -1 && comboBox2.SelectedItem.ToString() != "Сбросить фильтрацию")
            {
                genreFilter = comboBox2.SelectedItem.ToString();
            }

            string query = FillDataGridView(search, genreFilter);
            LoadData(query);
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Статус")
            {
                switch (e.Value?.ToString())
                {
                    case "Предстоящий":
                        e.CellStyle.BackColor = Color.LightGreen;
                        break;
                    case "Завершенный":
                        e.CellStyle.BackColor = Color.LightGray;
                        break;
                    case "Текущий":
                        e.CellStyle.BackColor = Color.LightBlue;
                        break;
                    default:
                        break;
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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonState();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFilteredData();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Получаем текст из текстбокса перед удалением
            string search = textBox1.Text.Trim();
            string genreFilter = string.Empty;

            if (comboBox2.SelectedItem != null)
            {
                // Если выбрано "Сбросить фильтрацию"
                if (comboBox2.SelectedItem.ToString() == "Сбросить фильтрацию")
                {
                    // Сбрасываем фильтры
                    textBox1.Text = string.Empty;
                    comboBox2.SelectedIndex = -1; // отмена выделения жанра

                    // обновляем данные без фильтрации
                    LoadFilteredData();
                }
                else
                {
                    // Устанавливаем фильтры по жанру
                    genreFilter = comboBox2.SelectedItem.ToString();
                    LoadFilteredData();
                }

            }
        }
        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            if (comboBox2.Items.Contains("Фильтрация по жанру"))
                comboBox2.Items.Remove("Фильтрация по жанру");
        }

        private void dataGridView1_ContextMenuStripChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitTestInfo = dataGridView1.HitTest(e.X, e.Y);
                if (hitTestInfo.RowIndex >= 0)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[hitTestInfo.RowIndex].Selected = true;
                    contextMenuStrip.Show(dataGridView1, e.Location);
                }
            }
        }
        private void LoadGameGenres()
        {
            using (MySqlConnection con = new MySqlConnection(data.conStr))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT GenreName FROM GameGenres", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["GenreName"].ToString());
                }

                comboBox2.Items.Add("Сбросить фильтрацию"); // Добавить опцию сброса фильтрации
            }
        }

        private void Tournaments_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private bool HasTournamentResults(int tournamentId)
        {
            using (MySqlConnection con = new MySqlConnection(data.conStr))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM MatchResults WHERE TournamentID = @TournamentId", con);
                cmd.Parameters.AddWithValue("@TournamentId", tournamentId);
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count > 0; // Если есть результаты, возвращаем true
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.ClearSelection();
        }
    }
}
