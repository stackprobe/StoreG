using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Security.Permissions;
using System.Windows.Forms;
using Charlotte.Commons;
using Charlotte.GUICommons;

namespace Charlotte
{
	public partial class MainWin : Form
	{
		public MainWin()
		{
			InitializeComponent();

			this.Size = new Size(this.Size.Width + 200, this.Size.Height);
			this.MinimumSize = this.Size;
			this.Status.Text = "";
		}

		private void MainWin_Load(object sender, EventArgs e)
		{
			// none
		}

		private void MainWin_Shown(object sender, EventArgs e)
		{
			this.BtnB6288.Focus();
			GUICommon.PostShown(this);
		}

		private void BtnB6288_Click(object sender, EventArgs e)
		{
			this.MakeID(
				"{B62ee-######################-######################-######################-######################}",
				SCommon.ALPHA_UPPER + SCommon.ALPHA_LOWER + SCommon.DECIMAL
				);
		}

		private void BtnB36100U_Click(object sender, EventArgs e)
		{
			this.MakeID(
				"{B36C-#########################-#########################-#########################-#########################}",
				SCommon.ALPHA_UPPER + SCommon.DECIMAL
				);
		}

		private void BtnB36100L_Click(object sender, EventArgs e)
		{
			this.MakeID(
				"{b36c-#########################-#########################-#########################-#########################}",
				SCommon.ALPHA_LOWER + SCommon.DECIMAL
				);
		}

		private void BtnB26110U_Click(object sender, EventArgs e)
		{
			this.MakeID(
				"{AZCX-######################-######################-######################-######################-######################}",
				SCommon.ALPHA_UPPER
				);
		}

		private void BtnB26110L_Click(object sender, EventArgs e)
		{
			this.MakeID(
				"{azcx-######################-######################-######################-######################-######################}",
				SCommon.ALPHA_LOWER
				);
		}

		private void BtnB16128U_Click(object sender, EventArgs e)
		{
			this.MakeID(
				"{BF80-################################-################################-################################-################################}",
				SCommon.HEXADECIMAL_UPPER
				);
		}

		private void BtnB16128L_Click(object sender, EventArgs e)
		{
			this.MakeID(
				"{bf80-################################-################################-################################-################################}",
				SCommon.HEXADECIMAL_LOWER
				);
		}

		private void BtnB10155_Click(object sender, EventArgs e)
		{
			this.MakeID(
				"{10155-###############################-###############################-###############################-###############################-###############################}",
				SCommon.DECIMAL
				);
		}

		/// <summary>
		/// 識別子を生成して画面にセットする。
		/// </summary>
		/// <param name="format"></param>
		/// <param name="chrSet"></param>
		private void MakeID(string format, string chrSet)
		{
			this.SetID(this.MakeID_Main(format, chrSet));
		}

		private string MakeID_Main(string format, string chrSet)
		{
			StringBuilder buff = new StringBuilder();
			char[] chrSetChrs = chrSet.ToArray();

			foreach (char chr in format)
			{
				if (chr == '#')
					buff.Append(SCommon.CRandom.ChooseOne(chrSetChrs));
				else
					buff.Append(chr);
			}
			return buff.ToString();
		}

		/// <summary>
		/// 生成した識別子を画面にセットする。
		/// </summary>
		/// <param name="ident">生成した識別子</param>
		private void SetID(string ident)
		{
			// 画面に識別子を表示する。
			this.IdentText.Text = ident;

			// ついでにクリップボードにもコピーする。
			Clipboard.SetText(ident);

			// ステータス表示
			this.SetStatus("生成した識別子をクリップボードにコピーしました。");
		}

		/// <summary>
		/// パスワードをクリアする。
		/// </summary>
		private void ClearID()
		{
			// 画面にセットしたパスワードをクリアする。
			this.IdentText.Text = "";

			// ついでにクリップボードもクリアする。
			Clipboard.Clear();

			// ステータス表示
			this.SetStatus("クリップボードもクリアしました。");
		}

		private void SetStatus(string text)
		{
			this.Status.Text = "[" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "] " + text;
		}

		private void IdentText_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 1) // ? ctrl_a
			{
				this.IdentText.SelectAll();
				e.Handled = true;
			}
		}

		private void BtnClear_Click(object sender, EventArgs e)
		{
			this.ClearID();
		}
	}
}
