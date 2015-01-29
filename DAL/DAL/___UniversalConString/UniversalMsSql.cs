
namespace DB.Abstraction
{


    public class MS_SQLUniversalConnectionStringBuilder : UniversalConnectionStringBuilder
    {
        protected new System.Data.SqlClient.SqlConnectionStringBuilder m_ConnectionStringBuilder;


        public MS_SQLUniversalConnectionStringBuilder()
        {
            this.m_ConnectionStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder();
        }

        public MS_SQLUniversalConnectionStringBuilder(string connectionString)
        {
            this.m_ConnectionStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
        }


        public override System.Data.Common.DbConnectionStringBuilder csb
        {
            get
            {
                return this.m_ConnectionStringBuilder;
            }
        }


        public override string ApplicationName
        {
            get
            {
                return this.m_ConnectionStringBuilder.ApplicationName;
            }
            set
            {
                this.m_ConnectionStringBuilder.ApplicationName = value;
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


        //public override int Port
        //{
        //    get
        //    {
        //        return this.m_ConnectionStringBuilder.Port;
        //    }
        //    set
        //    {
        //        this.m_ConnectionStringBuilder.Port = value;
        //    }
        //}


        public override string FailoverPartner
        {
            get
            {
                return this.m_ConnectionStringBuilder.FailoverPartner;
            }
            set
            {
                this.m_ConnectionStringBuilder.FailoverPartner = value;
            }
        }


        public override string DataBase
        {
            get
            {
                return this.m_ConnectionStringBuilder.InitialCatalog;
            }
            set
            {
                this.m_ConnectionStringBuilder.InitialCatalog = value;
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


        public override bool TrustServerCertificate
        {
            get
            {
                return this.m_ConnectionStringBuilder.TrustServerCertificate;
            }
            set
            {
                this.m_ConnectionStringBuilder.TrustServerCertificate = value;
            }
        }


        public override bool SSL
        {
            get
            {
                return this.m_ConnectionStringBuilder.Encrypt;
            }
            set
            {
                this.m_ConnectionStringBuilder.Encrypt = value;
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



        public override bool Replication
        {
            get
            {
                return this.m_ConnectionStringBuilder.Replication;
            }
            set
            {
                this.m_ConnectionStringBuilder.Replication = value;
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


        public override bool AsynchronousProcessing
        {
            get
            {
                return this.m_ConnectionStringBuilder.AsynchronousProcessing;
            }
            set
            {
                this.m_ConnectionStringBuilder.AsynchronousProcessing = value;
            }
        }



        public override int PacketSize
        {
            get
            {
                return this.m_ConnectionStringBuilder.PacketSize;
            }
            set
            {
                this.m_ConnectionStringBuilder.PacketSize = value;
            }
        }



        // ConnectionTimeout=0 is bad, make it something reasonable like 30 seconds. 
        public override int ConnectTimeout
        {
            get
            {
                return this.m_ConnectionStringBuilder.ConnectTimeout;
            }
            set
            {
                this.m_ConnectionStringBuilder.ConnectTimeout = value;
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


        public override string CurrentLanguage
        {
            get
            {
                return this.m_ConnectionStringBuilder.CurrentLanguage;
            }
            set
            {
                this.m_ConnectionStringBuilder.CurrentLanguage = value;
            }

        }


        public override string WorkstationID
        {
            get
            {
                return this.m_ConnectionStringBuilder.WorkstationID;
            }
            set
            {
                this.m_ConnectionStringBuilder.WorkstationID = value;
            }

        }


    } // End Class 


}
