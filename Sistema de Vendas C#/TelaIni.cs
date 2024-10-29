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
using MySql.Data.MySqlClient;

namespace Curso
{
    public partial class TelaIni : UserControl
    {
        public TelaIni()
        {
            InitializeComponent();
            configGrafico();
            configBox();
        }

        private void configGrafico()
        {
            gVendas.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.White;
            gVendas.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.White;
            gVendas.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            gVendas.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            gVendas.Series["Solicitações Atendidas"].Points.AddXY("Janeiro", 90);
            gVendas.Series["Solicitações Atendidas"].Points.AddXY("Fevereiro", 70);
            gVendas.Series["Solicitações Atendidas"].Points.AddXY("Março", 120);
            gVendas.BorderlineColor = Color.White;
        }

        private void configBox()
        {
            string data_source = "Server=localhost;Database=projeto;Uid=root;Pwd=1234";

            using (MySqlConnection conexao = new MySqlConnection(data_source))
            {
                conexao.Open();

                // Contar o número de clientes
                string sqlClientes = "SELECT COUNT(*) AS total FROM cliente";
                using (MySqlCommand comando = new MySqlCommand(sqlClientes, conexao))
                {
                    object result = comando.ExecuteScalar();
                    if (result != null)
                    {
                        int totalClientes = Convert.ToInt32(result);
                        txtCliente.Text = totalClientes.ToString();
                    }
                }

                // Contar o número de vendas realizadas
                string sqlVendas = "SELECT COUNT(*) AS total FROM historico_compras";
                using (MySqlCommand comando = new MySqlCommand(sqlVendas, conexao))
                {
                    object result = comando.ExecuteScalar();
                    if (result != null)
                    {
                        int totalVendas = Convert.ToInt32(result);
                        txtVendasRealizadas.Text = totalVendas.ToString();
                    }
                }

                // Calcular o lucro total das vendas
                string sqlLucro = "SELECT SUM(valor_total) AS total_lucro FROM historico_compras";
                using (MySqlCommand comando = new MySqlCommand(sqlLucro, conexao))
                {
                    object result = comando.ExecuteScalar();
                    if (result != null)
                    {
                        decimal totalLucro = result == DBNull.Value ? 0m : Convert.ToDecimal(result);
                        txtLucro.Text = totalLucro.ToString("C", new CultureInfo("pt-BR"));
                    }
                }

                // Contar o número de vendas realizadas na última hora
                string sqlVendasUltimaHora = "SELECT COUNT(*) AS total FROM historico_compras WHERE data_compra >= DATE_SUB(NOW(), INTERVAL 1 HOUR)";
                using (MySqlCommand comando = new MySqlCommand(sqlVendasUltimaHora, conexao))
                {
                    object result = comando.ExecuteScalar();
                    int totalVendasUltimaHora = result == null ? 0 : Convert.ToInt32(result);
                    txtVendasRealizadasNaUltimaHora.Text = totalVendasUltimaHora.ToString();
                }

                // Calcular o lucro obtido na última hora
                string sqlLucroUltimaHora = "SELECT SUM(valor_total) AS total_lucro_ultima_hora FROM historico_compras WHERE data_compra >= DATE_SUB(NOW(), INTERVAL 1 HOUR)";
                using (MySqlCommand comando = new MySqlCommand(sqlLucroUltimaHora, conexao))
                {
                    object result = comando.ExecuteScalar();
                    decimal totalLucroUltimaHora = result == DBNull.Value ? 0m : Convert.ToDecimal(result);
                    txtLucroObtidoNaUltimaHora.Text = totalLucroUltimaHora.ToString("C", new CultureInfo("pt-BR"));
                }

                conexao.Close();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
