
namespace DB.Abstraction
{


    partial class cNewDBtype : cDAL
    {


        public cNewDBtype()
            : this("")
        {
            // Crap ! 
        } // End Constructor


        public cNewDBtype(string strConnectionString)
        {
            //this.m_DatabaseConfiguration = dbcDBconfig;
            this.m_dbtDBtype = DataBaseEngine_t.Generic;
            //this.m_providerFactory = this.GetFactory();
            this.m_dictScriptTemplates = GetSQLtemplates();
            this.m_dblDBversion = 8.0;
            //this.m_ConnectionString = new ConnectionStringBuilder(strConnectionString);
        }


        public System.Data.Common.DbProviderFactory GetFactory()
        {
            //System.Data.Common.DbProviderFactory providerFactory = null;

            throw new System.NotImplementedException("GetFactory not implemented");

            //providerFactory = System.Data.Common.DbProviderFactories.GetFactory("System.Data.Odbc");
            //return providerFactory;
        }


        public override System.Data.IDbConnection GetConnection()
        {
            throw new System.NotImplementedException("GetConnection not implemented");
            //Connection sqlc = new Connection(this.m_ConnectionString.ConnectionString);
            //return sqlc;
        }


    } // End Class cNewDBtype


} // End Namespace DataBase.Tools
