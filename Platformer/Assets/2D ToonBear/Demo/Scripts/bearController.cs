using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEditor;

using Newtonsoft.Json;
//This Script is intended for demoing and testing animations only.


public class bearController : MonoBehaviour {
    float hp = 100;

	private float HSpeed = 10f;
	//private float maxVertHSpeed = 20f;
	private bool facingRight = true;
	private float moveXInput;

    //Used for flipping Character Direction
	public static Vector3 theScale;

	//Jumping Stuff
	public Transform groundCheck;
	public LayerMask whatIsGround;
	private bool grounded = false;
    private bool hasjetpack = false;
	private bool haskey = false;
	private bool dooropen = false;
	private float groundRadius = 0.15f;
	private float jumpForce = 14f;
    private float fuel = 100f;
	private int attempts = 0 ;

	private Animator anim;
	GameObject spawnpoint;
	GameObject grabbedKey = null;
	GameObject grabbedJetpack = null;
	// Use this for initialization
	void Awake ()
	{

		spawnpoint = GameObject.FindGameObjectWithTag("spawnpoint");
		GetComponent<Rigidbody2D>().position = spawnpoint.transform.position;
		
		
//		startTime = Time.time;
		anim = GetComponent<Animator> ();
        anim.SetBool("Dead", false);
        anim.SetBool("Dying", false);
        //hasjetpack = true;
        fuel = UnityEngine.Random.Range(30.0f, 100.0f);
		
		//GameObject JetpackCopy = GameObject.Instantiate(GameObject.FindGameObjectsWithTag("door")[0]);
		
		//EditorJsonUtility.FromJsonOverwrite(FileContent, JetpackCopy);
		//JetpackCopy.transform.Translate(1f, 0 , 0 );
		
    }

	void FixedUpdate ()
	{

		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool ("ground", grounded);


	}
	

	public void OnLoadGame(string FileName)
	{
		if(FileName.Length <=0){
		 FileName = GameObject.FindGameObjectWithTag("FileNameInputText").GetComponent<InputField>().text;
		}
		Debug.Log("loading " + FileName);
		
		
		
		//load stage from file
		var FileContent = System.IO.File.ReadAllText(@"C:\Users\yithu\OneDrive\Desktop\Unity\Platformer\" + FileName, Encoding.UTF8);
		
		var objsFromFile = JsonConvert.DeserializeObject<List<yh_game_object>>(FileContent);
		
		foreach(var thisObject in objsFromFile){
			Debug.Log("loading: " + thisObject.name);
			Debug.Log("putting: " + thisObject.name + " at pos " + thisObject.pos );
			if( thisObject.tag == "door"){
				GameObject DoorCopy = GameObject.Instantiate(GameObject.FindGameObjectsWithTag("door")[0]);
				
				DoorCopy.transform.position = thisObject.pos;
			
			}else if( thisObject.tag == "platform"){
				GameObject platformCopy = GameObject.Instantiate(GameObject.FindGameObjectsWithTag("platform")[0]);
				
				platformCopy.transform.position = thisObject.pos;
			
			}
		
		}
		
	
	}
	
	public void OnSaveGame(string FileName)
	{
		if(FileName.Length <=0){
		 FileName = GameObject.FindGameObjectWithTag("FileNameInputText").GetComponent<InputField>().text;
		}
		Debug.Log("saving game" + FileName);
		
		string FileContent = "[";
		 GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;
		 
		 var setting = new JsonSerializerSettings();
        setting.Formatting = Formatting.Indented;
        setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
		 foreach(var thisObject in allObjects){
			    try{
			   print(thisObject.name+" is an active object") ;
			   FileContent += "{\n\"name\": ";
				FileContent += JsonConvert.SerializeObject(thisObject.name, setting);
			
			   FileContent += ",";
			   FileContent += "\n\"tag\":";
				FileContent += JsonConvert.SerializeObject(thisObject.tag, setting);
			
			   FileContent += ",\n";
			   
			   FileContent += "\"pos\": ";
				FileContent += JsonConvert.SerializeObject(thisObject.transform.position, setting);
				
			
			
			   FileContent += "}";
			
			FileContent += ",";
			
			 }catch(Exception exc){
			 }
		 }
		FileContent += "]";
		
		  System.IO.File.WriteAllText(@"C:\Users\yithu\OneDrive\Desktop\Unity\Platformer\" + FileName, FileContent);
		
		
		
		
		
		
	}

	void Update()
	{

        if (anim.GetBool("Dying"))
        {
            anim.SetBool("Dead", true);
            anim.SetBool("Dying", false);

            anim.SetBool("flying", false);


        }
        if (anim.GetBool("Dead"))
        {
            anim.SetBool("flying", false);
            return;



        }

        moveXInput = Input.GetAxis("Horizontal");

        if(hasjetpack )

        {

            if (Input.GetKey("space"))
            {
                anim.SetBool("ground", false);

                Debug.Log("jetpack on");

                anim.SetBool("flying", true);
                Vector2 newPosition = GetComponent<Rigidbody2D>().position;
                newPosition.y -= 1;
                //GetComponent<Rigidbody2D>().position = newPosition;
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 3.0f);
                fuel = fuel - 0.2f;
                if(fuel <= 0.0f)
                {

                    fuel = UnityEngine.Random.Range(30.0f, 100.0f);
                    hasjetpack = false;

                    anim.SetBool("flying", false);
					
					if( grabbedJetpack != null ){
						grabbedJetpack.SetActive(true);
						
						grabbedJetpack= null;
					}



                }


            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, Physics2D.gravity.y);

                Debug.Log("jetpack off");

                anim.SetBool("flying", false);
            }
            Debug.Log("Fuel: " + fuel);
        }
        else
        if ((grounded) && Input.GetButtonDown("Jump"))
        {
            anim.SetBool("ground", false);
            float dice_number = UnityEngine.Random.Range(1.0f, 2.0f);
            Debug.Log("Number Rolled: " + dice_number);
                

            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.y, jumpForce*dice_number);
        }


