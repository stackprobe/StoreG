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
			this.PasswordText = new System.Windows.Forms.TextBox();
			this.BtnMkPwDig = new System.Windows.Forms.Button();
			this.BtnMkPwUpr = new System.Windows.Forms.Button();
			this.BtnMkPwLwr = new System.Windows.Forms.Button();
			this.BtnMkPw = new System.Windows.Forms.Button();
			this.BtnMkUUID = new System.Windows.Forms.Button();
			this.BtnClear = new System.Windows.Forms.Button();
			this.MkRand = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SpecPasswordLength = new System.Windows.Forms.NumericUpDown();
			this.BtnMkPwLen = new System.Windows.Forms.Button();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.Status = new System.Windows.Forms.ToolStripStatusLabel();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SpecPasswordLength)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// PasswordText
			// 
			this.PasswordText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PasswordText.Font = new System.Drawing.Font("メイリオ", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.PasswordText.Location = new System.Drawing.Point(12, 12);
			this.PasswordText.MaxLength = 0;
			this.PasswordText.Name = "PasswordText";
			this.PasswordText.ReadOnly = true;
			this.PasswordText.Size = new System.Drawing.Size(760, 48);
			this.PasswordText.TabIndex = 0;
			this.PasswordText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PasswordText_KeyPress);
			// 
			// BtnMkPwDig
			// 
			this.BtnMkPwDig.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.BtnMkPwDig.ForeColor = System.Drawing.Color.Navy;
			this.BtnMkPwDig.Location = new System.Drawing.Point(12, 66);
			this.BtnMkPwDig.Name = "BtnMkPwDig";
			this.BtnMkPwDig.Size = new System.Drawing.Size(200, 55);
			this.BtnMkPwDig.TabIndex = 1;
			this.BtnMkPwDig.Text = "[0-9]{40}";
			this.BtnMkPwDig.UseVisualStyleBackColor = true;
			this.BtnMkPwDig.Click += new System.EventHandler(this.BtnMkPwDig_Click);
			// 
			// BtnMkPwUpr
			// 
			this.BtnMkPwUpr.Location = new System.Drawing.Point(12, 127);
			this.BtnMkPwUpr.Name = "BtnMkPwUpr";
			this.BtnMkPwUpr.Size = new System.Drawing.Size(200, 55);
			this.BtnMkPwUpr.TabIndex = 2;
			this.BtnMkPwUpr.Text = "[0-9A-Z]{25} 2種";
			this.BtnMkPwUpr.UseVisualStyleBackColor = true;
			this.BtnMkPwUpr.Click += new System.EventHandler(this.BtnMkPwUpr_Click);
			// 
			// BtnMkPwLwr
			// 
			this.BtnMkPwLwr.Location = new System.Drawing.Point(218, 127);
			this.BtnMkPwLwr.Name = "BtnMkPwLwr";
			this.BtnMkPwLwr.Size = new System.Drawing.Size(200, 55);
			this.BtnMkPwLwr.TabIndex = 4;
			this.BtnMkPwLwr.Text = "[0-9a-z]{25} 2種";
			this.BtnMkPwLwr.UseVisualStyleBackColor = true;
			this.BtnMkPwLwr.Click += new System.EventHandler(this.BtnMkPwLwr_Click);
			// 
			// BtnMkPw
			// 
			this.BtnMkPw.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.BtnMkPw.ForeColor = System.Drawing.Color.Navy;
			this.BtnMkPw.Location = new System.Drawing.Point(218, 66);
			this.BtnMkPw.Name = "BtnMkPw";
			this.BtnMkPw.Size = new System.Drawing.Size(200, 55);
			this.BtnMkPw.TabIndex = 3;
			this.BtnMkPw.Text = "[0-9A-Za-z]{22} 3種";
			this.BtnMkPw.UseVisualStyleBackColor = true;
			this.BtnMkPw.Click += new System.EventHandler(this.BtnMkPw_Click);
			// 
			// BtnMkUUID
			// 
			this.BtnMkUUID.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.BtnMkUUID.ForeColor = System.Drawing.Color.Navy;
			this.BtnMkUUID.Location = new System.Drawing.Point(424, 66);
			this.BtnMkUUID.Name = "BtnMkUUID";
			this.BtnMkUUID.Size = new System.Drawing.Size(200, 55);
			this.BtnMkUUID.TabIndex = 5;
			this.BtnMkUUID.Text = "UUID";
			this.BtnMkUUID.UseVisualStyleBackColor = true;
			this.BtnMkUUID.Click += new System.EventHandler(this.BtnMkUUID_Click);
			// 
			// BtnClear
			// 
			this.BtnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnClear.Location = new System.Drawing.Point(672, 66);
			this.BtnClear.Name = "BtnClear";
			this.BtnClear.Size = new System.Drawing.Size(100, 55);
			this.BtnClear.TabIndex = 7;
			this.BtnClear.Text = "Clear";
			this.BtnClear.UseVisualStyleBackColor = true;
			this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
			// 
			// MkRand
			// 
			this.MkRand.Location = new System.Drawing.Point(424, 127);
			this.MkRand.Name = "MkRand";
			this.MkRand.Size = new System.Drawing.Size(200, 55);
			this.MkRand.TabIndex = 6;
			this.MkRand.Text = "[0-9a-f]{32}";
			this.MkRand.UseVisualStyleBackColor = true;
			this.MkRand.Click += new System.EventHandler(this.MkRand_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.SpecPasswordLength);
			this.groupBox1.Controls.Add(this.BtnMkPwLen);
			this.groupBox1.Location = new System.Drawing.Point(12, 188);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(760, 98);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "指定した長さのパスワード";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(240, 43);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(30, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "x =";
			// 
			// SpecPasswordLength
			// 
			this.SpecPasswordLength.Location = new System.Drawing.Point(276, 41);
			this.SpecPasswordLength.Maximum = new decimal(new int[] {
            22,
            0,
            0,
            0});
			this.SpecPasswordLength.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.SpecPasswordLength.Name = "SpecPasswordLength";
			this.SpecPasswordLength.Size = new System.Drawing.Size(120, 27);
			this.SpecPasswordLength.TabIndex = 2;
			this.SpecPasswordLength.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// BtnMkPwLen
			// 
			this.BtnMkPwLen.Location = new System.Drawing.Point(20, 26);
			this.BtnMkPwLen.Name = "BtnMkPwLen";
			this.BtnMkPwLen.Size = new System.Drawing.Size(200, 55);
			this.BtnMkPwLen.TabIndex = 0;
			this.BtnMkPwLen.Text = "[0-9A-Za-z]{x} 3種";
			this.BtnMkPwLen.UseVisualStyleBackColor = true;
			this.BtnMkPwLen.Click += new System.EventHandler(this.BtnMkPwLen_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status});
			this.statusStrip1.Location = new System.Drawing.Point(0, 289);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(784, 22);
			this.statusStrip1.TabIndex = 9;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// Status
			// 
			this.Status.Name = "Status";
			this.Status.Size = new System.Drawing.Size(769, 17);
			this.Status.Spring = true;
			this.Status.Text = "Status";
			this.Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MainWin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 311);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.MkRand);
			this.Controls.Add(this.BtnClear);
			this.Controls.Add(this.BtnMkUUID);
			this.Controls.Add(this.BtnMkPw);
			this.Controls.Add(this.BtnMkPwLwr);
			this.Controls.Add(this.BtnMkPwUpr);
			this.Controls.Add(this.BtnMkPwDig);
			this.Controls.Add(this.PasswordText);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainWin";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "パスワード生成 (128ビット)";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.MainWin_Load);
			this.Shown += new System.EventHandler(this.MainWin_Shown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.SpecPasswordLength)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox PasswordText;
		private System.Windows.Forms.Button BtnMkPwDig;
		private System.Windows.Forms.Button BtnMkPwUpr;
		private System.Windows.Forms.Button BtnMkPwLwr;
		private System.Windows.Forms.Button BtnMkPw;
		private System.Windows.Forms.Button BtnMkUUID;
		private System.Windows.Forms.Button BtnClear;
		private System.Windows.Forms.Button MkRand;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.NumericUpDown SpecPasswordLength;
		private System.Windows.Forms.Button BtnMkPwLen;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel Status;
	}
}

