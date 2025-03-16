using UnityEngine;

[CreateAssetMenu(menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Life")]
    public int enemyHealth = 50;
    [Space(5)]

    [Header("Fade")]
    [HideInInspector] public float fadeDuration = 1f;
    [Space(5)]

    [Header("Attack Settings")]
    public float attackInterval = 10f;
    [Space(5)]

    [Header("move Speed")]
    public float movementSpeed = 2.0f;
}
