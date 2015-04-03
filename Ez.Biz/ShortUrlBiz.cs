using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.BizContract;
using Ez.Dtos.Entities;
using Ez.Core;
using Ez.Helper;
using Ez.DBContract;
namespace Ez.Biz
{
    /// <summary>
    /// 短Url处理逻辑
    /// </summary>
    public class ShortUrlBiz:DefaultBiz,IShortUrlBiz
    {
        const string fileFlag = "file->";
        /// <summary>
        /// 获取Url地址
        /// </summary>
        /// <param name="shortcode">短命名地址</param>
        /// <returns>原始Url地址</returns>
        public FW_MappedUrl GetUrlMap(string shortcode)
        {
            return this.ProDb.GetEntity<FW_MappedUrl>("select source_url,short_code,with_data,create_time,login_id from FW_Mapped_Url where short_code=@scode", new DbParam("@scode", shortcode));
        }
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
        public string SetShortUrlMap(string url,string data,bool isfile,string shortcode)
        {
            if (isfile) url = fileFlag + url;
            string[] codes = Tools.ShortUrl(url);
            shortcode = string.IsNullOrEmpty(shortcode) ? codes[0] : shortcode;
            FW_MappedUrl entity = this.ProDb.GetEntity<FW_MappedUrl>("select source_url,short_code,create_time,login_id from FW_Mapped_Url where short_code=@scode", new DbParam("@scode", shortcode));
            if (entity == null)
            {
                this.UcDb.ExecuteSql("insert into FW_Mapped_Url(source_url,short_code,with_data,create_time,login_id) values(@url,@surl,@data,@time,@loginid)",
                      new DbParam("@url", url), new DbParam("@surl", shortcode), new DbParam("@data", data), new DbParam("@time", DateTime.Now), new DbParam("@loginid",this.CurrentUser.id));
            }
            return shortcode;
        }
        /// <summary>
        /// 设置Url映射(设置前会检测是否已对此Url进行了设置)
        /// </summary>
        /// <param name="url">Url必须是以http|FTP|https开头的</param>
        /// <param name="data">携带的业务数据格式如：gpmx-id,即业务标记-数据,常规的做法一般不需要设置此参数，如有地址带有文件下载信息时才会用到
        /// ：如果使用次参数请自行编写关于此地址的解析逻辑
        /// </param>
        /// <param name="isfile">是否为文件地址</param>
        /// <returns>是否设置成功</returns>
        public string SetShortUrlMap(string url, string data, bool isfile)
        {
            return SetShortUrlMap(url, data, isfile,"");
        }
        /// <summary>
        /// 设置Url映射(设置前会检测是否已对此Url进行了设置),非文件地址
        /// </summary>
        /// <param name="url">Url必须是以http|FTP|https开头的</param>
        /// <param name="data">携带的业务数据格式如：gpmx-id,即业务标记-数据,常规的做法一般不需要设置此参数，如有地址带有文件下载信息时才会用到
        /// ：如果使用次参数请自行编写关于此地址的解析逻辑
        /// </param>
        /// <param name="isfile">是否为文件地址</param>
        /// <returns>是否设置成功</returns>
        public string SetShortUrlMap(string url, string data)
        {
            return SetShortUrlMap(url, data, false, "");
        }
        /// <summary>
        /// 设置Url映射(设置前会检测是否已对此Url进行了设置)非文件地址且无携带数据
        /// </summary>
        /// <param name="url">Url必须是以http|FTP|https开头的</param>
        /// <returns>是否设置成功</returns>
        public string SetShortUrlMap(string url)
        {
            return SetShortUrlMap(url, "", false, "");
        }
        /// <summary>
        /// 生成可用的短URL
        /// </summary>
        /// <param name="url">原始URL</param>
        /// <returns></returns>
        public string GetUrl(string url)
        {
            return Tools.GetRootUrl("/url/" + Tools.ShortUrl(url)[0]);
        }
    }
}
