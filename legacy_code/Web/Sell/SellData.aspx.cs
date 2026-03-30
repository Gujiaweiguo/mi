using System;
using System.Data;

using System.IO;
using Base.DB;
using Base.Biz;
using Base;
using Base.Page;
using Sell;
using Lease.ConShop;
using System.Text;
using BaseInfo.User;


public partial class Sell_SellData : BasePage
{
    public string baseInfo;
    public string strFresh;
    string vsurl = "";  //上传到服务器url
    protected void Page_Load(object sender, EventArgs e)
    {
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Sell_SellData");
        strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        int intRows;
        SessionUser objSessionUser = (SessionUser)Session["UserAccessInfo"];
        doWithFile();  //上传销售数据

        TransSku transSku = new TransSku();
        TransSkuMedia transSkuMedia = new TransSkuMedia();
        BaseTrans baseTrans = new BaseTrans();

        string bancth = BaseApp.GetID("TransSku", "BatchID").ToString();

        string[] aryTemp;
        String input="";

        DataSet mediaMNoDS= GetMediaMNo();
        if (mediaMNoDS.Tables[0].Rows.Count > 0)
        {
            transSkuMedia.MediaMNo = Convert.ToInt32(mediaMNoDS.Tables[0].Rows[0]["MediaMNO"]);
            transSkuMedia.MediaMDesc = mediaMNoDS.Tables[0].Rows[0]["MediaMDesc"].ToString();
        }
        
        if (vsurl != "")
        {
            baseTrans.BeginTrans();
            intRows = 0;
            StreamReader sr = new StreamReader(vsurl, Encoding.GetEncoding("gb2312"));
            try
            {
                while ((input = sr.ReadLine()) != null)
                {
                    intRows = intRows + 1;
                    aryTemp = input.Split(',');

                    int flag = CheckShopID(Convert.ToInt32(aryTemp[0]));
                    if (flag == 0)
                    {
                        string sql = "  SELECT " +
                                    " ISNULL(ConShop.ShopID,0) as ShopID,ConShop.ShopName," +
                                    " ISNULL(ConShop.BuildingId,0) as BuildingId, BuildingName," +
                                    " ISNULL(ConShop.FloorId,0) as FloorId,FloorName," +
                                    " ISNULL(ConShop.LocationId,0) as LocationId,LocationName," +
                                    " ISNULL(ConShop.AreaId,0) as AreaId,AreaName," +
                                    " ISNULL(ConShop.BrandId,0) as BrandId,BrandName," +
                                    " ISNULL(TradeRelation.TradeId,0) as TradeId,TradeName," +
                                    " ISNULL(ConShop.StoreID,0) as StoreID," +             //StoreID
                                    " ISNULL(ConShop.MainLocationID,0) as MainLocationID," +
                                    " ISNULL(skumaster.skuid,0) as skuid," +
                                    " ISNULL(skumaster.skudesc,0) as skudesc" +
                                    " FROM ConShop LEFT JOIN Building On (ConShop.BuildingId=Building.BuildingId)" +
                                    " left join skumaster on (conshop.shopid=skumaster.tenantid)" +
                                    " LEFT JOIN Floors On (ConShop.FloorId=Floors.FloorId)" +
                                    " LEFT JOIN Location On (ConShop.LocationId=Location.LocationId)" +
                                    " LEFT JOIN Area On (ConShop.AreaId=Area.AreaId)" +
                                    " LEFT JOIN ConShopBrand On (ConShop.BrandId=ConShopBrand.BrandId)" +
                                    " LEFT JOIN Contract On(ConShop.ContractId=Contract.ContractId) " +
                                    " LEFT JOIN TradeRelation On(TradeRelation.TradeId=Contract.TradeId) " +
                                    " WHERE ShopId=" + Convert.ToInt32(aryTemp[0]);
                        BaseBO basebo = new BaseBO();
                        DataSet ds = basebo.QueryDataSet(sql);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //交易商品流水表
                            transSku.ShopID = Convert.ToInt32(aryTemp[0]);
                            transSku.ShopName = ds.Tables[0].Rows[0]["ShopName"].ToString();
                            transSku.PosID = aryTemp[1].ToString();
                            transSku.TransID = aryTemp[2].ToString();
                            transSku.PluCd = ds.Tables[0].Rows[0]["skuid"].ToString();
                            transSku.SkuID = ds.Tables[0].Rows[0]["skuid"].ToString();    //没有根据上传的skuid导入数据库，考虑上传的skuid不准确
                            transSku.SkuDesc = ds.Tables[0].Rows[0]["skudesc"].ToString();
                            transSku.PaidAmt = Convert.ToDecimal(aryTemp[5]);
                            transSku.PayAmt = Convert.ToDecimal(aryTemp[5]);
                            transSku.NewPrice = Convert.ToDecimal(aryTemp[5]);
                            transSku.BizDate = Convert.ToDateTime(Convert.ToDateTime(aryTemp[6]).ToShortDateString());
                            transSku.TransTime = Convert.ToDateTime(aryTemp[6]);
                            transSku.BatchID = bancth;
                            transSku.BuildingID = Convert.ToInt32(ds.Tables[0].Rows[0]["BuildingID"]);
                            transSku.BuildingName = ds.Tables[0].Rows[0]["BuildingName"].ToString();
                            transSku.FloorID = Convert.ToInt32(ds.Tables[0].Rows[0]["FloorID"]);
                            transSku.FloorName = ds.Tables[0].Rows[0]["FloorName"].ToString();
                            transSku.LocationID = Convert.ToInt32(ds.Tables[0].Rows[0]["LocationID"]);
                            transSku.LocationName = ds.Tables[0].Rows[0]["LocationName"].ToString();
                            transSku.AreaID = Convert.ToInt32(ds.Tables[0].Rows[0]["AreaID"]);
                            transSku.AreaName = ds.Tables[0].Rows[0]["AreaName"].ToString();
                            transSku.BrandID = Convert.ToInt32(ds.Tables[0].Rows[0]["BrandID"]);
                            transSku.BrandName = ds.Tables[0].Rows[0]["BrandName"].ToString();
                            transSku.Trade2ID = Convert.ToInt32(ds.Tables[0].Rows[0]["TradeId"]);
                            transSku.Trade2Name = ds.Tables[0].Rows[0]["TradeName"].ToString();
                            transSku.DataSource = TransSku.DATASOURCE_FILE;
                            transSku.StoreID = Convert.ToInt32(ds.Tables[0].Rows[0]["StoreID"]);
                            transSku.MainLocationID = Convert.ToInt32(ds.Tables[0].Rows[0]["MainLocationID"]);
                            transSku.UserID = objSessionUser.UserID;  //导入人员
                            transSku.ReceiptID = "0000";
                            transSku.Qty = 1;

                            //交易金额流水表

                            transSkuMedia.ShopID = Convert.ToInt32(aryTemp[0]);
                            transSkuMedia.PosID = aryTemp[1].ToString();
                            transSkuMedia.TransID = aryTemp[2].ToString();
                            transSkuMedia.SkuID = ds.Tables[0].Rows[0]["skuid"].ToString();
                            transSkuMedia.SkuDesc = ds.Tables[0].Rows[0]["skudesc"].ToString();
                            transSkuMedia.PaidAmt = Convert.ToDecimal(aryTemp[5]);
                            transSkuMedia.ShopName = ds.Tables[0].Rows[0]["ShopName"].ToString();
                            transSkuMedia.UserID = objSessionUser.UserID;
                            transSkuMedia.BizDate = Convert.ToDateTime(Convert.ToDateTime(aryTemp[6]).ToShortDateString());
                            transSkuMedia.TransTime = Convert.ToDateTime(aryTemp[6]);
                            transSkuMedia.BatchID = bancth;
                            transSkuMedia.DataSource = TransSkuMedia.DATASOURCE_FILE;
                            transSkuMedia.StoreID = Convert.ToInt32(ds.Tables[0].Rows[0]["StoreID"]);
                            transSkuMedia.MainLocationID = Convert.ToInt32(ds.Tables[0].Rows[0]["MainLocationID"]);
                            transSkuMedia.ReceiptID = "0000";
                            transSkuMedia.UserID = objSessionUser.UserID;


                            baseTrans.Insert(transSku);
                            baseTrans.Insert(transSkuMedia);
                            this.Label2.Text = (String)GetGlobalResourceObject("BaseInfo", "BankCard_TransmitSucceed") + "    " +(String)GetGlobalResourceObject("BaseInfo", "BankCard_TransmitClount") + intRows.ToString();
                        }
                        //Response.Write("<script language:javascript>javascript:parent.document.all.txtWroMessage.value='" + (String)GetGlobalResourceObject("BaseInfo", "BankCard_TransmitSucceed") + "';</script>");
                    }
                    else
                    {
                        //Msgbox(Literal1, (String)GetGlobalResourceObject("BaseInfo", "Sale_NoShopID"));
                        this.Label2.Text = (String)GetGlobalResourceObject("BaseInfo", "Sale_NoShopID") + "    ErrLine:" + intRows.ToString();
                        //Response.Write("<script language:javascript>javascript:parent.document.all.txtWroMessage.value='" + (String)GetGlobalResourceObject("BaseInfo", "Sale_NoShopID") + "';</script>");
                        baseTrans.Rollback();
                        baseTrans.Commit();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                baseTrans.Rollback();
                //Msgbox(Literal1, (String)GetGlobalResourceObject("BaseInfo", "BankCard_TransmitLost"));
                this.Label2.Text = (String)GetGlobalResourceObject("BaseInfo", "BankCard_TransmitLost") + "    ErrLine:" + intRows.ToString();
                //Response.Write("<script language:javascript>javascript:parent.document.all.txtWroMessage.value='" + (String)GetGlobalResourceObject("BaseInfo", "BankCard_TransmitLost") + "';</script>");
            }
            sr.Close();
            baseTrans.Commit();
        }
    }

    private DataSet GetMediaMNo()
    {
        string str_sql = "select A.MediaMNO,A.MediaMDesc from MediaM A,Media B where A.MediaNo = B.MediaNo and B.PayType = 1";
        BaseBO baseBO = new BaseBO();
        DataSet ds = baseBO.QueryDataSet(str_sql);
        return ds;
    }

    private int CheckShopID(int shopID)
    {
        int shopIDFlag;
        BaseBO baseBo = new BaseBO();
        baseBo.WhereClause = "ShopID = " + shopID;
        Resultset rs = baseBo.Query(new ConShop());
        if (rs.Count > 0)
            shopIDFlag = 0;
        else
            shopIDFlag = 1;
        return shopIDFlag;
    }

    private void doWithFile()
    {
        if (FileUpload1.HasFile)//判断文件是否为空
        {
            try
            {
                //string vsfullname = FileUpload1.PostedFile.FileName;//获取文件的名称包含路径
                string vsfilename = FileUpload1.FileName;//获取文件的名称
                int index = vsfilename.LastIndexOf(".");
                string vstype = vsfilename.Substring(index).ToLower();//取文件的扩展名
                string vsnewname = System.DateTime.Now.ToString("yyyyMMddHHmmssffff");//声称文件名，防止重复
                vsnewname = vsnewname + vstype;//完整的上传文件名
                string fullpath = Server.MapPath("upfile/");//文件的上传路径
                if (!Directory.Exists(fullpath))//判断上传文件夹是否存在，若不存在，则创建
                {
                    //创建文件夹
                    Directory.CreateDirectory(fullpath);//创建文件夹 
                }
                vsurl = Server.MapPath("upfile/") + vsnewname;
                FileUpload1.SaveAs(vsurl);
               
            }
            catch (Exception error)
            {
                Response.Write(error.ToString());
            }
        }
    }
}
