﻿
namespace DB.Abstraction
{
    

    public class PostGreUniversalConnectionStringBuilder : UniversalConnectionStringBuilder
    {
        protected new Npgsql.NpgsqlConnectionStringBuilder m_ConnectionStringBuilder;


        public PostGreUniversalConnectionStringBuilder()
        {
            this.m_ConnectionStringBuilder = new Npgsql.NpgsqlConnectionStringBuilder();
        }

        public PostGreUniversalConnectionStringBuilder(string connectionString)
        {
            this.m_ConnectionStringBuilder = new Npgsql.NpgsqlConnectionStringBuilder(connectionString);
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
                return this.m_ConnectionStringBuilder.Host;
            }
            set
            {
                this.m_ConnectionStringBuilder.Host = value;
            }
        }

        public override int Port
        {
            get
            {
                return this.m_ConnectionStringBuilder.Port;
            }
            set
            {
                this.m_ConnectionStringBuilder.Port = value;
            }
        }



        public override string DataBase
        {
            get
            {
                return this.m_ConnectionStringBuilder.Database;
            }
            set
            {
                this.m_ConnectionStringBuilder.Database = value;
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
                return this.m_ConnectionStringBuilder.UserName;
            }
            set
            {
                this.m_ConnectionStringBuilder.UserName = value;
            }
        }

        public override string Password
        {
            get
            {
                // return this.m_ConnectionStringBuilder.Password;
                return "********";
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


        // sqlcsb.Encrypt = true;
        public override bool SSL
        {
            get
            {
                return this.m_ConnectionStringBuilder.SSL;
            }
            set
            {
                this.m_ConnectionStringBuilder.SSL = value;
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


        public override bool PreloadReader
        {
            get
            {
                return this.m_ConnectionStringBuilder.PreloadReader;
            }
            set
            {
                this.m_ConnectionStringBuilder.PreloadReader = value;
            }
        }


        // Gets flag indicating if we are using Synchronous notification or not. The default value is false.
        public override bool SyncNotification
        {
            get
            {
                return this.m_ConnectionStringBuilder.SyncNotification;
            }
            set
            {
                this.m_ConnectionStringBuilder.SyncNotification = value;
            }
        }


        // ConnectionTimeout=0 is bad, make it something reasonable like 30 seconds. 
        public override int ConnectTimeout
        {
            get
            {
                return this.m_ConnectionStringBuilder.Timeout;
            }
            set
            {
                this.m_ConnectionStringBuilder.Timeout = value;
            }
        }



        // how long a connection lives before it is killed and recreated. 
        // A lifetime of 0 means never kill and recreate. 
        // Through various bugs your connections may get stuck in an unstable state 
        // (like when dealing with weird 3 way transactions).. 
        // but 99% of the time it is good to keep connection lifetime as infinite.
        public override int ConnectionLifeTime
        {
            get
            {
                return this.m_ConnectionStringBuilder.ConnectionLifeTime;
            }
            set
            {
                this.m_ConnectionStringBuilder.ConnectionLifeTime = value;
            }
        }

        public override int CommandTimeout
        {
            get
            {
                return this.m_ConnectionStringBuilder.CommandTimeout;
            }
            set
            {
                this.m_ConnectionStringBuilder.CommandTimeout = value;
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


        public override string SearchPath
        {
            get
            {
                return this.m_ConnectionStringBuilder.SearchPath;
            }
            set
            {
                this.m_ConnectionStringBuilder.SearchPath = value;
            }

        }


    } // End Class 


}
