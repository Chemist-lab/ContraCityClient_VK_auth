using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vk_login_v2
{
    public partial class EnterCodeForm : Form
    {
        public EnterCodeForm()
        {
            InitializeComponent();
        }

        public string Code { get { return textBox1.Text; } }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
