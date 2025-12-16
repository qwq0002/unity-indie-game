using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Respawn Settings")] 
    [SerializeField] private float respawnDelay = 0.5f;
    [SerializeField] private float invulnerabilityTime = 1.5f;
    [SerializeField] private float respawnFlashInterval = 0.1f;

    [Header("Visual Feedback")] 
    [SerializeField] private Color respawnColor = new Color(1, 1, 1, 0.5f);

    // 状态变量
    private bool isRespawning = false;
    private bool isInvulnerable = false;
    private float invulnerabilityTimer = 0f;

    // 组件引用
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private PlayerController playerController;
    private Color originalColor;

    // 事件
    public System.Action OnRespawnStart;
    public System.Action OnRespawnComplete;

    // 单例（可选，根据你的架构决定）
    public static PlayerRespawn Instance { get; private set; }

    private void Awake()
    {
        // 获取组件引用
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();

        // 设置单例（如果需要）
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // 保存原始颜色
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void Update()
    {
        // 处理无敌时间的闪烁效果
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;

            // 闪烁效果
            if (spriteRenderer != null)
            {
                float alpha = Mathf.PingPong(Time.time / respawnFlashInterval, 1f);
                spriteRenderer.color = new Color(respawnColor.r, respawnColor.g, respawnColor.b, alpha);
            }

            // 无敌时间结束
            if (invulnerabilityTimer <= 0f)
            {
                EndInvulnerability();
            }
        }
    }

    public void Respawn()
    {
        if (isRespawning) return;

        isRespawning = true;

        // 触发重生开始事件
        OnRespawnStart?.Invoke();

        // 禁用玩家控制
        if (playerController != null)
        {
            playerController.SetControlEnabled(false);
        }

        // 停止玩家移动
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        // 延迟后执行实际的重生
        Invoke(nameof(PerformRespawn), respawnDelay);
    }

    private void PerformRespawn()
    {
        // 查找重生点
        Vector3 respawnPosition = FindRespawnPosition();

        // 传送玩家到重生点
        transform.position = respawnPosition;

        // 进入无敌状态
        StartInvulnerability();

        // 重新启用玩家控制
        if (playerController != null)
        {
            playerController.SetControlEnabled(true);
        }

        isRespawning = false;

        // 触发重生完成事件
        OnRespawnComplete?.Invoke();
    }

    private Vector3 FindRespawnPosition()
    {
        // 优先使用LevelManager的重生点
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null)
        {
            return levelManager.GetRespawnPosition();
        }
        
        // 使用玩家初始位置
        return transform.position;
    }

    private void StartInvulnerability()
    {
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityTime;
    }

    private void EndInvulnerability()
    {
        isInvulnerable = false;

        // 恢复原始颜色
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    // 碰撞检测
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 碰到危险物
        if (other.CompareTag("Hazard"))
        {
            Respawn();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 碰撞到危险物
        if (collision.gameObject.CompareTag("Hazard"))
        {
            Respawn();
        }
    }

    // 公共方法
    public void SetRespawnPoint(Vector3 position)
    {
        // 这个方法可以被Checkpoint调用，设置自定义的重生点
        // 如果需要实现检查点系统，可以在这里添加逻辑
    }

    public bool IsRespawning()
    {
        return isRespawning;
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }
}
