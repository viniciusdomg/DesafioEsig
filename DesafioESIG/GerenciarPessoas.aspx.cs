//using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
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

                        // Limpa os campos e atualiza o Grid
                        // (coloque aqui seu código para limpar os textboxes)

                        gridPessoas.DataBind(); // ID do seu GridView
                    }
                    catch (Exception ex)
                    {
                        Response.Write("Erro ao adicionar: " + ex.Message);
                    }
                }
            }
        }

        protected void GridViewPessoas_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Esta linha diz ao Grid para entrar no modo de edição para a linha clicada
            gridPessoas.EditIndex = e.NewEditIndex;
            gridPessoas.DataBind(); 
        }

        protected void GridViewPessoas_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Esta linha diz ao Grid para sair do modo de edição
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

                // 2. Crie e execute o COMANDO PARA EXCLUIR O FILHO (pessoa_salario)
                using (OracleCommand cmdFilho = new OracleCommand(deleteFilhoSql, con))
                {
                    cmdFilho.Parameters.Add(":Id", OracleDbType.Int32).Value = Convert.ToInt32(pessoaId);
                    cmdFilho.ExecuteNonQuery(); // <-- EXCLUI O FILHO PRIMEIRO
                }

                // 3. Crie e execute o COMANDO PARA EXCLUIR O PAI (pessoa)
                using (OracleCommand cmdPai = new OracleCommand(deletePaiSql, con))
                {
                    cmdPai.Parameters.Add(":Id", OracleDbType.Int32).Value = Convert.ToInt32(pessoaId);
                    cmdPai.ExecuteNonQuery(); 
                }
            } 

            // Diz ao GridView que já cuidamos da exclusão
            e.Cancel = true;

            // Atualiza o Grid
            gridPessoas.DataBind();
        }

        protected void GridViewPessoas_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Pega o ID da linha que está sendo editada
            string pessoaId = gridPessoas.DataKeys[e.RowIndex].Value.ToString();

            // Pega os novos valores das TextBoxes da linha
            // IMPORTANTE: O 'FindControl' é sensível ao nome. O GridView geralmente 
            // coloca os BoundFields em TextBoxes com o ID "TextBox1", "TextBox2", etc.
            // É *MUITO MELHOR* usar TemplateFields.

            // Se você estiver usando BoundField, o ID pode ser "TextBox2" para a segunda coluna (NOME)
            TextBox txtNomeGrid = gridPessoas.Rows[e.RowIndex].Cells[2].Controls[0] as TextBox;
            TextBox txtCidadeGrid = gridPessoas.Rows[e.RowIndex].Cells[3].Controls[0] as TextBox;
            // ... e assim por diante para todas as colunas

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            // Crie seu comando UPDATE com os parâmetros :
            string updateSql = "UPDATE pessoa SET NOME = :Nome, CIDADE = :Cidade /*... adicione TODOS os campos ...*/ WHERE ID = :Id";

            using (OracleConnection con = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(updateSql, con))
                {
                    cmd.Parameters.Add(":Nome", OracleDbType.Varchar2).Value = txtNomeGrid.Text;
                    cmd.Parameters.Add(":Cidade", OracleDbType.Varchar2).Value = txtCidadeGrid.Text;
                    // ... Adicione os parâmetros para TODOS os seus campos (CEP, EMAIL, etc.) ...

                    cmd.Parameters.Add(":Id", OracleDbType.Int32).Value = Convert.ToInt32(pessoaId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // Sai do modo de edição e atualiza o Grid
            gridPessoas.EditIndex = -1;
            e.Cancel = true;
            gridPessoas.DataBind();
        }
    }
}