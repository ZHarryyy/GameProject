using UnityEngine;
using UnityEngine.UI;

namespace PlatformShoot
{
    public class MainPanel : MonoBehaviour
    {
        private Text mScoreText;

        private void Start()
        {
            mScoreText = transform.Find("ScoreText").GetComponent<Text>();
        }

        public void UpdateScoreText(int score)
        {
            int temp = int.Parse(mScoreText.text);
            mScoreText.text = (temp + score).ToString();
        }
    }
}
