using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Curso
{
    public partial class Funcionarios : Form
    {
        public Funcionarios()
        {
            InitializeComponent();
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Cadastro cadastroF = new Cadastro();
            panel1.Controls.Clear();
            cadastroF.TopLevel = false;
            panel1.Controls.Add(cadastroF);
            cadastroF.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            FuncionarioAtivos FuncionarioA = new FuncionarioAtivos();
            panel1.Controls.Clear();
            FuncionarioA.TopLevel = false;
            panel1.Controls.Add(FuncionarioA);
            FuncionarioA.Show();
        }
    }
}
