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

namespace Curso
{
    public partial class FuncionarioAtivos : Form
    {
        private MySqlConnection conexao;
        string data_source = "Server=localhost;Database=projeto;Uid=root;Pwd=1234";

        public FuncionarioAtivos()
        {
            InitializeComponent();
            ConfigurarListView();
            CarregarFuncionarios();
        }

        private void ConfigurarListView()
        {
            listaDeFuncionarios.View = View.Details;
            listaDeFuncionarios.LabelEdit = true;
            listaDeFuncionarios.AllowColumnReorder = true;
            listaDeFuncionarios.FullRowSelect = true;
            listaDeFuncionarios.GridLines = true;

            listaDeFuncionarios.Columns.Add("ID", 150);
            listaDeFuncionarios.Columns.Add("Nome", 200);
            listaDeFuncionarios.Columns.Add("Email", 200);
            listaDeFuncionarios.Columns.Add("Cpf", 200);
            listaDeFuncionarios.Columns.Add("Bairro", 200);
            listaDeFuncionarios.Columns.Add("CEP", 200);
            listaDeFuncionarios.Columns.Add("Cidade", 200);
            listaDeFuncionarios.Columns.Add("UF", 200);
            listaDeFuncionarios.Columns.Add("Numero", 200);
            listaDeFuncionarios.Columns.Add("Carteira", 200);
            listaDeFuncionarios.Columns.Add("Telefone", 200);
            listaDeFuncionarios.Columns.Add("Cargo", 200);
            listaDeFuncionarios.Columns.Add("Aniversario", 200);
            listaDeFuncionarios.Columns.Add("Nivel de Acesso", 200);
            listaDeFuncionarios.Columns.Add("Salario", 150);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Funcionarios funcionarios = new Funcionarios();
            funcionarios.TopLevel = false;
            panel1.Controls.Clear();
            panel1.Controls.Add(funcionarios);
            funcionarios.Show();
        }

        private void CarregarFuncionarios(string filtro = null)
        {
            try
            {
                using (conexao = new MySqlConnection(data_source))
                {
                    string sql = "SELECT id, nome, email, cpf, bairro, cep, cidade, uf, numero, carteira, telefone, cargo, aniversario, nivel_acesso, salario FROM funcionario";

                    if (!string.IsNullOrEmpty(filtro))
                    {
                        sql += " WHERE email LIKE @filtro OR cpf LIKE @filtro OR id LIKE @filtro";
                    }

                    conexao.Open();
                    MySqlCommand comando = new MySqlCommand(sql, conexao);

                    if (!string.IsNullOrEmpty(filtro))
                    {
                        comando.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                    }

                    MySqlDataReader reader = comando.ExecuteReader();

                    listaDeFuncionarios.Items.Clear();

                    if (!reader.HasRows)
                    {
                        MessageBox.Show("Nenhum registro encontrado!");
                        return;
                    }

                    while (reader.Read())
                    {
                        string id = reader.IsDBNull(0) ? string.Empty : reader.GetInt32(0).ToString();
                        string nome = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        string email = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        string cpf = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                        string bairro = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        string cep = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                        string cidade = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                        string uf = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                        string numero = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        string carteira = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                        string telefone = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
                        string cargo = reader.IsDBNull(11) ? string.Empty : reader.GetString(11);
                        string aniversario = reader.IsDBNull(12) ? string.Empty : reader.GetString(12);
                        int nivel_acesso = reader.IsDBNull(13) ? 0 : reader.GetInt32(13);
                        decimal salario = reader.IsDBNull(14) ? 0m : reader.GetDecimal(14);

                        string[] row = { id, nome, email, cpf, bairro, cep, cidade, uf, numero, carteira, telefone, cargo, aniversario, nivel_acesso.ToString(), salario.ToString("F2") };

                        var linha_Listview = new ListViewItem(row);
                        listaDeFuncionarios.Items.Add(linha_Listview);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message} \n {ex.StackTrace}");
            }
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            string filtro = txtPesquisa.Text.Trim();
            CarregarFuncionarios(filtro);
        }
    }
}
