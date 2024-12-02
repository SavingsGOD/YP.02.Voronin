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
    public partial class AddTournament : Form
    {
        private string connect = data.conStr;
        public AddTournament()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
            LoadGameGenres();

            DateTime now = DateTime.Now;
            dateTimePickerStartDate.Value = now.AddHours(2);
            dateTimePickerEndDate.Value = now.AddDays(1);

            dateTimePickerStartDate.Format = DateTimePickerFormat.Custom;
            dateTimePickerStartDate.CustomFormat = " dd.MM.yyyy HH:mm";
            dateTimePickerEndDate.Format = DateTimePickerFormat.Custom;
            dateTimePickerEndDate.CustomFormat = " dd.MM.yyyy HH:mm";
        }
        private void LoadGameGenres()
        {
            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT GenreName FROM GameGenres", con);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    comboBoxGameType.Items.Add(reader["GenreName"].ToString());
                }
            }
        }

        private bool ValidateFields(string tournamentName, DateTime startDate, DateTime endDate, string gameType, string status)
        {
            if (string.IsNullOrEmpty(tournamentName) || string.IsNullOrEmpty(gameType))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (endDate < startDate)
            {
                MessageBox.Show("Дата окончания не может быть раньше даты начала.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (IsTournamentNameExists(tournamentName))
            {
                MessageBox.Show("Турнир с таким названием уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(status))
            {
                MessageBox.Show("Неверная дата турнира.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void buttonAddTournament_Click(object sender, EventArgs e)
        {
            string tournamentName = textBoxTournamentName.Text.Trim();
            DateTime startDate = dateTimePickerStartDate.Value;
            DateTime endDate = dateTimePickerEndDate.Value;
            string gameType = comboBoxGameType.SelectedItem?.ToString();
            string status = GetTournamentStatus(startDate, endDate);

            if (ValidateFields(tournamentName, startDate, endDate, gameType, status))
            {
                AddTournamentToDatabase(tournamentName, startDate, endDate, gameType, status);
            }
        }

        private bool IsTournamentNameExists(string tournamentName)
        {
            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM Tournaments WHERE TournamentName = @TournamentName", con);
                cmd.Parameters.AddWithValue("@TournamentName", tournamentName);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private void AddTournamentToDatabase(string tournamentName, DateTime startDate, DateTime endDate, string gameType, string status)
        {
            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(@"
                    INSERT INTO Tournaments (TournamentName, StartDate, EndDate, GameType)
                    VALUES (@TournamentName, @StartDate, @EndDate, @GameType)", con);
                cmd.Parameters.AddWithValue("@TournamentName", tournamentName);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                cmd.Parameters.AddWithValue("@GameType", gameType);
                cmd.Parameters.AddWithValue("@Stutus", status);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Турнир успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;

                Tournaments_and_add_tournaments tournaments_And_Add_Tournaments = new Tournaments_and_add_tournaments();
                this.Visible = false;
                tournaments_And_Add_Tournaments.ShowDialog();
                this.Close();
            }
        }

        private void AddTournament_Load(object sender, EventArgs e)
        {

        }
        private string GetTournamentStatus(DateTime startDate, DateTime endDate)
        {
            DateTime now = DateTime.Now;

            if (startDate > now)
            {
                return "Предстоящий";
            }
            else if (startDate <= now && endDate >= now)
            {
                return "Текущий";
            }
            else
            {
                return " ";
            }
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Tournaments_and_add_tournaments tournaments_And_Add_Tournaments = new Tournaments_and_add_tournaments();
            this.Visible = false;
            tournaments_And_Add_Tournaments.ShowDialog();
            this.Close();
        }

        private void dateTimePickerStartDate_ValueChanged(object sender, EventArgs e)
        {
            DateTime startDate = dateTimePickerStartDate.Value;
            DateTime endDate = dateTimePickerEndDate.Value;
            string status = GetTournamentStatus(startDate, endDate);

            textBox6.Text = status;
        }

        private void dateTimePickerEndDate_ValueChanged(object sender, EventArgs e)
        {
            DateTime startDate = dateTimePickerStartDate.Value;
            DateTime endDate = dateTimePickerEndDate.Value;
            string status = GetTournamentStatus(startDate, endDate);

            textBox6.Text = status;
        }

        private void AddTournament_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
        bool IsValidLoginCharacter(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   (c >= 'А' && c <= 'Я') ||
                   (c >= 'а' && c <= 'я') ||
                   (c == 32) ||
                   (c >= '0' && c <= '9');
        }

        private void textBoxTournamentName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidLoginCharacter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
