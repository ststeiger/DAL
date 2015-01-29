using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp
{
    class Helper
    {

        // Helper.GetDefaultSQL(DAL.DBtype);
        public static string GetDefaultSQL(DB.Abstraction.cDAL.DataBaseEngine_t dbEngine)
        {

            switch (dbEngine)
            {
                case DB.Abstraction.cDAL.DataBaseEngine_t.PostGreSQL:
                    return @"";
                case DB.Abstraction.cDAL.DataBaseEngine_t.MySQL:
                    return @"";
                case DB.Abstraction.cDAL.DataBaseEngine_t.MS_SQL:
                    return @"
SELECT 
	 name
	,database_id
	,create_date
	,compatibility_level
	,collation_name
	,user_access
	,user_access_desc
	,is_read_only
	,is_auto_close_on
	,is_auto_shrink_on
	,state
	,state_desc
	,is_in_standby
	,is_cleanly_shutdown
	,is_supplemental_logging_enabled
	,snapshot_isolation_state
	,snapshot_isolation_state_desc
	,is_read_committed_snapshot_on
	,recovery_model
	,recovery_model_desc
	,page_verify_option
	,page_verify_option_desc
	,is_auto_create_stats_on
	,is_auto_update_stats_on
	,is_auto_update_stats_async_on
	,is_ansi_null_default_on
	,is_ansi_nulls_on
	,is_ansi_padding_on
	,is_ansi_warnings_on
	,is_arithabort_on
	,is_concat_null_yields_null_on
	,is_numeric_roundabort_on
	,is_quoted_identifier_on
	,is_recursive_triggers_on
	,is_cursor_close_on_commit_on
	,is_local_cursor_default
	,is_fulltext_enabled
	,is_trustworthy_on
	,is_db_chaining_on
	,is_parameterization_forced
	,is_master_key_encrypted_by_server
	,is_published
	,is_subscribed
	,is_merge_published
	,is_distributor
	,is_sync_with_backup
	,is_broker_enabled
	,log_reuse_wait
	,log_reuse_wait_desc
	,is_date_correlation_on
	,is_cdc_enabled
	,is_encrypted
	,is_honor_broker_priority_on
FROM master.sys.databases 

ORDER BY name 
";
                    
            } // End Switch 

            return "SELECT 123 AS abc";
}


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

                    return string.Format("{0} [{1}]:  {2}{3}/{4}",this.Name, m_csb.Engine.ToString(), m_csb.Server, m_csb.PortString, m_csb.DataBase);
                }
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
