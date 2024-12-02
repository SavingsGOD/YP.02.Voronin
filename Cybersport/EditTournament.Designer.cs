namespace Cybersport
{
    partial class EditTournament
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
            this.dateTimePickerStartDate = new System.Windows.Forms.DateTimePicker();
            this.labelGameType = new System.Windows.Forms.Label();
            this.labelEndDate = new System.Windows.Forms.Label();
            this.labelStartDate = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonEditTournament = new System.Windows.Forms.Button();
            this.comboBoxGameType = new System.Windows.Forms.ComboBox();
            this.dateTimePickerEndDate = new System.Windows.Forms.DateTimePicker();
            this.textBoxTournamentName = new System.Windows.Forms.TextBox();
            this.labelTournamentName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // dateTimePickerStartDate
            // 
            this.dateTimePickerStartDate.CalendarFont = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePickerStartDate.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePickerStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerStartDate.Location = new System.Drawing.Point(269, 88);
            this.dateTimePickerStartDate.MaxDate = new System.DateTime(2025, 12, 31, 0, 0, 0, 0);
            this.dateTimePickerStartDate.MinDate = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            this.dateTimePickerStartDate.Name = "dateTimePickerStartDate";
            this.dateTimePickerStartDate.Size = new System.Drawing.Size(299, 34);
            this.dateTimePickerStartDate.TabIndex = 70;
            this.dateTimePickerStartDate.ValueChanged += new System.EventHandler(this.dateTimePickerStartDate_ValueChanged);
            // 
            // labelGameType
            // 
            this.labelGameType.AutoSize = true;
            this.labelGameType.Location = new System.Drawing.Point(10, 197);
            this.labelGameType.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelGameType.Name = "labelGameType";
            this.labelGameType.Size = new System.Drawing.Size(111, 26);
            this.labelGameType.TabIndex = 69;
            this.labelGameType.Text = "Жанр игры";
            // 
            // labelEndDate
            // 
            this.labelEndDate.AutoSize = true;
            this.labelEndDate.Location = new System.Drawing.Point(10, 128);
            this.labelEndDate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelEndDate.Name = "labelEndDate";
            this.labelEndDate.Size = new System.Drawing.Size(157, 26);
            this.labelEndDate.TabIndex = 68;
            this.labelEndDate.Text = "Дата окончания";
            // 
            // labelStartDate
            // 
            this.labelStartDate.AutoSize = true;
            this.labelStartDate.Location = new System.Drawing.Point(10, 88);
            this.labelStartDate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelStartDate.Name = "labelStartDate";
            this.labelStartDate.Size = new System.Drawing.Size(125, 26);
            this.labelStartDate.TabIndex = 67;
            this.labelStartDate.Text = "Дата начала";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(102)))), ((int)(((byte)(0)))));
            this.buttonCancel.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCancel.Location = new System.Drawing.Point(15, 309);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(269, 36);
            this.buttonCancel.TabIndex = 66;
            this.buttonCancel.Text = "Назад";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonEditTournament
            // 
            this.buttonEditTournament.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonEditTournament.BackColor = System.Drawing.Color.White;
            this.buttonEditTournament.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonEditTournament.Location = new System.Drawing.Point(453, 309);
            this.buttonEditTournament.Name = "buttonEditTournament";
            this.buttonEditTournament.Size = new System.Drawing.Size(269, 36);
            this.buttonEditTournament.TabIndex = 65;
            this.buttonEditTournament.Text = "Подтвердить";
            this.buttonEditTournament.UseVisualStyleBackColor = false;
            this.buttonEditTournament.Click += new System.EventHandler(this.buttonEditTournament_Click);
            // 
            // comboBoxGameType
            // 
            this.comboBoxGameType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGameType.FormattingEnabled = true;
            this.comboBoxGameType.Location = new System.Drawing.Point(269, 197);
            this.comboBoxGameType.Name = "comboBoxGameType";
            this.comboBoxGameType.Size = new System.Drawing.Size(247, 34);
            this.comboBoxGameType.TabIndex = 64;
            // 
            // dateTimePickerEndDate
            // 
            this.dateTimePickerEndDate.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePickerEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEndDate.Location = new System.Drawing.Point(269, 128);
            this.dateTimePickerEndDate.Name = "dateTimePickerEndDate";
            this.dateTimePickerEndDate.Size = new System.Drawing.Size(299, 34);
            this.dateTimePickerEndDate.TabIndex = 63;
            this.dateTimePickerEndDate.ValueChanged += new System.EventHandler(this.dateTimePickerEndDate_ValueChanged);
            // 
            // textBoxTournamentName
            // 
            this.textBoxTournamentName.Location = new System.Drawing.Point(269, 19);
            this.textBoxTournamentName.MaxLength = 40;
            this.textBoxTournamentName.Name = "textBoxTournamentName";
            this.textBoxTournamentName.Size = new System.Drawing.Size(453, 34);
            this.textBoxTournamentName.TabIndex = 62;
            // 
            // labelTournamentName
            // 
            this.labelTournamentName.AutoSize = true;
            this.labelTournamentName.Location = new System.Drawing.Point(10, 19);
            this.labelTournamentName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelTournamentName.Name = "labelTournamentName";
            this.labelTournamentName.Size = new System.Drawing.Size(176, 26);
            this.labelTournamentName.TabIndex = 61;
            this.labelTournamentName.Text = "Название турнира";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 237);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 26);
            this.label1.TabIndex = 71;
            this.label1.Text = "Статус";
            // 
            // textBox6
            // 
            this.textBox6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBox6.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox6.Location = new System.Drawing.Point(269, 237);
            this.textBox6.MaxLength = 15;
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(247, 34);
            this.textBox6.TabIndex = 72;
            this.textBox6.Text = "Предстоящий";
            this.textBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // EditTournament
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 26F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(733, 361);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePickerStartDate);
            this.Controls.Add(this.labelGameType);
            this.Controls.Add(this.labelEndDate);
            this.Controls.Add(this.labelStartDate);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonEditTournament);
            this.Controls.Add(this.comboBoxGameType);
            this.Controls.Add(this.dateTimePickerEndDate);
            this.Controls.Add(this.textBoxTournamentName);
            this.Controls.Add(this.labelTournamentName);
            this.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "EditTournament";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактирование турнира";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditTournament_FormClosing);
            this.Load += new System.EventHandler(this.EditTournament_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DateTimePicker dateTimePickerStartDate;
        private System.Windows.Forms.Label labelGameType;
        private System.Windows.Forms.Label labelEndDate;
        private System.Windows.Forms.Label labelStartDate;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonEditTournament;
        private System.Windows.Forms.ComboBox comboBoxGameType;
        private System.Windows.Forms.DateTimePicker dateTimePickerEndDate;
        private System.Windows.Forms.TextBox textBoxTournamentName;
        private System.Windows.Forms.Label labelTournamentName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox6;
    }
}