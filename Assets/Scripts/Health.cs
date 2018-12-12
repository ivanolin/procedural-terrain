using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    public Image healthBar;
    public Image hurtPanel;
    public AudioSource hurtSound;
    public GameObject GameOverPanel;
    public TextMeshProUGUI GameOverText;
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
            StartCoroutine(HurtAnimation());

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
        GameOverText.text = "You're dog meat.\n You survived for " + ((int)timePassed)/60 + " minutes and " + ((int)timePassed)%60 + " seconds.";

        StartCoroutine(LoadMenu());
    }

    private IEnumerator HurtAnimation()
    {
        hurtSound.Play();
        float lerpAmount = 0;

        hurtPanel.color = Color.red;
        while(hurtPanel.color.a > 0)
        {
            yield return new WaitForEndOfFrame();
            lerpAmount += Time.deltaTime;
            hurtPanel.color = Color.Lerp(Color.red, new Color32(255, 0, 0, 0), lerpAmount);
        }
    }

    private IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(15);
        SceneManager.LoadScene("MainMenu");
    }

}
