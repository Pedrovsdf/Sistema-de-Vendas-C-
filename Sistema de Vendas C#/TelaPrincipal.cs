using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Vendas_e_Produtos;
using SGV_Pojeto;
using WindowsFormsApp1;

namespace Curso
{
    public partial class TelaPrincipal : Form
    {
        public TelaPrincipal()
        {
            InitializeComponent();
        }

        private void btnEstoque_Click(object sender, EventArgs e)
        {
            Estoque Estoque = new Estoque();
            Tela.Controls.Clear();
            Estoque.TopLevel = false;
            Tela.Controls.Add(Estoque);
            Estoque.Show();

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            
            Tela.Controls.Clear();
            TelaIni telaHome = new TelaIni();
            Tela.Controls.Add(telaHome);
            telaHome.Show();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            Produtos Produtos = new Produtos();
            Tela.Controls.Clear();
            Produtos.TopLevel = false;
            Tela.Controls.Add(Produtos);
            Produtos.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Funcionarios Funcionario = new Funcionarios();
            Tela.Controls.Clear();
            Funcionario.TopLevel = false;
            Tela.Controls.Add(Funcionario);
            Funcionario.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Vendas Vendas = new Vendas();
            Tela.Controls.Clear();
            Vendas.TopLevel = false;
            Tela.Controls.Add(Vendas);
            Vendas.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Clientes Cliente = new Clientes();
            Tela.Controls.Clear();
            Cliente.TopLevel = false;
            Tela.Controls.Add(Cliente);
            Cliente.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            config Config = new config();
            Tela.Controls.Clear();
            Config.TopLevel = false;
            Tela.Controls.Add(Config);
            Config.Show();
        }

        private void btnLogo_Click(object sender, EventArgs e)
        {
            Perfil perfil = new Perfil();
            Tela.Controls.Clear();
            perfil.TopLevel = false;
            Tela.Controls.Add(perfil);
            perfil.Show();
        }
        private void login()
        {
            Application.Run(new Login());
        }

        private void TelaPrincipal_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
