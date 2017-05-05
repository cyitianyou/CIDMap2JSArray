using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CIDMap2JSArray
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                var FilePath = Path.Combine(Environment.CurrentDirectory, "Adobe-GB1-5.cidmap");
                if (File.Exists(FilePath))
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(FilePath, System.Text.Encoding.UTF8);
                    this.txt_CIDMap.Text = sr.ReadToEnd().Replace("\n", "\r\n");  //各操作系统之间换行符有区别,注意转换
                }
            }
            catch (Exception)
            {
                WriteStatusMessage("未找到.cidmap文件");
            }

        }

        private void toolStripSplitButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(this.txt_CIDMap.Text)) return;
                var lines = this.txt_CIDMap.Text.Replace("\r", "").Split('\n');
                var ArrayBuild = new StringBuilder();  //普通Unicode存入数组
                var PolyphoneBuider = new StringBuilder();  //多音字单独存
                ArrayBuild.AppendLine("{");
                for (int i = 1; i < lines.Length; i++)  //从文件第二行开始,第一行存总数,忽略
                {
                    var line = lines[i];
                    if (string.IsNullOrEmpty(line)) continue;
                    DealWithLineString(line, ref ArrayBuild, ref PolyphoneBuider);
                }
                if (ArrayBuild.Length > 1)
                    ArrayBuild.Remove(ArrayBuild.Length - 1, 1);
                ArrayBuild.AppendLine("}");
                this.txt_JSArray.Text = ArrayBuild.ToString();
            }
            catch (Exception)
            {
                WriteStatusMessage("转换出错");
            }
        }
        private void DealWithLineString(string line, ref StringBuilder ArrayBuild, ref StringBuilder PolyphoneBuider)
        {
            try
            {
                Application.DoEvents();
                var splits = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (splits.Length != 2) return;
                var cid = splits[0];
                var unicode = splits[1];
                if (unicode.Contains("/")) return;
                if (!cid.Contains("..") && !unicode.Contains(","))  //line格式如: 100 02c9
                {
                    var value = Convert.ToInt32(cid);
                    ArrayBuild.AppendExt(unicode, value);
                }
                else if (cid.Contains("..") && !unicode.Contains(","))  //line格式如: 1..95 0020
                {
                    var values = cid.Replace("..", ".").Split('.');
                    if (values.Length != 2) return;
                    int start = Convert.ToInt32(values[0]);
                    int end = Convert.ToInt32(values[1]);
                    int int_unicode = unicode.HexStringToInt();
                    for (int i = 0; i +start <= end; i++)
                    {
                        ArrayBuild.AppendExt((int_unicode+i).IntToHexString(), start+i);
                    }
                }
                else if (!cid.Contains("..") && unicode.Contains(","))  //line格式如: 108 2026,22ef
                {
                    var unicodes = unicode.Split(',');
                    var value = Convert.ToInt32(cid);
                    foreach (var item in unicodes)
                    {
                        ArrayBuild.AppendExt(item, value);
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                Application.DoEvents();
            }
        }
        private void WriteStatusMessage(string Msg)
        {
            this.toolStripStatusLabel.Text = Msg;
        }
    }
}
