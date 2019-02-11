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
    public partial class Decrypt : Form
    {
        public Decrypt()
        {
            InitializeComponent();
        }

        string password;
        PasswordBox pwBox = new PasswordBox(); //Initializes Object Instance of Password Box

        //Shows OpenFileDialog on Decrypt Dialog Launch
        private void Decrypt_Load(object sender, EventArgs e)
        {
            //Calls Password Box and Prompts User to Enter Password
            DialogResult resultPw;
            do {
                pwBox = new PasswordBox();
                resultPw = pwBox.ShowDialog();
            } while (resultPw != DialogResult.Cancel && (pwBox.textBox1.Text == "" || pwBox.textBox2.Text == ""));

            if (resultPw != DialogResult.Cancel && pwBox.textBox1.Text == pwBox.textBox2.Text && pwBox.textBox1.Text != "")
            {
                //Sets Password
                password = pwBox.password;

                //Allows File Extension to be viewed on Browse Button Click
                generateKey();
                string fileType = password.Substring(4, 1) + password.Substring(6, 1) + password.Substring(0, 1) + password.Substring(2, 1) + password.Substring(3, 1);
                string s = fileType + " Files (*." + fileType + ")|*." + fileType;
                this.openFileDialog1.DefaultExt = s;
                this.openFileDialog1.Filter = s;

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
            //Calls Password Box and Prompts User to Enter Password
            DialogResult resultPw;
            do {
                pwBox = new PasswordBox();
                resultPw = pwBox.ShowDialog();
            } while (resultPw != DialogResult.Cancel && (pwBox.textBox1.Text == "" || pwBox.textBox2.Text == ""));

            if (resultPw != DialogResult.Cancel && pwBox.textBox1.Text == pwBox.textBox2.Text && pwBox.textBox1.Text != "")
            {
                //Sets Password
                password = pwBox.password;

                //Allows File Extension to be viewed on Browse Button Click
                generateKey();
                string fileType = password.Substring(4, 1) + password.Substring(6, 1) + password.Substring(0, 1) + password.Substring(2, 1) + password.Substring(3, 1);
                string s = fileType + " Files (*." + fileType + ")|*." + fileType;
                this.openFileDialog1.DefaultExt = s;
                this.openFileDialog1.Filter = s;

                //Shows openFileDialog
                DialogResult result = openFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                    textBox1.Text = openFileDialog1.FileName;
                else if (result == DialogResult.Cancel)
                    this.Close();
            }
            else this.Close();
        }

        #region Decryption

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

        //Decryption operations
        private void DecryptFile(string inputFile, string outputFile)
        {
            try
            {
                //generateKey Method Call
                byte[] key = generateKey();

                //Sets File Extention, based on Password
                string fileType = password.Substring(4, 1) + password.Substring(6, 1) + password.Substring(0, 1) + password.Substring(2, 1) + password.Substring(3, 1);
                string s = fileType + " Files (*." + fileType + ")|*." + fileType;
                openFileDialog1.Filter = s;

                //Crypto Operations
                string cryptFile = inputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Open);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(key, key), CryptoStreamMode.Read);
                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

                MessageBox.Show("Decryption Complete!", "DONE!");
                this.Close();
            }

            catch
            {
                MessageBox.Show("Decryption Failed!", "ERROR");
            }
        }

        //Calls DecryptFile Method on Button-Click
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                //Shows openFileDialog & Passes in File for Decryption
                DialogResult result = saveFileDialog1.ShowDialog();
                if (result == DialogResult.OK)//Checks for file existence
                {
                    string output = saveFileDialog1.FileName;//Variable filepath retrieval
                    DecryptFile(openFileDialog1.FileName, output);

                    GC.Collect();
                }
            }
            else MessageBox.Show("Select a File for Decryption First", "File Missing");
        }

        #endregion
    }
}
