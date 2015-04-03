using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.UI.HtmlExtends
{
    public enum TagType
    {
        /// <summary>
        /// 一般输入框
        /// </summary>
        TextBox,
        /// <summary>
        /// 密码输入框
        /// </summary>
        Password,
        /// <summary>
        /// 文本域
        /// </summary>
        TextArea,
        /// <summary>
        /// 富文本编辑器
        /// </summary>
        Editor,
        /// <summary>
        /// 上传空间
        /// </summary>
        UploadFile,
        /// <summary>
        /// 日期选择
        /// </summary>
        DatePicker,

        /// <summary>
        /// 单选按钮
        /// </summary>
        Radio,
        /// <summary>
        /// 复选框按钮
        /// </summary>
        CheckBox,
        /// <summary>
        /// 下拉列表
        /// </summary>
        Select,
        /// <summary>
        /// 隐藏控件
        /// </summary>
        Hidden,
        /// <summary>
        /// 根据特性自动检测，如果没有则默认是textbox
        /// </summary>
        Normal
    }
}
