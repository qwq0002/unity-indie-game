using UnityEngine;

// 移动系统基类
public abstract class MovementModule
{
    protected PlayerController _playerController;
    protected Rigidbody2D _rigidbody;
    protected GameSettings _gameSettings;
    protected Transform _playerTransform;
    
    public MovementModule(PlayerController playerController, GameSettings gameSettings)
    {
        _playerController = playerController;
        _rigidbody = playerController.GetComponent<Rigidbody2D>();
        _gameSettings = gameSettings;
        _playerTransform  = playerController.GetComponent<Transform>();
    }
    
    public abstract void LoadGameSettings();
    public abstract void HandleInput();
    public abstract void Execute();
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
}

// 水平移动模块
public class MoveModule : MovementModule
{
    private float _horizontalInput;
    private float _moveSpeed;
    
    public MoveModule(PlayerController playerController, GameSettings gameSettings) : base(playerController, gameSettings) { }

    public override void LoadGameSettings()
    {
        _moveSpeed = _gameSettings.moveSpeed;
    }

    public override void HandleInput()
    {
        _horizontalInput = 0f;
        if (Input.GetKey(InputManager.Instance.MoveLeft))
        {
            _horizontalInput -= 1f;
        }

        if (Input.GetKey(InputManager.Instance.MoveRight))
        {
            _horizontalInput += 1f;
        }

        if (_horizontalInput != 0)
        {
            Vector2 currentScale = _playerTransform.localScale;
            currentScale.x = Mathf.Abs(currentScale.x) * Mathf.Sign(_horizontalInput);
            _playerTransform.localScale = currentScale;
        }
    }
    
    public override void Execute()
    {
        if (_playerController.IsCollideWall)
        {
            return;
        }
        
        float targetSpeed = _horizontalInput * _moveSpeed;
        _rigidbody.velocity = new Vector2(targetSpeed, _rigidbody.velocity.y);

        // 切换动画状态
        if (_horizontalInput != 0f && Mathf.Abs(_rigidbody.velocity.y) < 0.3f)
        {
            _playerController.CurrentAnimState = PlayerController.AnimState.run;
        }
        else
        {
            _playerController.CurrentAnimState = PlayerController.AnimState.idle;
        }
    }
}

// 跳跃模块
public class JumpModule : MovementModule
{
    // 从 GameSettings 获取的配置
    private float _jumpForce;
    private float _jumpSpeed;
    private float _fallSpeed;
    private float _coyoteTime;
    private float _maxJumpHoldTime;
    
    // 内部状态变量
    private float _coyoteTimeCounter;
    private float _jumpHoldTimeCounter;
    
    private bool _isJumping = false;
    private bool _isDoubleJumping = false;
    
    public JumpModule(PlayerController playerController, GameSettings gameSettings) : base(playerController, gameSettings) { }
    
    public override void LoadGameSettings()
    {
        if (_gameSettings == null)
        {
            Debug.LogError("GameSettings is null in JumpModule!");
            return;
        }

        _jumpForce = _gameSettings.jumpForce;
        _jumpSpeed = _gameSettings.jumpSpeed;
        _fallSpeed = _gameSettings.fallSpeed;
        _coyoteTime = _gameSettings.coyoteTime;
        _maxJumpHoldTime = _gameSettings.maxJumpHoldTime;
        
    }
    
    public override void HandleInput()
    {
        if (Input.GetKeyDown(InputManager.Instance.Jump) && CanJump())
        {
            _isJumping = true;
            _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
        }

        if (Input.GetKeyUp(InputManager.Instance.Jump))
        {
            _isJumping = false;
        }
    }

    private bool CanJump()
    {
        if (_playerController.IsGrounded)
        {
            _isDoubleJumping = false;
            return true;
        }

        if (_playerController.enableDoubleJump && !_isDoubleJumping)
        {
            _isDoubleJumping = true;
            return true;
        }
        
        return false;
    }

    public override void Execute()
    {
        HandleCoyoteTime();
        PerformJump();
        
        // 落地时重置状态
        if (_playerController.IsGrounded && _rigidbody.velocity.y <= 0.1f)
        {
            ResetJumpState();
        }
    }
    
    private void HandleCoyoteTime()
    {   
        // 在玩家离开平台的一小段时间内仍可以跳跃
        if (_playerController.IsGrounded)
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void PerformJump()
    {
        // 上升逻辑
        if (_isJumping &&
            _coyoteTimeCounter > 0 &&
            _jumpHoldTimeCounter < _maxJumpHoldTime)
        {
            _rigidbody.velocity += new Vector2(0, _jumpSpeed * Time.deltaTime);
            _jumpHoldTimeCounter += Time.deltaTime;
        }
        else
        {
            _jumpHoldTimeCounter = 0;
            _rigidbody.velocity -= new Vector2(0, _fallSpeed * Time.deltaTime);
        }
    }
    
    private void ResetJumpState()
    {
        _isJumping = false;
        _jumpHoldTimeCounter = 0f;
        
        if (_rigidbody.velocity.y != 0)
        {
            Debug.Log("Jump state reset, default gravity restored");
        }
    }
    
    public override void OnEnter()
    {
        // 进入新状态时的初始化
        ResetJumpState();
    }
    
    public override void OnExit()
    {
        ResetJumpState();
    }
}
