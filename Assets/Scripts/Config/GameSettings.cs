using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Horizontal Movement Settings")]
    public float moveSpeed = 8f;
    
    [Header("Jump Settings")]
    public float jumpForce = 9.8f;
    public float jumpSpeed = 9.8f;
    public float fallSpeed = 9.8f;
    public float coyoteTime = 0.1f;
    public float maxJumpHoldTime = 0.3f;

}