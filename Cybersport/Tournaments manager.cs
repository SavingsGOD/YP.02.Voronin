using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cybersport
{
    public partial class Tournaments_and_add_tournaments : Form
    {
        private ContextMenuStrip contextMenuStrip;
        public Tournaments_and_add_tournaments()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
        }
        string connect = data.conStr;

        private void Tournaments_and_add_tournaments_Load(object sender, EventArgs e)
        {

            comboBox1.KeyPress += new KeyPressEventHandler(OnKeyPress);
            comboBox2.KeyPress += new KeyPressEventHandler(OnKeyPress);
            buttonDelete.Enabled = false;
            buttonEdit.Enabled = false;

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
            WHEN Tournaments.EndDate < NOW() THEN 'Завершённый'
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

        private void button4_Click(object sender, EventArgs e)
        {
            Manager manager = new Manager();
            this.Visible = false;
            manager.ShowDialog();
            this.Close();
        }
        bool IsValidLoginCharacter(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   (c == 32) ||
                   (c >= '0' && c <= '9');
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void Tournaments_and_add_tournaments_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

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
                    case "Завершённый":
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadFilteredData();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidLoginCharacter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
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

        private void dataGridView1_SelectionChanged_1(object sender, EventArgs e)
        {
            buttonDelete.Enabled = dataGridView1.SelectedRows.Count > 0;
            buttonEdit.Enabled = dataGridView1.SelectedRows.Count > 0;
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
                    textBox1.Text = string.Empty; // очистка текстбокса
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

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitTestInfo = dataGridView1.HitTest(e.X, e.Y);
                if (hitTestInfo.RowIndex >= 0)
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[hitTestInfo.RowIndex].Selected = true;
                }
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.Columns[0].Visible = false;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Manager manager = new Manager();
            this.Visible = false;
            manager.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddTournament addTournament = new AddTournament();
            this.Visible = false;
            addTournament.ShowDialog();
            this.Close();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить выбранный турнир?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int tournamentId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["TournamentID"].Value);
                    DeleteTournament(tournamentId);
                    MessageBox.Show("Турнир успешно удалён", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadFilteredData(); // Обновить данные после удаления
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите турнир для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteTournament(int tournamentId)
        {
            using (MySqlConnection con = new MySqlConnection(data.conStr))
            {
                con.Open();
                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        // Удаляем все записи из зависимых таблиц
                        var deleteMatchResultsCmd = new MySqlCommand("DELETE FROM MatchResults WHERE TournamentID = @TournamentID", con, transaction);
                        deleteMatchResultsCmd.Parameters.AddWithValue("@TournamentID", tournamentId);
                        deleteMatchResultsCmd.ExecuteNonQuery();

                        var deleteTournamentRegistrationsCmd = new MySqlCommand("DELETE FROM TournamentRegistrations WHERE TournamentID = @TournamentID", con, transaction);
                        deleteTournamentRegistrationsCmd.Parameters.AddWithValue("@TournamentID", tournamentId);
                        deleteTournamentRegistrationsCmd.ExecuteNonQuery();

                        var deleteTournamentsTeamsCmd = new MySqlCommand("DELETE FROM TournamentsTeams WHERE idTournaments = @TournamentID", con, transaction);
                        deleteTournamentsTeamsCmd.Parameters.AddWithValue("@TournamentID", tournamentId);
                        deleteTournamentsTeamsCmd.ExecuteNonQuery();

                        // Теперь удаляем сам турнир
                        var deleteTournamentCmd = new MySqlCommand("DELETE FROM Tournaments WHERE TournamentID = @TournamentID", con, transaction);
                        deleteTournamentCmd.Parameters.AddWithValue("@TournamentID", tournamentId);
                        deleteTournamentCmd.ExecuteNonQuery();

                        // Подтверждаем транзакцию
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // В случае ошибки откатываем изменения
                        transaction.Rollback();
                        MessageBox.Show($"Произошла ошибка при удалении турнира: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                int tournamentId = Convert.ToInt32(selectedRow.Cells["TournamentID"].Value);
                string tournamentName = selectedRow.Cells["Название"].Value.ToString();
                DateTime startDate = Convert.ToDateTime(selectedRow.Cells["Дата начала"].Value);
                DateTime endDate = Convert.ToDateTime(selectedRow.Cells["Дата окончания"].Value);
                string gameType = selectedRow.Cells["Жанр игр"].Value.ToString();

                EditTournament editForm = new EditTournament(tournamentId, tournamentName, startDate, endDate, gameType);
                this.Visible = false;
                editForm.ShowDialog();
                this.Close();
                LoadFilteredData(); // Обновляем данные после редактирования
            }

            else
            {
                MessageBox.Show("Пожалуйста, выберите турнир для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
