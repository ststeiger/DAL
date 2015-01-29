
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


        private void frmMain_Load(object sender, System.EventArgs e)
        {
            DB.Abstraction.UniversalConnectionStringBuilder csb = Helper.GetDefaultCSB();

            System.Collections.Generic.List<Helper.cbItem> ls = new System.Collections.Generic.List<Helper.cbItem>();
            ls.Add(new Helper.cbItem("Local MS_SQL", csb));

            this.cbConnections.Items.AddRange(ls.ToArray());
            this.cbConnections.SelectedIndex = 0;

            this.txtSQL.Text = Helper.GetDefaultSQL(DAL.DBtype);
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
        }


    } // End Class 


} // End Namespace 
