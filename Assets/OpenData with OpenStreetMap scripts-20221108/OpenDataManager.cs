using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class OpenDataManager : MonoBehaviour
{
    [SerializeField]
    GameObject poiPrefab;

    // Start is called before the first frame update
    //void Start()
    //{
      //  StartCoroutine(GetOpenData());
    //}


    public void DownLoadData()
    {
        StartCoroutine(GetOpenData());
    }


    IEnumerator GetOpenData()
    {
	String anterior = "";
	
         UnityWebRequest www = new UnityWebRequest("https://api.bsmsa.eu/ext/api/bsm/chargepoints/states");
        
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Or retrieve results as binary text
	    Debug.Log("Data set: " + www.downloadHandler.text);
	      String s = "{\"locations\":"+www.downloadHandler.text+'}';
	Debug.Log("Data set: " +s);
            JObject obj = (JObject)JObject.Parse(s);
            
            JArray chargePoints = (JArray)obj["locations"];

		int cont = 0;
		foreach (JObject item in chargePoints) // <-- Note that here we used JObject instead of usual JProperty
		{
			string actual = Convert.ToString(item.GetValue("Station_name"));
			if ( actual == anterior){
				cont++;
			}else{
    				double latitud = (double)item.GetValue("Station_lat");
				double longitud = (double)item.GetValue("Station_lng");
    				Debug.Log("Point: " +latitud+"-"+longitud);
			
			
				if(cont<500){
					GameObject poi = Instantiate(poiPrefab);
            				poi.GetComponent<PoiScript>().latObject = Convert.ToDouble(latitud);
            				poi.GetComponent<PoiScript>().lonObject = Convert.ToDouble(longitud);
            				poi.GetComponent<PoiScript>().textDescription = Convert.ToString(item.GetValue("Station_name"));
						poi.GetComponent<PoiScript>().textAddress = Convert.ToString(item.GetValue("Station_address"));
            				poi.GetComponent<PoiScript>().SendMessage("MapLocation");
				}
				cont++;
				anterior=actual;
			}
			
			
		}


        
        }
    }
}