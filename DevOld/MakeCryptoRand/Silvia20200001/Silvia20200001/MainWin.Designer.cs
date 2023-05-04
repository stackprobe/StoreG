namespace Charlotte
{
	partial class MainWin
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWin));
			this.RandText = new System.Windows.Forms.TextBox();
			this.RandStatus = new System.Windows.Forms.Label();
			this.BtnGenRand = new System.Windows.Forms.Button();
			this.BtnClear = new System.Windows.Forms.Button();
			this.MessageLabel = new System.Windows.Forms.Label();
			this.Numb行数 = new System.Windows.Forms.NumericUpDown();
			this.Label行数 = new System.Windows.Forms.Label();
			this.Combo桁数 = new System.Windows.Forms.ComboBox();
			this.Label桁数 = new System.Windows.Forms.Label();
			this.BtnSave = new System.Windows.Forms.Button();
			this.BtnReset = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.Numb行数)).BeginInit();
			this.SuspendLayout();
			// 
			// RandText
			// 
			this.RandText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.RandText.Font = new System.Drawing.Font("OCRB", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RandText.Location = new System.Drawing.Point(12, 32);
			this.RandText.Multiline = true;
			this.RandText.Name = "RandText";
			this.RandText.ReadOnly = true;
			this.RandText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.RandText.Size = new System.Drawing.Size(870, 600);
			this.RandText.TabIndex = 1;
			this.RandText.TextChanged += new System.EventHandler(this.RandText_TextChanged);
			this.RandText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RandText_KeyPress);
			// 
			// RandStatus
			// 
			this.RandStatus.AutoSize = true;
			this.RandStatus.Location = new System.Drawing.Point(12, 9);
			this.RandStatus.Name = "RandStatus";
			this.RandStatus.Size = new System.Drawing.Size(82, 20);
			this.RandStatus.TabIndex = 0;
			this.RandStatus.Text = "RandStatus";
			this.RandStatus.Click += new System.EventHandler(this.RandStatus_Click);
			// 
			// BtnGenRand
			// 
			this.BtnGenRand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnGenRand.Location = new System.Drawing.Point(636, 638);
			this.BtnGenRand.Name = "BtnGenRand";
			this.BtnGenRand.Size = new System.Drawing.Size(120, 60);
			this.BtnGenRand.TabIndex = 9;
			this.BtnGenRand.Text = "生成";
			this.BtnGenRand.UseVisualStyleBackColor = true;
			this.BtnGenRand.Click += new System.EventHandler(this.BtnGenRand_Click);
			// 
			// BtnClear
			// 
			this.BtnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnClear.Location = new System.Drawing.Point(762, 638);
			this.BtnClear.Name = "BtnClear";
			this.BtnClear.Size = new System.Drawing.Size(120, 60);
			this.BtnClear.TabIndex = 10;
			this.BtnClear.Text = "クリア";
			this.BtnClear.UseVisualStyleBackColor = true;
			this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
			// 
			// MessageLabel
			// 
			this.MessageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.MessageLabel.AutoSize = true;
			this.MessageLabel.Location = new System.Drawing.Point(12, 681);
			this.MessageLabel.Name = "MessageLabel";
			this.MessageLabel.Size = new System.Drawing.Size(98, 20);
			this.MessageLabel.TabIndex = 7;
			this.MessageLabel.Text = "MessageLabel";
			this.MessageLabel.Click += new System.EventHandler(this.MessageLabel_Click);
			// 
			// Numb行数
			// 
			this.Numb行数.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.Numb行数.Location = new System.Drawing.Point(336, 638);
			this.Numb行数.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
			this.Numb行数.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.Numb行数.Name = "Numb行数";
			this.Numb行数.Size = new System.Drawing.Size(60, 27);
			this.Numb行数.TabIndex = 5;
			this.Numb行数.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
			this.Numb行数.ValueChanged += new System.EventHandler(this.Numb行数_ValueChanged);
			// 
			// Label行数
			// 
			this.Label行数.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.Label行数.AutoSize = true;
			this.Label行数.Location = new System.Drawing.Point(272, 641);
			this.Label行数.Name = "Label行数";
			this.Label行数.Size = new System.Drawing.Size(58, 20);
			this.Label行数.TabIndex = 4;
			this.Label行数.Text = "/ 行数：";
			this.Label行数.Click += new System.EventHandler(this.Label行数_Click);
			// 
			// Combo桁数
			// 
			this.Combo桁数.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.Combo桁数.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Combo桁数.FormattingEnabled = true;
			this.Combo桁数.Items.AddRange(new object[] {
            "16文字 (8バイト, 64 bit)",
            "32文字 (16バイト, 128 bit)",
            "48文字 (24バイト, 192 bit)",
            "64文字 (32バイト, 256 bit)"});
			this.Combo桁数.Location = new System.Drawing.Point(66, 638);
			this.Combo桁数.Name = "Combo桁数";
			this.Combo桁数.Size = new System.Drawing.Size(200, 28);
			this.Combo桁数.TabIndex = 3;
			this.Combo桁数.SelectedIndexChanged += new System.EventHandler(this.Combo桁数_SelectedIndexChanged);
			// 
			// Label桁数
			// 
			this.Label桁数.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.Label桁数.AutoSize = true;
			this.Label桁数.Location = new System.Drawing.Point(12, 640);
			this.Label桁数.Name = "Label桁数";
			this.Label桁数.Size = new System.Drawing.Size(48, 20);
			this.Label桁数.TabIndex = 2;
			this.Label桁数.Text = "桁数：";
			this.Label桁数.Click += new System.EventHandler(this.Label桁数_Click);
			// 
			// BtnSave
			// 
			this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnSave.Location = new System.Drawing.Point(510, 638);
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.Size = new System.Drawing.Size(120, 60);
			this.BtnSave.TabIndex = 8;
			this.BtnSave.Text = "保存";
			this.BtnSave.UseVisualStyleBackColor = true;
			this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
			// 
			// BtnReset
			// 
			this.BtnReset.Location = new System.Drawing.Point(402, 638);
			this.BtnReset.Name = "BtnReset";
			this.BtnReset.Size = new System.Drawing.Size(60, 28);
			this.BtnReset.TabIndex = 6;
			this.BtnReset.Text = "Reset";
			this.BtnReset.UseVisualStyleBackColor = true;
			this.BtnReset.Click += new System.EventHandler(this.BtnReset_Click);
			// 
			// MainWin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(894, 710);
			this.Controls.Add(this.BtnReset);
			this.Controls.Add(this.BtnSave);
			this.Controls.Add(this.Label桁数);
			this.Controls.Add(this.Combo桁数);
			this.Controls.Add(this.Label行数);
			this.Controls.Add(this.Numb行数);
			this.Controls.Add(this.MessageLabel);
			this.Controls.Add(this.BtnClear);
			this.Controls.Add(this.BtnGenRand);
			this.Controls.Add(this.RandStatus);
			this.Controls.Add(this.RandText);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainWin";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MakeCryptoRand";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.MainWin_Load);
			this.Shown += new System.EventHandler(this.MainWin_Shown);
			((System.ComponentModel.ISupportInitialize)(this.Numb行数)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox RandText;
		private System.Windows.Forms.Label RandStatus;
		private System.Windows.Forms.Button BtnGenRand;
		private System.Windows.Forms.Button BtnClear;
		private System.Windows.Forms.Label MessageLabel;
		private System.Windows.Forms.NumericUpDown Numb行数;
		private System.Windows.Forms.Label Label行数;
		private System.Windows.Forms.ComboBox Combo桁数;
		private System.Windows.Forms.Label Label桁数;
		private System.Windows.Forms.Button BtnSave;
		private System.Windows.Forms.Button BtnReset;
	}
}

