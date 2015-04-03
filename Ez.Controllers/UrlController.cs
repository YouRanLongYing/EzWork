using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Controllers.Library;
using Ez.Helper;
using System.IO;
using Ez.Dtos.Entities;

namespace Ez.Controllers
{
    public class UrlController:DefaultController
    {
        const string fileFlag = "file->";
        /// <summary>
        /// 跳转
        /// </summary>
        public void Index(string code)
        {
            FW_MappedUrl entity = this.ShortUrlBizInstance.GetUrlMap(code);
            if (entity != null && !string.IsNullOrEmpty(entity.source_url))
            {
                string url = entity.source_url;
                if (url.StartsWith(fileFlag))
                {
                    url = entity.source_url.Substring(fileFlag.Length);
                    string path="";
                    string svrroot = Tools.GetRootUrl("");
                    if (url.StartsWith(svrroot))
                        url = url.Replace(svrroot,"");
                    if (!string.IsNullOrEmpty(url.TrimEnd('/')))
                    {
                        path = Tools.GetMapPath(url);
                        FileInfo finfo = new FileInfo(path);
                        if (finfo.Exists)
                        {
                            FileStream filestream = new FileStream(finfo.FullName, FileMode.Open, FileAccess.Read);
                            byte[] buffer = new byte[filestream.Length];
                            filestream.Read(buffer, 0, (int)filestream.Length - 1);

                            Response.ClearContent();
                            Response.ContentType = Tools.GetFileContentType(finfo.Extension); //"image/Png";
                            Response.BinaryWrite(buffer);
                            Response.End();
                        }
                        else
                        {
                            Response.Redirect("/404.html");
                        }
                    }
                    else
                    {
                        Response.Redirect("/404.html");
                    }
                }
                else if (url.ToLower().StartsWith("http://") || url.ToLower().StartsWith("https://") || url.ToLower().StartsWith("ftp://"))
                {
                    Response.Redirect(url);
                }
                else
                {
                    Response.Redirect("/404.html");
                }
            }
            else
            {
                Response.Redirect("/404.html");
            }
        }
    }
}
