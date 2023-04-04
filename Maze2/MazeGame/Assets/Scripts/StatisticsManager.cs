using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager instance;

    public TMP_Text StatisticsDisplay;
    public TMP_Text ScoreDisplay;

    public int PlayerWins = 0;
    public int AiWins = 0;

    private void Awake()
    {
        if (instance == null)  instance = this;
        else Destroy(this.gameObject);
    }

    public void UpdateStatisticsDisplay()
    {
        StatisticsDisplay.text = $"Player Wins: {PlayerWins} \n AI Wins: {AiWins}";
    }


}
