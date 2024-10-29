using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Curso
{
    public partial class Cadastro : Form
    {
        private MySqlConnection conexao;
        string data_source = "Server=localhost;Database=projeto;Uid=root;Pwd=1234";
        private readonly byte[] chave = GerarChave256Bits();

        public Cadastro()
        {
            InitializeComponent();
        }

        // Método para navegar para a tela de Funcionários
        private void button2_Click(object sender, EventArgs e)
        {
            Funcionarios funcionarios = new Funcionarios();
            funcionarios.TopLevel = false;
            panel1.Controls.Clear();
            panel1.Controls.Add(funcionarios);
            funcionarios.Show();
        }

        // Método para criptografar a senha usando AES
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
                    }
                    byte[] encrypetedContent = msEncrypt.ToArray();
                    byte[] result = new byte[iv.Length + encrypetedContent.Length];
                    Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                    Buffer.BlockCopy(encrypetedContent, 0, result, iv.Length, encrypetedContent.Length);
                    return Convert.ToBase64String(result);
                }
            }
        }

        // Método para gerar IV (Initialization Vector)
        private static byte[] GerarIV()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] iv = new byte[16];
                rng.GetBytes(iv);
                return iv;
            }
        }

        // Método para gerar uma chave de 256 bits
        private static byte[] GerarChave256Bits()
        {
            byte[] chave = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            return chave;
        }

        // Método para validar a senha
        private bool ValidarSenha(string senha)
        {
            if (senha.Length < 8)
                return false;

            if (!Regex.IsMatch(senha, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$"))
                return false;

            for (int i = 0; i < senha.Length - 1; i++)
            {
                if (senha[i] == senha[i + 1])
                    return false;
            }
            return true;
        }

        // Evento de clique no botão para navegar para outra tela
        private void button4_Click(object sender, EventArgs e)
        {
            CadastroC cadastroForm = new CadastroC();
            cadastroForm.FormClosed += (s, args) => this.Close();
            cadastroForm.Show();
        }

        // Método para validar o formato do email
        private bool ValidarEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Evento de clique no botão para cadastrar
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Valida a senha
                string senha = txtSenha.Text;
                if (!ValidarSenha(senha))
                {
                    MessageBox.Show("A senha não atende aos critérios de segurança!" +
                                    "\nPelo menos 8 caracteres de comprimento." +
                                    "\nPelo menos uma letra maiúscula (A-Z)." +
                                    "\nPelo menos um dígito numérico (0-9)." +
                                    "\nPelo menos um caractere especial (@, &, $, !).");
                    return;
                }

                // Valida o email
                string email = txtEmail.Text;
                if (!ValidarEmail(email))
                {
                    MessageBox.Show("Por favor, insira um endereço de email válido.");
                    return;
                }

                // Criptografa a senha
                string senhaCriptografada = CriptografarSenhaAES(senha);

                // Limpa e formata o salário
                string salarioLimpo = txtSalario.Text
                    .Replace(",", "")
                    .Replace(" ", "")
                    .Replace("R$", "");

                // Comando SQL para inserção
                string sql = @"INSERT INTO funcionario (nome, email, senha, cpf, bairro, cep, cidade, numero, uf, carteira, telefone, cargo, aniversario, nivel_acesso, salario)
                       VALUES (@nome, @nomeProduto, @senha, @cpf, @bairro, @cep, @cidade, @numero, @uf, @carteira, @telefone, @cargo, @aniversario, @nivel_acesso, @salario)";

                using (var conexao = new MySqlConnection(data_source))
                using (var comando = new MySqlCommand(sql, conexao))
                {
                    // Adiciona parâmetros
                    comando.Parameters.AddWithValue("@nome", txtNome.Text);
                    comando.Parameters.AddWithValue("@email", email);
                    comando.Parameters.AddWithValue("@senha", senhaCriptografada);
                    comando.Parameters.AddWithValue("@cpf", txtCpf.Text);
                    comando.Parameters.AddWithValue("@bairro", txtBairro.Text);
                    comando.Parameters.AddWithValue("@cep", txtCep.Text);
                    comando.Parameters.AddWithValue("@cidade", txtCidade.Text);
                    comando.Parameters.AddWithValue("@numero", txtNumero.Text);
                    comando.Parameters.AddWithValue("@uf", txtUf.Text);
                    comando.Parameters.AddWithValue("@carteira", txtCarteira.Text);
                    comando.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                    comando.Parameters.AddWithValue("@cargo", txtCargo.Text);
                    comando.Parameters.AddWithValue("@aniversario", txtAniversario.Text);
                    comando.Parameters.AddWithValue("@nivel_acesso", txtNivelacesso.Text);
                    comando.Parameters.AddWithValue("@salario", salarioLimpo);

                    // Abre a conexão e executa o comando
                    conexao.Open();
                    comando.ExecuteNonQuery();

                    MessageBox.Show("Dados cadastrados com sucesso!");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Erro ao acessar o banco de dados: {ex.Message}\n{ex.StackTrace}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}
