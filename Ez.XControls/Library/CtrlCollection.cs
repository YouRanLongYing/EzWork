using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ez.XControls.Library
{
    public class CtrlCollection : Control.ControlCollection
    {
        public IList<Type> AllowType { set; get; }
        public CtrlCollection(Control owner, IList<Type> allowType)
            : base(owner)
        {
            this.AllowType = allowType;
        }
        public override void Add(Control value)
        {
            if (AllowType.Any(p => p.Equals(value.GetType())))
            {
                this.Owner.Controls.Add(value);
                base.Add(value);
            }
            else
            {
                throw new Exception("不允许添加此类型的控件:" + value.GetType());
            }
        }
        public override int Count
        {
            get
            {
                return this.Owner.Controls.Count;
            }
        }

        public override IEnumerator GetEnumerator()
        {
            return new ControlCollectionEnumerator(this.Owner.Controls);
        }
        private class ControlCollectionEnumerator : IEnumerator
        {
            // Fields
            private Control.ControlCollection controls;
            private int current;
            private int originalCount;

            // Methods
            public ControlCollectionEnumerator(Control.ControlCollection controls)
            {
                this.controls = controls;
                this.originalCount = controls.Count;
                this.current = -1;

            }
            public bool MoveNext()
            {
                if ((this.current < (this.controls.Count - 1)) && (this.current < (this.originalCount - 1)))
                {
                    this.current++;
                    return true;
                }
                return false;

            }
            public void Reset()
            {
                this.current = -1;
            }

            // Properties
            public object Current
            {
                [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
                get
                {
                    if (this.current == -1)
                    {
                        return null;
                    }
                    return this.controls[this.current];
                }
            }
        }

        public override Control this[int index]
        {
            get
            {
                return this.Owner.Controls[index];
            }
        }
        public new int IndexOf(Control control){
               return this.Owner.Controls.IndexOf(control);
        }
    }
}
