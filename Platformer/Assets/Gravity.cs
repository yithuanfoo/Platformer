using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChromaSDK;
//using System;

public class Gravity : MonoBehaviour
{

    public float GravityModifier = 1f;
	//Random r;
    protected Rigidbody2D RB2D;
    protected Vector2 velocity;
	public float damage;
    // Start is called before the first frame update
    AudioSource AudioData;
    AudioClip impact;

        void OnEnable()
    {
        RB2D = GetComponent<Rigidbody2D>();
		//r = new Random();
    }
    void Awake()
    {

        UnityNativeChromaSDK.Init();

    }
    void Start()
    {
        RB2D.position  = new Vector2(RB2D.position.x, 7.5f);
        RB2D.velocity = new Vector2( Random.value * 2 - 1 , 0);
        AudioData = GetComponent<AudioSource>();
		damage = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
    void FixedUpdate()
    {
        velocity += GravityModifier * Physics2D.gravity * Time.deltaTime;
        Vector2 deltaPosition = velocity * Time.deltaTime;
        Vector2 move = new Vector2(velocity.x, Vector2.up.y * deltaPosition.y);
        Movement(move);
        //Debug.Log("Move" + move);
    }

    void Movement(Vector2 move)
    {

        GameObject bear = GameObject.FindWithTag("bear");
		if(RB2D.position.y > -5){
			//RB2D.position += move;
			RB2D.rotation += Random.Range(0f, Mathf.Abs(velocity.y) * 2f);
        }
        else if(true) {
            velocity.y = RB2D.velocity.y * Random.Range(-0.8f, -0.9f);
            velocity.x = RB2D.velocity.x * Random.Range(0.8f, 0.9f);
             AudioData.Play();
            float distanceToBear = Vector2.Distance(bear.transform.position, RB2D.position);

            AudioData.volume = Mathf.Max(0, 1.0f/(distanceToBear+0.1f)+ 0.001f * velocity.y * velocity.y );
            //Debug.Log("AudioData.volume: " + AudioData.volume);
            if ( velocity.magnitude < 0.01f)
            {
                RB2D.rotation = 0;
                AudioData.volume = 0;
                // UnityNativeChromaSDK.SetCurrentFrameName("anim.chroma", 1);
                RB2D.velocity = 0 * RB2D.velocity;
                UnityNativeChromaSDK.StopAnimationName("anim.chroma");

            }
            else
            {

              //  UnityNativeChromaSDK.SetCurrentFrameName("anim.chroma", 0);
            }

            UnityNativeChromaSDK.PlayAnimationName("anim.chroma", false);
           // RB2D.rotation = -RB2D.rotation;
            //RB2D.position -= move;
        }

        if(RB2D.position.x > 19 || RB2D.position.x < - 19){

            velocity.x = -velocity.x;


        }
        RB2D.velocity = velocity;




    }
} 


