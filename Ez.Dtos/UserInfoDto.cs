using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ez.UI;
using Ez.Dtos.Library;
using Ez.UI.Validations;
using Ez.UI.HtmlExtend;
using Ez.Lang;
using System.Runtime.Serialization;
using Ez.UI.HtmlExtends.FormAttributes;
namespace Ez.Dtos
{

    /// <summary>
    /// 用户信息
    /// created by kongjing
    /// </summary>
    [FromTag("EditMyInfoAsync", "UCenter", Subject = "账户信息维护")]
    [Serializable]
    public class UserInfoDto : DefaultDto
    {
        /// <summary>
        /// 信息编号
        /// </summary>
        public int id { set; get; }
        /// <summary>
        /// 账户编号
        /// </summary>
        public int login_id { set; get; }
        /// <summary>
        /// 用户名称
        /// </summary>
        [Required]
        [Email]
        [COM_N_UserName]
        [TextBoxUI(PlaceHolder = "请输入用户名,格式要求为Email格式")]
        public string username { set; get; }

        /// <summary>
        /// 用户性别
        /// </summary>
        public int gender { set; get; }
        private PropertyAgent agent_Gender;
        [COM_N_Gender]
        [RadioUI]
        public PropertyAgent Agent_Gender
        {
            get
            {
                if (this.agent_Gender == null)
                {
                    this.agent_Gender = new PropertyAgent(PropertyAgentType.RadioButton, p => this.gender, this.gender.ToString());
                    this.agent_Gender.Add(new PropertyAgentItem("男", "0"));
                    this.agent_Gender.Add(new PropertyAgentItem("女", "1"));
                    this.agent_Gender.Add(new PropertyAgentItem("保密", "-1"));
                }
                return this.agent_Gender;
            }
        }

        /// <summary>
        /// 用户年龄
        /// </summary>
        [Required]
        [InputType(InputType.Int)]
        [COM_N_Age]
        [TextBoxUI(PlaceHolder = "请输入年龄")]
        public int age { set; get; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        [Required]
        [IdCard]
        [COM_N_IdCard]
        [TextBoxUI(PlaceHolder = "请输入身份证")]
        public string id_card { set; get; }
        /// <summary>
        /// 座机号码
        /// </summary>
        [Required]
        [PhoneNumber]
        [COM_N_Tel]
        [TextBoxUI(PlaceHolder = "请输入座机号码")]
        public string telphone { set; get; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        [MobileNumber]
        [COM_N_Mobile]
        [TextBoxUI(PlaceHolder = "请输入手机号码")]
        public string mobile { set; get; }
        /// <summary>
        /// 联系QQ
        /// </summary>
        [Required]
        [InputType(InputType.Number)]
        [RangeLen(4, 11)]
        [COM_N_QQ]
        [TextBoxUI(PlaceHolder = "请输入QQ号码")]
        public string qq { set; get; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [Required]
        [Email]
        [COM_N_Email]
        [TextBoxUI(PlaceHolder = "请输入联系邮件地址")]
        public string email { set; get; }
        /// <summary>
        /// 住址
        /// </summary>
        [Required]
        [COM_N_Address]
        [TextBoxUI(PlaceHolder = "请输入住址")]
        public string address { set; get; }

        
        /// <summary>
        /// 单位名称
        /// </summary>
        [CDisplayName(LangKey = "COM_N_Company", DefaultName = "COM_N_Company")]
        [Required]
        [TextBoxUI(PlaceHolder = "请输入工作单位名称")]
        public string company { set; get; }
        [Required]
        public string textValidaty { set; get; }

        /// <summary>
        /// 业务扩展列（数据）
        /// </summary>
        public string ext_data { set; get; }

    }
}
