using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 750f;
    public int health = 5;
    public int resetTime = 3;
    
    public Text scoreText;
    public Text healthText;
    public Text winloseText;

    private Rigidbody Rb3D;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        Rb3D = GetComponent<Rigidbody>();
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        HealthCheck();

        if (Input.GetButton("Cancel"))
        {
            SceneManager.LoadScene("menu");
        }
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 roll = new Vector3(horizontal, 0, vertical);
        roll = roll * speed * Time.deltaTime;
        Rb3D.AddForce(roll);
    }

    private void HealthCheck()
    {
        if (health == 0)
        {
            Lose();
            StartCoroutine(LoadScene(resetTime));
        }
    }

    void OnTriggerEnter(Collider col)
    {
        switch (col.tag)
        {
            case "Pickup":
                score++;
                SetScoreText();
                Destroy(col.gameObject);
                break;
            case "Trap":
                health--;
                SetHealthText();
                break;
            case "Goal":
                Win();
                StartCoroutine(LoadScene(resetTime));
                break;
        }
    }

    void SetScoreText()
    {
        scoreText.text = $"Score: {score}";
    }

    void SetHealthText()
    {
        healthText.text = $"Health: {health}";
    }

    void Win()
    {
        GameObject parent = winloseText.transform.parent.gameObject;
        Image parentImage = parent.GetComponent<Image>();

        winloseText.text = "You Win!";
        winloseText.color = Color.black;
        parentImage.color = Color.green;
        parent.SetActive(true);
    }

    void Lose()
    {
        GameObject parent = winloseText.transform.parent.gameObject;
        Image parentImage = parent.GetComponent<Image>();

        winloseText.text = "Game Over!";
        winloseText.color = Color.white;
        parentImage.color = Color.red;
        parent.SetActive(true);
    }

    IEnumerator LoadScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        health = 5;
        score = 0;
        SceneManager.LoadScene("maze");
    }
}
