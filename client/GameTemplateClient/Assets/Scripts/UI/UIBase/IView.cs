using UnityEngine;
using System.Collections;

namespace GameTemplate
{
    public interface IView
    {
        #region 控制器
        IControl Control { set; get; }

        void SetControl();

        void ReleaseControl();
        #endregion

        #region 事件广播

        EventDispatcher Dispatcher { get; }

        void InitDispatcher();

        void ReleaseDispatcher();

        #endregion
    }
}
