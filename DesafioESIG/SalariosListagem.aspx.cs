using System;
using System.Data; 
using Oracle.ManagedDataAccess.Client; 
using System.Configuration;
using System.Threading.Tasks;

namespace DesafioESIG
{
    public partial class SalariosListagem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid(); 
            }
        }

        private void BindGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            DataTable dt = new DataTable();

            using (OracleConnection con = new OracleConnection(connectionString))
            {
                string query = "SELECT pessoa_id, pessoa_nome, cargo_nome, salario FROM pessoa_salario ORDER BY pessoa_nome";
                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        // Usa OracleDataAdapter para preencher um DataTable
                        using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                    catch (Exception ex)
                    {
                        // TODO: Tratar o erro de forma mais robusta (ex: exibir mensagem amigável)
                        Console.WriteLine("Erro ao buscar dados: " + ex.Message);
                        // Você pode adicionar um controle Label na página .aspx para mostrar erros
                        // lblErro.Text = "Erro ao carregar dados: " + ex.Message;
                    }
                }
            }


            GridViewSalarios.DataSource = dt;
            GridViewSalarios.DataBind(); 
        }

        protected async void BtnCalcularSalarios_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string procedureName = "PRC_CALCULAR_SALARIOS";
            lblError.Text = "";
            lblMensagem.Text = ""; 

            using (OracleConnection con = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(procedureName, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure; 

                    try
                    {
                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync(); 
                        lblMensagem.Text = "Salários calculados/recalculados com sucesso!";

                        // MUITO IMPORTANTE: Atualiza o GridView após a execução da procedure
                        BindGrid();
                    }
                    catch (OracleException oraEx)
                    {
                        lblError.Text = $"Erro Oracle ao calcular salários: {oraEx.Message} (Erro Oracle #: {oraEx.Number})";
                        System.Diagnostics.Debug.WriteLine($"Erro Oracle ao calcular: {oraEx.Message} (Erro#: {oraEx.Number})");
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Erro geral ao calcular salários: " + ex.Message;
                        System.Diagnostics.Debug.WriteLine("Erro geral ao calcular: " + ex.Message);
                    }
                }
            }
        }
    }
}