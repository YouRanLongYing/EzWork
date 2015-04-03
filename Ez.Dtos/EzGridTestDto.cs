using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Ez.UI.HtmlExtends.FormAttributes;
using Ez.Core.Attributes;
using Ez.UI;
using Ez.UI.Validations;
using Ez.Dtos.Entities;


namespace Ez.Dtos
{
    [FromTag("edit_async", "EzGridTest", Subject = "测试EzGrid")]
    public class EzGridTestDto : EzGrid_Test
    {
        [HiddenUI]
        [JsonItem]
        public override int zone_id
        {
            get
            {
                return base.zone_id;
            }
            set
            {
                base.zone_id = value;
            }
        }
        [TextBoxUI(PlaceHolder="请输入3至10长度的名称")]
        [Required]
        [RangeLen(3,10)]
        [CDisplayName(DefaultName="小区名称")]
        [JsonItem]
        public override string zone_name { set; get; }

        [UploadFileUI(Auto = true, Allownum = 1)]
        [CDisplayName(DefaultName = "小区图片")]
        [Required]
        public override string aerial_pic { set; get; }

        [UploadFileUI(Auto = true, Allownum = 1)]
        [CDisplayName(DefaultName = "大门图片")]
        [Required]
        public override string gate_pic { set; get; }

        [DatePickerUI(DateFormater = "format: 'yyyy-mm-dd'")]
        [CDisplayName(DefaultName = "建造时间")]
        [InputType(InputType.DateTime, DateFormat = DateTimeFormat.YYYY_MM_DD)]
        [Required]
        [JsonItem]
        public override DateTime dev_time { set; get; }

        [DatePickerUI(DateFormater = "format: 'yyyy-mm-dd'")]
        [CDisplayName(DefaultName = "竣工时间")]
        [JsonItem]
        [InputType(InputType.DateTime, DateFormat = DateTimeFormat.YYYY_MM_DD)]
        [Required]
        public override DateTime finish_time { set; get; }

        [EditorUI(PlaceHolder = "请输入小区的简介，要求不少于200字")]
        [CDisplayName(DefaultName = "小区简介")]
        [GreaterThanLen(200)]
        [Required]
        public override string introduction { set; get; }

        [HiddenUI]
        public override DateTime modifier_time
        {
            get
            {
                return base.modifier_time;
            }
            set
            {
                base.modifier_time = value;
            }
        }
        [HiddenUI]
        public override DateTime create_time
        {
            get
            {
                return base.create_time;
            }
            set
            {
                base.create_time = value;
            }
        }
        [HiddenUI]
        public override int creater_id
        {
            get
            {
                return base.creater_id;
            }
            set
            {
                base.creater_id = value;
            }
        }
         [HiddenUI]
        public override int modifier_id
        {
            get
            {
                return base.modifier_id;
            }
            set
            {
                base.modifier_id = value;
            }
        }
    }
}
