namespace LearnSmartGmbHEditor
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.txtMail = new System.Windows.Forms.TextBox();
            this.txtInt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSaveMail = new System.Windows.Forms.Button();
            this.btnSaveLong = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 19;
            this.listBox1.Location = new System.Drawing.Point(-1, 144);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(494, 270);
            this.listBox1.TabIndex = 0;
            // 
            // txtMail
            // 
            this.txtMail.Location = new System.Drawing.Point(12, 57);
            this.txtMail.Name = "txtMail";
            this.txtMail.Size = new System.Drawing.Size(131, 20);
            this.txtMail.TabIndex = 1;
            // 
            // txtInt
            // 
            this.txtInt.Location = new System.Drawing.Point(186, 57);
            this.txtInt.Name = "txtInt";
            this.txtInt.Size = new System.Drawing.Size(131, 20);
            this.txtInt.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Email-Adresse:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(183, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Service-Intervall in ms:";
            // 
            // btnSaveMail
            // 
            this.btnSaveMail.Location = new System.Drawing.Point(15, 95);
            this.btnSaveMail.Name = "btnSaveMail";
            this.btnSaveMail.Size = new System.Drawing.Size(75, 23);
            this.btnSaveMail.TabIndex = 5;
            this.btnSaveMail.Text = "speichern";
            this.btnSaveMail.UseVisualStyleBackColor = true;
            this.btnSaveMail.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSaveLong
            // 
            this.btnSaveLong.Location = new System.Drawing.Point(186, 95);
            this.btnSaveLong.Name = "btnSaveLong";
            this.btnSaveLong.Size = new System.Drawing.Size(75, 23);
            this.btnSaveLong.TabIndex = 6;
            this.btnSaveLong.Text = "speichern";
            this.btnSaveLong.UseVisualStyleBackColor = true;
            this.btnSaveLong.Click += new System.EventHandler(this.btnSaveLong_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 415);
            this.Controls.Add(this.btnSaveLong);
            this.Controls.Add(this.btnSaveMail);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInt);
            this.Controls.Add(this.txtMail);
            this.Controls.Add(this.listBox1);
            this.Name = "Form1";
            this.Text = "Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox txtMail;
        private System.Windows.Forms.TextBox txtInt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSaveMail;
        private System.Windows.Forms.Button btnSaveLong;
    }
}

