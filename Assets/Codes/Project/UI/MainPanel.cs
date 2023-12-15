using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace PlatformShoot
{
    public class MainPanel : MonoBehaviour, IController
    {
        private Text mScoreText;

        private void Start()
        {
            mScoreText = transform.Find("ScoreText").GetComponent<Text>();
            this.GetModel<IGameModel>().Score.RegisterWithInitValue(OnScoreChanged).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnScoreChanged(int score)
        {
            mScoreText.text = score.ToString();
        }

        public void UpdateScoreText(int score)
        {
            int temp = int.Parse(mScoreText.text);
            mScoreText.text = (temp + score).ToString();
        }

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return PlatformShootGame.Interface;
        }
    }
}
