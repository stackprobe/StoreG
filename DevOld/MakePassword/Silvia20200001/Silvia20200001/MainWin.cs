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

			this.MinimumSize = this.Size;
			this.Status.Text = "";
		}

		private void MainWin_Load(object sender, EventArgs e)
		{
			// none
		}

		private void MainWin_Shown(object sender, EventArgs e)
		{
			this.BtnMkPwDig.Focus();
			GUICommon.PostShown(this);
		}

		private void BtnMkPw_Click(object sender, EventArgs e)
		{
			this.MakePassword(new string[] { SCommon.DECIMAL, SCommon.ALPHA_UPPER, SCommon.ALPHA_LOWER }, 22);
		}

		private void BtnMkPwUpr_Click(object sender, EventArgs e)
		{
			this.MakePassword(new string[] { SCommon.DECIMAL, SCommon.ALPHA_UPPER }, 25);
		}

		private void BtnMkPwLwr_Click(object sender, EventArgs e)
		{
			this.MakePassword(new string[] { SCommon.DECIMAL, SCommon.ALPHA_LOWER }, 25);
		}

		private void BtnMkPwDig_Click(object sender, EventArgs e)
		{
			this.MakePassword(new string[] { SCommon.DECIMAL }, 40);
		}

		private void MkRand_Click(object sender, EventArgs e)
		{
			this.MakePassword(new string[] { SCommon.HEXADECIMAL_LOWER }, 32);
		}

		/// <summary>
		/// パスワードを生成して画面にセットする。
		/// </summary>
		/// <param name="chrSets">文字セットのリスト</param>
		/// <param name="length">パスワードの長さ</param>
		private void MakePassword(string[] chrSets, int length)
		{
			string password;

			do
			{
				password = this.TryMakePassword(string.Join("", chrSets), length);
			}
			while (!this.CheckPassword(chrSets, password));

			this.SetPassword(password);
		}

		private string TryMakePassword(string chrSet, int length)
		{
			return new string(
				Enumerable.Range(0, length)
					.Select(dummy => SCommon.CRandom.ChooseOne(chrSet.ToArray()))
					.ToArray()
				);
		}

		private bool CheckPassword(string[] chrSets, string password)
		{
			foreach (string chrSet in chrSets)
				if (!chrSet.Any(chr => password.Contains(chr)))
					return false;

			return true;
		}

		/// <summary>
		/// 生成したパスワードを画面にセットする。
		/// </summary>
		/// <param name="password">生成したパスワード</param>
		private void SetPassword(string password)
		{
			// 画面にパスワードを表示する。
			this.PasswordText.Text = password;

			// ついでにクリップボードにもコピーする。
			Clipboard.SetText(password);

			// ステータス表示
			this.SetStatus("生成したパスワードをクリップボードにコピーしました。");
		}

		/// <summary>
		/// パスワードをクリアする。
		/// </summary>
		private void ClearPassword()
		{
			// 画面にセットしたパスワードをクリアする。
			this.PasswordText.Text = "";

			// ついでにクリップボードもクリアする。
			Clipboard.Clear();

			// ステータス表示
			this.SetStatus("クリップボードもクリアしました。");
		}

		private void SetStatus(string text)
		{
			this.Status.Text = "[" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "] " + text;
		}

		private void PasswordText_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 1) // ? ctrl_a
			{
				this.PasswordText.SelectAll();
				e.Handled = true;
			}
		}

		private void BtnClear_Click(object sender, EventArgs e)
		{
			this.ClearPassword();
		}

		private void BtnMkUUID_Click(object sender, EventArgs e)
		{
			this.SetPassword(Guid.NewGuid().ToString("B"));
		}

		private void BtnMkPwLen_Click(object sender, EventArgs e)
		{
			this.MakePassword(new string[] { SCommon.DECIMAL, SCommon.ALPHA_UPPER, SCommon.ALPHA_LOWER }, (int)this.SpecPasswordLength.Value);
		}
	}
}
