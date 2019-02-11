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

namespace wOpener
{
    public partial class Form1 : Form
    {
        List<string> trsList = new List<string>();
        Dictionary<string, List<string>> parsList = new Dictionary<string, List<string>>();
        bool skip = false;
        bool enterPressed = false;
        int i = 0;
        string cmd = string.Empty;
        string lastButt = string.Empty;
        string sysName = string.Empty;
        string initText = @"[System]
Name={0}
Client={1}
Sysname={2}

[User]
Name=KOVARIKJ
Language=E
[Function]
Title=Open Transaction {3}
Command=*{3} {4}{5}
[Options]
Reuse=1";

        public Form1()
        {
            InitializeComponent();
            //treeView1.Nodes.Add("test1");
            //treeView1.Nodes.Add("test2");
            //treeView1.Nodes.Add("test3");
            //var tree = new TreeNode("tNode1");
            //tree.Nodes.Add("tN1");
            //tree.Nodes.Add("tN2");
            //tree.Nodes.Add("tN3");
            //treeView1.Nodes.Add(tree);             //treeView1.Nodes.Add("lkjlk");
            DirectoryInfo di = new DirectoryInfo(Application.StartupPath);
            FileInfo[] files = di.GetFiles("*.txt");
            rtbHintArea.Text += "\n";
            foreach (var f in files)
            {
                rtbHintArea.Text += f.Name + "\n";

                trsList.Add(f.Name);
            }

            FileMngr.ReadTransactions();
            FileMngr.ReadPars();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tbTransaction.Focus();
            rtbShortcutText.Text = initText;
            rtbShortcutText.Text = String.Format(rtbShortcutText.Text, "ER9", "003", "ER9 [SPACE]", "{0}", "{1}", "{2}");
            skip = false;
            sysName = "ER9_003_";
            lastButt = "ER9/003";
            setRepeatBtn("1234" + lastButt);
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_VisibleChanged(object sender, EventArgs e)
        {
            //richTextBox1.Text = initText;
        }

        private void tbTransaction_TextChanged(object sender, EventArgs e)
        {
            if (FileMngr.IsTrsThere(tbTransaction.Text))
            {
                int count = 0;

                List<TextBox> tboxes = new List<TextBox>();
                tboxes.Add(tbPar1); tboxes.Add(tbPar2); tboxes.Add(tbPar3);tboxes.Add(tbPar4);

                foreach (string par in FileMngr.givePars(tbTransaction.Text))
                {
                    tboxes[count].Text = par;
                    count++;
                }
            }
            else
            {
                List<TextBox> tboxes = new List<TextBox>();
                tboxes.Add(tbPar1); tboxes.Add(tbPar2); tboxes.Add(tbPar3);
                tboxes.Add(tbPar4); tboxes.Add(tbVal1); tboxes.Add(tbVal2);
                tboxes.Add(tbVal3); tboxes.Add(tbVal4);
                foreach (TextBox tb in tboxes) tb.Text = String.Empty;
            }

            
        }

        private void prepText()
        {   // Parse tbTransaction
            rtbShortcutText.Text = String.Format(rtbShortcutText.Text, tbTransaction.Text, "{0}", "{1}");
            cmd = tbTransaction.Text + " {0}" + "{1}";
            // Add parameter1
            if (tbPar1.Text == String.Empty)
            {
                rtbShortcutText.Text = String.Format(rtbShortcutText.Text, tbPar1.Text, "{0}");
                cmd = String.Format(cmd, tbPar1.Text, "{0}");
                tbVal1.Text = string.Empty;

                tbVal1_Validating(null, null);
            }

            else
            {
                rtbShortcutText.Text = String.Format(rtbShortcutText.Text, tbPar1.Text, "={0}");
                cmd = String.Format(cmd, tbPar1.Text, "={0}");
            }
            // Add value1 and run
            //if (tbVal1.Text == string.Empty)
              //  skip = true;

            //if (tbPar2.Text != string.Empty)
              //  skip = true;
            // Add condition: If more pars... and enter not preset :-)..cont with entering


            if (!skip)
            {
                rtbShortcutText.Text = String.Format(rtbShortcutText.Text, prepMatrix());
                cmd = String.Format(cmd, tbVal1.Text);
                string runFile = tbPath.Text + "\\" + sysName + tbTransaction.Text + "_" + tbPar1.Text + "_" + tbVal1.Text + ".sap";



                if (File.Exists(runFile)) File.Delete(runFile);

                using (StreamWriter sr = File.AppendText(runFile))
                {
                    sr.WriteLine(rtbShortcutText.Text); sr.Close();

                    if (!cbHistory.Items.Contains(cmd))
                        cbHistory.Items.Add(cmd);
                    i++;


                }

                System.Diagnostics.Process.Start(runFile);
                enterPressed = false;
            }
        }

        private string prepMatrix()
        {
            List<string> matrix = new List<string>();
            if (tbPar1.Text != string.Empty && tbVal1.Text != string.Empty)
                matrix.Add("" + tbVal1.Text);
            if (tbPar2.Text != string.Empty && tbVal2.Text != string.Empty)
                matrix.Add(tbPar2.Text+"="+tbVal2.Text);
            if (tbPar3.Text != string.Empty && tbVal3.Text != string.Empty)
                matrix.Add(tbPar3.Text + "=" + tbVal3.Text);
            if (tbPar4.Text != string.Empty && tbVal4.Text != string.Empty)
                matrix.Add(tbPar4.Text + "=" + tbVal4.Text);
            string ret=string.Empty;
            foreach (var s in matrix)
                ret = ret + s + ";";
            return ret;
        }

        private void tbTransaction_Leave(object sender, EventArgs e)
        {
            
        }

        private void tbPar1_Leave(object sender, EventArgs e)
        {
           
        }

        private void textBox3_Validated(object sender, EventArgs e)
        {
            //rtbShortcutText.Text = String.Format(rtbShortcutText.Text, tbVal1.Text);
            // History Command - into file name and repeat dropdown
            //cmd = String.Format(cmd, tbVal1.Text);

            // ulož do filu a spusť
        }

        private void tbVal1_Validating(object sender, CancelEventArgs e)
        {    
          

            
        }

        private void textBox3_AcceptsTabChanged(object sender, EventArgs e)
        {
            string s = string.Empty;

          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tbTransaction.Focus();
            rtbShortcutText.Text = initText;
            rtbShortcutText.Text = String.Format(rtbShortcutText.Text, "ER9", "500", "ER9 [SPACE]", "{0}", "{1}", "{2}");
            skip = false;
            sysName = "ER9_500_";
            lastButt = "ER9/500";
            setRepeatBtn("1234" + lastButt);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt) skip = true;
        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            string s = string.Empty;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            button3.Text = "&C  " + tbCustSys.Text + "/" + tbCustCli.Text;
            setRepeatBtn(button3.Text);

        }

