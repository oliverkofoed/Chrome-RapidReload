﻿namespace ChromeRapidReload {
	partial class DirectorySelector {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectorySelector));
			this.directories = new System.Windows.Forms.ListBox();
			this.removeButton = new System.Windows.Forms.Button();
			this.addButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// directories
			// 
			this.directories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.directories.FormattingEnabled = true;
			this.directories.Location = new System.Drawing.Point(12, 12);
			this.directories.Name = "directories";
			this.directories.Size = new System.Drawing.Size(513, 212);
			this.directories.TabIndex = 0;
			this.directories.SelectedIndexChanged += new System.EventHandler(this.directories_SelectedIndexChanged);
			// 
			// removeButton
			// 
			this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.removeButton.Enabled = false;
			this.removeButton.Location = new System.Drawing.Point(12, 231);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(115, 23);
			this.removeButton.TabIndex = 1;
			this.removeButton.Text = "Remove";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// addButton
			// 
			this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.addButton.Location = new System.Drawing.Point(409, 230);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(115, 23);
			this.addButton.TabIndex = 2;
			this.addButton.Text = "Add Directory";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// DirectorySelector
			// 
			this.AcceptButton = this.addButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(537, 264);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.directories);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DirectorySelector";
			this.Text = "Monitored Directories";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DirectorySelector_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox directories;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button addButton;
	}
}