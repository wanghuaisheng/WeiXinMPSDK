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

    文件名：SenparcHttpResponse.cs
    文件功能描述：统一封装HttpResonse请求，提供Http请求过程中的调试、跟踪等扩展能力


    创建标识：Senparc - 20171104

    修改描述：统一封装HttpResonse请求

----------------------------------------------------------------*/

using System.Net;
#if NET35 || NET40 || NET45

#else
using System.Net.Http;
using System.Net.Http.Headers;
#endif

namespace Senparc.Weixin.HttpUtilities
{
    /// <summary>
    /// 统一封装HttpResonse请求，提供Http请求过程中的调试、跟踪等扩展能力
    /// </summary>
    public class SenparcHttpResponse
    {
        public HttpWebResponse Result { get; set; }

        public SenparcHttpResponse(HttpWebResponse httpWebResponse)
        {
            Result = httpWebResponse;
        }
    }
}