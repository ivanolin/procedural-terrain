using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{

    public Image healthBar;
    public GameObject GameOverPanel;
    public TextMeshProUGUI GameOverText;
    public GameObject player;
    public int health;
    private int maxHealth;
    private float timePassed;

    private bool gameOver = false;

    void Start()
    {
        maxHealth = health;
        timePassed = 0;
    }

    void Update()
    {
        if (!gameOver)
            timePassed += Time.deltaTime;
    }

    public void Damage(int amount)
    {

        if (!gameOver)
        {
            health -= amount;

            if (health <= 0)
            {
                GameOver();
            }
            else
            {
                healthBar.fillAmount = (float)health / (float)maxHealth;
            }
        }
    }

    private void GameOver()
    {
        gameOver = true;
        GameOverPanel.SetActive(true);
        GameOverText.text = "GAME OVER\n SCORE: " + (int)timePassed;
    }

}
