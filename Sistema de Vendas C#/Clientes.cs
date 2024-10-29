using Curso;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Clientes : Form
    {
        public Clientes()
        {
            InitializeComponent();
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            CadastroC cadastroC = new CadastroC();
            panel1.Controls.Clear();
            cadastroC.TopLevel = false;
            panel1.Controls.Add(cadastroC);
            cadastroC.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Clientes_ativos clienteA = new Clientes_ativos();
            panel1.Controls.Clear();
            clienteA.TopLevel = false;
            panel1.Controls.Add(clienteA);
            clienteA.Show();
        }
    }
}
