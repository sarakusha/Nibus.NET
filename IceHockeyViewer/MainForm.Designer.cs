namespace IceHockeyViewer
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.labelHomeScore = new System.Windows.Forms.Label();
            this.labelVisitorScore = new System.Windows.Forms.Label();
            this.labelPrimaryTimer = new System.Windows.Forms.Label();
            this.labelSecondaryTimer = new System.Windows.Forms.Label();
            this.listBoxHomePenalty = new System.Windows.Forms.ListBox();
            this.listBoxVisitorPenalty = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonSynchronize = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelHomeScore
            // 
            this.labelHomeScore.AutoSize = true;
            this.labelHomeScore.Font = new System.Drawing.Font("Arial", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelHomeScore.Location = new System.Drawing.Point(70, 32);
            this.labelHomeScore.Name = "labelHomeScore";
            this.labelHomeScore.Size = new System.Drawing.Size(41, 44);
            this.labelHomeScore.TabIndex = 1;
            this.labelHomeScore.Text = "0";
            // 
            // labelVisitorScore
            // 
            this.labelVisitorScore.AutoSize = true;
            this.labelVisitorScore.Font = new System.Drawing.Font("Arial", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelVisitorScore.Location = new System.Drawing.Point(306, 32);
            this.labelVisitorScore.Name = "labelVisitorScore";
            this.labelVisitorScore.Size = new System.Drawing.Size(41, 44);
            this.labelVisitorScore.TabIndex = 2;
            this.labelVisitorScore.Text = "0";
            // 
            // labelPrimaryTimer
            // 
            this.labelPrimaryTimer.AutoSize = true;
            this.labelPrimaryTimer.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelPrimaryTimer.Location = new System.Drawing.Point(164, 92);
            this.labelPrimaryTimer.Name = "labelPrimaryTimer";
            this.labelPrimaryTimer.Size = new System.Drawing.Size(85, 32);
            this.labelPrimaryTimer.TabIndex = 3;
            this.labelPrimaryTimer.Text = "00:00";
            // 
            // labelSecondaryTimer
            // 
            this.labelSecondaryTimer.AutoSize = true;
            this.labelSecondaryTimer.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSecondaryTimer.Location = new System.Drawing.Point(178, 124);
            this.labelSecondaryTimer.Name = "labelSecondaryTimer";
            this.labelSecondaryTimer.Size = new System.Drawing.Size(0, 22);
            this.labelSecondaryTimer.TabIndex = 4;
            // 
            // listBoxHomePenalty
            // 
            this.listBoxHomePenalty.FormattingEnabled = true;
            this.listBoxHomePenalty.Location = new System.Drawing.Point(25, 157);
            this.listBoxHomePenalty.Name = "listBoxHomePenalty";
            this.listBoxHomePenalty.Size = new System.Drawing.Size(120, 95);
            this.listBoxHomePenalty.TabIndex = 5;
            // 
            // listBoxVisitorPenalty
            // 
            this.listBoxVisitorPenalty.FormattingEnabled = true;
            this.listBoxVisitorPenalty.Location = new System.Drawing.Point(265, 157);
            this.listBoxVisitorPenalty.Name = "listBoxVisitorPenalty";
            this.listBoxVisitorPenalty.Size = new System.Drawing.Size(120, 95);
            this.listBoxVisitorPenalty.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Удаления Хозяева";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(265, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Удаления Гости";
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(138, 264);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 9;
            this.buttonLoad.Text = "Загрузить";
            this.toolTip1.SetToolTip(this.buttonLoad, "Загрузить с пульта продолжительность таймеров");
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.OnLoadProviderClick);
            // 
            // buttonSynchronize
            // 
            this.buttonSynchronize.Location = new System.Drawing.Point(219, 264);
            this.buttonSynchronize.Name = "buttonSynchronize";
            this.buttonSynchronize.Size = new System.Drawing.Size(75, 23);
            this.buttonSynchronize.TabIndex = 10;
            this.buttonSynchronize.Text = "Обновить";
            this.toolTip1.SetToolTip(this.buttonSynchronize, "Обновить всю информацию");
            this.buttonSynchronize.UseVisualStyleBackColor = true;
            this.buttonSynchronize.Click += new System.EventHandler(this.OnSynchronizeClick);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(310, 264);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "Закрыть";
            this.toolTip1.SetToolTip(this.button3, "Закрыть программу");
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OnCloseClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 299);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.buttonSynchronize);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxVisitorPenalty);
            this.Controls.Add(this.listBoxHomePenalty);
            this.Controls.Add(this.labelSecondaryTimer);
            this.Controls.Add(this.labelPrimaryTimer);
            this.Controls.Add(this.labelVisitorScore);
            this.Controls.Add(this.labelHomeScore);
            this.Name = "MainForm";
            this.Text = "Хоккей (пример)";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnClosed);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelHomeScore;
        private System.Windows.Forms.Label labelVisitorScore;
        private System.Windows.Forms.Label labelPrimaryTimer;
        private System.Windows.Forms.Label labelSecondaryTimer;
        private System.Windows.Forms.ListBox listBoxHomePenalty;
        private System.Windows.Forms.ListBox listBoxVisitorPenalty;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button buttonSynchronize;
        private System.Windows.Forms.Button button3;
    }
}

