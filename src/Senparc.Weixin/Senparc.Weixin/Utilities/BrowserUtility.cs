﻿#region Apache License Version 2.0
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

    文件名：BrowserUtility.cs
    文件功能描述：浏览器公共类


    创建标识：Senparc - 20150419

    修改标识：Senparc - 20161219
    修改描述：v4.9.6 修改错别字：Browser->Browser

    修改标识：Senparc - 20161219
    修改描述：v4.11.2 修改SideInWeixinBrowser判断逻辑

----------------------------------------------------------------*/
using System.Web;

namespace Senparc.Weixin.Utilities
{
    /// <summary>
    /// 浏览器公共类
    /// </summary>
    public static class BrowserUtility
    {

        /// <summary>
        /// 判断是否在微信内置浏览器中
        /// </summary>
        /// <param name="httpContext">HttpContextBase对象</param>
        /// <returns>true：在微信内置浏览器内。false：不在微信内置浏览器内。</returns>
        public static bool SideInWeixinBrowser(this HttpContextBase httpContext)
        {
            var userAgent = httpContext.Request.UserAgent;
            return userAgent != null 
                   && (userAgent.Contains("MicroMessenger") || userAgent.Contains("Windows Phone"));
        }
    }
}
