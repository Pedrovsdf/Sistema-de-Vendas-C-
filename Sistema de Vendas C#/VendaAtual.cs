using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Namespace para interação com o banco de dados MySQL.
using MySql.Data.MySqlClient; 

namespace Vendas_e_Produtos
{
    public partial class VendaAtual : Form
    {
        //Objeto MySqlConnection para a conexão com o banco de dados.
        private MySqlConnection conexao;
        //String de conexão com o banco de dados.
        string data_source = "Server=localhost;Database=projeto;Uid=root;Pwd=1234";

        //O construtor inicializa o componente e configura a lista de produtos comprados chamando configList.
        public VendaAtual()
        {
            InitializeComponent();
            configList();
        }

        private void configList()
        {
            //Permite que os itens sejam exibidos com várias colunas, similar a uma planilha.
            produtosComprados.View = View.Details;
            //Permite que os rótulos dos itens sejam editáveis diretamente na interface.
            produtosComprados.LabelEdit = true;
            //Permite que as colunas sejam reordenadas pelo usuário.
            produtosComprados.AllowColumnReorder = true;
            //Permite que a linha inteira seja selecionada quando um item é clicado, não apenas a célula.
            produtosComprados.FullRowSelect = true;
            //Exibe linhas de grade entre os itens e subitens, facilitando a leitura dos dados.
            produtosComprados.GridLines = true;
            /*Adiciona colunas ao ListView com os títulos "Nome", "Quantidade" e "Preço",
            cada uma com uma largura de 200 pixels.*/
            produtosComprados.Columns.Add("Nome", 200);
            produtosComprados.Columns.Add("Quantidade", 200);
            produtosComprados.Columns.Add("Preço", 200);
        }

        //Muda para o formulário Vendas quando o botão "Voltar" é clicado.
        private void Voltar_Click(object sender, EventArgs e)
        {
            Vendas vendas = new Vendas();
            panel1.Controls.Clear();
            vendas.TopLevel = false;
            panel1.Controls.Add(vendas);
            vendas.Show();
        }

        //3 Placeholders para eventos que não têm implementação no momento.
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
        /*
            Armazena os produtos comprados durante a sessão de venda. Cada elemento da lista é uma Tuple
            (tupla) que contém três valores:
            Nome do Produto (string): O nome do produto que foi comprado.
            Quantidade (int): A quantidade do produto que foi comprada.
            Preço (decimal): O preço unitário do produto.
        */
        private List<Tuple<string, int, decimal>> produtosCompradosArray = new List<Tuple<string, int, decimal>>();
        private decimal valorTotalCompra = 0;


