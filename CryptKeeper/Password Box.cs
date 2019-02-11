using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace CryptKeeper
{
    public partial class PasswordBox : Form
    {
        public PasswordBox()
        {
            InitializeComponent();
        }

        internal string password;
        
        //Stores and Transfers Password
        internal string pwTransfer()
        {
            string pass;
            pass = textBox1.Text;
            return pass;
        }

        //OK Button - Stores Password
        internal void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == textBox2.Text && textBox1.Text != "")
            {
                password = pwTransfer();
                this.Hide();
            }
            else errorMessage();
        }

        //Displays Error Message Box
        internal void errorMessage()
        {
            if (textBox1.Text != textBox2.Text)
            {
                MessageBox.Show("Ensure Both Fields Match!", "Password Error!");
            }
            else if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Fields Must Not Be Empty!", "Password Error!");
            }
        }

        //Cancel Button
        internal void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
