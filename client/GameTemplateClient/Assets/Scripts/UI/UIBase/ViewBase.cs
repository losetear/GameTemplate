using System;
using UnityEngine;

namespace GameTemplate
{
    public class ViewBase : MonoBehaviour, IView
    {
        void Awake()
        {
            InitDispatcher();
            SetControl();
            InitControl();
            InitUI();
        }

        void Destroy()
        {
            ReleaseDispatcher();
            ReleaseControl();
            ReleaseUI();
        }

        void OnEnable()
        {
            ShowWindow();
            Dispatcher.Broadcast("Show");
        }

        void OnDisable()
        {
            HideWindow();
            Dispatcher.Broadcast("Hide");
        }

        void Update()
        {
            if (_control != null)
                _control.Tick();
            Tick();
        }

        #region 初始化

        public virtual void InitUI()
        {
        }
        public virtual void ReleaseUI()
        {
        }

        public virtual void ShowWindow()
        {
        }

        public virtual void HideWindow()
        {
        }

        public virtual void Tick()
        {
        }

        #endregion

        #region 中介

        private IControl _control;
        public IControl Control
        {
            get { return _control; }
            set { _control = value; }
        }

        public virtual void SetControl()
        {
        }
        public void InitControl()
        {
            if (_control != null)
            {
                _control.View = this;
                _control.Init();
            }
        }

        public virtual void ReleaseControl()
        {
            if (_control != null)
            {
                _control.Release();
                _control = null;
            }
        }
        #endregion
        
        #region 事件广播

        private readonly EventDispatcher _dispatcher = new EventDispatcher();
        public EventDispatcher Dispatcher
        {
            get { return _dispatcher; }
        }

        public void InitDispatcher()
        {
            _dispatcher.Init();
        }

        public void ReleaseDispatcher()
        {
            _dispatcher.Release();
        }
        #endregion

    }
}
