 using UnityEngine;
 using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEditor;

using Newtonsoft.Json;
 
 public class EditModeFunctions : EditorWindow
 {
	 
		private string stringToEdit = "";
     [MenuItem("YitHuan/GameEdit")]
     public static void ShowWindow()
     {
         GetWindow<EditModeFunctions>("Edit Mode Functions");
     }
 
     private void OnGUI()
     {
		 stringToEdit = EditorGUILayout.TextField("LevelName", stringToEdit);
         if (GUILayout.Button("SaveGame"))
         {
			 stringToEdit = EditorGUILayout.TextField("LevelName", stringToEdit);
             OnSaveGame(stringToEdit);
         }
		 if (GUILayout.Button("LoadGame"))
         {
			 stringToEdit = EditorGUILayout.TextField("LevelName", stringToEdit);
             OnLoadGame(stringToEdit);
         }
		 if (GUILayout.Button("ResetGame"))
         {
			 onResetGame();
         }
     }
	 
	 public static void onResetGame()
	{
		
		List<GameObject> rootObjects = new List<GameObject>();
		
		Scene scene = SceneManager.GetActiveScene();
		
		scene.GetRootGameObjects( rootObjects );
		
		foreach(GameObject thisObject in rootObjects){
			
			if( !thisObject.tag.Contains("template")  && ! (thisObject.tag == "Tiles") ){
				
				GameObject.DestroyImmediate(thisObject);
			
			}else{
			
				thisObject.SetActive( true );
			
			}
		
		}

	}
	 
	 public static void OnLoadGame( string FileName)
	{
		
		onResetGame();
		
		Debug.Log("loading " + FileName);
		
		//load stage from file
		var FileContent = System.IO.File.ReadAllText(@"C:\Users\yithu\OneDrive\Desktop\Unity\Platformer\" + FileName, Encoding.UTF8);
		
		var objsFromFile = JsonConvert.DeserializeObject<List<yh_game_object>>(FileContent);
		
		foreach(var thisObject in objsFromFile){
			Debug.Log("loading: " + thisObject.name);
			Debug.Log("putting: " + thisObject.name + " at pos " + thisObject.pos );
			if( thisObject.tag == "door"){
				GameObject DoorCopy = GameObject.Instantiate(GameObject.FindGameObjectsWithTag("template_door")[0]);
				DoorCopy.tag = "door";
				DoorCopy.transform.position = thisObject.pos;
			
			}else if( thisObject.tag == "key"){
				GameObject keyCopy = GameObject.Instantiate(GameObject.FindGameObjectsWithTag("template_key")[0]);
				keyCopy.tag = "key";
				keyCopy.transform.position = thisObject.pos;
			
			}else if( thisObject.tag == "bear"){
				GameObject platformCopy = GameObject.Instantiate(GameObject.FindGameObjectsWithTag("template_bear")[0]);
				platformCopy.tag = "bear";
				platformCopy.transform.position = thisObject.pos;
			
			}else if( thisObject.tag == "jetpack"){
				GameObject platformCopy = GameObject.Instantiate(GameObject.FindGameObjectsWithTag("template_jetpack")[0]);
				platformCopy.tag = "jetpack";
				platformCopy.transform.position = thisObject.pos;
			
			}else if( thisObject.tag == "platform"){
				GameObject platformCopy = GameObject.Instantiate(GameObject.FindGameObjectsWithTag("template_platform")[0]);
				platformCopy.tag = "platform";
				platformCopy.transform.position = thisObject.pos;
			
			}else if( thisObject.tag == "Cucumber"){
				GameObject platformCopy = GameObject.Instantiate(GameObject.FindGameObjectsWithTag("template_cucumber")[0]);
				platformCopy.tag = "Cucumber";
				platformCopy.transform.position = thisObject.pos;
			
			}else if( thisObject.tag == "Barrel"){
				GameObject platformCopy = GameObject.Instantiate(GameObject.FindGameObjectsWithTag("template_barrel")[0]);
				platformCopy.tag = "Barrel";
				platformCopy.transform.position = thisObject.pos;
			
			}else if( thisObject.tag == "spawnpoint"){
				GameObject platformCopy = GameObject.Instantiate(GameObject.FindGameObjectsWithTag("template_spawnpoint")[0]);
				platformCopy.tag = "spawnpoint";
				platformCopy.transform.position = thisObject.pos;
			
			}
		
		}
		
		List<GameObject> rootObjects = new List<GameObject>();
		
		Scene scene = SceneManager.GetActiveScene();
		
		scene.GetRootGameObjects( rootObjects );
		foreach(GameObject thisObject in rootObjects){
			
			if( thisObject.tag.Contains("template") && !(thisObject.tag == "Tiles")  ){
				
			thisObject.SetActive( false );
			
			}
			
		
		}
		
	
	}
 
    public void OnSaveGame( string FileName)
	{
		Debug.Log("saving game" + FileName);
		
		string FileContent = "[";
		 GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;
		 
		 var setting = new JsonSerializerSettings();
        setting.Formatting = Formatting.Indented;
        setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
		 foreach(var thisObject in allObjects){
			 
			 if(thisObject.tag.Contains("template"))
				 continue;
			    try{
			   //print(thisObject.name+" is an active object") ;
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
 }