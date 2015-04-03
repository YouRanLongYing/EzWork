using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace Ez.Helper
{
    /// <summary>
    /// 下载方式
    /// </summary>
    public enum DownLoadType
    {
        /// <summary>
        /// Response.TransmitFile方式
        /// </summary>
        TransmitFile,
        /// <summary>
        /// Response.WriteFile方式
        /// </summary>
        WriteFile,
        /// <summary>
        /// Response.OutputStream.Write分块下载方式
        /// </summary>
        StreamBlock,
        /// <summary>
        /// Response.BinaryWrite整个文件流方式
        /// </summary>
        Stream
    }
    public static class WebDownLoadExtenion
    {
        /// <summary>
        /// Web服务器文件下载
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="savename">文件保存名（下载时显示的文件名）</param>
        /// <param name="downLoadType">下载方式</param>
        /// <param name="delete">下载完是否删除文件(此功能暂不开放)</param>
        public static void DownLoad(this FileInfo fileInfo, string savename,DownLoadType downLoadType,bool delete=false)
        {
            string fileName = savename;//客户端保存的文件名
            if (fileInfo.Exists == true)
            {
                switch (downLoadType)
                {
                    case DownLoadType.TransmitFile:
                        {
                            HttpContext.Current.Response.ContentType = "application/x-zip-compressed";
                            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=z.zip");
                            HttpContext.Current.Response.TransmitFile(fileInfo.FullName);
                        }break;
                    case DownLoadType.WriteFile:
                        {
                            HttpContext.Current.Response.Clear();
                            HttpContext.Current.Response.ClearContent();
                            HttpContext.Current.Response.ClearHeaders();
                            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                            HttpContext.Current.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                            HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "binary");
                            HttpContext.Current.Response.ContentType = "application/octet-stream";
                            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;//.GetEncoding("gb2312");
                            HttpContext.Current.Response.WriteFile(fileInfo.FullName);
                            HttpContext.Current.Response.Flush();
                            HttpContext.Current.Response.End();
                        }break;
                    case DownLoadType.StreamBlock:
                        {
                            const long ChunkSize = 102400;//100K 每次读取文件，只读取100K，这样可以缓解服务器的压力
                            byte[] buffer = new byte[ChunkSize];

                            HttpContext.Current.Response.Clear();
                            System.IO.FileStream iStream = System.IO.File.OpenRead(fileInfo.FullName);
                            long dataLengthToRead = iStream.Length;//获取下载的文件总大小
                            HttpContext.Current.Response.ContentType = "application/octet-stream";
                            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                            while (dataLengthToRead > 0 && HttpContext.Current.Response.IsClientConnected)
                            {
                                int lengthRead = iStream.Read(buffer, 0, Convert.ToInt32(ChunkSize));//读取的大小
                                HttpContext.Current.Response.OutputStream.Write(buffer, 0, lengthRead);
                                HttpContext.Current.Response.Flush();
                                dataLengthToRead = dataLengthToRead - lengthRead;
                            }
                            HttpContext.Current.Response.Close();
                            iStream.Close();
                            iStream.Dispose();
                        }break;
                    case DownLoadType.Stream:
                        {
                            //以字符流的形式下载文件
                            FileStream fs = new FileStream(fileInfo.FullName, FileMode.Open);
                            byte[] bytes = new byte[(int)fs.Length];
                            fs.Read(bytes, 0, bytes.Length);
                            fs.Close();
                            HttpContext.Current.Response.ContentType = "application/octet-stream";
                            //通知浏览器下载文件而不是打开
                            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;  filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                            HttpContext.Current.Response.BinaryWrite(bytes);
                            HttpContext.Current.Response.Flush();
                            HttpContext.Current.Response.End();
                            fs.Close();
                            fs.Dispose();
                        }break;
                
                }
                //if (delete) fileInfo.Delete();
            }
        }
        /// <summary>
        /// Web服务器文件下载
        /// </summary>
        /// <param name="filePathName">文件物理路径</param>
        /// <param name="savename">文件保存名（下载时显示的文件名）</param>
        /// <param name="downLoadType">下载方式</param>
        /// <param name="delete">下载完是否删除文件(此功能暂不开放)</param>
        public static void DownLoad(string filePathName, string savename, DownLoadType downLoadType, bool delete = false)
        {
            string filePath = Tools.GetMapPath(filePathName);
            FileInfo fileInfo = new FileInfo(filePath);
            fileInfo.DownLoad(savename, downLoadType, delete);
        }
    }
}
