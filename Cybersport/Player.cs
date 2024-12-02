using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Cybersport
{
    public partial class Player : Form
    {
        public Player()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Запретить изменение размера
            this.MaximizeBox = false; // Запретить кнопку максимизации
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Tournaments tournaments = new Tournaments();
            this.Visible = false;
            tournaments.ShowDialog();
            this.Close();
        }

        private void Player_Load(object sender, EventArgs e)
        {
            label2.Text = data.usrName;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Authorization authorization = new Authorization();
            this.Visible = false;
            authorization.ShowDialog();
            this.Close();
        }

        private void Player_FormClosing(object sender, FormClosingEventArgs e)
        {
       
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Teams teams = new Teams();
            teams.ShowDialog();
            this.Close();
        }
    }
}
