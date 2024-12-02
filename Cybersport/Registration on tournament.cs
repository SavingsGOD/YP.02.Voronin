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
    public partial class Registration_on_tournament : Form
    {
        public int TournamentID { get; set; }
        public Registration_on_tournament()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Tournaments tournaments = new Tournaments();
            this.Visible = false;
            tournaments.ShowDialog();
            this.Close();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidLoginCharacter1(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        bool IsValidLoginCharacter1(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   (c >= 'а' && c <= 'я') ||
                   (c >= 'А' && c <= 'Я') ||
                   (c == 32) ||
                   (c >= '0' && c <= '9');
        }
        bool IsValidLoginCharacter(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   (c >= '0' && c <= '9');
        }

        private void Registration_on_tournament_Load(object sender, EventArgs e)
        {
            label14.Text = data.Login;
            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = data.conStr;

                con.Open();

                MySqlCommand cmd = new MySqlCommand(@"SELECT Users.Username, Users.Role
                FROM Users
                WHERE Users.Role = 'Участник'

;", con);

                IDataReader dataReader = cmd.ExecuteReader();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                while (dataReader.Read())
                {
                    comboBox1.Items.Add(dataReader.GetValue(0).ToString());
                    comboBox2.Items.Add(dataReader.GetValue(0).ToString());
                    comboBox3.Items.Add(dataReader.GetValue(0).ToString());
                    comboBox4.Items.Add(dataReader.GetValue(0).ToString());
                }
            }
        }
        public void SetTournamentDetails(string tournamentName, DateTime startDate, DateTime endDate, string captain)
        {
            label11.Text = tournamentName;
            label12.Text = startDate.ToString("g");
            label13.Text = endDate.ToString("g");
            label14.Text = captain;
        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            int tournamentId = this.TournamentID;  // Используем переданный TournamentID

            string teamName = textBox4.Text;

            if (string.IsNullOrWhiteSpace(teamName))
            {
                MessageBox.Show("Пожалуйста, введите имя команды.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (IsTeamNameExists(teamName, tournamentId))
            {
                MessageBox.Show("Команда с таким именем уже существует на данном турнире. Пожалуйста, выберите другое имя.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || comboBox3.SelectedItem == null || comboBox4.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите всех участников команды.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string captainUsername = label14.Text = data.Login;
            int captainId = GetUserIdByUsername(captainUsername);

            if (IsUserRegisteredForTournament(tournamentId, captainId))
            {
                MessageBox.Show("Вы уже зарегистрированы на данный турнир.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            HashSet<string> selectedPlayers = new HashSet<string>();
            foreach (ComboBox comboBox in new ComboBox[] { comboBox1, comboBox2, comboBox3, comboBox4 })
            {
                if (comboBox.SelectedItem != null && !string.IsNullOrWhiteSpace(comboBox.SelectedItem.ToString()))
                {
                    string playerName = comboBox.SelectedItem.ToString();

                    if (playerName.Equals(captainUsername, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Капитан не может быть участником команды.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    int playerId = GetUserIdByUsername(playerName);
                    if (IsUserRegisteredForTournament(tournamentId, playerId))
                    {
                        MessageBox.Show($"Игрок {playerName} уже зарегистрирован на данный турнир.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!selectedPlayers.Add(playerName))
                    {
                        MessageBox.Show($"Участник {playerName} уже добавлен в команду.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            using (MySqlConnection con = new MySqlConnection(data.conStr))
            {
                try
                {
                    con.Open();

                    // Добавление команды в таблицу Teams
                    MySqlCommand addTeamCmd = new MySqlCommand("INSERT INTO Teams (TeamName, CaptainID) VALUES (@TeamName, @CaptainID);", con);
                    addTeamCmd.Parameters.AddWithValue("@TeamName", teamName);
                    addTeamCmd.Parameters.AddWithValue("@CaptainID", captainId);
                    addTeamCmd.ExecuteNonQuery();

                    int teamId = (int)addTeamCmd.LastInsertedId;

                    // Регистрация команды на турнире
                    MySqlCommand registerTeamCmd = new MySqlCommand("INSERT INTO TournamentRegistrations (TournamentID, TeamID, CaptainID) VALUES (@TournamentID, @TeamID, @CaptainID);", con);
                    registerTeamCmd.Parameters.AddWithValue("@TournamentID", tournamentId);
                    registerTeamCmd.Parameters.AddWithValue("@TeamID", teamId);
                    registerTeamCmd.Parameters.AddWithValue("@CaptainID", captainId);
                    registerTeamCmd.ExecuteNonQuery();

                    // Добавление команды в TournamentsTeams
                    MySqlCommand teamsCmd = new MySqlCommand("INSERT INTO TournamentsTeams (idTournaments, idTeam) VALUES (@TournamentID, @TeamID);", con);
                    teamsCmd.Parameters.AddWithValue("@TournamentID", tournamentId);
                    teamsCmd.Parameters.AddWithValue("@TeamID", teamId);
                    teamsCmd.ExecuteNonQuery();

                    // Добавление игроков в Participants
                    AddPlayersToTeam(con, teamId);

                    MessageBox.Show("Команда успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Tournaments tournaments = new Tournaments();
                    this.Visible = false;
                    tournaments.ShowDialog();
                    this.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Произошла ошибка при добавлении команды в базу данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла непредвиденная ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool IsTeamNameExists(string teamName, int tournamentId)
        {
            using (MySqlConnection con = new MySqlConnection(data.conStr))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(@"
            SELECT COUNT(*) 
            FROM Teams t
            JOIN TournamentRegistrations tr ON t.TeamID = tr.TeamID
            WHERE t.TeamName = @TeamName AND tr.TournamentID = @TournamentID;", con);
                cmd.Parameters.AddWithValue("@TeamName", teamName);
                cmd.Parameters.AddWithValue("@TournamentID", tournamentId);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0; // Если count больше 0, значит команда с таким именем уже существует на данном турнире
            }
        }

        private bool IsUserRegisteredForTournament(int tournamentId, int userId)
        {
            using (MySqlConnection con = new MySqlConnection(data.conStr))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(@"
        SELECT COUNT(*) FROM TournamentRegistrations tr
        JOIN Teams t ON tr.TeamID = t.TeamID
        WHERE tr.TournamentID = @TournamentID AND (tr.CaptainID = @UserID OR t.TeamID IN (SELECT TeamID FROM Participants WHERE UserID = @UserID));
    ", con);
                cmd.Parameters.AddWithValue("@TournamentID", tournamentId);
                cmd.Parameters.AddWithValue("@UserID", userId);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0; // Если count > 0, значит пользователь уже зарегистрирован на турнире
            }
        }

        // Метод для очистки полей 
        private void ClearFormFields()
        {
            textBox4.Clear();
            comboBox1.SelectedItem = null;
            comboBox2.SelectedItem = null;
            comboBox3.SelectedItem = null;
            comboBox4.SelectedItem = null;
        }

        private int GetUserIdByUsername(string username)
        {
            using (MySqlConnection con = new MySqlConnection(data.conStr))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT UserID FROM Users WHERE Username = @Username;", con);
                cmd.Parameters.AddWithValue("@Username", username);

                var result = cmd.ExecuteScalar();
                if (result == null)
                {
                    MessageBox.Show("Пользователь не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1; // Возвращаем -1, чтобы указать, что пользователь не найден
                }

                return Convert.ToInt32(result);
            }
        }

        // Метод для добавления игроков в команду
        private void AddPlayersToTeam(MySqlConnection con, int teamId)
        {
            foreach (ComboBox comboBox in new ComboBox[] { comboBox1, comboBox2, comboBox3, comboBox4 })
            {
                if (comboBox.SelectedItem != null && !string.IsNullOrWhiteSpace(comboBox.SelectedItem.ToString()))
                {
                    int playerId = GetUserIdByUsername(comboBox.SelectedItem.ToString());
                    if (playerId != -1) // Проверка на существование пользователя
                    {
                        MySqlCommand addPlayerCmd = new MySqlCommand("INSERT INTO Participants (UserID, TeamID) VALUES (@UserID, @TeamID);", con);
                        addPlayerCmd.Parameters.AddWithValue("@UserID", playerId);
                        addPlayerCmd.Parameters.AddWithValue("@TeamID", teamId);
                        addPlayerCmd.ExecuteNonQuery();
                    }
                }
            }
        }
        private void Registration_on_tournament_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidLoginCharacter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidLoginCharacter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidLoginCharacter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void comboBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsValidLoginCharacter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
