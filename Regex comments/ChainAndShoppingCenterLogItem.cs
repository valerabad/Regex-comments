using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regex_comments
{
    public sealed class ChainAndShoppingCenterLogItem
    {
        public long RowId { get; set; }
        public string UserName { get; set; }

        public string Action { get; set; }

        public string Description { get; set; }

        public long? ChainId { get; set; }  

        public long? ShoppingCenterId { get; set; }

        public long? OrgId { get; set; }
    }
}
