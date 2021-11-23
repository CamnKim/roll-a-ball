using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{

    public float speed;
    private Rigidbody rb;
    public GameObject prefab;
    private float startTime;
    private float initialTime = 10;

    private GameObject[] prefabs;

    public Material inactive;
    public Material active;

    private bool canCollect;

    public TMP_Text scoreText;
    public TMP_Text winText;
    private int count;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        scoreText.text = "";
        winText.text = "";
        canCollect = false;
    }

    // Update is called once per frame
    void Update()
    {
        prefabs = GameObject.FindGameObjectsWithTag("Pickup2");
        //Debug.Log(Time.time);
        if (canCollect)
        {
            if (prefabs.Length == 0)
            {
                winText.text = "You Win!";
            }
            foreach (GameObject pickup in prefabs)
            {
                pickup.GetComponent<Renderer>().material = active;
            }
            scoreText.text = "Collect! Time: " + initialTime;
            if (Time.time > (startTime + initialTime))
            {
                scoreText.text = "";
                canCollect = false;
                initialTime -= 1f;
                foreach (GameObject pickup in prefabs)
                {
                    pickup.GetComponent<Renderer>().material = inactive;
                }

                if (initialTime <= 1)
                {
                    initialTime = 1f;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            startTime = Time.time;
            canCollect = true;
            
            
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pickup2")
        {
            if (canCollect)
            {
                collision.gameObject.SetActive(false);
            } else
            {
                bool spawned = false;
                while (!spawned)
                {
                    float checkRadius = prefab.GetComponent<BoxCollider>().bounds.extents.x;
                    float x = Random.Range(-24f, 24f);
                    float z = Random.Range(-24f, 24f);
                    Vector3 spawn = new Vector3(x, 1, z);

                    bool invalidSpawn = Physics.CheckBox(spawn, new Vector3(checkRadius, checkRadius, checkRadius));
                    if (!invalidSpawn)
                    {
                        spawned = true;
                        Instantiate(prefab, spawn, Quaternion.identity);
                    }
                }
            }
        }
    }

    void SetScoreText()
    {
        scoreText.text = "Count: " + count.ToString();
        if (count >= 10)
        {
            winText.text = "You Win!";
        }
    }

    
}
