using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Singleton
    private static InputManager _instance;
    public static InputManager Instance => _instance;
    
    [Header("Input Settings Reference")]
    [SerializeField] private InputSettings inputSettings;
    
    // 公开的键位属性，让其他脚本直接访问
    public KeyCode MoveLeft => inputSettings.moveLeft;
    public KeyCode MoveRight => inputSettings.moveRight;
    public KeyCode MoveLeftAlt => inputSettings.moveLeftAlt;
    public KeyCode MoveRightAlt => inputSettings.moveRightAlt;
    public KeyCode Jump => inputSettings.jump;
    public KeyCode JumpAlt => inputSettings.jumpAlt;
    public KeyCode Interact => inputSettings.interact;
    public KeyCode Pause => inputSettings.pause;
    public KeyCode Ability1 => inputSettings.ability1;
    public KeyCode Ability2 => inputSettings.ability2;
    public KeyCode Ability3 => inputSettings.ability3;

    private void Awake()
    {
        // Singleton初始化
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        // 自动加载设置
        if (inputSettings != null)
        {
            Debug.Log("Input Settings Applied.");
        }
        else
        {
            Debug.Log("Use Default Input Settings.");
        }
    }
    
    // 便捷方法：检查是否是有效的移动左键
    public bool IsMoveLeftKey(KeyCode key)
    {
        return key == MoveLeft || key == MoveLeftAlt;
    }
    
    // 便捷方法：检查是否是有效的移动右键
    public bool IsMoveRightKey(KeyCode key)
    {
        return key == MoveRight || key == MoveRightAlt;
    }
    
    // 便捷方法：检查是否是有效的跳跃键
    public bool IsJumpKey(KeyCode key)
    {
        return key == Jump || key == JumpAlt;
    }
    
    // 键位重映射
    public void RemapKey(string actionName, KeyCode newKey)
    {
        switch (actionName.ToLower())
        {
            case "moveleft":
                inputSettings.moveLeft = newKey;
                break;
            case "moveright":
                inputSettings.moveRight = newKey;
                break;
            case "jump":
                inputSettings.jump = newKey;
                break;
            case "interact":
                inputSettings.interact = newKey;
                break;
            case "ability1":
                inputSettings.ability1 = newKey;
                break;
            case "ability2":
                inputSettings.ability2 = newKey;
                break;
            case "ability3":
                inputSettings.ability3 = newKey;
                break;
            default:
                Debug.LogWarning($"Unknown action: {actionName}");
                break;
        }
        
        Debug.Log($"Remapped {actionName} to {newKey}");
    }
    
    // 获取动作的当前键位（用于UI显示）
    public KeyCode GetKeyForAction(string actionName)
    {
        switch (actionName.ToLower())
        {
            case "moveleft": return MoveLeft;
            case "moveright": return MoveRight;
            case "jump": return Jump;
            case "interact": return Interact;
            case "ability1": return Ability1;
            case "ability2": return Ability2;
            case "ability3": return Ability3;
            default: return KeyCode.None;
        }
    }
    
    // 重置为默认键位
    public void ResetToDefaultKeys()
    {
        inputSettings.moveLeft = KeyCode.A;
        inputSettings.moveRight = KeyCode.D;
        inputSettings.moveLeftAlt = KeyCode.LeftArrow;
        inputSettings.moveRightAlt = KeyCode.RightArrow;
        inputSettings.jump = KeyCode.Space;
        inputSettings.jumpAlt = KeyCode.W;
        inputSettings.interact = KeyCode.E;
        inputSettings.pause = KeyCode.Escape;
        inputSettings.ability1 = KeyCode.LeftShift;
        inputSettings.ability2 = KeyCode.Q;
        inputSettings.ability3 = KeyCode.R;
        
        Debug.Log("Input settings reset to defaults");
    }
}