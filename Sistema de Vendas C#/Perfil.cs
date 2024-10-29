using Curso;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Perfil : Form
    {
        public Perfil()
        {
            InitializeComponent();
            PreencherCampos();
        }

        private void Tela_Paint(object sender, PaintEventArgs e)
        {
        }

        private void PreencherCampos()
        {
            string data_source = "Server=localhost;Database=projeto;Uid=root;Pwd=1234";
            string sql = "SELECT nome, email, bairro, cep, cpf, telefone, cidade, numero FROM funcionario WHERE email = @email";

            using (MySqlConnection conexao = new MySqlConnection(data_source))
            {
                conexao.Open();

                using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                {
                    // Adicionando o parâmetro
                    comando.Parameters.AddWithValue("@email", Global.Login);

                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Preenche os campos de texto com os valores recuperados do banco de dados
                            txtNome.Text = reader["nome"].ToString();
                            txtEmail.Text = reader["email"].ToString();
                            txtBairro.Text = reader["bairro"].ToString();
                            txtCep.Text = reader["cep"].ToString();
                            txtCPF.Text = reader["cpf"].ToString();
                            txtTelefone.Text = reader["telefone"].ToString();
                            txtCidade.Text = reader["cidade"].ToString();
                            txtNumeroDeCasa.Text = reader["numero"].ToString();
                        }
                        else
                        {
                            // Caso não encontre os dados, pode-se limpar os campos ou definir uma mensagem padrão
                            txtNome.Text = "";
                            txtEmail.Text = "";
                            txtBairro.Text = "";
                            txtCep.Text = "";
                            txtCPF.Text = "";
                            txtTelefone.Text = "";
                            txtCidade.Text = "";
                            txtNumeroDeCasa.Text = "";
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            TelaIni telaHome = new TelaIni();
            panel1.Controls.Add(telaHome);
            telaHome.Show();
        }

        private void nome_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
