#region Apache License Version 2.0
/*----------------------------------------------------------------

Copyright 2017 Jeffrey Su & Suzhou Senparc Network Technology Co.,Ltd.

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file
except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the
License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
either express or implied. See the License for the specific language governing permissions
and limitations under the License.

Detail: https://github.com/JeffreySu/WeiXinMPSDK/blob/master/license.md

----------------------------------------------------------------*/
#endregion Apache License Version 2.0

/*----------------------------------------------------------------
    Copyright (C) 2017 Senparc

    文件名：Get.cs
    文件功能描述：Get


    创建标识：Senparc - 20150211

    修改标识：Senparc - 20150303
    修改描述：整理接口

    修改标识：zeje - 20160422
    修改描述：v4.5.19 为GetJson方法添加maxJsonLength参数

    修改标识：zeje - 20170305
    修改描述：MP v14.3.132 添加Get.DownloadAsync(string url, string dir)方法

    修改标识：Senparc - 20170409
    修改描述：v4.11.9 修改Download方法

    修改标识：Senparc - 20171101
    修改描述：v4.18.1 修改Get.Download()方法

----------------------------------------------------------------*/


using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
#if !NET35 && !NET40
using System.Net.Http;
using System.Threading.Tasks;
#endif
using System.Text;
#if NET35 || NET40 || NET45
using System.Web.Script.Serialization;
#endif
using Senparc.Weixin.Entities;
using Senparc.Weixin.Exceptions;

namespace Senparc.Weixin.HttpUtility
{
    /// <summary>
    /// Get 请求处理
    /// </summary>
    public static class Get
    {
        #region 同步方法

        /// <summary>
        /// GET方式请求URL，并返回T类型
        /// </summary>
        /// <typeparam name="T">接收JSON的数据类型</typeparam>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="maxJsonLength">允许最大JSON长度</param>
        /// <returns></returns>
        public static T GetJson<T>(string url, Encoding encoding = null, int? maxJsonLength = null)
        {
            string returnText = RequestUtility.HttpGet(url, encoding);

            WeixinTrace.SendApiLog(url, returnText);

            JavaScriptSerializer js = new JavaScriptSerializer();
            if (maxJsonLength.HasValue)
            {
                js.MaxJsonLength = maxJsonLength.Value;
            }

            if (returnText.Contains("errcode"))
            {
                //可能发生错误

                WxJsonResult errorResult = js.Deserialize<WxJsonResult>(returnText);
                //WxJsonResult errorResult =
                //    Newtonsoft.Json.JsonConvert.DeserializeObject<WxJsonResult>(returnText);

                if (errorResult.errcode != ReturnCodeMp.请求成功)
                {
                    //发生错误
                    throw new ErrorJsonResultException(
                        $"微信请求发生错误！错误代码：{(int)errorResult.errcode}，说明：{errorResult.errmsg}", null, errorResult, url);
                }
            }
            T result = js.Deserialize<T>(returnText);

            //T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(returnText);


            return result;
        }

        /// <summary>
        /// 从Url下载
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream"></param>
        public static void Download(string url, Stream stream)
        {
            WebClient wc = new WebClient();
            var data = wc.DownloadData(url);
            stream.Write(data, 0, data.Length);
        }

        //#if !NET35 && !NET40
        /// <summary>
        /// 从Url下载，并保存到指定目录
        /// </summary>
        /// <param name="url">需要下载文件的Url</param>
        /// <param name="filePathName"></param>
        /// <returns></returns>
        public static string Download(string url, string filePathName)
        {
            var dir = Path.GetDirectoryName(filePathName) ?? "/";
            Directory.CreateDirectory(dir);
            System.Net.Http.HttpClient httpClient = new HttpClient();
            using (var responseMessage = httpClient.GetAsync(url).Result)
            {
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var fullName = filePathName;// Path.Combine(dir, responseMessage.Content.Headers.ContentDisposition.FileName.Trim('"'));//ContentDisposition可能会为Null
                    using (var fs = File.Open(fullName, FileMode.Create))
                    {
                        using (var responseStream = responseMessage.Content.ReadAsStreamAsync().Result)
                        {
                            responseStream.CopyTo(fs);
                            return fullName;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        //#endif
        #endregion

        #region 异步方法

        /// <summary>
        /// 【异步方法】异步GetJson
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="maxJsonLength">允许最大JSON长度</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ErrorJsonResultException"></exception>
        public static async Task<T> GetJsonAsync<T>(string url, Encoding encoding = null, int? maxJsonLength = null)
        {
            string returnText = await RequestUtility.HttpGetAsync(url, encoding);

            JavaScriptSerializer js = new JavaScriptSerializer();
            if (maxJsonLength.HasValue)
            {
                js.MaxJsonLength = maxJsonLength.Value;
            }

            if (returnText.Contains("errcode"))
            {
                //可能发生错误
                WxJsonResult errorResult = js.Deserialize<WxJsonResult>(returnText);
                //WxJsonResult errorResult =
                //    Newtonsoft.Json.JsonConvert.DeserializeObject<WxJsonResult>(returnText);

                if (errorResult.errcode != ReturnCodeMp.请求成功)
                {
                    //发生错误
                    throw new ErrorJsonResultException(
                        $"微信请求发生错误！错误代码：{(int)errorResult.errcode}，说明：{errorResult.errmsg}", null, errorResult, url);
                }
            }
            T result = js.Deserialize<T>(returnText);
            //T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(returnText);
            return result;
        }

        /// <summary>
        /// 【异步方法】异步从Url下载
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task DownloadAsync(string url, Stream stream)
        {
            WebClient wc = new WebClient();
            var data = await wc.DownloadDataTaskAsync(url);
            await stream.WriteAsync(data, 0, data.Length);
        }

        /// <summary>
        /// 【异步方法】从Url下载，并保存到指定目录
        /// </summary>
        /// <param name="url">需要下载文件的Url</param>
        /// <param name="filePathName"></param>
        /// <returns></returns>
        public static async Task<string> DownloadAsync(string url, string filePathName)
        {
            var dir = Path.GetDirectoryName(filePathName) ?? "/";
            Directory.CreateDirectory(dir);

            System.Net.Http.HttpClient httpClient = new HttpClient();
            using (var responseMessage = await httpClient.GetAsync(url))
            {
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var fullName = filePathName;// Path.Combine(dir, responseMessage.Content.Headers.ContentDisposition.FileName.Trim('"'));//ContentDisposition可能会为Null
                    using (var fs = File.Open(fullName, FileMode.Create))
                    {
                        using (var responseStream = await responseMessage.Content.ReadAsStreamAsync())
                        {
                            await responseStream.CopyToAsync(fs);
                            return fullName;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

    }
}
