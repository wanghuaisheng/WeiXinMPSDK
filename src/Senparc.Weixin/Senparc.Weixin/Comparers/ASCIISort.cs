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
    
    文件名：ASCIISort.cs
    文件功能描述：ASCII排序
    
    
    创建标识：Senparc - 20170623

    修改标识：Senparc - 20170623
    修改描述：添加ASCIISort.Create()静态方法

----------------------------------------------------------------*/

using System;
using System.Collections;

namespace Senparc.Weixin.Comparers
{
    /// <summary>
    /// ASCII字典排序
    /// </summary>
    public class AsciiSort : IComparer
    {
        /// <summary>
        /// 创建新的ASCIISort实例
        /// </summary>
        /// <returns></returns>
        public static AsciiSort Create()
        {
            return new AsciiSort();
        }

        public int Compare(object x, object y)
        {
            byte[] xBytes = System.Text.Encoding.Default.GetBytes(x.ToString());
            byte[] yBytes = System.Text.Encoding.Default.GetBytes(y.ToString());

            int xLength = xBytes.Length;
            int yLength = yBytes.Length;
            int minLength = Math.Min(xLength, yLength);

            for (int i = 0; i < minLength; i++)
            {
                var xByte = xBytes[i];
                var yByte = yBytes[i];
                if (xByte > yByte)
                {
                    return 1;
                }
                if (xByte < yByte)
                {
                    return -1;
                }
            }

            if (xLength == yLength)
            {
                return 0;
            }
            if (xLength > yLength)
            {
                return 1;
            }
            return -1;
        }
    }
}
