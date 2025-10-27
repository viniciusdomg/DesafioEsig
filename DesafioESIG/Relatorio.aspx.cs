using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Configuration;
using System.Web;
using System.Web.UI;

namespace DesafioESIG
{
    public partial class Relatorio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            var csBuilder = new System.Data.Common.DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };

            //string servidor = csBuilder["Data Source"].ToString();
            string usuario = csBuilder["User ID"].ToString();
            string senha = csBuilder["Password"].ToString();

            ReportDocument report = new ReportDocument();

            report.Load(Server.MapPath("~/RelatorioSalarios.rpt"));


            TableLogOnInfo logOnInfo = new TableLogOnInfo();

            logOnInfo.ConnectionInfo.ServerName = "DesafioESIG_ODBC";
            logOnInfo.ConnectionInfo.DatabaseName = "";
            logOnInfo.ConnectionInfo.UserID = usuario;
            logOnInfo.ConnectionInfo.Password = senha;

            foreach (CrystalDecisions.CrystalReports.Engine.Table table in report.Database.Tables)
            {
                System.Diagnostics.Debug.WriteLine("Aplicando logon para a tabela: " + table.Name);
                table.ApplyLogOnInfo(logOnInfo);
            }


            CrystalReportViewer1.ReportSource = report;
            CrystalReportViewer1.DataBind();
        }
    }
}