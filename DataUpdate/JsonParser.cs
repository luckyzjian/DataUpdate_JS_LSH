using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using System.ComponentModel;
using System.Data;
using System.Drawing;
 
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices; //读取ini配置文件所需要的命名空间
using System.Text.RegularExpressions; //正则
using System.Drawing.Imaging;   //缩放图像
using System.Drawing.Drawing2D; //缩放图像
//using MSWord = Microsoft.Office.Interop.Word;


using System.Web;

using System.Web.Script.Serialization;



using Newtonsoft.Json.Converters;

using System.Runtime.Serialization;


namespace DataUpdate
{
    class JsonParser
    {
        private static Dictionary<string, string> lst_KeyValueData = null;

        public static Dictionary<string, string> SplitJsonStringToKeyValuePairs(string jsonStr)
        {
            char jsonBeginToken = '{';
            char jsonEndToken = '}';

            if (string.IsNullOrEmpty(jsonStr))
            {
                return null;
            }
            //验证json字符串格式
            if (jsonStr[0] != jsonBeginToken || jsonStr[jsonStr.Length - 1] != jsonEndToken)
            {
                throw new ArgumentException("非法的Json字符串!");
            }

            lst_KeyValueData = new Dictionary<string, string>();

            JObject jobj = new JObject();

            // 转换为json对象
            jobj = JObject.Parse(jsonStr);
            ParseJsonProperties(jobj);

            return lst_KeyValueData;

        }


        private static void ParseJsonProperties(JObject jObject)
        {
            IEnumerable<JProperty> jObject_Properties = jObject.Properties();

            JTokenType[] validPropertyValueTypes = { JTokenType.String, JTokenType.Integer, JTokenType.Float, JTokenType.Boolean, JTokenType.Null, JTokenType.Date, JTokenType.Bytes, JTokenType.Guid, JTokenType.Uri, JTokenType.TimeSpan };
            List<JTokenType> propertyTypes = new List<JTokenType>(validPropertyValueTypes);

            JTokenType[] validObjectTypes = { JTokenType.String, JTokenType.Array, JTokenType.Object };
            List<JTokenType> objectTypes = new List<JTokenType>(validObjectTypes);



            foreach (JProperty property in jObject_Properties)
            {


                try
                {
                    if (propertyTypes.Contains(property.Value.Type))
                    {
                        ParseJsonKeyValue(property, property.Name.ToString());
                    }
                    else if (objectTypes.Contains(property.Value.Type))
                    {
                        if (property.Value.Type == JTokenType.Array && property.Value.HasValues)
                        {
                            ParseJsonArray(property);
                        }

                        if (property.Value.Type == JTokenType.Object)
                        {
                            JObject jo = new JObject();
                            jo = JObject.Parse(property.Value.ToString());
                            string paramName = property.Name.ToString();

                            lst_KeyValueData.Add(paramName, property.Value.ToString());

                            if (jo.HasValues)
                            {
                                ParseJsonProperties(jo);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

            }

        }

        private static void ParseJsonKeyValue(JProperty item, string paramName)
        {
            lst_KeyValueData.Add(paramName, item.Value.ToString());
        }

        private static void ParseJsonArray(JProperty item)
        {
            JArray jArray = (JArray)item.Value;

            string paramName = item.Name.ToString();
            lst_KeyValueData.Add(paramName, item.Value.ToString());



            try
            {
                for (int i = 0; i < jArray.Count; i++)
                {

                    paramName = i.ToString();
                    lst_KeyValueData.Add(paramName, jArray.Values().ElementAt(i).ToString());

                    JObject jo = new JObject();
                    jo = JObject.Parse(jArray[i].ToString());
                    IEnumerable<JProperty> jArrayEnum = jo.Properties();

                    foreach (JProperty jaItem in jArrayEnum)
                    {
                        var paramNameWithJaItem = jaItem.Name.ToString();

                        var itemValue = jaItem.Value.ToString(Formatting.None);
                        if (itemValue.Length > 0)
                        {
                            switch (itemValue.Substring(0, 1))
                            {
                                case "[":
                                    ParseJsonArray(jaItem);
                                    break;
                                case "{":
                                    JObject joObject = new JObject();
                                    joObject = JObject.Parse(itemValue);

                                    ParseJsonProperties(joObject);
                                    break;
                                default:
                                    ParseJsonKeyValue(jaItem, paramNameWithJaItem);
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }













    }
}
