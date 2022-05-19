
namespace SP_Generator
{
    partial class Home
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
            this.SQL = new System.Windows.Forms.Button();
            this.MySQL = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SQL
            // 
            this.SQL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SQL.Location = new System.Drawing.Point(148, 135);
            this.SQL.Name = "SQL";
            this.SQL.Size = new System.Drawing.Size(205, 46);
            this.SQL.TabIndex = 0;
            this.SQL.Text = "SQL";
            this.SQL.UseVisualStyleBackColor = true;
            this.SQL.Click += new System.EventHandler(this.SQL_Click);
            // 
            // MySQL
            // 
            this.MySQL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MySQL.Location = new System.Drawing.Point(407, 135);
            this.MySQL.Name = "MySQL";
            this.MySQL.Size = new System.Drawing.Size(205, 46);
            this.MySQL.TabIndex = 1;
            this.MySQL.Text = "MySQL";
            this.MySQL.UseVisualStyleBackColor = true;
            this.MySQL.Click += new System.EventHandler(this.MySQL_Click);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 349);
            this.ControlBox = false;
            this.Controls.Add(this.MySQL);
            this.Controls.Add(this.SQL);
            this.Name = "Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SQL;
        private System.Windows.Forms.Button MySQL;
    }
}