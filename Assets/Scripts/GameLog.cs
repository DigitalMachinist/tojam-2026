using UnityEngine;

public static class GameLog
{
    private const string DamageColor = "E74C3C";
    private const string HealColor = "2ECC71";
    private const string DeathColor = "C0392B";
    private const string FullColor = "16A085";
    private const string IframeColor = "F1C40F";
    private const string ContactColor = "E67E22";
    private const string ShadowColor = "9B59B6";
    private const string WeaponColor = "F39C12";
    private const string InputColor = "3498DB";
    private const string HealthColor = "95A5A6";
    private const string EnemyColor = "1ABC9C";

    public static void Damage(string msg, Object context = null)  => Tag(DamageColor,  "Damage",  msg, context);
    public static void Heal(string msg, Object context = null)    => Tag(HealColor,    "Heal",    msg, context);
    public static void Death(string msg, Object context = null)   => Tag(DeathColor,   "Death",   msg, context);
    public static void Full(string msg, Object context = null)    => Tag(FullColor,    "Full",    msg, context);
    public static void Iframe(string msg, Object context = null)  => Tag(IframeColor,  "Iframe",  msg, context);
    public static void Contact(string msg, Object context = null) => Tag(ContactColor, "Contact", msg, context);
    public static void Shadow(string msg, Object context = null)  => Tag(ShadowColor,  "Shadow",  msg, context);
    public static void Weapon(string msg, Object context = null)  => Tag(WeaponColor,  "Weapon",  msg, context);
    public static void Input(string msg, Object context = null)   => Tag(InputColor,   "Input",   msg, context);
    public static void Health(string msg, Object context = null)  => Tag(HealthColor,  "Health",  msg, context);
    public static void Enemy(string msg, Object context = null)   => Tag(EnemyColor,   "Enemy",   msg, context);

    private static void Tag(string hex, string category, string msg, Object context)
    {
        Debug.Log($"<color=#{hex}>[{category}]</color> {msg}", context);
    }
}
