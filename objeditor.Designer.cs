namespace wrenk
{
    partial class objeditor
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
            this.keySelector = new System.Windows.Forms.ComboBox();
            this.jsonEditor = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // keySelector
            // 
            this.keySelector.FormattingEnabled = true;
            this.keySelector.Items.AddRange(new object[] {
            "elder",
            "skeline",
            "wormfield",
            "totem",
            "playerstart"});
            this.keySelector.Location = new System.Drawing.Point(12, 12);
            this.keySelector.Name = "keySelector";
            this.keySelector.Size = new System.Drawing.Size(307, 21);
            this.keySelector.TabIndex = 0;
            // 
            // jsonEditor
            // 
            this.jsonEditor.Location = new System.Drawing.Point(12, 39);
            this.jsonEditor.Multiline = true;
            this.jsonEditor.Name = "jsonEditor";
            this.jsonEditor.Size = new System.Drawing.Size(307, 283);
            this.jsonEditor.TabIndex = 1;
            // 
            // objeditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 334);
            this.ControlBox = false;
            this.Controls.Add(this.jsonEditor);
            this.Controls.Add(this.keySelector);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "objeditor";
            this.Text = "objeditor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox keySelector;
        private System.Windows.Forms.TextBox jsonEditor;
    }
}