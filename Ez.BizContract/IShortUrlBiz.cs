using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Dtos.Entities;

namespace Ez.BizContract
{
    public interface IShortUrlBiz:IDefaultBiz
    {
        /// <summary>
        /// 获取Url地址Map
        /// </summary>
        /// <param name="shortcode">短命名地址</param>
        /// <returns>原始Url地址</returns>
        FW_MappedUrl GetUrlMap(string shortcode);
        /// <summary>
        /// 设置Url映射(设置前会检测是否已对此Url进行了设置)
        /// </summary>
        /// <param name="url">Url必须是以http|FTP|https开头的</param>
        /// <param name="data">携带的业务数据格式如：gpmx-id,即业务标记-数据,常规的做法一般不需要设置此参数，如有地址带有文件下载信息时才会用到
        /// ：如果使用次参数请自行编写关于此地址的解析逻辑
        /// </param>
        /// <param name="isfile">是否为文件地址</param>
        /// <param name="shortcode">不为空时使用指定的短命名代码</param>
        /// <returns>是否设置成功</returns>
        string SetShortUrlMap(string url,string data, bool isfile, string shortcode);
        /// <summary>
        /// 设置Url映射(设置前会检测是否已对此Url进行了设置)
        /// </summary>
        /// <param name="url">Url必须是以http|FTP|https开头的</param>
        /// <param name="data">携带的业务数据格式如：gpmx-id,即业务标记-数据,常规的做法一般不需要设置此参数，如有地址带有文件下载信息时才会用到
        /// ：如果使用次参数请自行编写关于此地址的解析逻辑
        /// </param>
        /// <param name="isfile">是否为文件地址</param>
        /// <returns>是否设置成功</returns>
        string SetShortUrlMap(string url, string data, bool isfile);
        /// <summary>
        /// 设置Url映射(设置前会检测是否已对此Url进行了设置),非文件地址
        /// </summary>
        /// <param name="url">Url必须是以http|FTP|https开头的</param>
        /// <param name="data">携带的业务数据格式如：gpmx-id,即业务标记-数据,常规的做法一般不需要设置此参数，如有地址带有文件下载信息时才会用到
        /// ：如果使用次参数请自行编写关于此地址的解析逻辑
        /// </param>
        /// <returns>是否设置成功</returns>
        string SetShortUrlMap(string url, string data);
        /// <summary>
        /// 设置Url映射(设置前会检测是否已对此Url进行了设置)非文件地址且无携带数据
        /// </summary>
        /// <param name="url">Url必须是以http|FTP|https开头的</param>
        /// <returns>是否设置成功</returns>
        string SetShortUrlMap(string url);



        /// <summary>
        /// 生成可用的短URL
        /// </summary>
        /// <param name="url">原始URL</param>
        /// <returns></returns>
        string GetUrl(string url);
    }
}
