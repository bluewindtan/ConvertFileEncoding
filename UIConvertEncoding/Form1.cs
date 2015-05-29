using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConvertFileEncoding;
using System.IO;

namespace UIConvertEncoding
{
	public partial class Form1 : Form
	{
		public static string s_SelectDir = "请选择目录";

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// 构建数据源 
			DataTable dataT1 = new DataTable();
			dataT1.Columns.Add("name");
			dataT1.Columns.Add("value");
			DataTable dataT2 = new DataTable();
			dataT2.Columns.Add("name");
			dataT2.Columns.Add("value");
			for (int i = 0; i < (int)CConvertEncoding.FileEncoding.MAX; i++)
			{
				dataT1.Rows.Add(new string[] { CConvertEncoding.m_EncoidingName[i], CConvertEncoding.m_EncoidingValue[i] });
				dataT2.Rows.Add(new string[] { CConvertEncoding.m_EncoidingName[i], CConvertEncoding.m_EncoidingValue[i] });
			}

			// 绑定下拉框
			_InitComboBox(comboBox1, dataT1);
			_InitComboBox(comboBox2, dataT2);
			comboBox2.SelectedIndex = 1;
		}

		private void _InitComboBox(ComboBox cb, DataTable dt)
		{
			cb.DataSource = dt;
			cb.DisplayMember = "name";
			cb.ValueMember = "value";
			cb.AutoCompleteSource = AutoCompleteSource.ListItems;
			cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (0 == comboBox1.SelectedValue.ToString().CompareTo(comboBox2.SelectedValue.ToString()))
			{
				MessageBox.Show("Process failed: please input two different encodings.");
				return;
			}
			FolderBrowserDialog dlg = new FolderBrowserDialog();
			dlg.RootFolder = Environment.SpecialFolder.MyComputer;
			dlg.SelectedPath = Environment.CurrentDirectory;
			dlg.Description = s_SelectDir;
			if (DialogResult.OK == dlg.ShowDialog())
			{
				string sDirectory = dlg.SelectedPath;
				CConvertEncoding convertEncoding = new CConvertEncoding();
				if (convertEncoding != null)
				{
					// 设置编码 
					convertEncoding.SetSourceEncoding(comboBox1.SelectedValue.ToString());
					convertEncoding.SetConvertEncoding(comboBox2.SelectedValue.ToString());
					ProcessDirectory(convertEncoding, sDirectory);
					MessageBox.Show("Process finished！OK！");
				}
				else
				{
					MessageBox.Show("Process failed: new CConvertEncoding.");
				}
			}
		}

		private void ProcessDirectory(CConvertEncoding clsConvert, string strDir)
		{
			// 判断是否目录
			if (Directory.Exists(strDir))
			{
				DirectoryInfo dirInfo = new DirectoryInfo(strDir);
				foreach (FileSystemInfo fsInfo in dirInfo.GetFileSystemInfos())
				{
					if (fsInfo is FileInfo)
					{
						FileInfo fi = fsInfo as FileInfo;
						clsConvert.DoAll(fi.FullName);
					}
					else if (fsInfo is DirectoryInfo)
					{
						ProcessDirectory(clsConvert, fsInfo.FullName);
					}
				}
			}
		}

		private void btnSwitch_Click(object sender, EventArgs e)
		{
			int objTemp = comboBox1.SelectedIndex;
			comboBox1.SelectedIndex = comboBox2.SelectedIndex;
			comboBox2.SelectedIndex = objTemp;
		}
	}
}
