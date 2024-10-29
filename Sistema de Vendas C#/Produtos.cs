using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace Vendas_e_Produtos
{
    public partial class Produtos : Form
    {
        private MySqlConnection conexao;
        string data_source =  "Server=localhost;Database=projeto;Uid=root;Pwd=1234";
        public Produtos()
        {
            InitializeComponent();
        }

        

        private void Produtos_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void txtTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Cria uma nova conexão com o banco de dados usando a string de conexão fornecida
                using (conexao = new MySqlConnection(data_source))
                {
                    // Comando SQL para inserir dados na tabela "produto"
                    string sql = "INSERT INTO produto (nome, preco, quantidade, unidade_por_compra, tipo, marca, peso, altura, largura, descricao) " +
                                 "VALUES (@nome, @preco, @quantidade, @unidade_por_compra, @tipo, @marca, @peso, @altura, @largura, @descricao)";

                    // Cria um comando MySQL para executar o SQL
                    using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                    {
                        // Obtém o texto selecionado no ComboBox "txtTipo"
                        string tipoSelecionado = txtTipo.SelectedItem.ToString();

                        // Adiciona parâmetros ao comando SQL
                        comando.Parameters.AddWithValue("@nome", txtNome.Text);
                        comando.Parameters.AddWithValue("@preco", Convert.ToDecimal(txtPreco.Text));
                        comando.Parameters.AddWithValue("@quantidade", Convert.ToInt32(txtQuantidade.Text));
                        comando.Parameters.AddWithValue("@unidade_por_compra", Convert.ToInt32(txtUnidadePorCompra.Text));
                        comando.Parameters.AddWithValue("@tipo", tipoSelecionado);
                        comando.Parameters.AddWithValue("@marca", txtMarca.Text);
                        comando.Parameters.AddWithValue("@peso", Convert.ToDecimal(txtPeso.Text));
                        comando.Parameters.AddWithValue("@altura", Convert.ToDecimal(txtAltura.Text));
                        comando.Parameters.AddWithValue("@largura", Convert.ToDecimal(txtLargura.Text));
                        comando.Parameters.AddWithValue("@descricao", txtDescricao.Text);

                        // Abre a conexão com o banco de dados
                        conexao.Open();

                        // Executa o comando SQL para inserir os dados
                        comando.ExecuteNonQuery();
                    }

                    // Exibe uma mensagem informando que os dados foram inseridos com sucesso
                    MessageBox.Show("Dados Inseridos!");
                }
            }
            catch (Exception ex)
            {
                // Exibe uma mensagem de erro caso ocorra uma exceção
                MessageBox.Show("Ocorreu um erro: " + ex.Message);
            }
        }


    }
}
