using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;


namespace Base.XML
{
   public  class ColorLumpXMLInfo : BasePO

    {

        //  Colortype=0 业态分布
        //  Colortype=1 合同状态
        //  Colortype=2 评效分析
        //  Colortype=3 销售分析
       private int Colortype = 0;
       string sql = "";

       public ColorLumpXMLInfo(int colortype)
       {
           Colortype = colortype;

           if (Colortype == 0)
           {

               sql = "select TradeName classname,rb,gb,bb from TradeRelation ";
           }
           if (Colortype == 1)
           {

               sql = @"select distinct
                    case when datediff(day,getdate(),isnull(contract.stopdate,conenddate))>0 and 30>datediff(day,getdate(),isnull(contract.stopdate,conenddate)) then '即将到期' 
                    when datediff(day,getdate(),isnull(contract.stopdate,conenddate))>30 then '正常' when datediff(day,getdate(),isnull(contract.stopdate,conenddate))<=0 then '到期' 
                     end as classname,
					                    case when datediff(day,getdate(),isnull(contract.stopdate,conenddate))>0 and 30>datediff(day,getdate(),isnull(contract.stopdate,conenddate)) then 255 
                    when datediff(day,getdate(),isnull(contract.stopdate,conenddate))>30 then 0 when datediff(day,getdate(),isnull(contract.stopdate,conenddate))<=0 then 255 
                    end as rb,
					                    case when datediff(day,getdate(),isnull(contract.stopdate,conenddate))>0 and 30>datediff(day,getdate(),isnull(contract.stopdate,conenddate)) then 255 
                    when datediff(day,getdate(),isnull(contract.stopdate,conenddate))>30 then 255 when datediff(day,getdate(),isnull(contract.stopdate,conenddate))<=0 then 0 
                    end as gb,
					                    case when datediff(day,getdate(),isnull(contract.stopdate,conenddate))>0 and 30>datediff(day,getdate(),isnull(contract.stopdate,conenddate)) then 0
                    when datediff(day,getdate(),isnull(contract.stopdate,conenddate))>30 then 0 when datediff(day,getdate(),isnull(contract.stopdate,conenddate))<=0 then 0 
                    end as bb from contract";

           }

           if (Colortype == 2)
           {

               sql = @"select distinct case when transshopmth.paidamt<10000 then '小于10000' when transshopmth.paidamt>=10000 and transshopmth.paidamt<30000 then '大于10000小于30000' 
						 when transshopmth.paidamt>=30000 and transshopmth.paidamt<50000 then '大于30000小于50000' when transshopmth.paidamt>=50000 and transshopmth.paidamt<100000 then '大于50000小于100000'  else '大于100000' end as classname,
                       case when transshopmth.paidamt<10000 then 255 when transshopmth.paidamt>=10000 and transshopmth.paidamt<30000 then 0 
						 when transshopmth.paidamt>=30000 and transshopmth.paidamt<50000 then 255 when transshopmth.paidamt>=50000 and transshopmth.paidamt<100000 then 26  else 0 end as rb,
					   case when transshopmth.paidamt<10000 then 0 when transshopmth.paidamt>=10000 and transshopmth.paidamt<30000 then 255 
					  	 when transshopmth.paidamt>=30000 and transshopmth.paidamt<50000 then 255 when transshopmth.paidamt>=50000 and transshopmth.paidamt<100000 then 0  else 0 end as gb,
				   	case when transshopmth.paidamt<10000 then 0 when transshopmth.paidamt>=10000 and transshopmth.paidamt<30000 then 0 
						 when transshopmth.paidamt>=30000 and transshopmth.paidamt<50000 then 0 when transshopmth.paidamt>=50000 and transshopmth.paidamt<100000 then 184  else 0 end as bb from transshopmth where month='" + DateTime.Now.Year.ToString() + "-" + DateTime.Now.AddMonths(-1).Month.ToString() + "-01'";

           }

       }


        public override String GetTableName()
        {
            return "";
        }

        public override String GetColumnNames()
        {
            return "";
        }

        public override String GetUpdateColumnNames()
        {
            return "";
        }
        public override String GetInsertColumnNames()
        {
            return "";

        }
        public override string GetQuerySql()
        {

            return sql;




        }


    }
}