        anim.SetFloat("HSpeed", Mathf.Abs(moveXInput));
        //anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);


        GetComponent<Rigidbody2D>().velocity = new Vector2((moveXInput * HSpeed), GetComponent<Rigidbody2D>().velocity.y);

        //if (Input.GetButtonDown("Fire1") && (grounded)) { anim.SetTrigger("Punch"); }

        if (Input.GetButton("Fire2"))
        {
            anim.SetBool("Sprint", true);
            HSpeed = 14f;
}
        else
        {
            anim.SetBool("Sprint", false);
            HSpeed = 10f;
        }

        //Flipping direction character is facing based on players Input
        if (moveXInput > 0 && !facingRight)
            Flip();
        else if (moveXInput < 0 && facingRight)
            Flip();

        foreach (var cukeman in GameObject.FindGameObjectsWithTag("Cucumber"))
        {

            if ((Vector2.Distance(cukeman.transform.position, GetComponent<Rigidbody2D>().position) < 3))
            {


                cukeman.BroadcastMessage("appear");
            }
            else
            {
                cukeman.BroadcastMessage("disappear");
            }

        }
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Barrel");

        foreach (var obj in objs)
        {
            if ( Mathf.Abs(Vector2.Distance(obj.transform.position, GetComponent<Rigidbody2D>().position)) < 0.5f)
            {
                int speedOfBarrel = (int)Mathf.Round(obj.GetComponent<Rigidbody2D>().velocity.magnitude);
				
                hp = hp - 25;//speedOfBarrel/10f ;
            }
        }

        if (hp <= 0)
        {

			hp = 0;
            anim.SetBool("Dying", true);


        }


		foreach( var jetpack in GameObject.FindGameObjectsWithTag("jetpack") ){
			if ((Vector2.Distance( jetpack.transform.position, GetComponent<Rigidbody2D>().position) < 0.5f))
			{
				hasjetpack = true;
				
				
				grabbedJetpack = jetpack;
				jetpack.SetActive(false);
			
				
			}
		}
		//if bear gets the key
		
		foreach( var key in GameObject.FindGameObjectsWithTag("key") ){
			if ((Vector2.Distance(key.transform.position, GetComponent<Rigidbody2D>().position) < 0.5f))
			{
				haskey = true;
				
				//hide the key
				grabbedKey = key;
				key.SetActive(false);
				//GameObject.FindGameObjectWithTag("key").transform.position =  new Vector3(GameObject.FindGameObjectsWithTag("key")[0].transform.position.x, - 10.0f, GameObject.FindGameObjectsWithTag("key")[0].transform.position.z);
				
			}
		
		}
		//bear fall to death
		if (GetComponent<Rigidbody2D>().position.y < -20)
			
		{
			
				GetComponent<Rigidbody2D>().position = spawnpoint.transform.position;
				//GameObject.FindGameObjectWithTag("key").transform.position = new Vector2(-17, 3);
				
				if( grabbedKey != null ){
					grabbedKey.SetActive(true);
					grabbedKey= null;
				}
				if( grabbedJetpack != null ){
					grabbedJetpack.SetActive(true);
					
					grabbedJetpack= null;
				}
				GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0.1f);
				haskey = false;
				attempts++;
				
				
		}
		
		
		if ((Vector2.Distance(GameObject.FindGameObjectsWithTag("door")[0].transform.position, GetComponent<Rigidbody2D>().position) < 3))
		{
				if(haskey)
					
				{
					dooropen = true;
					//hide the door
					GameObject.FindGameObjectWithTag("door").SetActive(false);
					EditModeFunctions.OnLoadGame("level1");
					//GameObject.FindGameObjectsWithTag("door")[0].transform.position =  new Vector3(GameObject.FindGameObjectsWithTag("door")[0].transform.position.x, - 10.0f, //GameObject.FindGameObjectsWithTag("door")[0].transform.position.z);
				}
		}

         //Debug.Log("hp" + hp);
       // GameObject.FindGameObjectsWithTag("Health_Text")[0].GetComponent<Text>().text = "\n Health: " + ((int)hp) +" Fuel: " + ((int)fuel) +"\n Attempts: " + (attempts);

    }


    ////Flipping direction of character
    void Flip()
	{
		facingRight = !facingRight;
		theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
