using UnityEngine;
using System.Collections;



public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    // Code for Game over screen
    [Header("Gameover")]
    [SerializeField] public GameObject gameOverScreen;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        gameOverScreen.SetActive(false);
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
       
        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
            StartCoroutine(Invunerability());
            Debug.Log("Dog has taken damage, one life lost, currnt health: "+ currentHealth);
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("Die");
                Debug.Log("Dog has no lives (dead)");
                
                GameOver(true);


                if (GetComponentInParent<playerController>() != null)
                {
                    GetComponentInParent<playerController>().enabled = false;
                }


                if (GetComponentInParent<EnemyPatrol>() != null)
                { 
                    GetComponentInParent<EnemyPatrol>().enabled = false; 
                }

                if (GetComponent<EnemyAttack>() != null)
                {
                    GetComponent<EnemyAttack>().enabled = false; 
                }

                dead = true;
            }
        }
    }

    public void AddHealth(float _value)
    {
        Debug.Log("Health token consumed, increased health of :" + _value);
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }


// Code below is for the death screen and a 2 second delay.
    public void GameOver(bool status)
    {   
        StartCoroutine(GameOverWithDelay(status, 2f));
    }

    private IEnumerator GameOverWithDelay(bool status, float delay)
    {
        yield return new WaitForSeconds(delay);
        gameOverScreen.SetActive(status);
    }
}