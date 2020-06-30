/* A healthDisplay is a child object of the Canvas , tagged "HealthDisplay", which has an
 * arbitrary number of children.
 * The UI Image scripts (right-click -> UI -> Image) of these child objects are then
 * accessed and treated as the hearths being toggled on and off. The number of such
 * objects determines the maxHealth.
 * LivesText is the UI Text component attached to a gameObject tagged "LivesText".
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    private int health = 3, maxHealth, lives;
    [SerializeField] int startLives = 2, maxLives = 5;
    List<Image> hearthList = new List<Image>();
    Text livesText;

    private void Start()
    {
        Transform healthDisplay = GameObject.FindGameObjectWithTag("HealthDisplay").GetComponent<Transform>();
        if (healthDisplay) {
            foreach (Transform child in healthDisplay)
            {
                hearthList.Add(child.gameObject.GetComponent<Image>());
            }
        }

        livesText = GameObject.FindGameObjectWithTag("LivesDisplay").GetComponent<Text>();

        lives = startLives;
        maxHealth = hearthList.Count;
        health = maxHealth;

        UpdateHearthDisplay();
        UpdateLivesDisplay();
    }

    public int GetHealth() { return health; }

    public void TakeDamage(int n)
    {
        health -= n;

        if (health <= 0)
        {
            if (lives > 0)
            {
                health = maxHealth;
                lives -= 1;
            }
            
        }

        UpdateHearthDisplay();
        UpdateLivesDisplay();
    }

    private void UpdateHearthDisplay()
    {
        int c = hearthList.Count;
        for (int iii = 0; iii != c; iii++)
        {
            if (iii < health) { hearthList[iii].enabled = true; }
            else { hearthList[iii].enabled = false; }
        }
    }

    private void UpdateLivesDisplay()
    {
        livesText.text = lives.ToString();
    }
}
