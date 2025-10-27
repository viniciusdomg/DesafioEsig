using Oracle.ManagedDataAccess.Client; 
using System;
using System.Configuration;
using System.Data; 
using System.Threading.Tasks;
using System.Web.UI.WebControls;

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

        private void BindGrid(string filtroNome = null, string filtroCargoId = null)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            DataTable dt = new DataTable();

            using (OracleConnection con = new OracleConnection(connectionString))
            {
                string query = "SELECT pessoa_id, pessoa_nome, cargo_nome, salario FROM pessoa_salario ";

                var conditions = new System.Collections.Generic.List<string>();

                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    if (!string.IsNullOrWhiteSpace(filtroNome))
                    {
                        conditions.Add("UPPER(pessoa_nome) LIKE :filtroNome");
                        cmd.Parameters.Add(":filtroNome", "%" + filtroNome.ToUpper() + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(filtroCargoId))
                    {
                        conditions.Add("UPPER(cargo_nome) LIKE :filtroCargoNome");
                        string cargoNome = ddlBuscaCargo.SelectedItem.Text;
                        if (ddlBuscaCargo.SelectedValue != "")
                        {
                            cmd.Parameters.Add(":filtroCargoNome", "%" + cargoNome.ToUpper() + "%");
                        }
                    }

                    if (conditions.Count > 0)
                    {
                        query += " WHERE " + string.Join(" AND ", conditions);
                    }

                    query += " ORDER BY pessoa_nome";

                    cmd.CommandText = query;
                    cmd.Connection = con;

                    try
                    {
                        con.Open();
                        using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                    catch (Exception ex)
                    {
                        // TODO: Tratar o erro de forma mais robusta (ex: exibir mensagem amigável)
                        Console.WriteLine("Erro ao buscar dados: " + ex.Message);

                    }
                }
            }


            GridViewSalarios.DataSource = dt;
            GridViewSalarios.DataBind();
        }

        protected void GridViewSalarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewSalarios.PageIndex = e.NewPageIndex;

            BindGrid(txtBuscaNome.Text, ddlBuscaCargo.SelectedValue);
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            BindGrid(txtBuscaNome.Text, ddlBuscaCargo.SelectedValue);
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

                        Response.Redirect(Request.RawUrl, false);
                    }
                    catch (OracleException oraEx)
                    {
                        lblError.Text = $"Erro Oracle ao calcular salários: {oraEx.Message} (Erro Oracle #: {oraEx.Number})";
                        System.Diagnostics.Debug.WriteLine($"Erro Oracle ao calcular: {oraEx.Message} (Erro#: {oraEx.Number})");
                        BindGrid();
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Erro geral ao calcular salários: " + ex.Message;
                        System.Diagnostics.Debug.WriteLine("Erro geral ao calcular: " + ex.Message);
                        BindGrid();
                    }
                }
            }
        }
    }
}