
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


        protected static DB.Abstraction.cDAL DAL;
        protected System.Data.DataTable dt;

		public class cRawConnectionString
		{
			public cRawConnectionString(){}
			public cRawConnectionString(string connectionString, string providerName)
			{
				this.ConnectionString = connectionString;
				this.Engine = ProviderName2EngineType(providerName);
			}

			public string ConnectionString;
			public DB.Abstraction.cDAL.DataBaseEngine_t Engine;
		}

		public System.Collections.Generic.Dictionary<string, cRawConnectionString> dict;


		public static DB.Abstraction.cDAL.DataBaseEngine_t ProviderName2EngineType(string providerName)
		{
			if(System.StringComparer.InvariantCultureIgnoreCase.Equals("System.Data.SqlClient", providerName))
				return DB.Abstraction.cDAL.DataBaseEngine_t.MS_SQL;

			if(System.StringComparer.InvariantCultureIgnoreCase.Equals("Npgsql", providerName))
				return DB.Abstraction.cDAL.DataBaseEngine_t.PostGreSQL;

			if(System.StringComparer.InvariantCultureIgnoreCase.Equals("MySql.Data.MySqlClient", providerName))
				return DB.Abstraction.cDAL.DataBaseEngine_t.MySQL;

			if(System.StringComparer.InvariantCultureIgnoreCase.Equals("System.Data.OracleClient", providerName))
				return DB.Abstraction.cDAL.DataBaseEngine_t.Oracle;

			if(System.StringComparer.InvariantCultureIgnoreCase.Equals("FirebirdSql.Data.FirebirdClient", providerName))
				return DB.Abstraction.cDAL.DataBaseEngine_t.FireBird;

			if(System.StringComparer.InvariantCultureIgnoreCase.Equals("Mono.Data.Sqlite", providerName))
				return DB.Abstraction.cDAL.DataBaseEngine_t.SQLite;

			if(System.StringComparer.InvariantCultureIgnoreCase.Equals("System.Data.Odbc", providerName))
				return DB.Abstraction.cDAL.DataBaseEngine_t.ODBC;

			return DB.Abstraction.cDAL.DataBaseEngine_t.ODBC;
		}


        private void frmMain_Load(object sender, System.EventArgs e)
        {
			// Walk through the collection and return the first 
			// connection string matching the connectionString name.
			System.Configuration.ConnectionStringSettingsCollection settings = System.Configuration.ConfigurationManager.ConnectionStrings;

			dict = new System.Collections.Generic.Dictionary<string, cRawConnectionString>
				(System.StringComparer.InvariantCultureIgnoreCase) 
			;

			if ((settings != null))
			{
				foreach (System.Configuration.ConnectionStringSettings cs in settings)
				{
					if (System.StringComparer.InvariantCultureIgnoreCase.Equals (cs.Name, "LocalSqlServer"))
						continue;

					if (System.StringComparer.InvariantCultureIgnoreCase.Equals (cs.Name, "LocalSqliteServer"))
						continue;

					if (System.StringComparer.InvariantCultureIgnoreCase.Equals (cs.Name, "server"))
						continue;

					string strFixMonoBug = System.Text.RegularExpressions.Regex.Replace(
						 cs.ConnectionString
						,@"Security\s*=\s*SSPI\s*;", "Security=true;"
						,System.Text.RegularExpressions.RegexOptions.IgnoreCase
					);

					dict.Add (
						 cs.Name
						,new cRawConnectionString (strFixMonoBug, cs.ProviderName)
					);
				} // Next cs

			} // End if ((settings != null))


			System.Collections.Generic.List<Helper.cbItem> ls = new System.Collections.Generic.List<Helper.cbItem>();

			int i = 0;
			int selIndex = 0;

			foreach (var kvp in dict)
			{
				DB.Abstraction.UniversalConnectionStringBuilder csb = 
					DB.Abstraction.UniversalConnectionStringBuilder.CreateInstance (
						 kvp.Value.Engine
						,kvp.Value.ConnectionString
					)
				;

				if(kvp.Key.StartsWith(System.Environment.MachineName, System.StringComparison.InvariantCultureIgnoreCase))
					selIndex = i;

				ls.Add(new Helper.cbItem(kvp.Key, csb));
				++i;
			} // End kvp 

            
            this.cbConnections.Items.AddRange(ls.ToArray());
            this.cbConnections.SelectedIndex = selIndex;
        } // End Sub frmMain_Load 


        private void btnExecute_Click(object sender, System.EventArgs e)
        {
            if (dt != null)
                dt.Dispose();

            dt = DAL.GetDataTable(this.txtSQL.Text);
            this.dgvResult.DataSource = dt;
        }


        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }


        public static void MsgBox(object obj)
        {
            if (obj != null)
                System.Windows.Forms.MessageBox.Show(obj.ToString());
            else
                System.Windows.Forms.MessageBox.Show("obj IS NULL");
        }


        private void cbConnections_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            object obj = cb.Items[cb.SelectedIndex];
            Helper.cbItem item = (Helper.cbItem)obj;

            DB.Abstraction.UniversalConnectionStringBuilder csb = item.ConnectionStringBuilder;
            DAL = DB.Abstraction.cDAL.CreateInstance(csb.Engine.ToString(), csb.ConnectionString);

			this.txtSQL.Text = DAL.GetDataBasesQueryText ();
        }


    } // End Class 


} // End Namespace 