        private void tbCustCli_TextChanged(object sender, EventArgs e)
        {
            button3.Text = "&C " + tbCustSys.Text + "/" + tbCustCli.Text;
            setRepeatBtn(button3.Text);
        }

        private void tbCustCli_Validated(object sender, EventArgs e)
        {
            tbTransaction.Focus();
            rtbShortcutText.Text = initText;
            rtbShortcutText.Text = String.Format(rtbShortcutText.Text, tbCustSys.Text, tbCustCli.Text, tbCustSys.Text + " [SPACE]", "{0}", "{1}", "{2}");
            skip = false;
            sysName = tbCustSys.Text + "_" + tbCustCli.Text + "_";
            setRepeatBtn("1234" + tbCustSys.Text + "/" + tbCustCli.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tbCustSys.Focus();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            tbCustSys1.Focus();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tbCustSys2.Focus();
        }

        private void tbCustSys1_TextChanged(object sender, EventArgs e)
        {
            button4.Text = "&D  " + tbCustSys1.Text + "/" + tbCustCli1.Text;
            setRepeatBtn(button4.Text);
        }

        private void tbCustCli1_TextChanged(object sender, EventArgs e)
        {
            button4.Text = "&D  " + tbCustSys1.Text + "/" + tbCustCli1.Text;
            setRepeatBtn(button4.Text);
        }

        private void tbCustSys2_TextChanged(object sender, EventArgs e)
        {
            button5.Text = "&F  " + tbCustSys2.Text + "/" + tbCustCli2.Text;
        }

        private void tbCustCli2_TextChanged(object sender, EventArgs e)
        {
            button5.Text = "&F  " + tbCustSys2.Text + "/" + tbCustCli2.Text;
        }

        private void btnRepeat_Click(object sender, EventArgs e)
        {
            string s = string.Empty;
            if (cbHistory.SelectedItem as string != String.Empty)
            {
                rtbShortcutText.Text = initText;
                string sys = sysName.Substring(0, 3); string cli = sysName.Substring(4, 3);
                string cmd = cbHistory.SelectedItem.ToString();
                rtbShortcutText.Text = String.Format(rtbShortcutText.Text, sys, cli, sys +" [SPACE]", cmd , "", "");
                cmd = cmd.Replace("=", "_").Replace(";", ".");
                string runFile = tbPath.Text + "\\" + sysName + cmd + ".sap";

                if (File.Exists(runFile)) File.Delete(runFile);

                using (StreamWriter sr = File.AppendText(runFile))
                {
                    sr.WriteLine(rtbShortcutText.Text);
                    sr.Close();

                    //if (!cb1.Items.Contains(cmd))
                    //    cb1.Items.Add(cmd);
                    i++;
                }
                System.Diagnostics.Process.Start(runFile);
            }
        }

        private void cb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRepeat_Click(null, null);
        }

        private void setRepeatBtn(string text)
        {  // Text on repeat button, 1234 as delimiter, to keep W space and rest.
            btnRepeat.Text = "&W  "+ text.Remove(0,4);
        }

        private void tbCustCli1_Validated(object sender, EventArgs e)
        {
            tbTransaction.Focus();
            rtbShortcutText.Text = initText;
            rtbShortcutText.Text = String.Format(rtbShortcutText.Text, tbCustSys1.Text, tbCustCli1.Text, tbCustSys1.Text + " [SPACE]", "{0}", "{1}", "{2}");
            skip = false;
            sysName = tbCustSys1.Text + "_" + tbCustCli1.Text + "_";
            setRepeatBtn("1234" + tbCustSys1.Text + "/" + tbCustCli1.Text);
        }

        private void tbCustCli2_Validated(object sender, EventArgs e)
        {
            tbTransaction.Focus();
            rtbShortcutText.Text = initText;
            rtbShortcutText.Text = String.Format(rtbShortcutText.Text, tbCustSys2.Text, tbCustCli2.Text, tbCustSys2.Text + " [SPACE]", "{0}", "{1}", "{2}");
            skip = false;
            sysName = tbCustSys2.Text + "_" + tbCustCli2.Text + "_";
            setRepeatBtn("1234" + tbCustSys2.Text + "/" + tbCustCli2.Text);
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void IsEnter_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {  if (e.KeyCode == Keys.Enter)
                prepText();// enterPressed = true;  //Run file!
                else
                enterPressed = false;
            


        }
    }
}
