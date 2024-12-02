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
    public partial class Manager : Form
    {
        public Manager()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Tournaments_and_add_tournaments tournaments = new Tournaments_and_add_tournaments();
            this.Visible = false;
            tournaments.ShowDialog();
            this.Close();
        }

        private void Manager_Load(object sender, EventArgs e)
        {
            label2.Text = data.usrName;
        }

        private void Manager_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Authorization authorization = new Authorization();
            this.Visible = false;
            authorization.ShowDialog();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
