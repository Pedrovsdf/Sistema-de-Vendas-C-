using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Curso;

namespace Vendas_e_Produtos
{
    public partial class Vendas : Form
    {
        public Vendas()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            VendaAtual vendaA = new VendaAtual();
            panel1.Controls.Clear();
            vendaA.TopLevel = false;
            panel1.Controls.Add(vendaA);
            vendaA.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Devolução dev = new Devolução();
            panel1.Controls.Clear();
            dev.TopLevel = false;
            panel1.Controls.Add(dev);
            dev.Show();
        }
    }
}
