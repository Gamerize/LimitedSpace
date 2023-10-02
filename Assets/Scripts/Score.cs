using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int m_Score;

    [SerializeField] TextMeshProUGUI m_ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        m_Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayScore();
    }

    private void DisplayScore()
    {
        m_ScoreText.text = m_Score.ToString();
    }
}
