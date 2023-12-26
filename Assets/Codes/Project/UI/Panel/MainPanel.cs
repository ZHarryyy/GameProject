using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System;

namespace PlatformShoot
{
    public class MainPanel : PlatformShootGameController
    {
        private Text mScoreText;

        public event Action GetScoreAction;

        private void Start()
        {
            mScoreText = transform.Find("ScoreText").GetComponent<Text>();

            transform.Find("SettingBtn").GetComponent<Button>().onClick.AddListener(OnOpenSetting);

            this.GetModel<IGameModel>().Score.RegisterWithInitValue(OnScoreChanged).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnOpenSetting()
        {
            ResHelper.AsyncLoad<GameObject>("Item/SettingPanel", o=>
            {
                o.transform.SetParent(GameObject.Find("Canvas").transform);
                (o.transform as RectTransform).anchoredPosition = Vector2.zero;
            });
        }

        private void OnScoreChanged(int score)
        {
            mScoreText.text = score.ToString();

            if(GetScoreAction != null) GetScoreAction();
        }

        public void UpdateScoreText(int score)
        {
            int temp = int.Parse(mScoreText.text);
            mScoreText.text = (temp + score).ToString();
        }
    }
}
