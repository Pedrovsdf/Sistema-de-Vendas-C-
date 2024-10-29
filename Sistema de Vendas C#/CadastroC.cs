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
using MySql.Data.MySqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Curso
{
    public partial class CadastroC : Form {
        private MySqlConnection conexao;
        string data_source = "Server=localhost;Database=projeto;Uid=root;Pwd=1234";
        private readonly byte[] chave = GerarChave256Bits();

        public CadastroC()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clientes clientes = new Clientes();
            panel1.Controls.Clear();
            clientes.TopLevel = false;
            panel1.Controls.Add(clientes);
            clientes.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                using (conexao = new MySqlConnection(data_source))
                {

                    string sql = "INSERT INTO cliente (nome,email,senha,cpf,bairro,cep,cidade,numero,uf,complemento,telefone)" +
                        "VALUES(@nome,@email,@senha,@cpf,@bairro,@cep,@cidade,@numero,@uf,@complemento,@telefone)";

                    string senha = txtSenha.Text;

                    using (MySqlCommand comando = new MySqlCommand(sql, conexao))
                    {
                        if (!ValidarSenha(senha))
                        {

                            MessageBox.Show("A senha nao atende aos criterios de Segurança!!!");
                            MessageBox.Show("Pelo menos 8 Caracteres de comprimento. \nPelo menos uma Letra Maiscula (A-Z).\nPelo menos um digito numerico (1,2,3,4 ....)  \n Pelo menos um caracter especial (@,&,$,!)");
                            return;

                        }
                        string senhaCriptografada = CriptografarSenhaAES(senha);

                        comando.Parameters.AddWithValue("@nome",txtNome.Text);
                        comando.Parameters.AddWithValue("@email",txtEmail.Text);
                        comando.Parameters.AddWithValue("@senha",senhaCriptografada);
                        comando.Parameters.AddWithValue("@cpf",txtCpf.Text);
                        comando.Parameters.AddWithValue("@bairro",txtBairro.Text);
                        comando.Parameters.AddWithValue("@cep",txtCep.Text);
                        comando.Parameters.AddWithValue("@cidade",txtCidade.Text);
                        comando.Parameters.AddWithValue("@numero",txtNumero.Text);
                        comando.Parameters.AddWithValue("@uf",txtUf.Text);
                        comando.Parameters.AddWithValue("@complemento",txtComplemento.Text);
                        comando.Parameters.AddWithValue("@telefone",txtTelefone.Text);

                        conexao.Open();
                        comando.ExecuteNonQuery();

                    }

                    MessageBox.Show("Dados Cadastrados!");

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show($"Erro{ex.Message}\n {ex.StackTrace}");

            }
            
        }
        private string CriptografarSenhaAES(string senha)
        {

            if (chave.Length != 32)


                throw new ArgumentException("A chave deve ter exatamente 32 bits");

            byte[] iv = GerarIV();

            using (Aes aesAlg = Aes.Create())
            {

                aesAlg.Key = chave;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {

                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(senha);
                        }

                        byte[] encrypetedContent = msEncrypt.ToArray();
                        byte[] result = new byte[iv.Length + encrypetedContent.Length];
                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(encrypetedContent, 0, result, iv.Length, encrypetedContent.Length);
                        return Convert.ToBase64String(result);
                    }

                }
            }

        }
        private static byte[] GerarIV()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] iv = new byte[16];
                rng.GetBytes(iv);
                return iv;
            }

        }

        private static Byte[] GerarChave256Bits()
        {

            byte[] chave = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            return chave;

        }
        private bool ValidarSenha(String senha)
        {

            if (senha.Length < 8)
            {
                return false;
            }
            if (!Regex.IsMatch(senha, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$"))
            {
                return false;
            }

            for (int i = 0; i < senha.Length - 1; i++)
            {

                if (senha[i] == senha[i + 1])
                {
                    return false;
                }

            }
            return true;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Cadastro cadastroForm = new Cadastro();
            cadastroForm.FormClosed += (s, args) => this.Close();
            cadastroForm.Show();
        }
    }
}
