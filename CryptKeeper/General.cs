using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CryptKeeper
{
    public partial class General : Form
    {
        public General()
        {
            InitializeComponent();
        }

        internal void button1_Click(object sender, EventArgs e)
        {
            Encrypt Crypt = new Encrypt();
            Crypt.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Decrypt Crypt = new Decrypt();
            Crypt.ShowDialog();
        }
    }
}
