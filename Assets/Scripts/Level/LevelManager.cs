using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private string levelName = "Level_1_1";
    [SerializeField] private string nextLevelName = "Level_1_2";

    private void Start()
    {
        // 设置玩家位置
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null && startPoint != null)
        {
            player.transform.position = startPoint.position;
        }

        // 标记为最后游玩的关卡
        SaveManager.Instance.SetLastPlayedLevel(levelName);
    }

    public void CompleteLevel()
    {
        // 解锁下一关
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            SaveManager.Instance.UnlockLevel(nextLevelName);
        }
        
        // 加载下一关或返回选择界面
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            GameManager.Instance.LoadLevel(nextLevelName);
        }
        else
        {
            GameManager.Instance.ReturnToMainMenu();
        }
    }

    public Vector3 GetRespawnPosition()
    {
        return startPoint != null ? startPoint.position : Vector3.zero;
    }
}
