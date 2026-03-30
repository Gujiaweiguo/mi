using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Base.Biz;
using Base.DB;

namespace Associator.Associator
{
    /// <summary>
    /// 查询会员信息PO
    /// </summary>
    public class QueryAssociatorPO
    {
        /// <summary>
        /// 根据查询条件查询会员信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetLCustByCondition(string strWhere)
        {
            BaseBO baseBO = new BaseBO();
            string str_sql = "SELECT Member.MembId,MembCode,LOtherId,Salutation,MemberName,DateJoint,Addr1,Addr2,Addr3,OffPhone,HomePhone,MobilPhone,Email,Dob,NatNm,RaceNm,IncomeId,BizNm,JobTitleNm," +
                                " SexNm,MStatusNm,MAnnDate,EduLevelNm,DistanceId,RecreationNm1,RecreationNm2,RecreationNm3,PreferMerNm1,PreferMerNm2,PreferMerNm3," +
                                " PreferGiftNm1,PreferGiftNm2,PreferGiftNm3,MyField1Id,MyField2Id,Remarks,PostalCode1,PostalCode2,PostalCode3,ComefromNM," +
                                " CustPassword,MemberPic," +
                                " MembCard.MembCardId,MembCard.CardClassId,MembCard.DateIssued,MembCard.CardTypeId,MembCard.CardStatusId,MembCard.CardOwner,MembCard.PrLCustId,MembCard.ExpDate,MembCard.NewMembCardID,MembCard.ChangeReason,MembCard.CreateUserID,MembCard.CreateTime,MembCard.ModifyUserID,MembCard.ModifyTime,MembCard.OprRoleID,MembCard.OprDeptID" +
                                " FROM Member,MembCard " +
                                " WHERE " + strWhere +
                                " AND Member.MembId = MembCard.MembId";
            DataSet ds = baseBO.QueryDataSet(str_sql);
            return ds;
        }
                            
    }
}
