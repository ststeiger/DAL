
namespace DB.Abstraction
{


    public class GenericUniversalConnectionStringBuilder : UniversalConnectionStringBuilder
    {

        public GenericUniversalConnectionStringBuilder()
        {
            this.m_ConnectionStringBuilder = new System.Data.Common.DbConnectionStringBuilder();
        }

        public GenericUniversalConnectionStringBuilder(string connectionString)
        {
            this.m_ConnectionStringBuilder = new System.Data.Common.DbConnectionStringBuilder();
            this.m_ConnectionStringBuilder.ConnectionString = connectionString;
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
                return (string) this.m_ConnectionStringBuilder["Application Name"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Application Name"] = value;
            }
        }


        public override string Server
        {
            get
            {
                return (string)this.m_ConnectionStringBuilder["Server"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Server"] = value;
            }
        }

        public override int Port
        {
            get
            {
                return (int)this.m_ConnectionStringBuilder["Port"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Port"] = value;
            }
        }


        public override string FailoverPartner
        {
            get
            {
                return (string)this.m_ConnectionStringBuilder["Failover Partner"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Failover Partner"] = value;
            }
        }


        public override string DataBase
        {
            get
            {
                return (string)this.m_ConnectionStringBuilder["Database"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Database"] = value;
            }
        }


        public override bool IntegratedSecurity
        {
            get
            {
                return (bool)this.m_ConnectionStringBuilder["Integrated Security"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Integrated Security"] = value;
            }
        }


        public override string UserName
        {
            get
            {
                return (string)this.m_ConnectionStringBuilder["UserName"];
            }
            set
            {
                this.m_ConnectionStringBuilder["UserName"] = value;
            }
        }

        public override string Password
        {
            get
            {
                return (string)this.m_ConnectionStringBuilder["Password"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Password"] = value;
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
                return (bool)this.m_ConnectionStringBuilder["TrustServerCertificate"];
            }
            set
            {
                this.m_ConnectionStringBuilder["TrustServerCertificate"] = value;
            }
        }


        public override bool SSL
        {
            get
            {
                return (bool)this.m_ConnectionStringBuilder["SSL"];
            }
            set
            {
                this.m_ConnectionStringBuilder["SSL"] = value;
            }
        }


        public override bool PersistSecurityInfo
        {
            get
            {
                return (bool)this.m_ConnectionStringBuilder["PersistSecurityInfo"];
            }
            set
            {
                this.m_ConnectionStringBuilder["PersistSecurityInfo"] = value;
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
                return (bool)this.m_ConnectionStringBuilder["Replication"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Replication"] = value;
            }
        }


        public override bool Pooling
        {
            get
            {
                return (bool)this.m_ConnectionStringBuilder["Pooling"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Pooling"] = value;
            }
        }


        public override int MinPoolSize
        {
            get
            {
                return (int)this.m_ConnectionStringBuilder["MinPoolSize"];
            }
            set
            {
                this.m_ConnectionStringBuilder["MinPoolSize"] = value;
            }
        }


        public override int MaxPoolSize
        {
            get
            {
                return (int)this.m_ConnectionStringBuilder["MaxPoolSize"];
            }
            set
            {
                this.m_ConnectionStringBuilder["MaxPoolSize"] = value;
            }
        }


        public override bool Enlist
        {
            get
            {
                return (bool)this.m_ConnectionStringBuilder["Enlist"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Enlist"] = value;
            }
        }


        public override bool PreloadReader
        {
            get
            {
                return (bool)this.m_ConnectionStringBuilder["PreloadReader"];
            }
            set
            {
                this.m_ConnectionStringBuilder["PreloadReader"] = value;
            }
        }


        // Gets flag indicating if we are using Synchronous notification or not. The default value is false.
        public override bool SyncNotification
        {
            get
            {
                return (bool)this.m_ConnectionStringBuilder["SyncNotification"];
            }
            set
            {
                this.m_ConnectionStringBuilder["SyncNotification"] = value;
            }
        }


        public override bool AsynchronousProcessing
        {
            get
            {
                return (bool)this.m_ConnectionStringBuilder["AsynchronousProcessing"];
            }
            set
            {
                this.m_ConnectionStringBuilder["AsynchronousProcessing"] = value;
            }
        }


        public override int PacketSize
        {
            get
            {
                return (int)this.m_ConnectionStringBuilder["PacketSize"];
            }
            set
            {
                this.m_ConnectionStringBuilder["PacketSize"] = value;
            }
        }


        // ConnectionTimeout=0 is bad, make it something reasonable like 30 seconds. 
        public override int ConnectTimeout
        {
            get
            {
                return (int)this.m_ConnectionStringBuilder["ConnectTimeout"];
            }
            set
            {
                this.m_ConnectionStringBuilder["ConnectTimeout"] = value;
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
                return (int)this.m_ConnectionStringBuilder["ConnectionLifeTime"];
            }
            set
            {
                this.m_ConnectionStringBuilder["ConnectionLifeTime"] = value;
            }
        }


        public override int CommandTimeout
        {
            get
            {
                return (int)this.m_ConnectionStringBuilder["CommandTimeout"];
            }
            set
            {
                this.m_ConnectionStringBuilder["CommandTimeout"] = value;
            }
        }


        public override int LoadBalanceTimeout
        {
            get
            {
                return (int)this.m_ConnectionStringBuilder["LoadBalanceTimeout"];
            }
            set
            {
                this.m_ConnectionStringBuilder["LoadBalanceTimeout"] = value;
            }
        }


        public override int MaxPageCount
        {
            get
            {
                return (int)this.m_ConnectionStringBuilder["MaxPageCount"];
            }
            set
            {
                this.m_ConnectionStringBuilder["MaxPageCount"] = value;
            }
        }


        public override int PageSize
        {
            get
            {
                return (int)this.m_ConnectionStringBuilder["PageSize"];
            }
            set
            {
                this.m_ConnectionStringBuilder["PageSize"] = value;
            }
        }


        public override int DbCachePages
        {
            get
            {
                return (int)this.m_ConnectionStringBuilder["DbCachePages"];
            }
            set
            {
                this.m_ConnectionStringBuilder["DbCachePages"] = value;
            }
        }


        public override int Dialect
        {
            get
            {
                return (int)this.m_ConnectionStringBuilder["Dialect"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Dialect"] = value;
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
                return (string)this.m_ConnectionStringBuilder["SearchPath"];
            }
            set
            {
                this.m_ConnectionStringBuilder["SearchPath"] = value;
            }

        }


        public override string CurrentLanguage
        {
            get
            {
                return (string)this.m_ConnectionStringBuilder["CurrentLanguage"];
            }
            set
            {
                this.m_ConnectionStringBuilder["CurrentLanguage"] = value;
            }

        }


        public override string WorkstationID
        {
            get
            {
                return (string)this.m_ConnectionStringBuilder["WorkstationID"];
            }
            set
            {
                this.m_ConnectionStringBuilder["WorkstationID"] = value;
            }

        }


        public override string ClientLibrary
        {
            get
            {
                return (string)this.m_ConnectionStringBuilder["ClientLibrary"];
            }
            set
            {
                this.m_ConnectionStringBuilder["ClientLibrary"] = value;
            }

        }


        public override string Charset
        {
            get
            {
                return (string)this.m_ConnectionStringBuilder["Charset"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Charset"] = value;
            }

        }


        public override string Role
        {
            get
            {
                return (string)this.m_ConnectionStringBuilder["Role"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Role"] = value;
            }

        }


        public override string Driver
        {
            get
            {
                return (string)this.m_ConnectionStringBuilder["Driver"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Driver"] = value;
            }

        }


        public override string Dsn
        {
            get
            {
                return (string)this.m_ConnectionStringBuilder["Dsn"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Dsn"] = value;
            }

        }


        public override string FileName
        {
            get
            {
                return (string)this.m_ConnectionStringBuilder["FileName"];
            }
            set
            {
                this.m_ConnectionStringBuilder["FileName"] = value;
            }

        }


        public override string Provider
        {
            get
            {
                return (string)this.m_ConnectionStringBuilder["Provider"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Provider"] = value;
            }

        }


        public override int OleDbServices
        {
            get
            {
                return (int)this.m_ConnectionStringBuilder["OleDbServices"];
            }
            set
            {
                this.m_ConnectionStringBuilder["OleDbServices"] = value;
            }
        }


        public override bool OmitOracleConnectionName
        {
            get
            {
                return (bool)this.m_ConnectionStringBuilder["OmitOracleConnectionName"];
            }
            set
            {
                this.m_ConnectionStringBuilder["OmitOracleConnectionName"] = value;
            }

        }


        public override bool Unicode
        {
            get
            {
                return (bool)this.m_ConnectionStringBuilder["Unicode"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Unicode"] = value;
            }

        }


        public override bool ContextConnection
        {
            get
            {
                return (bool)this.m_ConnectionStringBuilder["ContextConnection"];
            }
            set
            {
                this.m_ConnectionStringBuilder["ContextConnection"] = value;
            }

        }


        public override bool ReturnRecordsAffected
        {
            get
            {
                return (bool)this.m_ConnectionStringBuilder["ReturnRecordsAffected"];
            }
            set
            {
                this.m_ConnectionStringBuilder["ReturnRecordsAffected"] = value;
            }

        }



        public new static void Tests()
        {
            System.Data.Common.DbConnectionStringBuilder dbcsb = new System.Data.Common.DbConnectionStringBuilder();


            System.Data.SqlClient.SqlConnectionStringBuilder sqlcsb =
                new System.Data.SqlClient.SqlConnectionStringBuilder();

            sqlcsb.NetworkLibrary = "";
            //sqlcsb.TransactionBinding
            sqlcsb.TypeSystemVersion = "";
            sqlcsb.UserInstance = true;




            Npgsql.NpgsqlConnectionStringBuilder csb =
                new Npgsql.NpgsqlConnectionStringBuilder();

            // csb.Protocol
            // csb.Compatible
            //csb.SslMode = Npgsql.SslMode.Require;
            // csb.UseExtendedTypes

            System.Data.OracleClient.OracleConnectionStringBuilder ocb = new System.Data.OracleClient.OracleConnectionStringBuilder();

            FirebirdSql.Data.FirebirdClient.FbConnectionStringBuilder fbcsb = new FirebirdSql.Data.FirebirdClient.FbConnectionStringBuilder();
            fbcsb.ServerType = FirebirdSql.Data.FirebirdClient.FbServerType.Embedded;


            Mono.Data.Sqlite.SqliteConnectionStringBuilder litecsb = new Mono.Data.Sqlite.SqliteConnectionStringBuilder();
            litecsb.BinaryGUID = true;
            litecsb.DateTimeFormat = Mono.Data.Sqlite.SQLiteDateFormats.ISO8601;
            litecsb.DefaultIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            litecsb.DefaultTimeout = 123;
            litecsb.FailIfMissing = true;
            litecsb.JournalMode = Mono.Data.Sqlite.SQLiteJournalModeEnum.Off;
            litecsb.LegacyFormat = false;
            litecsb.SyncMode = Mono.Data.Sqlite.SynchronizationModes.Full;
            litecsb.Uri = "";

            litecsb.Version = 123;



            MySql.Data.MySqlClient.MySqlConnectionStringBuilder mycb = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder();
            mycb.AllowBatch = true;
            mycb.CertificateFile = "";
            mycb.CertificatePassword = "";
            mycb.CertificateStoreLocation = MySql.Data.MySqlClient.MySqlCertificateStoreLocation.LocalMachine;
            mycb.CertificateThumbprint = "";
            mycb.CharacterSet = "";
            mycb.CheckParameters = true;
            mycb.ConnectionLifeTime = 123;
            mycb.ConnectionProtocol = MySql.Data.MySqlClient.MySqlConnectionProtocol.Tcp;
            mycb.ConnectionReset = true;
            mycb.ConvertZeroDateTime = false;
            mycb.DefaultCommandTimeout = 123;
            mycb.FunctionsReturnString = true;
            mycb.IgnorePrepare = true;
            mycb.InteractiveSession = false;
            mycb.Keepalive = 123;
            mycb.PipeName = "";


            System.Data.Odbc.OdbcConnectionStringBuilder odbc = new System.Data.Odbc.OdbcConnectionStringBuilder();
            //System.Data.OleDb.OleDbConnectionStringBuilder oledb = new System.Data.OleDb.OleDbConnectionStringBuilder();



        } // End Sub 


    } // End Class 


}
