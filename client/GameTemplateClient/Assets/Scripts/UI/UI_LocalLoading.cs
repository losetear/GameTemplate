using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameTemplate
{
    public class UI_LocalLoading : MonoBehaviour
    {
        private Slider _slider;
        private Text _tip;
        private string _origTip;
        void Awake()
        {
            _slider = transform.Find("Slider").GetComponent<Slider>();
            _tip = transform.Find("Tip").GetComponent<Text>();
            _origTip = _tip.text;
        }

        void OnEnable()
        {
            Reset();
            Messenger.AddListener<string>(XEvent.FinishLoadAssetBundle, OnFinishLoadAssetBundle);
        }

        void OnDisable()
        {
            Messenger.RemoveListener<string>(XEvent.FinishLoadAssetBundle, OnFinishLoadAssetBundle);
        }

        // Update is called once per frame
        void Update()
        {
            Tick();
        }

        private float _timer;
        private bool _loadOver;
        private float _dotTimer;
        private float _dotCount;
        private float _percent;
        private void Reset()
        {
            _timer = 0;
            _slider.value = 0;
            _loadOver = false;
            _dotTimer = 0;
            _dotCount = 0;
            _percent = 0;
        }

        private void Tick()
        {
            transform.SetAsLastSibling();
            _timer += Time.deltaTime;
            if (_timer > _dotTimer)
            {
                _dotTimer = _timer + 0.4f;
                _dotCount = (_dotCount + 1) % 4;
                string dotStr = "";
                for (int i = 0; i < _dotCount; i++)
                {
                    dotStr += ".";
                }
                _tip.text = _origTip + dotStr;
            }
            if (!_loadOver)
            {
                _percent = _timer / 60;
                _percent = Mathf.Min(_percent, 0.99f);
            }
            else
            {
                _percent += 0.03f;
            }
            _slider.value = _percent;
            if (_percent >= 1)
            {
                End();
            }
        }

        private void OnFinishLoadAssetBundle(string abType)
        {
            if (abType == AssetBundleType.Lua.ToString())
            {
                FinishLoading();
            }
        }

        private void FinishLoading()
        {
            _loadOver = true;
        }

        private void End()
        {
            this.gameObject.SetActive(false);
            Messenger.Broadcast(XEvent.FinishLoading);
        }

    }
}

