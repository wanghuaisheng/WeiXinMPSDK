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

    文件名：LocalContainerCacheStrategy.cs
    文件功能描述：本地容器缓存。


    创建标识：Senparc - 20160308

    修改标识：Senparc - 20160812
    修改描述：v4.7.4  解决Container无法注册的问题

    修改标识：Senparc - 20170205
    修改描述：v0.2.0 重构分布式锁

 ----------------------------------------------------------------*/


using System;
using System.Collections.Generic;
using Senparc.Weixin.Caches.CacheStrategy;
using Senparc.Weixin.Caches.ContainerCacheStrategy;
using Senparc.Weixin.Caches.Lock;
using Senparc.Weixin.Caches.ObjectCacheStrategy;
using Senparc.Weixin.CachesLocal.ContainerCacheStragegy;
using Senparc.Weixin.CachesLocal.Lock;

namespace Senparc.Weixin.CachesLocal.ObjectCacheStrategy
{
    /// <summary>
    /// 全局静态数据源帮助类
    /// </summary>
    public static class LocalObjectCacheHelper
    {
        /// <summary>
        /// 所有数据集合的列表
        /// </summary>
        internal static IDictionary<string, object> LocalObjectCache { get; set; }

        static LocalObjectCacheHelper()
        {
            LocalObjectCache = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// 本地容器缓存策略
    /// </summary>
    public class LocalObjectCacheStrategy : BaseCacheStrategy, IObjectCacheStrategy
    //where TContainerBag : class, IBaseContainerBag, new()
    {
        #region 数据源

        private readonly IDictionary<string, object> _cache = LocalObjectCacheHelper.LocalObjectCache;

        #endregion

        #region 单例

        //静态LocalCacheStrategy
        public static LocalObjectCacheStrategy Instance => Nested.instance;

        class Nested
        {
            static Nested()
            {
            }
            //将instance设为一个初始化的LocalCacheStrategy新实例
            internal static readonly LocalObjectCacheStrategy instance = new LocalObjectCacheStrategy();
        }


        #endregion

        #region IObjectCacheStrategy 成员

        public IContainerCacheStrategy ContainerCacheStrategy => LocalContainerCacheStrategy.Instance;

        public void InsertToCache(string key, object value)
        {
            if (key == null || value == null)
            {
                return;
            }

            var finalKey = GetFinalKey(key);

            _cache[finalKey] = value;
        }

        public void RemoveFromCache(string key, bool isFullKey = false)
        {
            var cacheKey = GetFinalKey(key, isFullKey);
            _cache.Remove(cacheKey);
        }

        public object Get(string key, bool isFullKey = false)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            if (!CheckExisted(key, isFullKey))
            {
                return null;
                //InsertToCache(key, new ContainerItemCollection());
            }

            var cacheKey = GetFinalKey(key, isFullKey);
            return _cache[cacheKey];
        }
        

        public IDictionary<string, object> GetAll()
        {
            return _cache;
        }

        public bool CheckExisted(string key, bool isFullKey = false)
        {
            var cacheKey = GetFinalKey(key, isFullKey);
            return _cache.ContainsKey(cacheKey);
        }

        public long GetCount()
        {
            return _cache.Count;
        }

        public void Update(string key, object value, bool isFullKey = false)
        {
            var cacheKey = GetFinalKey(key, isFullKey);
            _cache[cacheKey] = value;
        }

        public void UpdateContainerBag(string key, object bag, bool isFullKey = false)
        {
            Update(key, bag, isFullKey);
        }

        #endregion

        #region ICacheLock

        public override ICacheLock BeginCacheLock(string resourceName, string key, int retryCount = 0, TimeSpan retryDelay = new TimeSpan())
        {
            return new LocalCacheLock(this, resourceName, key, retryCount, retryDelay);
        }

        #endregion

    }
}
