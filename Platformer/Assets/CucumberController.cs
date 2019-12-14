using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CucumberController : MonoBehaviour
{
    public bool hiding = true;
    // Start is called before the first frame update
    AudioSource AudioData;
    AudioClip pop;
    void Start()
    {
        hiding = true;
        AudioData = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void appear()
    {
        if (hiding)
        {
            gameObject.transform.Translate(0, 0.8f, 0);
            hiding = false;
            AudioData.Play();

            Debug.Log("Cucumber Man appears");

            GameObject[] objs = GameObject.FindGameObjectsWithTag("Barrel");

            foreach (var obj in objs)
            {
                obj.BroadcastMessage("Start");
            }
        }








    }

    void disappear()
    {

        if (!hiding)
        {

            Debug.Log("Cucumber Man disappears");
            gameObject.transform.Translate(0, -0.8f, 0);
            
            hiding = true;

        }

    }

}
