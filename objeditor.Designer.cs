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
            this.regionCheckbox = new System.Windows.Forms.CheckBox();
            this.widthBox = new System.Windows.Forms.TextBox();
            this.heightBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // keySelector
            // 
            this.keySelector.BackColor = System.Drawing.Color.Black;
            this.keySelector.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keySelector.ForeColor = System.Drawing.Color.Yellow;
            this.keySelector.FormattingEnabled = true;
            this.keySelector.Items.AddRange(new object[] {
            "[ PLAYER ]",
            "player_start",
            "[ ENVIRONMENT ]",
            "totem",
            "light",
            "[ DOORS ]",
            "door_pin",
            "door_end",
            "door_sensor",
            "[ NPC ]",
            "elder",
            "skeline",
            "acolyte",
            "cleric",
            "[ LEVEL ]",
            "area_switch"});
            this.keySelector.Location = new System.Drawing.Point(12, 12);
            this.keySelector.Name = "keySelector";
            this.keySelector.Size = new System.Drawing.Size(307, 22);
            this.keySelector.TabIndex = 0;
            // 
            // jsonEditor
            // 
            this.jsonEditor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.jsonEditor.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jsonEditor.Location = new System.Drawing.Point(12, 39);
            this.jsonEditor.Multiline = true;
            this.jsonEditor.Name = "jsonEditor";
            this.jsonEditor.Size = new System.Drawing.Size(307, 283);
            this.jsonEditor.TabIndex = 1;
            // 
            // regionCheckbox
            // 
            this.regionCheckbox.AutoSize = true;
            this.regionCheckbox.Location = new System.Drawing.Point(326, 16);
            this.regionCheckbox.Name = "regionCheckbox";
            this.regionCheckbox.Size = new System.Drawing.Size(65, 17);
            this.regionCheckbox.TabIndex = 2;
            this.regionCheckbox.Text = "is region";
            this.regionCheckbox.UseVisualStyleBackColor = true;
            // 
            // widthBox
            // 
            this.widthBox.Location = new System.Drawing.Point(325, 39);
            this.widthBox.Name = "widthBox";
            this.widthBox.Size = new System.Drawing.Size(100, 20);
            this.widthBox.TabIndex = 3;
            // 
            // heightBox
            // 
            this.heightBox.Location = new System.Drawing.Point(325, 65);
            this.heightBox.Name = "heightBox";
            this.heightBox.Size = new System.Drawing.Size(100, 20);
            this.heightBox.TabIndex = 4;
            // 
            // objeditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 334);
            this.ControlBox = false;
            this.Controls.Add(this.heightBox);
            this.Controls.Add(this.widthBox);
            this.Controls.Add(this.regionCheckbox);
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
        private System.Windows.Forms.CheckBox regionCheckbox;
        private System.Windows.Forms.TextBox widthBox;
        private System.Windows.Forms.TextBox heightBox;
    }
}