        private void btnInserirProduto_Click(object sender, EventArgs e)
        {
            try
            {
                /*
                    Abertura da Conexão com o Banco de Dados:
                    Utiliza um bloco using para garantir que a conexão será fechada corretamente após o uso.
                    Abre a conexão com o banco de dados MySQL utilizando a string de conexão data_source.
                */
                using (conexao = new MySqlConnection(data_source))
                {
                    conexao.Open();

                    //Lê os valores dos campos de texto para CPF, nome do produto, preço e quantidade.
                    string cpfCliente = txtCPF.Text;
                    string nomeDoProduto = txtProduto.Text;
                    int quantidade;
                    decimal precoProduto;

                    //Verifica se a quantidade inserida é um número inteiro válido.
                    if (!Int32.TryParse(txtQuantidade.Text, out quantidade))
                    {
                        MessageBox.Show("Por favor, insira um valor numérico válido para a quantidade.");
                        return;
                    }

                    //Verifica se o cliente com o CPF fornecido existe no banco de dados.
                    if (!ClienteExiste(cpfCliente))
                    {
                        MessageBox.Show("Cliente não encontrado.");
                        return;
                    }

                    //Verifica se o produto existe e obtém o preço e a quantidade disponível em estoque.
                    if (!ProdutoExiste(nomeDoProduto, out precoProduto, out int quantidadeEstoque))
                    {
                        MessageBox.Show("Produto não existe ou não tem estoque.");
                        return;
                    }

                    // Verifica se a quantidade solicitada é maior do que a quantidade em estoque.
                    if (quantidade > quantidadeEstoque)
                    {
                        MessageBox.Show("A quantidade solicitada excede a quantidade disponível em estoque.");
                        return;
                    }

                    //Calcula o valor total do item com base no preço unitário e na quantidade.
                    decimal totalItem = precoProduto * quantidade;

                    /*
                        Verifica se o produto já está na lista de produtos comprados (ListView). 
                        Se estiver, atualiza a quantidade e o preço total. 
                        Caso contrário, adiciona o novo produto à lista.
                    */
                    bool produtoExistente = false;
                    foreach (ListViewItem item in produtosComprados.Items)
                    {
                        if (item.Text == nomeDoProduto)
                        {
                            int quantidadeAtual = int.Parse(item.SubItems[1].Text);
                            if (quantidadeAtual + quantidade > quantidadeEstoque)
                            {
                                MessageBox.Show("A quantidade total do item excede o estoque disponível.");
                                return;
                            }
                            quantidadeAtual += quantidade;
                            item.SubItems[1].Text = quantidadeAtual.ToString();
                            item.SubItems[2].Text = (precoProduto * quantidadeAtual).ToString("C", new CultureInfo("pt-BR"));

                            valorTotalCompra += totalItem;
                            txtTotalDaCompra.Text = valorTotalCompra.ToString("C", new CultureInfo("pt-BR"));

                            produtoExistente = true;
                            break;
                        }
                    }

                    if (!produtoExistente)
                    {
                        ListViewItem item = new ListViewItem(nomeDoProduto);
                        item.SubItems.Add(quantidade.ToString());
                        item.SubItems.Add(precoProduto.ToString("C", new CultureInfo("pt-BR")));
                        item.SubItems.Add((precoProduto * quantidade).ToString("C", new CultureInfo("pt-BR")));

                        //Adiciona o produto ao produtosCompradosArray
                        produtosComprados.Items.Add(item);

                        produtosCompradosArray.Add(Tuple.Create(nomeDoProduto, quantidade, precoProduto));

                        valorTotalCompra += totalItem;
                        txtTotalDaCompra.Text = valorTotalCompra.ToString("C", new CultureInfo("pt-BR"));
                    }

                    //Fecha a conexão com o banco de dados.
                    conexao.Close();
                }
            }
            //Caso ocorra algum erro durante a execução do método, exibe uma mensagem de erro.
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}\n{ex.StackTrace}");
            }
        }

        //Verifica se um cliente existe no banco de dados com base no CPF.
        private bool ClienteExiste(string cpf)
        {
            using (conexao = new MySqlConnection(data_source))
            {
                string sql = "SELECT COUNT(*) FROM cliente WHERE cpf = @cpf";
                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@cpf", cpf);
                    conexao.Open();
                    int count = Convert.ToInt32(comando.ExecuteScalar());
                    conexao.Close();
                    return count > 0;
                }
            }
        }

        //Verifica se um produto existe no banco de dados e obtém seu preço e quantidade em estoque.
        private bool ProdutoExiste(string nomeProduto, out decimal preco, out int quantidadeEstoque)
        {
            using (conexao = new MySqlConnection(data_source))
            {
                string sql = "SELECT preco, quantidade FROM produto WHERE nome = @nomeProduto";
                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                {
                    comando.Parameters.AddWithValue("@nomeProduto", nomeProduto);
                    conexao.Open();
                    using (MySqlDataReader leitor = comando.ExecuteReader())
                    {
                        if (leitor.Read())
                        {
                            preco = leitor.GetDecimal("preco");
                            quantidadeEstoque = leitor.GetInt32("quantidade");
                            conexao.Close();
                            return true;
                        }
                        else
                        {
                            preco = 0;
                            quantidadeEstoque = 0;
                            conexao.Close();
                            return false;
                        }
                    }
                }
            }
        }

        private void btnFinalizarCompra_Click(object sender, EventArgs e)
        {
            try
            {
                //Verifica se o valor recebido é suficiente.
                decimal valorRecebido;
                if (!decimal.TryParse(txtDinheiroRecebido.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out valorRecebido))
                {
                    MessageBox.Show("Por favor, insira um valor numérico válido para o dinheiro recebido.");
                    return;
                }

                if (valorRecebido < valorTotalCompra)
                {
                    MessageBox.Show("Dinheiro recebido é insuficiente.");
                    return;
                }

                //Calcula o troco.
                decimal troco = valorRecebido - valorTotalCompra;
                txtTroco.Text = troco.ToString("C", new CultureInfo("pt-BR"));

                // Adicionar a compra ao banco de dados
                AdicionarCompraAoBanco(txtCPF.Text, valorRecebido, troco);

                // Atualizar o estoque dos produtos comprados
                AtualizarEstoqueProdutos();

                // Exibir mensagem final
                MessageBox.Show("Compra finalizada com sucesso!");
                //Limpa os dados da compra atual.
                LimparCompra();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}\n{ex.StackTrace}");
            }
        }

        //Insere os detalhes da compra no banco de dados, incluindo os itens comprados em formato JSON.
        private void AdicionarCompraAoBanco(string cpfCliente, decimal dinheiroRecebido, decimal troco)
        {
            try
            {
                using (conexao = new MySqlConnection(data_source))
                {
                    conexao.Open();

                    /* 
                        A lista de itens comprados é convertida em uma string JSON manualmente
                        para ser armazenada no banco de dados.
                    */
                    var itensJson = new StringBuilder();
                    itensJson.Append("[");
                    foreach (ListViewItem item in produtosComprados.Items)
                    {
                        //Extrai o nome, quantidade, preço e valor total do item.
                        string nome = item.Text;
                        string quantidade = item.SubItems[1].Text;
                        string preco = item.SubItems[2].Text;
                        string valorTotalItem = item.SubItems[3].Text;

                        //Adiciona os detalhes do produto ao JSON.
                        itensJson.Append("{");
                        itensJson.AppendFormat("\"nome\": \"{0}\", ", nome);
                        itensJson.AppendFormat("\"quantidade\": \"{0}\", ", quantidade);
                        itensJson.AppendFormat("\"preço\": \"{0}\", ", preco);
                        itensJson.AppendFormat("\"valor_total\": \"{0}\"", valorTotalItem);
                        //Fecha o objeto JSON.
                        itensJson.Append("},");
                    }
                    if (itensJson.Length > 1) // Remove a última vírgula
                    {
                        itensJson.Length--;
                    }
                    itensJson.Append("]");

                    /*
                        Uma vez que a string JSON é criada, os dados da compra são 
                        inseridos no banco de dados usando um comando SQL INSERT.
                    */
                    string sql = @"
            INSERT INTO historico_compras (cpf_cliente, data_compra, valor_total, dinheiro_recebido, troco, lista_itens)
            VALUES (@cpf_cliente, @data_compra, @valor_total, @dinheiro_recebido, @troco, @lista_itens)";
                    using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                    {
                        //Adiciona os parâmetros ao comando SQL.
                        comando.Parameters.AddWithValue("@cpf_cliente", cpfCliente);
                        comando.Parameters.AddWithValue("@data_compra", DateTime.Now);
                        comando.Parameters.AddWithValue("@valor_total", valorTotalCompra);
                        comando.Parameters.AddWithValue("@dinheiro_recebido", dinheiroRecebido);
                        comando.Parameters.AddWithValue("@troco", troco);
                        comando.Parameters.AddWithValue("@lista_itens", itensJson.ToString());

                        //Executa o comando SQL para inserir os dados.
                        comando.ExecuteNonQuery();
                    }

                    conexao.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}\n{ex.StackTrace}");
            }
        }

        //Usa o produtosCompradosArray para atualizar a quantidade de produtos no banco de dados:
        private void AtualizarEstoqueProdutos()
        {
            try
            {
                using (conexao = new MySqlConnection(data_source))
                {
                    conexao.Open();

                    foreach (var produto in produtosCompradosArray)
                    {
                        string nomeProduto = produto.Item1;
                        int quantidadeComprada = produto.Item2;

                        string sql = "UPDATE produto SET quantidade = quantidade - @quantidade WHERE nome = @nomeProduto";

                        using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                        {
                            comando.Parameters.AddWithValue("@quantidade", quantidadeComprada);
                            comando.Parameters.AddWithValue("@nomeProduto", nomeProduto);
                            comando.ExecuteNonQuery();
                        }
                    }

                    conexao.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}\n{ex.StackTrace}");
            }
        }

        /*
            Após a finalização de uma compra, o método LimparCompra é chamado para 
            limpar os dados atuais da compra, incluindo o produtosCompradosArray:
        */
        private void LimparCompra()
        {
            produtosComprados.Items.Clear();
            produtosCompradosArray.Clear();
            valorTotalCompra = 0;
            txtTotalDaCompra.Text = "R$ 0,00";
            txtDinheiroRecebido.Text = "R$ 0,00";
            txtTroco.Text = "R$ 0,00";
            txtCPF.Clear();
            txtProduto.Clear();
            txtQuantidade.Clear();
        }
    }
}
