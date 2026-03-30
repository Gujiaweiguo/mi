using System;
using System.Collections.Generic;
using System.Text;
using Base.DB;
namespace Sell
{
public class CasherPayment : BasePO
{
private decimal paymentid = 0;
private string bizdate = "";
private string userid = "";
private string mediacd = "";
private decimal amountt = 0;
private decimal localamt = 0;
private decimal payamt = 0;
private string paystatus = "";
private DateTime entryat = DateTime.Now;
private string entryby = "";
private DateTime checkat = DateTime.Now;
private string checkby = "";
private int createuserid = 0;
private DateTime createtime = DateTime.Now;
private int modifyuserid = 0;
private DateTime modifytime = DateTime.Now;
private int oprroleid = 0;
private int oprdeptid = 0;
public override String GetTableName()
{
return "CasherPayment";
}
public override String GetColumnNames()
{
return "PaymentID,BizDate,UserId,MediaCd,Amountt,LocalAmt,PayAmt,PayStatus,EntryAt,EntryBy,CheckAt,CheckBy,CreateUserID,CreateTime,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
}
public override String GetUpdateColumnNames()
{
return "BizDate,UserId,MediaCd,Amountt,LocalAmt,PayAmt,PayStatus,EntryAt,EntryBy,CheckAt,CheckBy,ModifyUserID,ModifyTime,OprRoleID,OprDeptID";
}
public decimal PaymentID
{
get{return paymentid;}
set{paymentid=value;}
}
public string BizDate
{
get{return bizdate;}
set{bizdate=value;}
}
public string UserId
{
get{return userid;}
set{userid=value;}
}
public string MediaCd
{
get{return mediacd;}
set{mediacd=value;}
}
public decimal Amountt
{
get{return amountt;}
set{amountt=value;}
}
public decimal LocalAmt
{
get{return localamt;}
set{localamt=value;}
}
public decimal PayAmt
{
get{return payamt;}
set{payamt=value;}
}
public string PayStatus
{
get{return paystatus;}
set{paystatus=value;}
}
public DateTime EntryAt
{
get{return entryat;}
set{entryat=value;}
}
public string EntryBy
{
get{return entryby;}
set{entryby=value;}
}
public DateTime CheckAt
{
get{return checkat;}
set{checkat=value;}
}
public string CheckBy
{
get{return checkby;}
set{checkby=value;}
}
public int CreateUserID
{
get{return createuserid;}
set{createuserid=value;}
}
public DateTime CreateTime
{
get{return createtime;}
set{createtime=value;}
}
public int ModifyUserID
{
get{return modifyuserid;}
set{modifyuserid=value;}
}
public DateTime ModifyTime
{
get{return modifytime;}
set{modifytime=value;}
}
public int OprRoleID
{
get{return oprroleid;}
set{oprroleid=value;}
}
public int OprDeptID
{
get{return oprdeptid;}
set{oprdeptid=value;}
}
}
}