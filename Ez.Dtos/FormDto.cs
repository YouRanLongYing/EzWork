using System;
using System.Collections.Generic;
using Ez.Dtos.Library;
using Ez.UI;
using Ez.UI.HtmlExtends;
using Ez.UI.HtmlExtends.Controls;
using Ez.UI.Validations;
using System.ComponentModel.DataAnnotations;
using Ez.UI.HtmlExtends.FormAttributes;
namespace Ez.Dtos
{
    [FromTag("FormTestAsync", "Test", Subject = "这是一个用于测试的表单")]
    public class FormDto : DefaultDto
    {
        [TextBoxUI(PlaceHolder="请输入用户名,格式要求为Email格式")]
        [COM_N_UserName]
        [Email]
        [Required]
        public string Name { set; get; }

        [PasswordUI(PlaceHolder = "请输入一个不小于6位长度的密码")]
        [GreaterThanLen(6,canequal:false)]
        [COM_N_Password]
        [Required]
        public string Password { set; get; }

        [CheckBoxUI()]
        [CDisplayName(DefaultName = "是否已工作")]
        public bool IsWork { set; get; }

        #region 代理性别属性
        [RadioUI()]
        [CDisplayName(DefaultName = "性别")]
        public PropAgent Agent_Sex { set; get; }
        public int Sex { set; get; }
        #endregion

        #region 代理爱好属性
        [CheckBoxUI()]
        [CDisplayName(DefaultName = "爱好")]
        public PropAgent Agent_Hob { set; get; }
        public IList<string> Hob { set; get; }
        #endregion

        #region 代理工作年限属性

        [SelectUI()]
        [CDisplayName(DefaultName = "工作年限")]
        public PropAgent Agent_Workyear { set; get; }
        public string Workyear { set; get; }
        #endregion



        [EditorUI(PlaceHolder = "请输入您的个人简介，要求不少于300字")]
        [CDisplayName(DefaultName = "个人简介")]
        [GreaterThanLen(300)]
        [Required]
        public string TextArea { set; get; }


        [UploadFileUI(Auto=true,Allownum=1)]
        [CDisplayName(DefaultName = "头像")]
        public string Fileup { set; get; }

        //data-am-datepicker= "{format: 'yyyy-mm-dd', viewMode: 'years'}";
        [DatePickerUI(DateFormater = "format: 'yyyy-mm-dd'")]
        [CDisplayName(DefaultName = "生日")]
        [InputType(InputType.DateTime,DateFormat=DateTimeFormat.YYYY_MM_DD)]
        [Required]
        public DateTime Brithday { set; get; }

        [HiddenUI]
        public string hidden { set; get; }

    }
}
