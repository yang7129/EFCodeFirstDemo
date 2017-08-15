namespace TEST20160627NullCode1
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    public class MyDBModel : DbContext
    {
        // 您的內容已設定為使用應用程式組態檔 (App.config 或 Web.config)
        // 中的 'MyDBModel' 連接字串。根據預設，這個連接字串的目標是
        // 您的 LocalDb 執行個體上的 'TEST20160627NullCode1.MyDBModel' 資料庫。
        // 
        // 如果您的目標是其他資料庫和 (或) 提供者，請修改
        // 應用程式組態檔中的 'MyDBModel' 連接字串。
        public MyDBModel(int ConnNum, string DBName, string DBIP)
            : base("data source=1.1.1.1;initial catalog=DB;user id=AA;password=1234;MultipleActiveResultSets=True;App=EntityFramework;")//base(ConnStr(ConnNum, DBName, DBIP))//
        {
        }
        static Dbfuction myDbfnc = new Dbfuction();
        public static System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
        public static string ConnStr(int ConnNum, string DBName, string DBIP)
        {
            string DBUserID = "";
            string DBPW = "";
            //正式環境用連19 , 開發用連100 ,測試用Test
            if (TEST20160627NullCode1.Properties.Settings.Default.IsTest.ToUpper() == "FALSE")
            {
                switch (ConnNum)
                {
                    case 11: conn = myDbfnc.open11Db(); break;
                    case 12: conn = myDbfnc.open12Db(); break;
                    case 13: conn = myDbfnc.open13Db(); break;
                    case 14: conn = myDbfnc.open14Db(); break;
                    case 15: conn = myDbfnc.open15Db(); break;
                    case 16: conn = myDbfnc.open16Db(); break;
                    case 17: conn = myDbfnc.open17Db(); break;
                    case 19: conn = myDbfnc.open19Db(); break;
                    case 20: conn = myDbfnc.open20Db(); break;
                    case 21: conn = myDbfnc.open21Db(); break;
                    case 23: conn = myDbfnc.open23Db(); break;
                    default: conn = myDbfnc.open100Db(); break;
                }
            }
            else if (TEST20160627NullCode1.Properties.Settings.Default.IsTest.ToUpper() == "TEST")
            {
                if (DBIP.Trim() == "1.1.1.1")
                    conn = myDbfnc.open100Db();
                else
                    conn = myDbfnc.openTestDb();
            }
            else
            {
                if (DBIP.Trim() == "1.1.1.1")
                    conn = myDbfnc.openTestDb();
                else
                    conn = myDbfnc.open100Db();
            }
            //重組
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            for (int i = 0; i < conn.ConnectionString.Split(';').Length; i++)
            {
                switch (conn.ConnectionString.Split(';')[i].Split('=')[0].Trim())
                {
                    case "data source":
                        if (string.IsNullOrEmpty(DBIP).Equals(false))
                            builder.DataSource = DBIP;
                        else
                            builder.DataSource = conn.ConnectionString.Split(';')[i].Split('=')[1].Trim();
                        break;
                    case "initial catalog":
                        if (string.IsNullOrEmpty(DBName).Equals(false))
                            builder.InitialCatalog = DBName;
                        else
                            builder.InitialCatalog = conn.ConnectionString.Split(';')[i].Split('=')[1].Trim();
                        break;
                    case "user id":
                        builder.UserID = conn.ConnectionString.Split(';')[i].Split('=')[1].Trim();
                        break;
                    case "password":
                        builder.Password = conn.ConnectionString.Split(';')[i].Split('=')[1].Trim();
                        break;
                    case "MultipleActiveResultSets":
                        builder.MultipleActiveResultSets = Convert.ToBoolean(conn.ConnectionString.Split(';')[i].Split('=')[1].Trim());
                        break;
                    case "packet size":
                        builder.PacketSize = Convert.ToInt32(conn.ConnectionString.Split(';')[i].Split('=')[1].Trim());
                        break;
                    case "persist security info":
                        builder.PersistSecurityInfo = Convert.ToBoolean(conn.ConnectionString.Split(';')[i].Split('=')[1].Trim());
                        break;
                }
            }
            return builder.ConnectionString;
        }

        // 針對您要包含在模型中的每種實體類型新增 DbSet。如需有關設定和使用
        // Code First 模型的詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=390109。


        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<TempBilling> TempBillings { get; set; }
        public virtual DbSet<TempInGame> TempInGames { get; set; }
        public virtual DbSet<TempMemberCost> TempMemberCosts { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}

    public class TempBilling
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Type { get; set; }
        public string FactoryId { get; set; }
        public string FactoryTypeDesc { get; set; }
        public string FactoryName { get; set; }
        public string Fir_SDK_URL { get; set; }
        public string Sec_SDK_URL { get; set; }
        public string StoreProductServiceId { get; set; }
        public string FactoryType_Priority { get; set; }
        public string Factory_Priority { get; set; }
        public string Global_Currency { get; set; }
    }
    public class TempInGame
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Type { get; set; }
        public string FactoryTypeDesc { get; set; }
        public string FactoryName { get; set; }
        public string Fir_SDK_URL { get; set; }
        public string Sec_SDK_URL { get; set; }
        public string GameFacId { get; set; }
        public string PROD_ID { get; set; }
    }
    public class TempMemberCost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Type { get; set; }
        public string FactoryTypeDesc { get; set; }
        public string FactoryName { get; set; }
        public string Fir_SDK_URL { get; set; }
        public string Sec_SDK_URL { get; set; }
        public string FacSerId { get; set; }
        public string ServiceTypeSn { get; set; }
    }
}