using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Base.Biz;
using Base.DB;
using Base.Page;
using Base.Sys;
using Base.Util;
using Base;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Web;

public partial class RentableArea_Building_UnitDataExport : BasePage
{
    private DataSet dsExcel;
    private string ExcelFilePath;
    private OleDbConnection odbcon;
    private OleDbCommand odbcmd;
    private DbTransaction trans = null;
    private string SelectedSheetName;
    private string strLocationID = "";
    private string strStoreID = "";
    private string strLoadFilepath = "";//文件上传到服务器的相对路径
    protected void Page_Load(object sender, EventArgs e)
    {
        this.strLoadFilepath = Server.MapPath(@"UploadExcel");
        if (!this.IsPostBack)
        {
            try
            {
                this.strLocationID = Request["LocationID"].ToString();
                if (this.strLocationID.ToString() != "")
                    this.strStoreID = this.strLocationID.Substring(0, 3);
                this.ClearExcel();
                this.SetBuildingName();
                this.SetFloorName();
                this.SetLocationName();
                this.SetShopTypeName();
                this.SetTradeName();
                this.SetUnitTypeName();
                this.SetAreaName();
            }
            catch(Exception ex) { }
        }
    }
    /// <summary>
    /// 连接Excel数据文件
    /// </summary>
    /// <param name="strExcelPath"></param>
    public void TLoadDataFrmExcel(string strExcelPath)
    {
        this.dsExcel = new DataSet();
        this.ExcelFilePath = strExcelPath;
        this.SelectedSheetName = "";
        this.ExcelFilePath = strExcelPath;
        this.odbcon = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=\"Excel 8.0;\";Data Source=" + this.ExcelFilePath);
    }
    /// <summary>
    /// 填充数据集
    /// </summary>
    private void FillDataSet()
    {
        new OleDbDataAdapter(odbcmd).Fill(dsExcel);
    }
    /// <summary>
    /// 得到Excel中的数据
    /// </summary>
    /// <returns></returns>
    public DataSet GetExcelData()
    {
        //this.SelectedSheetName = "Sheet1";
        this.odbcmd = new OleDbCommand("SELECT  * FROM [" + "Sheet1" + "$]", this.odbcon);
        //OleDbDataAdapter objAdapter = new OleDbDataAdapter();
        //DataSet ds = new DataSet();
        //objAdapter.Fill(ds);
        //return ds;
        this.FillDataSet();
        return this.dsExcel;
    }
    /// <summary>
    /// 格式下载
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDown_Click(object sender, EventArgs e)
    {
        #region
        //string Js = "<script>window.open('DownLoad.xls','');</script>";
        //this.Response.Write(Js);

        //try
        //{
        //    System.IO.FileInfo file = new System.IO.FileInfo("DownLoad.xls");
        //    Response.Buffer = true;
        //    Response.Clear();
        //    Response.Charset = "GB2312";
        //    Response.ContentEncoding = System.Text.Encoding.Default;
        //    // 添加头信息，为"文件下载/另存为"对话框指定默认文件名
        //    Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode("DownLoad.xls"));
        //    // 添加头信息，指定文件大小，让浏览器能够显示下载进度

        //    Response.AddHeader("Content-Length", file.Length.ToString());
        //    // 指定返回的是一个不能被客户端读取的流，必须被下载

        //    Response.ContentType = "application/ms-excel";
        //    // 把文件流发送到客户端

        //    Response.WriteFile(file.FullName);
        //    // 停止页面的执行

        //    Response.End();
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();
        //}
        //catch (Exception ex)
        //{
        //    //Response.Write("<script>alert('系统出现以下错误:\\n" + ex.Message + "!\\n请尽快与管理员联系.')</script>");
        //}
        //try
        //{
        //    this.ClearExcel();
        //    this.SetBuildingName();
        //    //this.SetFloorName();
        //    //this.SetLocationName();
        //    //this.SetShopTypeName();
        //    //this.SetTradeName();
        //    //this.SetUnitTypeName();
        //    //this.SetAreaName();
        //}
        //catch(Exception ex)
        //{ }
        //Response.ContentType = "application/ms-download";
        //System.IO.FileInfo file = new System.IO.FileInfo(Server.MapPath("DownLoad.xls"));
        //Response.Clear();
        //Response.AddHeader("Content-Type", "application/octet-stream");
        //Response.Charset = "utf-8";
        //Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(file.Name, System.Text.Encoding.UTF8));
        //Response.AddHeader("Content-Length", file.Length.ToString());
        //Response.WriteFile(file.FullName);
        //Response.Flush();
        //Response.Clear();
        //Response.End();
        #endregion
        try
        {
            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition", "attachment;filename=DownLoad.xls");
            string filename = Server.MapPath("DownLoad.xls");
            Response.TransmitFile(filename);
        }
        catch { }
    }
    /// <summary>
    /// 清空Excel数据
    /// </summary>
    private void ClearExcel()
    {
        this.ExcelFilePath = Server.MapPath("DownLoad.xls");
        this.SelectedSheetName = "Sheet1";
        BaseBO objBaseBo = new BaseBO();
        try
        {
            this.TLoadDataFrmExcel(this.ExcelFilePath);
            this.odbcon.Open();
            this.BeginTrans();
            this.Insert("UPDATE [" + "Sheet1" + "$] SET  Building='',Floor='',Location='',UnitType='',Area='',Trade='',ShopType='' where 1=1");
            this.Commit();
        }
        catch (Exception ex)
        {
            this.Rollback();
        }
    }
    /// <summary>
    /// 加载大楼名称
    /// </summary>
    private void SetBuildingName()
    {
        this.ExcelFilePath = Server.MapPath(@"DownLoad.xls");
        BaseBO objBaseBo = new BaseBO();
        
        DataSet ds = objBaseBo.QueryDataSet("select BuildingName from Building where storeID='"+this.strStoreID+"'");
        this.SetExcelData(ds, this.ExcelFilePath, 22);
    }
    /// <summary>
    /// 加载楼层名称
    /// </summary>
    private void SetFloorName()
    {
        this.ExcelFilePath = Server.MapPath("DownLoad.xls");
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select distinct FloorName from Floors where storeID='" + this.strStoreID + "'");
        this.SetExcelData(ds, this.ExcelFilePath, 23);
    }
    /// <summary>
    /// 加载方位名称
    /// </summary>
    private void SetLocationName()
    {
        this.ExcelFilePath = Server.MapPath("DownLoad.xls");
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select distinct LocationName from location where storeID='" + this.strStoreID + "'");
        this.SetExcelData(ds, this.ExcelFilePath, 24);
    }
    /// <summary>
    /// 加载单元类别
    /// </summary>
    private void SetUnitTypeName()
    {
        this.ExcelFilePath = Server.MapPath("DownLoad.xls");
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select distinct UnitTypeName from UnitType");
        this.SetExcelData(ds, this.ExcelFilePath, 25);
    }
    /// <summary>
    /// 加载经营区域
    /// </summary>
    private void SetAreaName()
    {
        this.ExcelFilePath = Server.MapPath("DownLoad.xls");
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select distinct AreaName from Area where storeID='" + this.strStoreID + "'");
        this.SetExcelData(ds, this.ExcelFilePath, 26);
    }
    /// <summary>
    /// 加载液态
    /// </summary>
    private void SetTradeName()
    {
        this.ExcelFilePath = Server.MapPath("DownLoad.xls");
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select distinct TradeName from traderelation");
        this.SetExcelData(ds, this.ExcelFilePath, 27);
    }
    /// <summary>
    /// 加载商铺类别
    /// </summary>
    private void SetShopTypeName()
    {
        this.ExcelFilePath = Server.MapPath("DownLoad.xls");
        BaseBO objBaseBo = new BaseBO();
        DataSet ds = objBaseBo.QueryDataSet("select distinct ShopTypeName from ShopType");
        this.SetExcelData(ds, this.ExcelFilePath, 28);
    }
    /// <summary>
    /// 插入数据
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    private int Insert(String sql)
    {
        int count = 0;
        try
        {
            DbCommand comm = this.odbcon.CreateCommand();
            comm.Transaction = this.trans;
            comm.CommandText = sql;
            count = comm.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Rollback();
            throw e;
        }
        return count;
    }
    /**
         * 回滚事务
         */
    internal void Rollback()
    {
        try
        {
            if (this.trans != null)
            {
                this.trans.Rollback();
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            this.odbcon.Close();
            this.odbcon = null;
            this.trans = null;
        }
    }
    /**
        * 提交事务
        */
    private void Commit()
    {
        try
        {
            if (this.odbcon != null)
            {
                this.trans.Commit();
            }
        }
        catch (Exception e)
        {
            this.trans.Rollback();
            throw e;
        }
        finally
        {
            this.odbcon.Close();
            this.odbcon = null;
            this.trans = null;
        }
    }
    /**
         * 开始事务
         */
    private void BeginTrans()
    {
        try
        {
            this.trans = this.odbcon.BeginTransaction();
        }
        catch (Exception e)
        {
            this.odbcon = null;
            throw e;
        }
    }
    /// <summary>
    /// 上传文件
    /// </summary>
    /// <returns></returns>
    private string UpLoadxlsFile()
    {
        Random rd = new Random();
        string strFileName = DateTime.Now.ToString("yyyyMMddHHmmssffffff") + rd.Next(10000).ToString();
        try
        {
            this.File1.PostedFile.SaveAs(this.strLoadFilepath + "\\" + strFileName + ".xls");
        }
        catch(Exception ex) { }
        return this.strLoadFilepath + "\\" + strFileName + ".xls";
    }
    /// <summary>
    /// 导入数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (this.File1.Value.Trim() == "")
            return;
        //this.ExcelFilePath = this.File1.Value;
        this.ExcelFilePath = this.UpLoadxlsFile();

        DataSet ds = new DataSet();
        try
        {
            this.TLoadDataFrmExcel(this.ExcelFilePath);
            this.odbcon.Open();
            ds = this.GetExcelData();
            this.odbcon.Close();
        }
        catch(Exception ex) { }
        Session["dsExcel"] = ds;
        this.RegisterClientScriptBlock("Scri", "<script>WinClose()</script>");
    }
    private void SetExcelData(DataSet ds, string strPath, int iColumn)
    {
        try
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.ApplicationClass();
            app.Visible = false;
            Microsoft.Office.Interop.Excel.Workbook wBook = app.Workbooks._Open(strPath,
            Missing.Value, Missing.Value, Missing.Value, Missing.Value
            , Missing.Value, Missing.Value, Missing.Value, Missing.Value
            , Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            Microsoft.Office.Interop.Excel.Worksheet wSheet = (Microsoft.Office.Interop.Excel.Worksheet)wBook.Sheets[1];
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                wSheet.Cells[i + 2, iColumn] = ds.Tables[0].Rows[i][0].ToString();
            }
            //设置禁止弹出保存和覆盖的询问提示框 
            app.DisplayAlerts = false;
            app.AlertBeforeOverwriting = false;
            //保存工作簿 
            wSheet.SaveAs(strPath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            wBook.SaveAs(strPath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value
               , Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value

            , Missing.Value, Missing.Value);
            //保存excel文件 
            //app.Save(strPath);
            //app.SaveWorkspace(strPath);
            wSheet = null;
            wBook = null;
            app.Quit();
            app = null;
        }
        catch (Exception err)
        {
        }
    }
}
