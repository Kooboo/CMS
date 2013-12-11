using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.VSExtensionTemplate.Extensions.MembershipConnectClient
{
    public class TencentGraphData
    {
        public TencentGraphData()
        {
        }

        public string Msg { get; set; }
        public int Ret { get; set; }
        public string Gender { get; set; }
        public string NickName { get; set; }

        public string Figureurl { get; set; }
        public string Figureurl_1 { get; set; }
        public string Figureurl_2 { get; set; }

        public string Figureurl_qq_1 { get; set; }
        public string Figureurl_qq_2 { get; set; }
        public int Is_yellow_vip { get; set; }
        public int Vip { get; set; }
        public int Yellow_vip_level { get; set; }
        public int Level { get; set; }
    }
}
