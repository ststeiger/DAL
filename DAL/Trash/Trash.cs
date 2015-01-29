
#if false


using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Trashcan
{
    internal class Trash
    {

        public DB.Abstraction.cDAL.DataBaseEngine_t dbeDialect;
        public bool bEmbeddedDatabase = false;
        public int iOleDbServices = 0;

        public string strProvider = "";
        public string strServerName = "";
        public string strInstanceName = "";
        public int iPort = 0;
        public string strInitialCatalog = "";
        
        public bool bIntegratedSecurity = false;
        public string strUserName = "";

        public System.Security.SecureString ssSecurePassword = DB.Abstraction.cDAL.String2SecureString("");

        public int iConnectionTimeout = 15;


        public Trash() // cDataBaseConfiguration 
        {
            //SetMicrosoft();
            //SetMicrosoftLocal();
            //SetMicrosoftSRGLocal();
            //SetFirebird();
        } // End Constructor


        // http://yacoding.blogspot.com/2006/04/connecting-to-firebird-in-c.html
        public void SetAccess()
        {
            this.dbeDialect = DB.Abstraction.cDAL.DataBaseEngine_t.OleDB;
            this.bEmbeddedDatabase = true;

            this.strServerName = "localhost";
            this.strProvider = "Microsoft.Jet.OLEDB.4.0";
            //this.iPort = 3050;
            //this.strInitialCatalog = @"E:\SQLData_Firebird\TESTDB.fdb";
            this.strInitialCatalog = @"D:\Stefan.Steiger\Desktop\myfram_code_10\myfram_code_10\bin\db\myfram_entwicklung.mdb";
            this.iOleDbServices = -4;
            //this.strUserName = "sysdba";
            //this.ssSecurePassword = DB.Abstraction.cDAL.String2SecureString("masterkey");

            this.iConnectionTimeout = 15;
        } // End Sub SetFirebird


        public void SetMicrosoftLocal()
        {
            this.dbeDialect = DB.Abstraction.cDAL.DataBaseEngine_t.MS_SQL;
            this.bEmbeddedDatabase = false;

            this.strServerName = "localhost";
            this.strInstanceName = "";
            this.iPort = 0;
            this.strInitialCatalog = "DNSdata";

            this.bIntegratedSecurity = true;
            this.strUserName = "MyUserName";
            this.ssSecurePassword = DB.Abstraction.cDAL.String2SecureString("PasswordForMyUserName");

            this.iConnectionTimeout = 15;
        } // End Sub SetMicrosoftLocal


        // http://yacoding.blogspot.com/2006/04/connecting-to-firebird-in-c.html
        public void SetFirebird()
        {
            this.dbeDialect = DB.Abstraction.cDAL.DataBaseEngine_t.FireBird;
            this.bEmbeddedDatabase = false;

            this.strServerName = "localhost";
            this.iPort = 3050;
            //this.strInitialCatalog = @"E:\SQLData_Firebird\TESTDB.fdb";
            this.strInitialCatalog = @"/var/lib/firebird/2.5/data/DNSdata.fdb";

            this.strUserName = "sysdba";
            this.ssSecurePassword = DB.Abstraction.cDAL.String2SecureString("masterkey");

            this.iConnectionTimeout = 15;
        } // End Sub SetFirebird


    }
}
#endif
