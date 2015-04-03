using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ez.Lang;

namespace Ez.UI
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CDisplayNameAttribute : Attribute, IMetadataAware
    {
        public string DefaultName { get; set; } //默认显示名称
        public string LangKey { get; set; } //默认显示名称
        public bool StartWithConfirmName{ get; set; }
        public CDisplayNameAttribute() {}
        protected CDisplayNameAttribute(string key)
        {
            this.LangKey = key;
        }
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            string displayName = null;
            if (!string.IsNullOrEmpty(this.LangKey))
            {
                displayName = EzLanguage.ResourceManager.GetString(this.LangKey, System.Threading.Thread.CurrentThread.CurrentUICulture);
                if (this.StartWithConfirmName)
                {
                   string perstr = EzLanguage.ResourceManager.GetString("SYS_Val_StartWithComfirm", System.Threading.Thread.CurrentThread.CurrentUICulture);
                   displayName = perstr + displayName;
                }
                

            }
            metadata.DisplayName = string.IsNullOrEmpty(displayName) ? this.LangKey : displayName;
            if (string.IsNullOrEmpty(this.DefaultName)&&metadata.DisplayName.Equals(this.LangKey))
            {
                //metadata.ModelMetadataProvider
                //如果显示的名字和key相同说明可能在产品框架的语言包中有定义，那么交给CRequiredAttributeAdapter的桥接器处理,这里需要增加一个标识
                metadata.DisplayName = "Please_fetch_from_ProResource_" + this.LangKey;
            }
            else if (!string.IsNullOrEmpty(this.DefaultName))
            {
                metadata.DisplayName = this.DefaultName;
            }
        }
    }
}
