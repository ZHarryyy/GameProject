using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformShoot
{
    public class AchievementSystem :MonoBehaviour
    {
        public Achievement getAllScore;

        public Transform achievementPanel;
        public Text achievementNameText;
        public Text achievementDescriptionText;

        int scoreAmount;

        private void Start()
        {
            FindObjectOfType<MainPanel>().GetScoreAction += GetAllScore;
        }

        private void GetAllScore()
        {
            if(getAllScore.unlocked) return;

            scoreAmount++;
            if(scoreAmount >= 3)
            {
                PopNewAchievement(getAllScore);
            }
        }

        private void PopNewAchievement(Achievement ach)
        {
            achievementNameText.text = ach.achievementName;
            achievementDescriptionText.text = ach.achievementDescription;

            ach.unlocked = true;

            StartCoroutine(PopThePanel());
        }

        IEnumerator PopThePanel()
        {
            float percent = 0;
            float amount = 165f;

            while(percent < 1)
            {
                percent += Time.deltaTime / 1f;
                achievementPanel.position += Vector3.down * amount * Time.deltaTime / 1f;

                yield return null;
            }

            yield return new WaitForSeconds(1);

            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / 1f;
                achievementPanel.position += Vector3.up * amount * Time.deltaTime / 1f;

                yield return null;
            }
        }
    }

    [Serializable]
    public class Achievement
    {
        public string achievementName;
        public string achievementDescription;
        public bool unlocked;
    }
}
