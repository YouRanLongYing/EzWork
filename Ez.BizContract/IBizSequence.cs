using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ez.BizContract
{
    public interface IBizSequence : IDefaultBiz
    {
        /// <summary>
        /// 获取业务序列
        /// </summary>
        /// <param name="currentSequence">当前序列</param>
        /// <param name="prefix">前缀</param>
        /// <param name="min_len">最小长度</param>
        /// <param name="supple_char">小于最小长度时补齐的字符</param>
        /// <returns>业务序列</returns>
        string GetSequence(int currentSequence, string prefix, int min_len, string supple_char);
    }
}
