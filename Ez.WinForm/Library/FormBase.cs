using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ez.Plug;

namespace Ez.WinForm.Library
{
    public class FormBase :Form 
    {
        protected Hashtable QueryCollection = new Hashtable();
        public FormBase()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
        }
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected object GetQuery(string key)
        {
            if (QueryCollection.ContainsKey(key))
                return QueryCollection[key];
            else
                return null;
        }
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetQuery(string key, object value)
        {
            if (QueryCollection.ContainsKey(key))
                QueryCollection[key] = value;
            else
                QueryCollection.Add(key, value);
        }
        /// <summary>
        /// 注入参数
        /// </summary>
        /// <param name="querystring"></param>
        public void InjectQuery(string querystring)
        {
            string[] keyvalues = querystring.Split('&');
            foreach (string keyvalue in keyvalues)
            {
                string[] key_value = keyvalue.Split('=');
                if (key_value != null && key_value.Length == 2)
                {
                    this.SetQuery(key_value[0], key_value[1]);
                }
            }
        
        }
        protected Control GetCtrlInstance(string uri)
        {
            int userid = 0;
            int.TryParse(GetQuery("uid").ToString(), out userid);
            return Ez.WinForm.Library.Utils.GetFormInstance(uri, new TransData { CurrentUserID =userid, CurrentUserName = "endfalse" });
        }
    }
}
