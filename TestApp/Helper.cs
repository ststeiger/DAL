
namespace TestApp
{


    class Helper
    {


        public class cbItem
        {
            public string Name;
            protected DB.Abstraction.UniversalConnectionStringBuilder m_csb;


            public DB.Abstraction.UniversalConnectionStringBuilder ConnectionStringBuilder
            {
                get
                {
                    return this.m_csb;
                }

            }

            public string Text
            {
                get
                {
                    if (m_csb == null)
                        return "ENGINE SERVER:Port/Database";

                    return string.Format("{0} [{1}]:  {2}{3}/{4}"
						,this.Name
						,m_csb.Engine.ToString()
						,m_csb.Server
						,m_csb.PortString
						,m_csb.DataBase
					);
                } // End Get
            }


            public cbItem() { }
            public cbItem(string name, DB.Abstraction.UniversalConnectionStringBuilder csb)
            {
                this.Name = name;
                this.m_csb = csb;
            }


            public override string ToString()
            {
                return Text;
            }
        }


        public static DB.Abstraction.UniversalConnectionStringBuilder GetDefaultCSB()
        {
            return GetDefaultCSB(DB.Abstraction.cDAL.DataBaseEngine_t.MS_SQL);
        }


        public static DB.Abstraction.UniversalConnectionStringBuilder GetDefaultCSB(DB.Abstraction.cDAL.DataBaseEngine_t dataBaseEngine)
        {
            DB.Abstraction.UniversalConnectionStringBuilder csb = DB.Abstraction.UniversalConnectionStringBuilder.CreateInstance(
                    dataBaseEngine
                );

            // OMG
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
                csb.Server = "127.0.0.1";
            else
                csb.Server = System.Environment.MachineName;

            switch (dataBaseEngine)
            {
                case DB.Abstraction.cDAL.DataBaseEngine_t.PostGreSQL:
                    csb.Port = 5432;
                    csb.DataBase = "postgres";
                    break;
                case DB.Abstraction.cDAL.DataBaseEngine_t.MySQL:
                    csb.Port = 3306;
                    break;
                case DB.Abstraction.cDAL.DataBaseEngine_t.MS_SQL:
                    csb.IntegratedSecurity = true;
                    csb.DataBase = "master";
                    break;
            } // End switch (dataBaseEngine) 

            if (!csb.IntegratedSecurity)
            {
                csb.UserName = "pgwebservices";
                csb.Password = "foobar2000";
            } // End if(!csb.IntegratedSecurity)

            return csb;
        }


    }


}
