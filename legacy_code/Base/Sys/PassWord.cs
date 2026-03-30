using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
namespace Base.Sys
{
    public class PassWord
    {
 /// <summary>
    /// 加/解密与MD5加密
    /// </summary>
        #region 私有属性

        /// <summary>
        /// 加密的字符串
        /// </summary>
        private string encryptDecryptStr = "";

        /// <summary>
        /// 返回的字符串
        /// </summary>
        private string mydesStr = "";

        /// <summary>
        /// 错误信息
        /// </summary>
        private string messAge = "";

        #endregion

        #region 公共属性
        /// <summary>
        /// 加密解密的字符串
        /// </summary>
        public string EncryptDecrypStr
        {
            get { return encryptDecryptStr; }
            set { encryptDecryptStr = value; }
        }

        /// <summary>
        /// 返回的字符串
        /// </summary>
        public string MyDesStr
        {
            get { return mydesStr; }
            set { mydesStr = value; }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message
        {
            get { return messAge; }
            set { messAge = value; }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 执行加密
        /// </summary>
        public void DesEncrypt()
        {
            try
            {
                mydesStr = "";
                byte[] MyStr_E = Encoding.UTF8.GetBytes(encryptDecryptStr);
                for (int i = 0; i < MyStr_E.Length; i++)
                {
                    mydesStr = mydesStr + MyStr_E[i].ToString();
                }
            }
            catch (Exception Error)
            {
                this.messAge = "加密出错："+Error.Message;
            }
        }
        #endregion
    }
}



