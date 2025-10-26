<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalariosListagem.aspx.cs" Inherits="DesafioESIG.SalariosListagem" Async="true" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
        <div class="p-5">
            <h1>Listagem de Salários</h1>
            <div class="d-flex justify-content-end my-4">   
                <asp:Button ID="btnCalcularSalarios" runat="server" Text="Calcular/Recalcular Salários" OnClick="BtnCalcularSalarios_Click" 
                    CssClass="btn btn-primary align-items-end" />
            </div>

            <asp:Label ID="lblMensagem" runat="server" ForeColor="Green"></asp:Label> <%-- Para mensagens de sucesso/status --%>
            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label> <%-- Label de erro que já adicionamos --%>

            <div class="d-flex justify-content-between align-items-center mb-3 gap-3">
                <div class="d-flex gap-3 flex-grow-1">
                    <div class="col-md-3">
                        <asp:TextBox ID="txtBuscaNome" runat="server" CssClass="form-control" placeholder="Buscar por Nome..."></asp:TextBox>
                    </div>
                    <div class="col-md-6">
                        <asp:DropDownList ID="ddlBuscaCargo" runat="server" CssClass="form-select" DataSourceID="SqlDataSourceCargo" 
                            DataTextField="NOME" DataValueField="ID" AppendDataBoundItems="true">
                            <asp:ListItem Text="-- Todos os Cargos --" Value=""></asp:ListItem>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceCargo" runat="server" ConnectionString='<%$ ConnectionStrings:ConnectionString %>' 
                            ProviderName="Oracle.ManagedDataAccess.Client" SelectCommand='SELECT "ID", "NOME" FROM "CARGO"'></asp:SqlDataSource>
                    </div>
                </div>
                <div class="">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="BtnBuscar_Click" CssClass="btn btn-secondary w-100" />
                </div>
            </div>

            <asp:GridView ID="GridViewSalarios" runat="server"
                AutoGenerateColumns="false" 
                CssClass="table table-striped table-bordered" 
                EmptyDataText="Nenhum salário calculado encontrado."
                 AllowPaging="true">
                <Columns>
                    <asp:BoundField DataField="pessoa_id" HeaderText="ID Pessoa" />
                    <asp:BoundField DataField="pessoa_nome" HeaderText="Nome Pessoa" />
                    <asp:BoundField DataField="cargo_nome" HeaderText="Cargo" />
                    <asp:BoundField DataField="salario" HeaderText="Salário" DataFormatString="{0:C}" /> <%-- Formata como moeda --%>
                </Columns>
            </asp:GridView>

        </div>
        
</asp:Content>