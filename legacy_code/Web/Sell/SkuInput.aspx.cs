using System;
using System.Data;
using System.Text;
using System.Web.UI;

using System.IO;
using Base.Biz;
using Base.Page;
using Sell;
using Lease.ConShop;

public partial class Sell_SkuInput : BasePage
{
    public string baseInfo;
    private string result;
    public string strFresh;
    protected void Page_Load(object sender, EventArgs e)
    {
        baseInfo = (String)GetGlobalResourceObject("BaseInfo", "Menu_SkuInput");
         strFresh = (String)GetGlobalResourceObject("BaseInfo", "Page_Refresh");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        WithFile();
    }

    private void WithFile()
    {
        string vsurl = "";
        SkuMaster skuMaster = new SkuMaster();
        BaseTrans baseTrans = new BaseTrans();

        string[] aryTemp;
        String input = "";

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
                string fullpath = Server.MapPath("upfileReceipt/");//文件的上传路径

                if (!Directory.Exists(fullpath))//判断上传文件夹是否存在，若不存在，则创建
                {
                    //创建文件夹
                    Directory.CreateDirectory(fullpath);//创建文件夹 
                }
                vsurl = Server.MapPath("upfileReceipt/") + vsnewname;
                FileUpload1.SaveAs(vsurl);

                /*存入数据库*/
               // StreamReader sr = File.OpenText(vsurl);
                StreamReader sr = new StreamReader(vsurl, Encoding.GetEncoding("gb2312"));

                //存入数据库逻辑：
                //1.通过第一列的“商铺编码”查询到ShopID
                //2.删除Skumaster中该商铺已有的商品信息，delete from skumaster where TenantID=ShopID
                //3.导入商品信息：tenantid=shopid,skuid累加，skudesc商品名称，unitprice商品价格，其他的默认为0，
                string sss = "";
                string strShopID = "";
                string[] aryTemps;
                sss = sr.ReadLine();
                aryTemp = sss.Split(',');
                BaseBO baseBo = new BaseBO();
               // baseBo.WhereClause = "ShopStatus=" + ConShop.CONSHOP_TYPE_INGEAR + " And ShopCode='" + aryTemp[0].ToString().Trim() + "'";
                baseBo.WhereClause = "ShopID='" + aryTemp[0].ToString().Trim() + "'";
                DataSet dsNew = baseBo.QueryDataSet(new ConShop());
                if (dsNew.Tables[0].Rows.Count != 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '商铺ID不存在,请检查!'", true);
                    return;
                }
                strShopID=dsNew.Tables[0].Rows[0]["ShopId"].ToString();
                BaseTrans basetrans = new BaseTrans();
                basetrans.BeginTrans();
                string delSQL = "delete from SkuMaster where TenantID='" + strShopID  + "'";
                if (basetrans.ExecuteUpdate(delSQL) !=-1)
                {
                    basetrans.Commit();
                }
                else
                {
                    basetrans.Rollback();
                    return;
                }


                while ((input = sr.ReadLine()) != null)
                {
                    baseTrans.BeginTrans();
                    aryTemp = input.Split(',');
                    BaseBO baseBO = new BaseBO();
                    DataSet ds = new DataSet();
                    string SkuID;
                    ds = baseBO.QueryDataSet("select max(SkuID) from SkuMaster where TenantID='" + strShopID  + "'");//查询数据库是否存在编号
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0][0].ToString().Trim() != "" && ds.Tables[0].Rows[0][0].ToString().Trim() != null)
                        {
                            int len = ds.Tables[0].Rows[0][0].ToString().Trim().Length;
                            putskuid(ds.Tables[0].Rows[0][0].ToString().Trim().Substring(len-3));
                            SkuID =result;
                        }
                        else
                        {
                            SkuID =  "001";
                        }
                    }
                    else
                    {
                        SkuID = "001";
                    }
                
                    skuMaster.TenantId = strShopID ;
                    skuMaster.SkuId = strShopID  + SkuID;
                    skuMaster.SkuDesc = aryTemp[1].ToString().Trim();
                    skuMaster.UnitPrice = Convert.ToDecimal(aryTemp[2].ToString().Trim());
                    skuMaster.DeptId = "0";
                    skuMaster.CatgId = "0";
                    skuMaster.ClassId = "0";
                    skuMaster.Pcode = "";
                    skuMaster.Brand = "0";
                    skuMaster.Spec = "";
                    skuMaster.color = "";
                    skuMaster.Unit = "";
                    skuMaster.Produce = "";
                    skuMaster.Level = "";
                    skuMaster.Status = "V";
                    skuMaster.DataSource = "M";
                    skuMaster.IsClassCode = "N";
                    skuMaster.EntryAt = DateTime.Now;
                    skuMaster.EntryBy = "sys";
                    skuMaster.ModifyAt = DateTime.Now;
                    skuMaster.ModifyBy = "sys";
                    skuMaster.Component = "";
                    skuMaster.dPriceMin = Convert.ToDecimal("0.01");
                    skuMaster.dPriceMax = Convert.ToDecimal("999999");
                    skuMaster.chLocked = "0";
                    skuMaster.dStock = 0;
                    skuMaster.isDiscountCode = "N";
                    skuMaster.DiscountPcentRate = Convert.ToDecimal("0");
                    skuMaster.BonusGpPer = Convert.ToDecimal("0");
                    if (baseTrans.Insert(skuMaster) < 1)
                    { 
                        baseTrans.Rollback();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '添加失败'", true);
                        return;
                    }
                    baseTrans.Commit();
                }
                sr.Close();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "message", "parent.document.all.txtWroMessage.value =  '添加成功'", true);
            }
            catch (Exception error)
            {
                return;
            }
        }
    }
    #region 商品编号后缀计算方法
    private void  putskuid(string skuid)
    {
        int s1;
        int s2;
        int s3;
        int result2;

        s1 = Convert.ToInt32(skuid.Substring(0, 1));
        s2 = Convert.ToInt32(skuid.Substring(1, 1));
        s3 = Convert.ToInt32(skuid.Substring(2, 1));

        if (s1 == 0)
        {

            if (s2 == 0)
            {
                result2 = s3 + 1;
                if (result2.ToString().Length == 1)
                {
                    result = "00" + result2.ToString();
                }
                if (result2.ToString().Length == 2)
                {
                    result = "0" + result2.ToString();
                }
            }
            else
            {
                result2 = s2 * 10 + s3 + 1;
                if (result2.ToString().Length == 2)
                {
                    result = "0" + result2.ToString();
                }
                if (result2.ToString().Length == 3)
                {
                    result = result2.ToString();
                }

            }
        }
        else
        {
            result2 = Convert.ToInt32(skuid) + 1;
            result = result2.ToString();
        }
        
    }
#endregion
}

