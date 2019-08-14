using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Configuration;

namespace MergeTextRowByRow
{
	public partial class Form1 : Form
	{
		private int _SpaceLen = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("SpaceLen")) ? 64 : int.Parse(ConfigurationManager.AppSettings.Get("SpaceLen"));
		private bool _SpaceOn = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("SpaceOn")) ? true : bool.Parse(ConfigurationManager.AppSettings.Get("SpaceOn"));
		private char _SplitChar = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("SplitChar")) ? '\t' : (char)int.Parse(ConfigurationManager.AppSettings.Get("SplitChar"));
		private bool _CSV = string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("CSV")) ? false : bool.Parse(ConfigurationManager.AppSettings.Get("CSV"));
		public Form1()
		{
			InitializeComponent();
			Init();
		}

		private void Init()
		{
			label1.Text = "";
			label1.ForeColor = Color.Black;
			textBox3.ForeColor = Color.Black;
			textBox3.Text = "";
		}

		private string _sResult = "";

		private async void button1_Click(object sender, EventArgs e)
		{
			Init();
			IEnumerator<string> Text1 = GetEnume(textBox1.Text).GetEnumerator();
			IEnumerator<string> Text2 = GetEnume(textBox2.Text).GetEnumerator();
			Timer tim = new Timer();
			tim.Tick += new EventHandler(tim_Tick);
			tim.Interval = (int)numericUpDown1.Value;
			tim.Start();
			Func<string> func = () =>
								{
									StringBuilder sbText = new StringBuilder();
									bool hasRow1 = true;
									bool hasRow2 = true;
									while (hasRow1 || hasRow2)
									{
										hasRow1 = Text1.MoveNext();
										hasRow2 = Text2.MoveNext();
										System.Threading.Thread.Sleep((int)numericUpDown1.Value);
										if (hasRow1 || hasRow2)
										{
											sbText.AppendLine((hasRow1 ? Text1.Current : "") + GetFeedTab(hasRow1 ? Text1.Current.Length : 0, _SpaceLen) + (hasRow2 ? Text2.Current : "") + (_CSV ? _SplitChar.ToString() : ""));
										}
										_sResult = sbText.ToString();
									}
									tim.Tick -= new EventHandler(tim_Tick);
									tim.Stop();
									return sbText.ToString();
								};

			string x = await Task.Factory.StartNew(func);
			label1.Text = "Xong.";
			label1.ForeColor = Color.Black;
			textBox3.Text = x;
			textBox3.ForeColor = Color.Black;
		}

		public class MergeTextEventArgs : EventArgs
		{
			public MergeTextEventArgs(string text) : base()
			{
				this.Text = text;
			}

			public string Text { get; private set; }
		}

		private void tim_Tick(object sender, EventArgs e)
		{
			if (label1.Text == "Vui lòng đợi...")
			{
				label1.Text = "Vui lòng đợi..";
				label1.ForeColor = Color.Red;
			}
			else if (label1.Text == "Vui lòng đợi..")
			{
				label1.Text = "Vui lòng đợi.";
				label1.ForeColor = Color.Purple;
			}
			else
			{
				label1.Text = "Vui lòng đợi...";
				label1.ForeColor = Color.Blue;
			}
			textBox3.Text = _sResult;
			textBox3.ForeColor = Color.Blue;
		}

		private string GetFeedTab(int lenght, int maxlengt)
		{

			string res = _SplitChar.ToString();
			if (_SpaceOn)
			{
				if (lenght + 4 < maxlengt)
				{
					res = new string(_SplitChar, (maxlengt - lenght) / 4);
				}
			}
			return res;
		}
		private IEnumerable<string> GetEnume(string text)
		{
			using (StringReader read = new StringReader(text))
			{
				while (read.Peek() != -1)
				{
					yield return read.ReadLine();
				}
			}
		}
	}
}
