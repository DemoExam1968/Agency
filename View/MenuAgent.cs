using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Agency.View
{
    public partial class MenuAgent : Form
    {
        public MenuAgent()
        {
            InitializeComponent();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Работа с заявками
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWorkRequest_Click(object sender, EventArgs e)
        {
            View.WorkRequests workRequests = new View.WorkRequests();
            this.Hide();
            workRequests.ShowDialog();
            this.Show();
        }
    }
}
