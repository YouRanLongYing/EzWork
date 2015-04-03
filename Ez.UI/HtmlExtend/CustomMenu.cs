using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web;

namespace Ez.UI.HtmlExtend
{
    public enum LinkTarget
    {
        _Blank,
        _Parent
    }
    public class LinkBtn
    {
        public string LinkText { set; get; }
        public string Href { set; get; }
        public LinkTarget Target { set; get; }
        public LinkBtn(string linkText, string href, LinkTarget target)
        {
            this.LinkText = linkText;
            this.Href = href;
            this.Target = target;
        }
    }
    public class LinkBtns : IEnumerable
    {
        public string Title { set; get; }
        public IList<LinkBtn> Collection { set; get; }
        public LinkBtns(string title)
        {
            Title = title;
        }
        public void Add(LinkBtn linkBtn)
        {
            if (Collection == null) Collection = new List<LinkBtn>();
            this.Collection.Add(linkBtn);
        }
        public IEnumerator GetEnumerator()
        {
            foreach (LinkBtn btn in Collection)
            {
                yield return btn;
            }
        }
    }
    public class CustomMenu : IEnumerable
    {
        private IDictionary<string, IList<LinkBtn>> collection = new Dictionary<string, IList<LinkBtn>>();

        public void Add(LinkBtns linkBtns)
        {
            if (!this.collection.ContainsKey(linkBtns.Title))
            {
                this.collection.Add(linkBtns.Title, linkBtns.Collection);
            }
        }

        /// <summary>
        /// 生成菜单的请求来自
        /// </summary>
        public string From { get {
            return HttpContext.Current.Request.RawUrl;
        } }
        public IEnumerator GetEnumerator()
        {
            foreach (string title in collection.Keys)
            {
                yield return title;
            }
        }
        public IList<LinkBtn> GetValues(string title)
        {
            return this.collection[title];
        }
    }
}
