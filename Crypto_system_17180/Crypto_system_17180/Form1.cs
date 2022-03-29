using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crypto_system_17180
{
    public partial class Form1 : Form
    {
        private SystemWatcher systemWatcher;

        public Form1()
        {
            InitializeComponent();
            systemWatcher = new SystemWatcher();
            this.buttonEncrypt.Enabled = false;
            this.buttonDecrypt.Enabled = false;
            this.comboBox1.SelectedIndex = 1;
        }

        private void BrowseFolder(TextBox tbResult)
        {
            DialogResult result = this.fbdBrowseFolder.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbResult.Text = fbdBrowseFolder.SelectedPath;
            }
        }

        private void EnableButtons(bool value)
        {
            this.buttonBrowseDest.Enabled = value;
            this.buttonSetDest.Enabled = value;
            this.buttonBrowseDecryptedDest.Enabled = value;
            this.buttonBrowseEncSource.Enabled = value;
            this.btnBrowseDecyptFile.Enabled = value;
            this.tbDecryptFilePath.Text = "";
            this.tbDecryptedDest.Text = "";
            this.tbEncodePath.Text = "";


        }

        private void buttonWatch_Click(object sender, EventArgs e)
        {
            if (!systemWatcher.SystemWatcherOn())
            {
                try
                {
                    systemWatcher.StartSystemWatcher();
                    buttonWatch.Text = "STOP Watcher";
                    this.EnableButtons(false);
                    MessageBox.Show("File Watcher System Info", "Starting File Watcher System started successfuly!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Starting File Watcher System failed!" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                systemWatcher.StopSystemWatcher();
                buttonWatch.Text = "START Watcher";
                this.EnableButtons(true);

            }
        }

        private void buttonBrowseDest_Click(object sender, EventArgs e)
        {
            this.BrowseFolder(this.tbDestinationPath);
            
        }

        private void buttonSetDest_Click(object sender, EventArgs e)
        {

            if (Directory.Exists(tbDestinationPath.Text))
            {
                string dest = tbDestinationPath.Text;
                this.systemWatcher.SetDestinationDirectory(tbDestinationPath.Text);
                MessageBox.Show("Destination directory Set!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Invalid directory name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void buttonBrowseEncSource_Click(object sender, EventArgs e)
        {
            this.listFilesToEncript.Items.Clear();
            this.BrowseFolder(this.tbEncodePath);
            if (Directory.Exists(tbEncodePath.Text))
            {
                this.buttonEncrypt.Enabled = true;
                foreach (string txtFile in Directory.GetFiles(tbEncodePath.Text, "*.txt"))
                    this.listFilesToEncript.Items.Add(txtFile);
            }
            else
            {
                this.buttonEncrypt.Enabled = false;
                MessageBox.Show("Invalid directory name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                
        }
                 
        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                this.systemWatcher.SetAlgorithm(this.comboBox1.SelectedIndex);
                this.systemWatcher.EncryptFolderFiles(tbEncodePath.Text);
                MessageBox.Show("Successful encoding!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBrowseDecyptFile_Click(object sender, EventArgs e)
        {
            this.ofdBrowseFile.Filter = "Text|*.txt";
            DialogResult result = this.ofdBrowseFile.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.tbDecryptFilePath.Text = this.ofdBrowseFile.FileName;
            }
        }

        private void buttonBrowseDecryptedDest_Click(object sender, EventArgs e)
        {
            this.BrowseFolder(this.tbDecryptedDest);
            this.buttonDecrypt.Enabled = true;
        }

        private void buttonDecrypt_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(tbDecryptedDest.Text) && File.Exists(tbDecryptFilePath.Text))
                try
                {
                    this.systemWatcher.DectyptTextFile(tbDecryptFilePath.Text, tbDecryptedDest.Text);
                    MessageBox.Show("Successful decoding!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            else
            {
                MessageBox.Show("Invalid file or directory name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.systemWatcher.SetAlgorithm(this.comboBox1.SelectedIndex);
        }

        private void pcbc_CheckedChanged(object sender, EventArgs e)
        {
            this.systemWatcher.PCBC_ON_OFF(this.pcbc.Checked);
            
        }
    }
}
