using UnityEngine;

[CreateAssetMenu(fileName = "InputSettings", menuName = "Game/Input Settings")]
public class InputSettings : ScriptableObject
{
    [Header("Movement Keys")]
    public KeyCode moveLeft = KeyCode.LeftArrow;
    public KeyCode moveRight = KeyCode.RightArrow;
    public KeyCode moveLeftAlt = KeyCode.A;
    public KeyCode moveRightAlt = KeyCode.D;
    
    [Header("Jump Keys")]
    public KeyCode jump = KeyCode.Space;
    public KeyCode jumpAlt = KeyCode.W;
    
    [Header("Action Keys")]
    public KeyCode interact = KeyCode.E;
    public KeyCode pause = KeyCode.Escape;
    
    [Header("Ability Keys")]
    public KeyCode ability1 = KeyCode.LeftShift;
    public KeyCode ability2 = KeyCode.Q;
    public KeyCode ability3 = KeyCode.R;
}