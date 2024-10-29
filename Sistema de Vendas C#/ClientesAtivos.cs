using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace Curso
{
    public partial class Clientes_ativos : Form
    {
        private MySqlConnection conexao;
        string data_source = "Server=localhost;Database=projeto;Uid=root;Pwd=1234";

        public Clientes_ativos()
        {
            InitializeComponent();
            ConfigurarListView();
        }

        private void ConfigurarListView()
        {
            listView1.View = View.Details;
            listView1.LabelEdit = true;
            listView1.AllowColumnReorder = true;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Columns.Add("ID", 250).TextAlign = HorizontalAlignment.Center;
            listView1.Columns.Add("Nome", 300).TextAlign = HorizontalAlignment.Center;
            listView1.Columns.Add("Email", 300).TextAlign = HorizontalAlignment.Center;
            listView1.Columns.Add("Senha", 300).TextAlign = HorizontalAlignment.Center;
            listView1.Columns.Add("Cpf", 300).TextAlign = HorizontalAlignment.Center;
            listView1.Columns.Add("Bairro", 300).TextAlign = HorizontalAlignment.Center;
            listView1.Columns.Add("CEP", 300).TextAlign = HorizontalAlignment.Center;
            listView1.Columns.Add("Cidade", 300).TextAlign = HorizontalAlignment.Center;
            listView1.Columns.Add("UF", 300).TextAlign = HorizontalAlignment.Center;
            listView1.Columns.Add("Numero", 300).TextAlign = HorizontalAlignment.Center;
            listView1.Columns.Add("Complemento", 300).TextAlign = HorizontalAlignment.Center;
            listView1.Columns.Add("Telefone", 250).TextAlign = HorizontalAlignment.Center;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clientes clientes = new Clientes();
            panel1.Controls.Clear();
            clientes.TopLevel = false;
            panel1.Controls.Add(clientes);
            clientes.Show();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {

                using (conexao = new MySqlConnection(data_source))
                {

                    string query = "%" + txtPesquisa.Text + "%";

                    string sql = "SELECT id, nome, email, senha, cpf, bairro, cep, cidade, uf, numero, complemento, telefone " +
                                 "FROM cliente WHERE nome LIKE @query";


                    conexao.Open();
                    MySqlCommand comando = new MySqlCommand(sql, conexao);
                    comando.Parameters.AddWithValue("@query", query);
                    MySqlDataReader reader = comando.ExecuteReader();

                    listView1.Items.Clear();
                    if (!reader.HasRows)
                    {

                        MessageBox.Show("Nenhum registro encontrado!");

                    }
                    while (reader.Read())
                    {

                        string id = reader.IsDBNull(0) ? string.Empty : reader.GetInt32(0).ToString();
                        string nome = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        string email = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        string senha = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        string cpf = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        string bairro = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                        string cep = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                        string cidade = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                        string uf = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        string numero = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                        string complemento = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
                        string telefone = reader.IsDBNull(11) ? string.Empty : reader.GetString(11);

                        string[] row = {id,nome,email,senha,cpf,bairro,cep,cidade,uf,numero,complemento,telefone};

                        var linha_Listview = new ListViewItem(row);
                        listView1.Items.Add(linha_Listview);

                    }

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show($"Erro: {ex.Message} \n {ex.StackTrace}");

            }
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
