<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalariosListagem.aspx.cs" Inherits="DesafioESIG.SalariosListagem" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Salarios</title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="p-5">
            <h1>Listagem de Salários</h1>
            <div class="d-flex justify-content-end my-4">
                <asp:Button ID="btnCalcularSalarios" runat="server" Text="Calcular/Recalcular Salários" OnClick="BtnCalcularSalarios_Click" 
                    CssClass="btn btn-primary align-items-end" />
            </div>

            <asp:Label ID="lblMensagem" runat="server" ForeColor="Green"></asp:Label> <%-- Para mensagens de sucesso/status --%>
            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label> <%-- Label de erro que já adicionamos --%>

            <asp:GridView ID="GridViewSalarios" runat="server"
                AutoGenerateColumns="false" 
                CssClass="table table-striped table-bordered" 
                EmptyDataText="Nenhum salário calculado encontrado.">
                <Columns>
                    <asp:BoundField DataField="pessoa_id" HeaderText="ID Pessoa" />
                    <asp:BoundField DataField="pessoa_nome" HeaderText="Nome Pessoa" />
                    <asp:BoundField DataField="cargo_nome" HeaderText="Cargo" />
                    <asp:BoundField DataField="salario" HeaderText="Salário" DataFormatString="{0:C}" /> <%-- Formata como moeda --%>
                </Columns>
            </asp:GridView>

        </div>
    </form>
</body>
</html>
