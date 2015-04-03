using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.Lang
{
    public partial class EzLanguage
    {
        /// <summary>
        ///产品资源入口
        /// </summary>
        public static Ez.Lang.LanguageAssemblyProvider AssemblyProvider { set; get; }
        private static global::Ez.Lang.CResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public EzLanguage()
        {
        }

        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::Ez.Lang.CResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::Ez.Lang.CResourceManager temp = new global::Ez.Lang.CResourceManager("Ez.Lang.EzLanguage", typeof(EzLanguage).Assembly);
                    if (EzLanguage.AssemblyProvider != null && !string.IsNullOrEmpty(EzLanguage.AssemblyProvider.AssemblyName) && EzLanguage.AssemblyProvider.AssemblyType != null)
                    {
                        temp.CustomResourceManager = new global::System.Resources.ResourceManager(EzLanguage.AssemblyProvider.AssemblyName, EzLanguage.AssemblyProvider.AssemblyType.Assembly);
                    }

                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }
    }
}
