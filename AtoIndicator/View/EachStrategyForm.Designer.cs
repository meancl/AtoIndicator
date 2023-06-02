
namespace AtoIndicator.View.EachStrategy

{
    partial class EachStrategyForm
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
            this.eachStrategyListView = new System.Windows.Forms.ListView();
            this.choosedInfoTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // eachStrategyListView
            // 
            this.eachStrategyListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eachStrategyListView.FullRowSelect = true;
            this.eachStrategyListView.HideSelection = false;
            this.eachStrategyListView.Location = new System.Drawing.Point(11, 12);
            this.eachStrategyListView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.eachStrategyListView.Name = "eachStrategyListView";
            this.eachStrategyListView.Size = new System.Drawing.Size(805, 582);
            this.eachStrategyListView.TabIndex = 0;
            this.eachStrategyListView.UseCompatibleStateImageBehavior = false;
            // 
            // choosedInfoTextBox
            // 
            this.choosedInfoTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.choosedInfoTextBox.Location = new System.Drawing.Point(822, 12);
            this.choosedInfoTextBox.Multiline = true;
            this.choosedInfoTextBox.Name = "choosedInfoTextBox";
            this.choosedInfoTextBox.ReadOnly = true;
            this.choosedInfoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.choosedInfoTextBox.Size = new System.Drawing.Size(311, 582);
            this.choosedInfoTextBox.TabIndex = 1;
            // 
            // EachStrategyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1145, 606);
            this.Controls.Add(this.choosedInfoTextBox);
            this.Controls.Add(this.eachStrategyListView);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "EachStrategyForm";
            this.Text = "EachStrategyForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ListView eachStrategyListView;
        private System.Windows.Forms.TextBox choosedInfoTextBox;
    }
}