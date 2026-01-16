using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int Score { get; private set; }

    public static void Add(int amount)
    {
        Score += amount;
        Debug.Log("Score: " + Score);
    }

    public static void ResetScore()
    {
        Score = 0;
    }
}