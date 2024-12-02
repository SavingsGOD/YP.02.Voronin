namespace Cybersport
{
    partial class AddUser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.fioTextBox = new System.Windows.Forms.TextBox();
            this.roleComboBox = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Логин";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Пароль";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 26);
            this.label3.TabIndex = 2;
            this.label3.Text = "Email";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 26);
            this.label4.TabIndex = 3;
            this.label4.Text = "ФИО";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 248);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 26);
            this.label5.TabIndex = 4;
            this.label5.Text = "Телефон";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 288);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 26);
            this.label6.TabIndex = 5;
            this.label6.Text = "Роль";
            // 
            // button4
            // 
            this.button4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(102)))), ((int)(((byte)(0)))));
            this.button4.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button4.Location = new System.Drawing.Point(12, 383);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(269, 36);
            this.button4.TabIndex = 54;
            this.button4.Text = "Назад";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(461, 383);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(269, 36);
            this.button2.TabIndex = 53;
            this.button2.Text = "Подтвердить";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Location = new System.Drawing.Point(290, 39);
            this.usernameTextBox.MaxLength = 20;
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(442, 34);
            this.usernameTextBox.TabIndex = 55;
            this.usernameTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.usernameTextBox_KeyPress);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(290, 79);
            this.passwordTextBox.MaxLength = 20;
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(442, 34);
            this.passwordTextBox.TabIndex = 56;
            this.passwordTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.passwordTextBox_KeyPress);
            // 
            // emailTextBox
            // 
            this.emailTextBox.Location = new System.Drawing.Point(288, 159);
            this.emailTextBox.MaxLength = 30;
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(271, 34);
            this.emailTextBox.TabIndex = 57;
            this.emailTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.emailTextBox_KeyPress);
            // 
            // fioTextBox
            // 
            this.fioTextBox.Location = new System.Drawing.Point(288, 119);
            this.fioTextBox.MaxLength = 30;
            this.fioTextBox.Name = "fioTextBox";
            this.fioTextBox.Size = new System.Drawing.Size(442, 34);
            this.fioTextBox.TabIndex = 58;
            this.fioTextBox.TextChanged += new System.EventHandler(this.fioTextBox_TextChanged);
            this.fioTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fioTextBox_KeyPress);
            // 
            // roleComboBox
            // 
            this.roleComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.roleComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.roleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.roleComboBox.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.roleComboBox.FormattingEnabled = true;
            this.roleComboBox.Location = new System.Drawing.Point(290, 288);
            this.roleComboBox.MaxLength = 30;
            this.roleComboBox.Name = "roleComboBox";
            this.roleComboBox.Size = new System.Drawing.Size(232, 34);
            this.roleComboBox.TabIndex = 52;
            // 
            // comboBox1
            // 
            this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(556, 159);
            this.comboBox1.MaxLength = 30;
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(174, 34);
            this.comboBox1.TabIndex = 60;
            // 
            // maskedTextBox1
            // 
            this.maskedTextBox1.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.maskedTextBox1.Location = new System.Drawing.Point(290, 248);
            this.maskedTextBox1.Mask = "+7 (000) 000-00-00";
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.Size = new System.Drawing.Size(232, 34);
            this.maskedTextBox1.TabIndex = 61;
            // 
            // AddUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 26F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(742, 431);
            this.Controls.Add(this.maskedTextBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.fioTextBox);
            this.Controls.Add(this.emailTextBox);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.roleComboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "AddUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Добавление пользователя";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddUser_FormClosing);
            this.Load += new System.EventHandler(this.AddUser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.TextBox fioTextBox;
        private System.Windows.Forms.ComboBox roleComboBox;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.MaskedTextBox maskedTextBox1;
    }
}