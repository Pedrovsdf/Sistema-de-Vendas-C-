using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vendas_e_Produtos
{
    public partial class Estoque : Form
    {

        private MySqlConnection conexao;
        string data_source = "Server=localhost;Database=projeto;Uid=root;Pwd=1234";

        public Estoque()
        {
            InitializeComponent();
            CarregarListaDeProdutos();
        }

        private void CarregarListaDeProdutos()
        {
            try
            {
                // Cria uma nova conexão com o banco de dados usando a string de conexão fornecida
                using (conexao = new MySqlConnection(data_source))
                {
                    // Comando SQL para selecionar todos os produtos da tabela "produto"
                    string sql = "SELECT nome, preco, quantidade, unidade_por_compra, tipo, marca, peso, altura, largura, descricao FROM produto";

                    // Cria um comando MySQL para executar a consulta
                    using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                    {
                        // Abre a conexão com o banco de dados
                        conexao.Open();

                        // Executa a consulta e obtém um leitor de dados
                        using (MySqlDataReader leitor = comando.ExecuteReader())
                        {
                            // Configura o ListView
                            produtos.Items.Clear();
                            produtos.View = View.Details;
                            produtos.LabelEdit = false;
                            produtos.AllowColumnReorder = true;
                            produtos.FullRowSelect = true;
                            produtos.GridLines = true;

                            // Configura as colunas do ListView
                            produtos.Columns.Clear();
                            produtos.Columns.Add("Nome", 250);
                            produtos.Columns.Add("Preço", 100);
                            produtos.Columns.Add("Quantidade", 100);
                            produtos.Columns.Add("U. Compra", 100);
                            produtos.Columns.Add("Tipo", 150);
                            produtos.Columns.Add("Marca", 150);
                            produtos.Columns.Add("Peso", 100);
                            produtos.Columns.Add("Altura", 100);
                            produtos.Columns.Add("Largura", 100);
                            produtos.Columns.Add("Descrição", 200);

                            // Itera sobre os registros retornados pela consulta
                            while (leitor.Read())
                            {
                                // Cria um novo item para o ListView
                                ListViewItem item = new ListViewItem(leitor["nome"].ToString());

                                // Formata o preço como moeda BRL
                                string precoFormatado = Convert.ToDecimal(leitor["preco"]).ToString("C", new System.Globalization.CultureInfo("pt-BR"));

                                // Adiciona os subitens ao ListViewItem
                                item.SubItems.Add(precoFormatado);
                                item.SubItems.Add(leitor["quantidade"].ToString());
                                item.SubItems.Add(leitor["unidade_por_compra"].ToString());
                                item.SubItems.Add(leitor["tipo"].ToString());
                                item.SubItems.Add(leitor["marca"].ToString());
                                item.SubItems.Add(leitor["peso"].ToString());
                                item.SubItems.Add(leitor["altura"].ToString());
                                item.SubItems.Add(leitor["largura"].ToString());
                                item.SubItems.Add(leitor["descricao"].ToString());

                                // Adiciona o item ao ListView
                                produtos.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Exibe uma mensagem de erro caso ocorra uma exceção
                MessageBox.Show($"Erro: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void CarregarListaDeProdutos(string filtro = "")
        {
            try
            {
                using (conexao = new MySqlConnection(data_source))
                {
                    // Ajusta a consulta SQL para incluir um filtro baseado no texto da pesquisa
                    string sql = "SELECT nome, preco, quantidade, unidade_por_compra, tipo, marca, peso, altura, largura, descricao FROM produto";

                    // Adiciona o filtro à consulta se houver texto de pesquisa
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        sql += " WHERE nome LIKE @filtro OR descricao LIKE @filtro";
                    }

                    using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                    {
                        // Adiciona o parâmetro de filtro se houver texto de pesquisa
                        if (!string.IsNullOrEmpty(filtro))
                        {
                            comando.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                        }

                        conexao.Open();

                        using (MySqlDataReader leitor = comando.ExecuteReader())
                        {
                            // Configura o ListView
                            produtos.Items.Clear();
                            produtos.View = View.Details;
                            produtos.LabelEdit = false;
                            produtos.AllowColumnReorder = true;
                            produtos.FullRowSelect = true;
                            produtos.GridLines = true;

                            // Configura as colunas do ListView
                            produtos.Columns.Clear();
                            produtos.Columns.Add("Nome", 250);
                            produtos.Columns.Add("Preço", 100);
                            produtos.Columns.Add("Quantidade", 100);
                            produtos.Columns.Add("U. Compra", 100);
                            produtos.Columns.Add("Tipo", 150);
                            produtos.Columns.Add("Marca", 150);
                            produtos.Columns.Add("Peso", 100);
                            produtos.Columns.Add("Altura", 100);
                            produtos.Columns.Add("Largura", 100);
                            produtos.Columns.Add("Descrição", 200);

                            while (leitor.Read())
                            {
                                // Cria um novo item para o ListView
                                ListViewItem item = new ListViewItem(leitor["nome"].ToString());

                                // Formata o preço como moeda BRL
                                string precoFormatado = Convert.ToDecimal(leitor["preco"]).ToString("C", new System.Globalization.CultureInfo("pt-BR"));

                                item.SubItems.Add(precoFormatado);
                                item.SubItems.Add(leitor["quantidade"].ToString());
                                item.SubItems.Add(leitor["unidade_por_compra"].ToString());
                                item.SubItems.Add(leitor["tipo"].ToString());
                                item.SubItems.Add(leitor["marca"].ToString());
                                item.SubItems.Add(leitor["peso"].ToString());
                                item.SubItems.Add(leitor["altura"].ToString());
                                item.SubItems.Add(leitor["largura"].ToString());
                                item.SubItems.Add(leitor["descricao"].ToString());

                                produtos.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}\n{ex.StackTrace}");
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            // Obtém o texto da caixa de pesquisa
            string textoPesquisa = txtPesquisa.Text.Trim();

            // Atualiza a lista de produtos com base no texto da pesquisa
            CarregarListaDeProdutos(textoPesquisa);
        }


    }
    
}
