using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Cybersport
{
    public partial class EditTournament : Form
    {
        private int tournamentId;
        private string connect = data.conStr;

        public EditTournament(int tournamentId, string tournamentName, DateTime startDate, DateTime endDate, string gameType)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
            LoadGameGenres();
            this.tournamentId = tournamentId;

            textBoxTournamentName.Text = tournamentName;
            dateTimePickerStartDate.Value = startDate;
            dateTimePickerEndDate.Value = endDate;
            comboBoxGameType.SelectedItem = gameType;

            dateTimePickerStartDate.Format = DateTimePickerFormat.Custom;
            dateTimePickerStartDate.CustomFormat = " dd.MM.yyyy HH:mm";
            dateTimePickerEndDate.Format = DateTimePickerFormat.Custom;
            dateTimePickerEndDate.CustomFormat = " dd.MM.yyyy HH:mm";
        }
        private void EditTournament_Load(object sender, EventArgs e)
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
                return "Завершенный";
            }
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

        private bool ValidateFields(string tournamentName, DateTime startDate, DateTime endDate, string gameType)
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

            return true;
        }

        private void buttonEditTournament_Click(object sender, EventArgs e)
        {
            string tournamentName = textBoxTournamentName.Text.Trim();
            DateTime startDate = dateTimePickerStartDate.Value;
            DateTime endDate = dateTimePickerEndDate.Value;
            string gameType = comboBoxGameType.SelectedItem?.ToString();

            if (ValidateFields(tournamentName, startDate, endDate, gameType))
            {
                SaveTournamentChanges(tournamentName, startDate, endDate, gameType);
            }
        }

        private bool IsTournamentNameExists(string tournamentName)
        {
            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM Tournaments WHERE TournamentName = @TournamentName AND TournamentID != @TournamentID", con);
                cmd.Parameters.AddWithValue("@TournamentName", tournamentName);
                cmd.Parameters.AddWithValue("@TournamentID", tournamentId);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private void SaveTournamentChanges(string tournamentName, DateTime startDate, DateTime endDate, string gameType)
        {
            using (MySqlConnection con = new MySqlConnection(connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(@"
                    UPDATE Tournaments 
                    SET TournamentName = @TournamentName, 
                        StartDate = @StartDate, 
                        EndDate = @EndDate, 
                        GameType = @GameType 
                    WHERE TournamentID = @TournamentID", con);

                cmd.Parameters.AddWithValue("@TournamentName", tournamentName);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                cmd.Parameters.AddWithValue("@GameType", gameType);
                cmd.Parameters.AddWithValue("@TournamentID", tournamentId);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Турнир успешно обновлён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;

                Tournaments_and_add_tournaments tournaments_And_Add_Tournaments = new Tournaments_and_add_tournaments();
                this.Visible = false;
                tournaments_And_Add_Tournaments.ShowDialog();
                this.Close();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Tournaments_and_add_tournaments tournaments_And_Add_Tournaments = new Tournaments_and_add_tournaments();
            this.Visible = false;
            tournaments_And_Add_Tournaments.ShowDialog();
            this.Close();
        }

        private void EditTournament_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
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
    }
}
