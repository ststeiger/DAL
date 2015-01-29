
namespace DB.Abstraction
{


    public class OracleUniversalConnectionStringBuilder : UniversalConnectionStringBuilder
    {
        protected new System.Data.OracleClient.OracleConnectionStringBuilder m_ConnectionStringBuilder;


        public OracleUniversalConnectionStringBuilder()
        {
            this.m_ConnectionStringBuilder = new System.Data.OracleClient.OracleConnectionStringBuilder();
        }

        public OracleUniversalConnectionStringBuilder(string connectionString)
        {
            this.m_ConnectionStringBuilder = new System.Data.OracleClient.OracleConnectionStringBuilder(connectionString);
        }


        public override System.Data.Common.DbConnectionStringBuilder csb
        {
            get
            {
                return this.m_ConnectionStringBuilder;
            }
        }


        public override string Server
        {
            get
            {
                return this.m_ConnectionStringBuilder.DataSource;
            }
            set
            {
                this.m_ConnectionStringBuilder.DataSource = value;
            }
        }


        public override bool IntegratedSecurity
        {
            get
            {
                return this.m_ConnectionStringBuilder.IntegratedSecurity;
            }
            set
            {
                this.m_ConnectionStringBuilder.IntegratedSecurity = value;
            }
        }


        public override string UserName
        {
            get
            {
                return this.m_ConnectionStringBuilder.UserID;
            }
            set
            {
                this.m_ConnectionStringBuilder.UserID = value;
            }
        }


        public override string Password
        {
            get
            {
                return this.m_ConnectionStringBuilder.Password;
            }
            set
            {
                this.m_ConnectionStringBuilder.Password = value;
            }

        }


        public override bool IsReadOnly
        {
            get
            {
                return this.m_ConnectionStringBuilder.IsReadOnly;
            }
        }


        public override bool PersistSecurityInfo
        {
            get
            {
                return this.m_ConnectionStringBuilder.PersistSecurityInfo;
            }
            set
            {
                this.m_ConnectionStringBuilder.PersistSecurityInfo = value;
            }
        }


        public override bool IsFixedSize
        {
            get
            {
                return this.m_ConnectionStringBuilder.IsFixedSize;
            }
        }


        public override bool Pooling
        {
            get
            {
                return this.m_ConnectionStringBuilder.Pooling;
            }
            set
            {
                this.m_ConnectionStringBuilder.Pooling = value;
            }
        }


        public override int MinPoolSize
        {
            get
            {
                return this.m_ConnectionStringBuilder.MinPoolSize;
            }
            set
            {
                this.m_ConnectionStringBuilder.MinPoolSize = value;
            }
        }


        public override int MaxPoolSize
        {
            get
            {
                return this.m_ConnectionStringBuilder.MaxPoolSize;
            }
            set
            {
                this.m_ConnectionStringBuilder.MaxPoolSize = value;
            }
        }


        public override bool Enlist
        {
            get
            {
                return this.m_ConnectionStringBuilder.Enlist;
            }
            set
            {
                this.m_ConnectionStringBuilder.Enlist = value;
            }
        }


        public override int LoadBalanceTimeout
        {
            get
            {
                return this.m_ConnectionStringBuilder.LoadBalanceTimeout;
            }
            set
            {
                this.m_ConnectionStringBuilder.LoadBalanceTimeout = value;
            }
        }


        public override string ConnectionString
        {
            get
            {
                return this.m_ConnectionStringBuilder.ConnectionString;
            }
            set
            {
                this.m_ConnectionStringBuilder.ConnectionString = value;
            }

        }


    } // End Class 


}
