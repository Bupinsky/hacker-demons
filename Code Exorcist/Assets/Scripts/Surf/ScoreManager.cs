using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ScoreManager : MonoBehaviour
{
    public GameObject score;
    public GameObject highscore;
    // Start is called before the first frame update
    void Start()
    {
        score.GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + PlayerPrefs.GetInt("score").ToString();
        highscore.GetComponent<TMPro.TextMeshProUGUI>().text = "Highscore: " + PlayerPrefs.GetInt("highscore").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
