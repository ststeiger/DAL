
using System.Windows.Forms;


namespace TestApp
{


    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        } // End Constructor



        private void txtSQL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.A))
            {
                if (sender != null)
                    ((TextBox)sender).SelectAll();
                e.Handled = true;
            } // End if (e.Control && (e.KeyCode == Keys.A)) 

        } // End Sub txtSQL_KeyDown 


        public static DB.Abstraction.cDAL SetupDAL()
        {
            return SetupDAL(DB.Abstraction.cDAL.DataBaseEngine_t.MS_SQL);
        } // End Function SetupDAL


        public static DB.Abstraction.cDAL SetupDAL(DB.Abstraction.cDAL.DataBaseEngine_t dataBaseEngine)
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

            return DB.Abstraction.cDAL.CreateInstance(dataBaseEngine.ToString(), csb.ConnectionString);
        } // End Function SetupDAL


        protected static DB.Abstraction.cDAL DAL;

        protected System.Data.DataTable dt;


        private void frmMain_Load(object sender, System.EventArgs e)
        {
            if(System.Environment.OSVersion.Platform == System.PlatformID.Unix)
                DAL = SetupDAL(DB.Abstraction.cDAL.DataBaseEngine_t.PostGreSQL);
            else
                DAL = SetupDAL();


            switch (DAL.DBtype)
            {
                case DB.Abstraction.cDAL.DataBaseEngine_t.PostGreSQL:
                    this.txtSQL.Text = @"";
                    break;
                case DB.Abstraction.cDAL.DataBaseEngine_t.MySQL:
                    this.txtSQL.Text = @"";
                    break;
                case DB.Abstraction.cDAL.DataBaseEngine_t.MS_SQL:
                    this.txtSQL.Text = @"
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
                    break;
            } // End Switch 

        } // End Sub frmMain_Load 


        private void btnExecute_Click(object sender, System.EventArgs e)
        {
            if (dt != null)
                dt.Dispose();

            dt = DAL.GetDataTable(this.txtSQL.Text);
            this.dgvResult.DataSource = dt;
        } // End Sub btnExecute_Click 


    } // End Class 


} // End Namespace 
