using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Regex_comments
{
    public class Helper
    {
        public Helper()
        {
            if (File.Exists("C:\\Users\\User\\Documents\\ClearExtraLogs.txt"))
                File.Delete("C:\\Users\\User\\Documents\\ClearExtraLogs.txt");
        }
        public async Task<IEnumerable<ChainAndShoppingCenterLogItem>> GetActionLogs()
        {
            using var c = new SqlConnection("data source=sql03c;initial catalog=EXTERNAL2 ;persist security info=True;user id=sa;password=JA48063;Trust Server Certificate=true");

            var procedureName = "TMP_GetAllDescriptionLog";
            var interval = new TimeSpan(0, 2, 0).TotalSeconds;

            var result = await c.QueryAsync<ChainAndShoppingCenterLogItem>(procedureName,
                commandTimeout: (int)interval, //(int)options.Value.Timeout.TotalSeconds,
                commandType: CommandType.StoredProcedure);

            return result;
        }

        public IEnumerable<ChainAndShoppingCenterLogItem> UpdateDescriptionActionLogs(ChainAndShoppingCenterLogItem item,  string newDescription)
        {
            using var c = new SqlConnection("data source=sql03c;initial catalog=EXTERNAL2 ;persist security info=True;user id=sa;password=JA48063;Trust Server Certificate=true");

            var procedureName = "TMP_UpdateDescriptionOneTime";
            var interval = new TimeSpan(0, 2, 0).TotalSeconds;

            var result = c.Query<ChainAndShoppingCenterLogItem>(procedureName,
                param: new { rowId = item.RowId, newDescription = newDescription },
                commandTimeout: (int)interval, //(int)options.Value.Timeout.TotalSeconds,
                commandType: CommandType.StoredProcedure) ;

            return result;
        }

        public static void WriteProcessLog(string comment, ChainAndShoppingCenterLogItem item)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


                // Append text to an existing file named "WriteLines.txt".
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "ClearExtraLogs2.txt"), true))
                {
                    outputFile.WriteLine($"{comment} {item?.RowId} {item?.UserName} {item?.ChainId} {item?.ShoppingCenterId} {item?.OrgId}");
                }
        }
    }
}
