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
using Vendas_e_Produtos;

namespace Curso
{
    public partial class Devolução : Form
    {
        private MySqlConnection conexao;
        string data_source = "Server=localhost;Database=projeto;Uid=root;Pwd=1234";

        public Devolução()
        {
            InitializeComponent();
            txtNomeDoProduto.TextChanged += TxtNomeDoProduto_TextChanged; // Adiciona o evento TextChanged ao txtNomeDoProduto
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Vendas vendas = new Vendas();
            panel1.Controls.Clear();
            vendas.TopLevel = false;
            panel1.Controls.Add(vendas);
            vendas.Show();
        }

        private void btnDevolver_Click(object sender, EventArgs e)
        {
            string cpf = txtCpf.Text.Trim();
            string motivo = txtMotivo.Text.Trim();
            string nomeProduto = txtNomeDoProduto.Text.Trim();
            string valorProduto = txtValorDoProduto.Text.Trim();

            if (string.IsNullOrEmpty(cpf))
            {
                MessageBox.Show("Por favor, insira o CPF.");
                return;
            }

            if (string.IsNullOrEmpty(motivo))
            {
                MessageBox.Show("Por favor, insira o motivo da devolução.");
                return;
            }

            if (string.IsNullOrEmpty(valorProduto))
            {
                MessageBox.Show("Por favor, insira o valor do produto.");
                return;
            }

            try
            {
                using (conexao = new MySqlConnection(data_source))
                {
                    conexao.Open();

                    // Verificar se o usuário existe
                    string sqlVerificarUsuario = "SELECT COUNT(*) FROM cliente WHERE cpf = @cpf";
                    MySqlCommand comandoVerificarUsuario = new MySqlCommand(sqlVerificarUsuario, conexao);
                    comandoVerificarUsuario.Parameters.AddWithValue("@cpf", cpf);

                    int usuarioExistente = Convert.ToInt32(comandoVerificarUsuario.ExecuteScalar());

                    if (usuarioExistente == 0)
                    {
                        MessageBox.Show("Usuário não encontrado.");
                        return;
                    }

                    // Buscar o valor base e o estoque do produto
                    string sqlBuscar = "SELECT preco, quantidade FROM produto WHERE nome = @nomeProduto";
                    MySqlCommand comandoBuscar = new MySqlCommand(sqlBuscar, conexao);
                    comandoBuscar.Parameters.AddWithValue("@nomeProduto", nomeProduto);

                    MySqlDataReader reader = comandoBuscar.ExecuteReader();

                    if (reader.Read())
                    {
                        decimal valorBase = reader.IsDBNull(0) ? 0m : reader.GetDecimal(0);
                        int estoqueAtual = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);

                        txtValorDoProduto.Text = valorBase.ToString("F2");

                        reader.Close(); // Fechar o leitor antes de executar outro comando

                        // Atualizar o estoque do produto
                        string sqlAtualizar = "UPDATE produto SET quantidade = @novoEstoque WHERE nome = @nomeProduto";
                        MySqlCommand comandoAtualizar = new MySqlCommand(sqlAtualizar, conexao);
                        comandoAtualizar.Parameters.AddWithValue("@novoEstoque", estoqueAtual + 1);
                        comandoAtualizar.Parameters.AddWithValue("@nomeProduto", nomeProduto);

                        comandoAtualizar.ExecuteNonQuery();

                        txtDevolucaoAprovada.Text = "aprovada";
                        MessageBox.Show("Produto devolvido com sucesso! Estoque atualizado.");
                    }
                    else
                    {
                        MessageBox.Show("Produto não encontrado.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message} \n {ex.StackTrace}");
            }
        }

        private void TxtNomeDoProduto_TextChanged(object sender, EventArgs e)
        {
            string nomeProduto = txtNomeDoProduto.Text.Trim();

            if (string.IsNullOrEmpty(nomeProduto))
            {
                txtValorDoProduto.Text = string.Empty;
                return;
            }

            try
            {
                using (conexao = new MySqlConnection(data_source))
                {
                    conexao.Open();

                    // Buscar o valor base do produto
                    string sqlBuscar = "SELECT preco FROM produto WHERE nome = @nomeProduto";
                    MySqlCommand comandoBuscar = new MySqlCommand(sqlBuscar, conexao);
                    comandoBuscar.Parameters.AddWithValue("@nomeProduto", nomeProduto);

                    MySqlDataReader reader = comandoBuscar.ExecuteReader();

                    if (reader.Read())
                    {
                        decimal valorBase = reader.IsDBNull(0) ? 0m : reader.GetDecimal(0);
                        txtValorDoProduto.Text = valorBase.ToString("F2");
                    }
                    else
                    {
                        txtValorDoProduto.Text = string.Empty;
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Apenas exibe a mensagem de erro no MessageBox sem lançar uma exceção.
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }
    }
}
