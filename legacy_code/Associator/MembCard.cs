using System;
using System.Collections.Generic;
using System.Text;

using Base.DB;

namespace Associator
{
    /// <summary>
    /// 会员卡信息
    /// </summary>
    public class MembCard : BasePO
    {
        private string membCardID = "";
        private int membID = 0;
        private int cardClassID = 0;
        private int membCardType = 0;
        private int membCardStatus = 0;
        private DateTime issueDate = DateTime.Now;
        private string mainCardNum = "";
        private int totalScore = 0;
        private DateTime expiredDate = DateTime.Now.AddYears(1);
        private string newCardID = "";
        private string changeReason = "";

        private MEMBCARDSTATUS_
    }
}
