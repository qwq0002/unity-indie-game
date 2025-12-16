using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public void OnNewGameButtonClicked()
  {
    // 加载游戏场景
    SceneManager.LoadScene("Level_1_1");
  }

  public void OnQuitButtonClicked()
  {
    // 退出游戏
    Application.Quit();
  }
}

