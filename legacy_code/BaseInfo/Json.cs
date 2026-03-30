using System;
using System.Collections.Generic;
using System.Text;

namespace BaseInfo
{
    using Newtonsoft.Json;
    using System.Data;
    public class Json
    {
        /// <summary>
        /// 返回Json格式的字符串
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>string</returns>
        public static string GetJson(DataTable dt)
        {
            return JavaScriptConvert.SerializeObject(dt, new DataTableConverter());
        }

        /// <summary>
        /// 返回Json格式的字符串
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <returns>string</returns>
        public static string GetJson(DataSet ds)
        {
            return JavaScriptConvert.SerializeObject(ds, new DataSetConverter());
        }

        /// <summary>
        /// 返回符合SigmGrid数据格式的Json字符串
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>String</returns>
        public static string GetJsonForSigma(DataTable dt)
        {
            string strTemp = ("{\"data\":") + JavaScriptConvert.SerializeObject(dt, new DataTableConverter())
                             +",\"pageInfo\":{\"totalRowNum\":" + dt.Rows.Count +"},\"exception\":null}";
            return strTemp;
        }

        private class DataSetConverter : Newtonsoft.Json.JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return typeof(DataSet).IsAssignableFrom(objectType);
            }

            public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value)
            {
                DataSet ds = (DataSet)value;
                writer.WriteStartObject();
                foreach (DataTable dt in ds.Tables)
                {
                    writer.WritePropertyName(dt.TableName);
                    writer.WriteStartArray();
                    foreach (DataRow dr in dt.Rows)
                    {
                        writer.WriteStartObject();
                        foreach (DataColumn dc in dt.Columns)
                        {
                            writer.WritePropertyName(dc.ColumnName);
                            writer.WriteValue(dr[dc].ToString());
                        }
                        writer.WriteEndObject();
                    }
                    writer.WriteEndArray();
                }
                writer.WriteEndObject();
            }
        }
        private class DataTableConverter : Newtonsoft.Json.JsonConverter
        {
            public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value)
            {
                DataTable dt = (DataTable)value;
                
                writer.WriteStartArray();
                foreach (DataRow dr in dt.Rows)
                {
                    writer.WriteStartObject();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        writer.WritePropertyName(dc.ColumnName);
                        writer.WriteValue(dr[dc].ToString());
                    }
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }

            public override bool CanConvert(Type objectType)
            {
                return typeof(DataTable).IsAssignableFrom(objectType);
            }
        }
    }
}
