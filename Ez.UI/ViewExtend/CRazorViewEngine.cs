using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections;


namespace Ez.UI.ViewExtend
{

    public sealed class CRazorViewEngine : RazorViewEngine
    {
        /*
           {0} filename
         * {1} Views
         * 
         */
        private static IList<string> _viewLocationFormats;
        private static int bootPathLength;
        private static DirectoryInfo[] dirs;
        private static string[] unScanDirs;
        private static string path;

        static CRazorViewEngine()
        {
            unScanDirs = new string[] { "images", "image", "content", "bin", "files", "style", "scripts", "js", "data", "obj", "properties", "App_Code" };
            _viewLocationFormats = new List<string>();
            ScanCatalogPath();
            _viewLocationFormats.Add("~/{1}/{0}.cshtml");
        }

        public CRazorViewEngine()
        {
            string[] formatRules = new string[_viewLocationFormats.Count];
            for (int i = 0; i < formatRules.Length; i++)
            {
                formatRules[i] = _viewLocationFormats[i];
            }
            base.ViewLocationFormats=formatRules;
            base.PartialViewLocationFormats=formatRules;
            base.MasterLocationFormats = (new string[] { "~/{1}/{0}.cshtml", "~/{1}/master/{0}.cshtml", "~/Shared/{0}.cshtml", "~/Shared/{1}/{0}.cshtml" });
        }

        /// <summary>
        /// 添加目录
        /// </summary>
        /// <param name="filePath"></param>
        private static void AddViewFormatRule(string filePath)
        {
            path = filePath.Substring(bootPathLength);
            if (path.Length != 0)
            {
                foreach (string str in unScanDirs)
                {
                    if (Regex.IsMatch(path, "(/)*" + str + "(/)*", RegexOptions.IgnoreCase))
                    {
                        return;
                    }
                }
                _viewLocationFormats.Add("~/" + path + "/{1}/{0}.cshtml");
            }
        }


        /// <summary>
        /// 扩展View目录
        /// </summary>
        /// <param name="dir"></param>
        private static void ExploreDirectory(DirectoryInfo dir)
        {
            AddViewFormatRule(dir.FullName);
            dirs = dir.GetDirectories();
            foreach (DirectoryInfo d in dirs)
            {
                ExploreDirectory(d);
            }
        }

        /// <summary>
        /// 定位扩展的View目录
        /// </summary>
        public static void ScanCatalogPath()
        {
            DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath("~/Views/"));
            bootPathLength = dir.FullName.Length - 6;
            
            ExploreDirectory(dir);
        }
    }
}
