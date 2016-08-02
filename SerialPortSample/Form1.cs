using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;

namespace SerialPortSample
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			string[] name = GetComPort();
			if (name != null)
			{
				for( int i=0;i<name.Length;i++ )
				{
					textBox1.Text += name[i] + "\r\n";
				}
			}
			else
				textBox1.Text = "NULL";


/*			serialPort1.PortName = "COM3";
			serialPort1.BaudRate = 119200;
			serialPort1.Parity = System.IO.Ports.Parity.None;
			serialPort1.DataBits = 8;
			serialPort1.StopBits = System.IO.Ports.StopBits.One;
			serialPort1.Open();*/
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
//			serialPort1.Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Byte[] dat = System.Text.Encoding.UTF8.GetBytes("reboot\n");
			serialPort1.Write(dat, 0 , dat.GetLength( 0 ));
		}

		private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
		{
			Byte[] dat = new Byte[serialPort1.BytesToRead];
			serialPort1.Read(dat, 0, dat.GetLength(0));

//			System.IO.File.WriteAllText("111.txt", System.Text.Encoding.GetEncoding("SHIFT-JIS").GetString(dat), System.Text.Encoding.GetEncoding("Shift_JIS"));

			textBox1.Text = System.Text.Encoding.GetEncoding("SHIFT-JIS").GetString(dat);
//			textBox1.Text = System.Text.Encoding.UTF8.GetString(dat);

//			if (textBox1.Text.StartsWith("3"))
//				MessageBox.Show( textBox1.Text );

//			MessageBox.Show(System.Text.Encoding.GetEncoding("S HIFT-JIS").GetString(ifdat));
		}

		public string[] GetComPort()
		{
			System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("(COM[1-9][0-9]?[0-9]?)");

			//	ソリューションエクスプローラの参照→参照設定から(System.Management)を選択する
			ManagementClass managementClass = new ManagementClass("Win32_PnPEntity");
			ManagementObjectCollection manageObjCol = managementClass.GetInstances();

			foreach( ManagementObject manageObj in manageObjCol )
			{
				string caption = manageObj.GetPropertyValue("Caption").ToString();
				if (caption != null)
				{
					string deviceId = manageObj.GetPropertyValue("DeviceID").ToString();
					if (deviceId.StartsWith("FTDI"))
					{
						if (regex.IsMatch(caption))
							arrayList.Add(caption);
					}
				}
			}

			if( arrayList.Count > 0 )
			{
				string[] comPortNames = new string[arrayList.Count];
				for( int i=0;i<arrayList.Count;i++ )
					comPortNames[i] = arrayList[i].ToString();
				return comPortNames;
			}
			return null;
		}
	}
}
