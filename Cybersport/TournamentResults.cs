using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Cybersport
{
    public partial class TournamentResults : Form
    {
        private string tournamentName;
        private int tournamentId;

        public TournamentResults(int tournamentId)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
            this.tournamentId = tournamentId;
        }

        private void TournamentResults_Load(object sender, EventArgs e)
        {
            if (data.role == "Администратор")
            {
                button2.Visible = true;
            }
            if (data.role == "Участник")
            {
                button2.Visible = false;
            }

            dataGridView1.ClearSelection();
            LoadTournamentResults();
        }

        private void LoadTournamentResults()
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ClearSelection();
            List<int> userTeamIds = GetUserTeamIds(); // Получаем идентификаторы команд пользователя

            string connectionString = data.conStr;

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = @"
        SELECT 
            mr.MatchID AS MatchID,
            t.TournamentName AS 'Название турнира',
            ta.TeamID AS TeamAID, 
            ta.TeamName AS 'Команда А',
            tb.TeamID AS TeamBID, 
            tb.TeamName AS 'Команда Б',
            mr.ScoreTeamA AS 'Очки команды А',
            mr.ScoreTeamB AS 'Очки команды Б',
            mr.MatchDate AS 'Дата матча',
            CASE 
                WHEN mr.Winner = mr.TeamAID THEN ta.TeamName 
                WHEN mr.Winner = mr.TeamBID THEN tb.TeamName 
                ELSE 'No Winner' 
            END AS Победитель
        FROM 
            MatchResults mr
        INNER JOIN 
            Tournaments t ON mr.TournamentID = t.TournamentID
        INNER JOIN 
            Teams ta ON mr.TeamAID = ta.TeamID
        INNER JOIN 
            Teams tb ON mr.TeamBID = tb.TeamID
        WHERE 
            mr.TournamentID = @TournamentID";

                MySqlCommand command = new MySqlCommand(query, con);
                command.Parameters.AddWithValue("@TournamentID", tournamentId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable();

                try
                {
                    con.Open();
                    adapter.Fill(dt);
                    dataGridView1.ClearSelection();
                    dataGridView1.DataSource = dt;

                    // Сохраняем название турнира для дальнейшего использования
                    if (dt.Rows.Count > 0)
                    {
                        tournamentName = dt.Rows[0]["Название турнира"].ToString();
                    }

                    // Выделяем строки с командами, в которых зарегистрирован пользователь
                    foreach (DataRow row in dt.Rows)
                    {
                        int teamAID = Convert.ToInt32(row["TeamAID"]);
                        int teamBID = Convert.ToInt32(row["TeamBID"]);

                        if (userTeamIds.Contains(teamAID) || userTeamIds.Contains(teamBID))
                        {
                            int rowIndex = dt.Rows.IndexOf(row);
                            dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 153);
                            dataGridView1.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.White;
                        }
                    }

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                    // Убираем выделение с первой строки
                    if (dataGridView1.Rows.Count > 0)
                    {
                        dataGridView1.Rows[0].Selected = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
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

        private void TournamentResults_FormClosing(object sender, FormClosingEventArgs e)
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
            this.Visible = false;
            Tournaments tournaments = new Tournaments();
            tournaments.ShowDialog();
            this.Close();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.ClearSelection();
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[4].Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Word Document|*.docx";
                saveFileDialog.Title = "Сохранить отчет о турнире";
                saveFileDialog.FileName = "Отчет результатов турнира";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var wordApp = new Microsoft.Office.Interop.Word.Application();
                    var document = wordApp.Documents.Add();

                    // Добавляем заголовок с названием турнира
                    var titleRange = document.Content;
                    titleRange.Text = $"Отчет {tournamentName}"; // Добавляем заголовок
                    titleRange.Font.Size = 11;
                    titleRange.Font.Bold = 1; // Установить жирный шрифт
                    titleRange.Paragraphs.Add(); // Добавляем новый параграф для отступа
                    titleRange.Paragraphs[1].Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    // Сжимаем диапазон, чтобы перейти к следующей строке после заголовка
                    titleRange.Collapse(Microsoft.Office.Interop.Word.WdCollapseDirection.wdCollapseEnd);

                    // Добавляем таблицу под заголовком
                    var dataTable = document.Tables.Add(titleRange, dataGridView1.Rows.Count + 1, dataGridView1.Columns.Count - 3); // Уменьшаем количество столбцов

                    // Заполняем заголовки таблицы
                    int columnIndex = 0;
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        if (dataGridView1.Columns[i].Name != "MatchID" && dataGridView1.Columns[i].Name != "TeamAID" && dataGridView1.Columns[i].Name != "TeamBID")
                        {
                            dataTable.Cell(1, columnIndex + 1).Range.Text = dataGridView1.Columns[i].HeaderText;
                            columnIndex++;
                        }
                    }

                    // Заполняем данные таблицы
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        columnIndex = 0; // Сбрасываем индекс столбца для каждой строки
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            if (dataGridView1.Columns[j].Name != "MatchID" && dataGridView1.Columns[j].Name != "TeamAID" && dataGridView1.Columns[j].Name != "TeamBID")
                            {
                                var cell = dataTable.Cell(i + 2, columnIndex + 1);
                                cell.Range.Text = dataGridView1.Rows[i].Cells[j].Value?.ToString();

                                cell.Range.Font.Size = 10;
                                cell.Range.Font.Bold = 0;

                                columnIndex++;
                            }
                        }
                    }

                    // Форматируем таблицу
                    dataTable.Borders.Enable = 1; // Включаем границы
                    dataTable.AutoFitBehavior(Microsoft.Office.Interop.Word.WdAutoFitBehavior.wdAutoFitContent); // Авто подгонка
                    document.SaveAs(saveFileDialog.FileName);
                    document.Close();
                    wordApp.Quit();

                    MessageBox.Show("Отчет успешно создан по адресу: " + saveFileDialog.FileName, "Отчет", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при создании отчета: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }
    }
}
