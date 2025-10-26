<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GerenciarPessoas.aspx.cs" Inherits="DesafioESIG.GerenciarPessoas" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

        <div>
            <h3>Adicionar Nova Pessoa</h3>
            <div class="card p-3 my-4">
                <div class="row g-3">
                    <div class="col-md-6">
                        <asp:Label ID="Nome" runat="server" Text="Nome" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <asp:Label ID="Cargo" runat="server" Text="Cargo" CssClass="form-label"></asp:Label>
                        <asp:DropDownList ID="ddlCargo" runat="server" DataSourceID="SqlDataSourceCargo" 
                            DataTextField="NOME" DataValueField="ID" AutoPostBack="True" CssClass="form-select">
                            <asp:ListItem Text="-- Todos os Cargos --" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-6">
                        <asp:Label ID="Cidade" runat="server" Text="Cidade" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtCidade" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <asp:Label ID="Pais" runat="server" Text="País" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtPais" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <asp:Label ID="Email" runat="server" Text="Email" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <asp:Label ID="CEP" runat="server" Text="CEP" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtCep" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <asp:Label ID="Endereco" runat="server" Text="Endereco" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtEndereco" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <asp:Label ID="Telefone" runat="server" Text="Telefone" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtTelefone" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <asp:Label ID="Usuario" runat="server" Text="Usuario" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="col-md-6">
                        <asp:Label ID="DataNascimento" runat="server" Text="Data de Nascimento" CssClass="form-label"></asp:Label>
                        <asp:TextBox ID="txtDataNascimento" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>

                    <div class="col-12">
                        <asp:Button ID="btnAdicionar" runat="server" Text="Adicionar Pessoa" OnClick="BtnAdicionar_Click" 
                             CssClass="shadow-sm rounded-3" />
                    </div>
                </div>
            </div>

            <div class="row g-3 mb-3">
                <div class="col-md-10">
                    <asp:TextBox ID="txtBuscaPessoa" runat="server" CssClass="form-control" placeholder="Buscar por Nome ou Email..."></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btnBuscarPessoa" runat="server" Text="Buscar" OnClick="BtnBuscarPessoa_Click" CssClass="btn btn-secondary w-100" />
                </div>
            </div>

            <asp:GridView ID="gridPessoas" runat="server" DataSourceID="SqlDataSourcePessoa" DataKeyNames="ID"
                CssClass="table table-striped table-bordered" 
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
                                
            <asp:SqlDataSource ID="SqlDataSourceCargo" runat="server" ConnectionString='<%$ ConnectionStrings:ConnectionString %>' 
                 ProviderName="Oracle.ManagedDataAccess.Client" SelectCommand='SELECT "ID", "NOME" FROM "CARGO"'></asp:SqlDataSource>
                
        </div>
</asp:Content>