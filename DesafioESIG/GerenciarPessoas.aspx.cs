//using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DesafioESIG
{
    public partial class GerenciarPessoas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnAdicionar_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string insertSql = @"INSERT INTO pessoa 
            (NOME, CIDADE, EMAIL, CEP, ENDERCO, PAIS, USUARIO, TELEFONE, DATA_NASCIMENTO, CARGO_ID) 
            VALUES 
            (:Nome, :Cidade, :Email, :Cep, :Enderco, :Pais, :Usuario, :Telefone, :DataNascimento, :CargoID)";

            using (OracleConnection con = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(insertSql, con))
                {
                    cmd.Parameters.Add(":Nome", OracleDbType.Varchar2).Value = txtNome.Text;
                    cmd.Parameters.Add(":Cidade", OracleDbType.Varchar2).Value = txtCidade.Text;
                    cmd.Parameters.Add(":Email", OracleDbType.Varchar2).Value = txtEmail.Text;
                    cmd.Parameters.Add(":Cep", OracleDbType.Varchar2).Value = txtCep.Text;
                    cmd.Parameters.Add(":Enderco", OracleDbType.Varchar2).Value = txtEndereco.Text;
                    cmd.Parameters.Add(":Pais", OracleDbType.Varchar2).Value = txtPais.Text;
                    cmd.Parameters.Add(":Usuario", OracleDbType.Varchar2).Value = txtUsuario.Text;
                    cmd.Parameters.Add(":Telefone", OracleDbType.Varchar2).Value = txtTelefone.Text;

                    if (DateTime.TryParse(txtDataNascimento.Text, out DateTime dataNasc))
                    {
                        cmd.Parameters.Add(":DataNascimento", OracleDbType.Date).Value = dataNasc;
                    }
                    else
                    {
                        cmd.Parameters.Add(":DataNascimento", OracleDbType.Date).Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(":CargoID", OracleDbType.Int32).Value = Convert.ToInt32(ddlCargo.SelectedValue);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();


                        gridPessoas.DataBind(); 
                    }
                    catch (Exception ex)
                    {
                        Response.Write("Erro ao adicionar: " + ex.Message);
                    }
                }
            }
        }

        protected void GridPessoas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridPessoas.PageIndex = e.NewPageIndex;

            BtnBuscarPessoa_Click(sender, e); 
        }

        protected void BtnBuscarPessoa_Click(object sender, EventArgs e)
        {
            string termo = txtBuscaPessoa.Text.Trim().ToUpper();

            SqlDataSourcePessoa.SelectParameters.Clear();

            if (string.IsNullOrWhiteSpace(termo))
            {
                SqlDataSourcePessoa.SelectCommand = "SELECT * FROM pessoa";
            }
            else
            {
                //o "OR" na consulta vai buscar tanto pelo nome como email, caso obtenha algum resultado ele retorna
                SqlDataSourcePessoa.SelectCommand = "SELECT * FROM pessoa WHERE UPPER(NOME) LIKE :termo OR UPPER(EMAIL) LIKE :termo";
                SqlDataSourcePessoa.SelectParameters.Add("termo", "%" + termo + "%");
            }

            gridPessoas.DataBind();
        }

        protected void GridViewPessoas_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridPessoas.EditIndex = e.NewEditIndex;
            gridPessoas.DataBind(); 
        }

        protected void GridViewPessoas_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gridPessoas.EditIndex = -1;
            gridPessoas.DataBind(); 
        }

        protected void GridViewPessoas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string pessoaId = gridPessoas.DataKeys[e.RowIndex].Value.ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string deleteFilhoSql = "DELETE FROM pessoa_salario WHERE pessoa_id = :Id";
            string deletePaiSql = "DELETE FROM pessoa WHERE ID = :Id";

            using (OracleConnection con = new OracleConnection(connectionString))
            {
                con.Open(); 

                using (OracleCommand cmdFilho = new OracleCommand(deleteFilhoSql, con))
                {
                    cmdFilho.Parameters.Add(":Id", OracleDbType.Int32).Value = Convert.ToInt32(pessoaId);
                    cmdFilho.ExecuteNonQuery(); 
                }

                using (OracleCommand cmdPai = new OracleCommand(deletePaiSql, con))
                {
                    cmdPai.Parameters.Add(":Id", OracleDbType.Int32).Value = Convert.ToInt32(pessoaId);
                    cmdPai.ExecuteNonQuery(); 
                }
            } 

            e.Cancel = true;

            gridPessoas.DataBind();
        }

        protected void GridViewPessoas_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string pessoaId = gridPessoas.DataKeys[e.RowIndex].Value.ToString();

            // Pega os novos valores das TextBoxes da linha
            
            TextBox txtNomeGrid = gridPessoas.Rows[e.RowIndex].Cells[2].Controls[0] as TextBox;
            TextBox txtCidadeGrid = gridPessoas.Rows[e.RowIndex].Cells[3].Controls[0] as TextBox;

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string updateSql = @"UPDATE pessoa SET NOME = :Nome, CIDADE = :Cidade, EMAIL = :Email, CEP = :Cep, 
                ENDERCO = :Enderco, PAIS = :Pais, USUARIO = :Usuario, TELEFONE = :Telefone, DATA_NASCIMENTO = :DataNascimento, CARGO_ID = :CargoID 
                WHERE ID = :Id";

            using (OracleConnection con = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(updateSql, con))
                {
                    cmd.Parameters.Add(":Nome", OracleDbType.Varchar2).Value = txtNomeGrid.Text;
                    cmd.Parameters.Add(":Cidade", OracleDbType.Varchar2).Value = txtCidadeGrid.Text;
                    cmd.Parameters.Add(":Email", OracleDbType.Varchar2).Value = txtEmail.Text;
                    cmd.Parameters.Add(":Cep", OracleDbType.Varchar2).Value = txtCep.Text;
                    cmd.Parameters.Add(":Enderco", OracleDbType.Varchar2).Value = txtEndereco.Text;
                    cmd.Parameters.Add(":Pais", OracleDbType.Varchar2).Value = txtPais.Text;
                    cmd.Parameters.Add(":Usuario", OracleDbType.Varchar2).Value = txtUsuario.Text;
                    cmd.Parameters.Add(":Telefone", OracleDbType.Varchar2).Value = txtTelefone.Text;

                    if (DateTime.TryParse(txtDataNascimento.Text, out DateTime dataNasc))
                    {
                        cmd.Parameters.Add(":DataNascimento", OracleDbType.Date).Value = dataNasc;
                    }
                    else
                    {
                        cmd.Parameters.Add(":DataNascimento", OracleDbType.Date).Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(":CargoID", OracleDbType.Int32).Value = Convert.ToInt32(ddlCargo.SelectedValue);

                    cmd.Parameters.Add(":Id", OracleDbType.Int32).Value = Convert.ToInt32(pessoaId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            gridPessoas.EditIndex = -1;
            e.Cancel = true;
            gridPessoas.DataBind();
        }
    }
}
