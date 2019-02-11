using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace CryptKeeper
{
    public partial class Encrypt : Form
    {
        public Encrypt()
        {
            InitializeComponent();
        }

        string password;
        PasswordBox pwBox = new PasswordBox(); //Initializes Object Instance of Password Box

        //Shows OpenFileDialog on Encrypt Dialog Launch
        private void Encrypt_Load(object sender, EventArgs e)
        {
            //Calls Password Box and Prompts User to Create Password
            DialogResult resultPw;
            do {
                pwBox = new PasswordBox();
                pwBox.Text = "Create Password";
                resultPw = pwBox.ShowDialog();
            } while (resultPw != DialogResult.Cancel && (pwBox.textBox1.Text != pwBox.textBox2.Text || pwBox.textBox1.Text == "" || pwBox.textBox2.Text == ""));

            if (resultPw != DialogResult.Cancel && pwBox.textBox1.Text == pwBox.textBox2.Text && pwBox.textBox1.Text != "")
            {
                //Sets Password
                password = pwBox.password;

                //Allows File Extension to be viewed on Encrypt Button-Click in saveFileDialog
                generateKey();
                string fileType = password.Substring(4, 1) + password.Substring(6, 1) + password.Substring(0, 1) + password.Substring(2, 1) + password.Substring(3, 1);
                string s = fileType + " Files (*." + fileType + ")|*." + fileType;
                this.saveFileDialog1.DefaultExt = s;
                this.saveFileDialog1.Filter = s;

                //Shows openFileDialog
                DialogResult result = openFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                    textBox1.Text = openFileDialog1.FileName;
                else if (result == DialogResult.Cancel)
                    this.Close();
            }
            else this.Close();
        }

        //Prevents User From Changing Text in TextBox
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = openFileDialog1.FileName;
        }

        //Browse Button
        private void button1_Click(object sender, EventArgs e)
        {
            //Calls Password Box and Prompts User to Create Password
            DialogResult resultPw;
            do {
                pwBox = new PasswordBox();
                pwBox.Text = "Create Password";
                resultPw = pwBox.ShowDialog();
            } while (resultPw != DialogResult.Cancel && (pwBox.textBox1.Text == "" || pwBox.textBox2.Text == ""));

            if (resultPw != DialogResult.Cancel && pwBox.textBox1.Text == pwBox.textBox2.Text && pwBox.textBox1.Text != "")
            {
                //Sets Password
                password = pwBox.password;

                //Allows File Extension to be viewed on Encrypt Button-Click in saveFileDialog
                generateKey();
                string fileType = password.Substring(4, 1) + password.Substring(6, 1) + password.Substring(0, 1) + password.Substring(2, 1) + password.Substring(3, 1);
                string s = fileType + " Files (*." + fileType + ")|*." + fileType;
                this.saveFileDialog1.DefaultExt = s;
                this.saveFileDialog1.Filter = s;

                //Shows openFileDialog
                DialogResult result = openFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                    textBox1.Text = openFileDialog1.FileName;
                else if (result == DialogResult.Cancel)
                    this.Close();
            }
            else this.Close();
        }

        #region Encryption

        //Key Generation
        internal byte[] generateKey()
        {            
            while (password.Length < 8 && password != null && password != "")
                password += password;
            password = password.Substring(0, 8);

            byte[] key = null;
            UnicodeEncoding UE = new UnicodeEncoding();
            key = UE.GetBytes(password);

            return key;
        }

        //Encryption Operations
        internal void EncryptFile(string inputFile, string outputFile)
        {
            try
            {
                //generateKey Method Call
                byte[] key = generateKey();

                //Sets File Extention, Based on Password
                this.saveFileDialog1.DefaultExt = password.Substring(4, 1) + password.Substring(6, 1) + password.Substring(0, 1) + password.Substring(2, 1) + password.Substring(3, 1);
                string fileType = password.Substring(4, 1) + password.Substring(6, 1) + password.Substring(0, 1) + password.Substring(2, 1) + password.Substring(3, 1);
                string s = fileType + " Files (*." + fileType + ")|*." + fileType;
                saveFileDialog1.Filter = s;

                //Crypto Operations
                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(outputFile, FileMode.Create);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateEncryptor(key, key), CryptoStreamMode.Write);
                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);

                fsIn.Close();
                cs.Close();
                fsCrypt.Close();

                MessageBox.Show("Ecryption Complete!", "DONE!");
                this.Close();
            }
            catch
            {
                MessageBox.Show("Encryption Failed!", "ERROR");
            }
        }

        //Calls EncryptFile Method on Button-Click
        internal void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                //Shows openFileDialog & Passes in File for Encryption
                DialogResult result = saveFileDialog1.ShowDialog();
                if (result == DialogResult.OK)//Checks for file existence
                {
                    string output = saveFileDialog1.FileName;//Variable filepath retrieval
                    EncryptFile(openFileDialog1.FileName, output);

                    GC.Collect();
                }                
            }
            else MessageBox.Show("Select a File for Encryption First", "File Missing");
        }

        #endregion
    }
}
