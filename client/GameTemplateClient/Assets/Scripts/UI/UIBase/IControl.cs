using UnityEngine;
using System.Collections;

namespace GameTemplate
{
    public class IControl
    {
        #region 初始化

        public IView View { set; get; }

        public virtual void Init()
        {
            View.Dispatcher.AddListener("Show", Show);
            View.Dispatcher.AddListener("Hide", Hide);
        }

        public virtual void Release()
        {
            View.Dispatcher.RemoveListener("Show", Show);
            View.Dispatcher.RemoveListener("Hide", Hide);
        }

        public virtual void Show() { }

        public virtual void Hide() { }

        public virtual void Tick() { }

        #endregion
    }
}

