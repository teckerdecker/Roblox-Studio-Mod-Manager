namespace RobloxModManager
{
    partial class Launcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Launcher));
            this.launchStudio = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.manageMods = new System.Windows.Forms.Button();
            this.dataBaseSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // launchStudio
            // 
            this.launchStudio.Cursor = System.Windows.Forms.Cursors.Default;
            this.launchStudio.Location = new System.Drawing.Point(53, 160);
            this.launchStudio.Name = "launchStudio";
            this.launchStudio.Size = new System.Drawing.Size(155, 23);
            this.launchStudio.TabIndex = 6;
            this.launchStudio.Text = "Launch Roblox Studio";
            this.launchStudio.UseVisualStyleBackColor = true;
            this.launchStudio.Click += new System.EventHandler(this.launchStudio_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(53, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(155, 142);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // manageMods
            // 
            this.manageMods.Cursor = System.Windows.Forms.Cursors.Default;
            this.manageMods.Location = new System.Drawing.Point(53, 189);
            this.manageMods.Name = "manageMods";
            this.manageMods.Size = new System.Drawing.Size(155, 23);
            this.manageMods.TabIndex = 9;
            this.manageMods.Text = "Manage Mod Files";
            this.manageMods.UseVisualStyleBackColor = true;
            this.manageMods.Click += new System.EventHandler(this.manageMods_Click);
            // 
            // dataBaseSelect
            // 
            this.dataBaseSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataBaseSelect.FormattingEnabled = true;
            this.dataBaseSelect.Items.AddRange(new object[] {
            "roblox",
            "gametest1.robloxlabs",
            "gametest2.robloxlabs",
            "gametest3.robloxlabs",
            "gametest4.robloxlabs",
            "gametest5.robloxlabs"});
            this.dataBaseSelect.Location = new System.Drawing.Point(53, 231);
            this.dataBaseSelect.Name = "dataBaseSelect";
            this.dataBaseSelect.Size = new System.Drawing.Size(155, 21);
            this.dataBaseSelect.TabIndex = 10;
            this.dataBaseSelect.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.CausesValidation = false;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(50, 215);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Database to use: ";
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DodgerBlue;
            this.ClientSize = new System.Drawing.Size(259, 279);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataBaseSelect);
            this.Controls.Add(this.manageMods);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.launchStudio);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Launcher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RbxStudio Mod Manager";
            this.Load += new System.EventHandler(this.Launcher_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button launchStudio;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button manageMods;
        private System.Windows.Forms.ComboBox dataBaseSelect;
        private System.Windows.Forms.Label label1;
    }
}

