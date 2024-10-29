using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Linq.Expressions;
using System.IO;
using System.Security.Cryptography;

namespace Curso
{
    public partial class Login : Form
    {
        Thread nt;
        private MySqlConnection conexao;
        string data_souce = "Server=localhost;Database=projeto;Uid=root;Pwd=1234";
        private readonly byte[] chave = GerarChave256Bits();
        private readonly byte[] iv = GerarIV();

        public Login()
        {
            InitializeComponent();
        }
        private void Tela1()
        {
            Application.Run(new TelaPrincipal());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = txtLogin.Text;
            string senha = txtSenha.Text;
            Global.Login = txtLogin.Text;
            
            try
            {

                using (conexao = new MySqlConnection(data_souce))
                {

                    string sql = "SELECT senha FROM funcionario WHERE email = @email";

                    MySqlCommand comando = new MySqlCommand(sql, conexao);

                    comando.Parameters.AddWithValue("@email", email);

                    conexao.Open();
                    object resultado = comando.ExecuteScalar();

                    if (resultado != null)
                    {

                        string senhaCriptografada = resultado.ToString();
                        string senhaDescriptografada = DescriptografarSenhaAes(senhaCriptografada);

                        if (senhaDescriptografada == senha)
                        {

                            this.Close();
                            nt = new Thread(Tela1);
                            nt.SetApartmentState(ApartmentState.STA);
                            nt.Start();


                        }
                        else
                        {
                            MessageBox.Show("Usuario ou Senha Incorreta!");
                        }

                    }
                    else
                    {
                        MessageBox.Show("Usuario ou Senha Incorreta!!!");
                    }
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show($"Erro:{ex.Message}\n{ex.StackTrace}");

            }
        }
        private string DescriptografarSenhaAes(string senhaCriptografada)
        {

            try
            {

                byte[] encryptBytesWitchIV = Convert.FromBase64String(senhaCriptografada);
                byte[] iv = new byte[16];
                byte[] encryptedBytes = new byte[encryptBytesWitchIV.Length - iv.Length];

                Buffer.BlockCopy(encryptBytesWitchIV, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(encryptBytesWitchIV, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

                using (Aes aesAlg = Aes.Create())
                {

                    aesAlg.Key = chave;
                    aesAlg.IV = iv;

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
                    {

                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }

                        }

                    }

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show($"ERRO na descriptografia:{ex.StackTrace}");
                return null;

            }

        }
        private static byte[] GerarChave256Bits()
        {

            string chaveString = "12345678901234567890123456789012";
            return Encoding.UTF8.GetBytes(chaveString);

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

        private void button2_Click(object sender, EventArgs e)
        {
            CadastroC cadastroForm = new CadastroC();
            cadastroForm.FormClosed += (s, args) => this.Close();
            cadastroForm.Show();
        }

    }
}

        
    

