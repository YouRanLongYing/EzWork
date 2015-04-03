using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Ez.Core.Attributes;
using Ez.Dtos.Library;
using Ez.UI.HtmlExtends.FormAttributes;
using Ez.UI;

namespace Ez.Dtos.Entities
{
    [AsTable]
    public class EzGrid_Test : BaseEntity
    {
        [AsField(Primary=true,Auto=true)]
        public virtual int zone_id { set; get; }

        public virtual string zone_name { set; get; }

        [UploadFileUI(Auto = true, Allownum = 1)]
        [CDisplayName(DefaultName = "小区图片")]
        [Required]
        public virtual string aerial_pic { set; get; }

        [UploadFileUI(Auto = true, Allownum = 1)]
        [CDisplayName(DefaultName = "小区图片")]
        [Required]
        public virtual string gate_pic { set; get; }
        public virtual string map_file { set; get; }
        public virtual decimal lon { set; get; }
        public virtual decimal lat { set; get; }
        public virtual DateTime dev_time { set; get; }
        public virtual DateTime finish_time { set; get; }
        public virtual string introduction { set; get; }
        public virtual DateTime create_time { set; get; }
        public virtual int creater_id { set; get; }
        public virtual int modifier_id { set; get; }
        public virtual DateTime modifier_time { set; get; }
    }
}
