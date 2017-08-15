using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TEST20160627NullCode1
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼、svc 和組態檔中的類別名稱 "Service1"。
    // 注意: 若要啟動 WCF 測試用戶端以便測試此服務，請在 [方案總管] 中選取 Service1.svc 或 Service1.svc.cs，然後開始偵錯。
    public class Service1 : IService1
    {
        public ReturnResult MyBillingPaySDKQueryA02()
        {
            ReturnResult ReturnResult = new ReturnResult();
            //14, "Mycard_backup")
            using (var db = new MyDBModel(14, "DB", "1.1.1.1"))
            {
                db.Database.Initialize(force: false);
                SqlCommand cmd = new SqlCommand();//  db.Database.Connection.CreateCommand();
                cmd.Connection = new SqlConnection(db.Database.Connection.ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "DB.dbo.SP";
                cmd.Parameters.Add("@P1", SqlDbType.VarChar, 50).Value = "1";//


                SqlParameter iReturnMsgNo = cmd.Parameters.Add("@ReturnMsgNo", SqlDbType.Int);
                iReturnMsgNo.Direction = ParameterDirection.Output;
                SqlParameter sReturnMsg = cmd.Parameters.Add("@ReturnMsg", SqlDbType.NVarChar, 1024);
                sReturnMsg.Direction = ParameterDirection.Output;
                SqlParameter sErrorCode = cmd.Parameters.Add("@ErrorCode", SqlDbType.VarChar, 20);
                sErrorCode.Direction = ParameterDirection.Output;
                SqlParameter iLogSn = cmd.Parameters.Add("@LogSn", SqlDbType.Int);
                iLogSn.Direction = ParameterDirection.Output; 
                try
                {
                    // Run the sproc
                    //db.Database.Connection.Open();
                    cmd.Connection.Open();
                    var reader = cmd.ExecuteReader();

                    var billings = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext.Translate<TempBilling>(reader);//, "TempBillings", System.Data.Entity.Core.Objects.MergeOption.AppendOnly);
                    //BillingList = billings.ToList();
                    ReturnResult.BillingList = billings.ToList();
                    reader.NextResult();
                    var InGames = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext.Translate<TempInGame>(reader);//, "TempInGames", System.Data.Entity.Core.Objects.MergeOption.AppendOnly);
                    //InGameList = InGames.ToList();
                    ReturnResult.InGameList = InGames.ToList();
                    reader.NextResult();
                    var CostPoints = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext.Translate<TempMemberCost>(reader);//, "TempMemberCosts", System.Data.Entity.Core.Objects.MergeOption.AppendOnly);
                    //CostPointList = CostPoints.ToList();
                    ReturnResult.CostPointList = CostPoints.ToList();
                    reader.NextResult();
                    ReturnResult.ReturnMsgNo = iReturnMsgNo.Value == null ? 0 : (int)iReturnMsgNo.Value;
                    ReturnResult.ReturnMsg = sReturnMsg.Value == null ? "" : sReturnMsg.Value.ToString();
                    ReturnResult.ErrorCode = sErrorCode.Value == null ? "" : sErrorCode.Value.ToString();
                    ReturnResult.LogSn = string.IsNullOrEmpty(Convert.ToString(iLogSn.Value)) == true ? 0 : (int)iLogSn.Value;
                    ReturnResult.ErrorCode = sErrorCode.Value == null ? "" : sErrorCode.Value.ToString(); 
                }
                finally
                {
                    db.Database.Connection.Close();
                }
                return ReturnResult;
            }
        }
    }
    public class ReturnResult
    {
        public int ReturnMsgNo { get; set; }
        public string ReturnMsg { get; set; }
        public string ErrorCode { get; set; }
        public int LogSn { get; set; }
        public int returnSDKLogoStatus { get; set; }
        public List<TempBilling> BillingList { get; set; }
        public List<TempInGame> InGameList { get; set; }
        public List<TempMemberCost> CostPointList { get; set; }
    }
}
