using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    GameObject centerMap;
    [SerializeField]
    GameObject right;

    public static double TileX;
    public static double TileY;
    public static double TileZ;
    public static int zoom;

    // Start is called before the first frame update
    void Start()
    {

        zoom = 12;
        GetTile(41.40645,2.15223);
        StartCoroutine(GetOpenStreetMap());
    }

    IEnumerator GetOpenStreetMap()
    {
 	Debug.Log("https://a.tile.openstreetmap.org/" + zoom + "/" + Math.Floor(TileX) + "/" +Math.Floor(TileY) + ".png");
      	using (UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://a.tile.openstreetmap.org/"+zoom+"/"+Math.Floor(TileX)+"/"+Math.Floor(TileY)+".png"))
        {
            yield return www.Send();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture myTexture = DownloadHandlerTexture.GetContent(www);
		centerMap.GetComponent<RawImage>().texture = myTexture;
            }
        }
       
            
        
        using (UnityWebRequest www2 = UnityWebRequestTexture.GetTexture("https://a.tile.openstreetmap.org/"+zoom+"/"+Math.Floor(TileX+1)+"/"+Math.Floor(TileY)+".png"))
	{
        	yield return www2.Send();

        	if (www2.isNetworkError)
        	{
            		Debug.Log(www2.error);
        	}
        	else
        	{
            		Texture myTexture = ((DownloadHandlerTexture)www2.downloadHandler).texture;
			right.GetComponent<RawImage>().texture = myTexture;

            		GameObject openData = GameObject.Find("OpenDataManager");
            		openData.SendMessage("DownLoadData");
        	}
    	}
    }

    public void GetTile(double lat, double lon)
    {
        TileX = (float)((lon + 180.0) / 360.0 * (1 << zoom));
        TileY = (float)((1.0 - Math.Log(Math.Tan(lat * Math.PI / 180.0) +
        1.0 / Math.Cos(lat * Math.PI / 180.0)) / Math.PI) / 2.0 * (1 <<
        zoom));
	TileZ = (float)((1.0 + Math.Log(Math.Tan(lat * Math.PI / 180.0) +
        1.0 / Math.Cos(lat * Math.PI / 180.0)) / Math.PI) / 2.0 * (1 <<
        zoom));

    }
}
