/*----------------------------------------------------------------
    Copyright (C) 2017 Senparc

    文件名：WebSocketRouteHandler.cs
    文件功能描述：WebSocketRouteHandler，处理WebSocket请求


    创建标识：Senparc - 20170126

----------------------------------------------------------------*/
using System.Web;
using System.Web.Routing;

namespace Senparc.WebSocket
{
    /// <summary>
    /// WebSocketHansler，处理WebSocket请求
    /// </summary>
    public class WebSocketRouteHandler: IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new WebSocketHandler(requestContext);
        }
    }
}
