<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GerenciarPessoas.aspx.cs" Inherits="DesafioESIG.GerenciarPessoas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Nome" runat="server" Text="Nome"></asp:Label>
            <asp:TextBox ID="txtNome" runat="server"></asp:TextBox>
            
            <asp:Label ID="Cargo" runat="server" Text="Cargo"></asp:Label>
            <asp:DropDownList ID="ddlCargo" runat="server" DataSourceID="SqlDataSourceCargo" DataTextField="NOME" DataValueField="ID" AutoPostBack="True"></asp:DropDownList>

            <asp:Label ID="Cidade" runat="server" Text="Cidade"></asp:Label>
            <asp:TextBox ID="txtCidade" runat="server"></asp:TextBox>

            <asp:Label ID="Pais" runat="server" Text="País"></asp:Label>
            <asp:TextBox ID="txtPais" runat="server"></asp:TextBox>

            <asp:Label ID="Email" runat="server" Text="Email"></asp:Label>
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>

            <asp:Label ID="CEP" runat="server" Text="CEP"></asp:Label>
            <asp:TextBox ID="txtCep" runat="server"></asp:TextBox>

            <asp:Label ID="Endereco" runat="server" Text="Endereco"></asp:Label>
            <asp:TextBox ID="txtEndereco" runat="server"></asp:TextBox>

            <asp:Label ID="Telefone" runat="server" Text="Telefone"></asp:Label>
            <asp:TextBox ID="txtTelefone" runat="server"></asp:TextBox>

            <asp:Label ID="Usuario" runat="server" Text="Usuario"></asp:Label>
            <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>

            <asp:Label ID="DataNascimento" runat="server" Text="Data de Nascimento"></asp:Label>
            <asp:TextBox ID="txtDataNascimento" runat="server"></asp:TextBox>

            <asp:Button ID="btnAdicionar" runat="server" Text="Adicionar Pessoa" OnClick="BtnAdicionar_Click" />

            <asp:GridView ID="gridPessoas" runat="server" DataSourceID="SqlDataSourcePessoa" DataKeyNames="ID" 
                AllowPaging="True" AllowSorting="True" 
                OnRowEditing="GridViewPessoas_RowEditing"
                OnRowCancelingEdit="GridViewPessoas_RowCancelingEdit"
                OnRowUpdating="GridViewPessoas_RowUpdating" 
                OnRowDeleting="GridViewPessoas_RowDeleting">
                <Columns>
                    <asp:CommandField ShowCancelButton="True" ShowEditButton="True" ShowDeleteButton="True"></asp:CommandField>
                </Columns>
            </asp:GridView>

            <asp:SqlDataSource ID="SqlDataSourcePessoa" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
                ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" 
                SelectCommand="SELECT * FROM PESSOA">
            </asp:SqlDataSource>
                                
            <asp:SqlDataSource ID="SqlDataSourceCargo" runat="server" ConnectionString='<%$ ConnectionStrings:ConnectionString %>' ProviderName="Oracle.ManagedDataAccess.Client" SelectCommand='SELECT "ID", "NOME" FROM "CARGO"'></asp:SqlDataSource>

        </div>
    </form>
</body>
</html>
