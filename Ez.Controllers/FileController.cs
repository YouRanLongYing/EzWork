using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Controllers.Library;
using System.Web;
using System.IO;
using Ez.Helper;
using Ez.UI;

namespace Ez.Controllers
{
    public class FileController : DefaultController
    {
        public JsResult Uploads()
        {
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "text/json";
            string action =Request["action"];
            if (string.IsNullOrEmpty(action))
            {
                if (System.Web.HttpContext.Current.Request.Files.Count > 0)
                {
                    IList<object> jsons = new List<object>();
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase http_file = Request.Files[i];
                        int extend_index = http_file.FileName.LastIndexOf('.');
                        string ext = http_file.FileName.Substring(extend_index + 1);
                        string newname = string.Format("{0}_{1}_{3}.{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), this.CurrentUser == null ? 0 : this.CurrentUser.id, ext, i);
                        string sitepath = Request["folder"] ?? "/Files/";
                        sitepath = sitepath.TrimEnd('/') + "/";
                        string savefilepath = Tools.GetMapPath(sitepath);
                        if (!Directory.Exists(savefilepath)) Directory.CreateDirectory(savefilepath);
                        string newfilepath = savefilepath + newname;
                        http_file.SaveAs(newfilepath);
                        string src = Tools.GetRootUrl(sitepath + newname);
                        if (string.IsNullOrEmpty(Request["from"]))
                        {
                            jsons.Add(new { sourcesrc = src, tmpname = newname });
                        }
                        else
                        {
                            //editor
                            jsons.Add(new { link = src });
                        }
                    }
                    if (string.IsNullOrEmpty(Request["from"]))
                    {

                        return new JsResult(true, jsons, "") { JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet };
                    }
                    else
                    {
                        return new JsResult(jsons.FirstOrDefault()) { JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet };
                    }

                }
                else
                {
                    return new JsResult(false, null, "不存在上传文件！") { JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet };
                }
            }
            else if (action == "del"&&!string.IsNullOrEmpty(Request["src"]))
            {
                //删除文件
                string src = Request["src"];
                string urlHead = string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority);
                if (src.StartsWith(urlHead))
                {
                    string abspath = Tools.GetMapPath(src.Replace(string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority), ""));

                    if (System.IO.File.Exists(abspath))
                    {
                        try
                        {
                            System.IO.File.Delete(abspath);
                            return new JsResult(new { del = "success" }) { JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet };
                        }
                        catch
                        {

                        }
                    }
                }
                return new JsResult(new { del = "fail" }) { JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet };
                
            }
            else if (action == "load")
            {
                //获取图片文件列表
                return new JsResult(null) { JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new JsResult(null) { JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet };
            }
        }
    }
}
