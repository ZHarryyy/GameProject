using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PlatformShoot
{
    public class GamePassPanel : MonoBehaviour
    {
        private void Start()
        {
            transform.Find("ResetGameBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                SceneManager.LoadScene("SampleScene");
            });
        }
    }
}
