
namespace AtoIndicator.View.StatisticResult
{
    partial class StatisticResultForm
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
            this.updateButton = new System.Windows.Forms.Button();
            this.sharedTimeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(672, 13);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(108, 35);
            this.updateButton.TabIndex = 0;
            this.updateButton.Text = "업데이트( U )";
            this.updateButton.UseVisualStyleBackColor = true;
            // 
            // sharedTimeLabel
            // 
            this.sharedTimeLabel.AutoSize = true;
            this.sharedTimeLabel.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sharedTimeLabel.Location = new System.Drawing.Point(471, 18);
            this.sharedTimeLabel.Name = "sharedTimeLabel";
            this.sharedTimeLabel.Size = new System.Drawing.Size(0, 19);
            this.sharedTimeLabel.TabIndex = 1;
            // 
            // StatisticResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.sharedTimeLabel);
            this.Controls.Add(this.updateButton);
            this.Name = "StatisticResultForm";
            this.Text = "StatisticResultForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Label sharedTimeLabel;
    }
}