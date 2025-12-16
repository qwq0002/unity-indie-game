using System;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerController : MonoBehaviour
{
    [Header("Settings References")] 
    [SerializeField] public GameSettings gameSettings;

    [Header("Ground Detection")] 
    public CollisionCheck groundCheck;
    public CollisionCheck wallCheck;

    // 组件引用
    [Header("Component References")] 
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    // 运动模块
    [Header("Movement Modules")] 
    private MovementModule[] _movementModules;

    // 能力配置
    public bool enableDoubleJump = false;
    
    // 内部状态
    public bool IsGrounded { get; private set; } = true;
    public bool IsCollideWall { get; private set; } = true;
    public bool ControlEnabled { get; private set; } = true; // 控制启用状态
    
    
    // 动画状态
    public enum AnimState
    {
        idle,
        run,
        jump,
        fall
    }
    public AnimState CurrentAnimState = AnimState.idle;
    // 哈希值
    private static readonly int _hashStates = Animator.StringToHash("states");
    
    
    // main
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        
        // 初始化运动模块
        InitializeModules();

        Debug.Log("[PlayerController:Awake] PlayerController initialized!");
    }

    private void Start()
    {
        LoadGameSettings();
    }

    private void Update()
    {
        if (!ControlEnabled) return;
        HandleInputs();
        CheckCollision();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (!ControlEnabled) return;
        ExecuteMovement();
    }

    // manage submodules
    private void InitializeModules()
    {
        // 使用数组初始化所有移动模块
        _movementModules = new MovementModule[]
        {
            new MoveModule(this, gameSettings),
            new JumpModule(this, gameSettings)
            // 后续添加新模块只需在这里添加一行
        };

        Debug.Log($"[PlayerController:InitializeModules] Initialized {_movementModules.Length} movement modules");
    }

    private void LoadGameSettings()
    {
        foreach (var module in _movementModules)
        {
            module?.LoadGameSettings();
        }

        Debug.Log("[PlayerController:LoadGameSettings] All module settings loaded");
    }

    private void HandleInputs()
    {
        foreach (var module in _movementModules)
        {
            module?.HandleInput();
        }
    }

    private void ExecuteMovement()
    {
        foreach (var module in _movementModules)
        {
            module?.Execute();
        }
    }

    // internal functions
    private void CheckCollision()
    {
        IsGrounded = groundCheck.isColliding;
        IsCollideWall = wallCheck.isColliding;
    }

    // 控制启用/禁用函数
    public void SetControlEnabled(bool isEnabled)
    {
        ControlEnabled = isEnabled;

        // 可选：禁用控制时停止玩家移动
        if (!isEnabled && _rigidbody != null)
        {
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        }

        Debug.Log($"[PlayerController:SetControlEnabled] Control {(isEnabled ? "enabled" : "disabled")}");
    }

    // 切换控制状态
    public void ToggleControl()
    {
        SetControlEnabled(!ControlEnabled);
    }

    // 更新动画状态
    private void UpdateAnimation()
    {
        if (_rigidbody.velocity.y < -0.3f)
        {
            CurrentAnimState = AnimState.fall;
        }
        else if (_rigidbody.velocity.y > 0.3f)
        {
            CurrentAnimState = AnimState.jump;
        }
        else if (CurrentAnimState != AnimState.run)
        {
            CurrentAnimState = AnimState.idle;
        }
        
        _animator.SetInteger(_hashStates, (int)CurrentAnimState);
        
    }
    
}
