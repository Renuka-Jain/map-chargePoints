using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PoiScript: MonoBehaviour
{
    [SerializeField]
    public double latObject;
    [SerializeField]
    public double lonObject;
    [SerializeField]
    public string textDescription;
    [SerializeField]
    public string typeConnector;
    [SerializeField]
    public string weekdays;
    [SerializeField]
    public string hourDays;
    [SerializeField]
    public string textAddress;


  
    GameObject mapCanvas;


    public void MapLocation()
    {
	mapCanvas = GameObject.Find("Center");
	GameObject o = GameObject.Find("Canvas");
        this.transform.SetParent(o.transform);

         double x = Math.Floor(MapManager.TileX);
            double y = Math.Floor(MapManager.TileY);
            int zoom = MapManager.zoom;

            double a = DrawCubeX(lonObject, TileToWorldPos(x, y, zoom).X, TileToWorldPos(x + 1, y, zoom).X);
            double b = DrawCubeY(latObject, TileToWorldPos(x, y + 1, zoom).Y, TileToWorldPos(x, y, zoom).Y);
        Debug.Log("width/2 " + mapCanvas.GetComponent<RectTransform>().rect.width / 2);
        Debug.Log("x :" + mapCanvas.transform.localPosition.x);
        Debug.Log("height/2 " + mapCanvas.GetComponent<RectTransform>().rect.height / 2);
        Debug.Log("y :" + mapCanvas.transform.localPosition.y);
        a = (a* mapCanvas.GetComponent<RectTransform>().rect.width) + (mapCanvas.transform.localPosition.x - (mapCanvas.GetComponent<RectTransform>().rect.width/2));
        b = (b* mapCanvas.GetComponent<RectTransform>().rect.height) + (mapCanvas.transform.localPosition.y - (mapCanvas.GetComponent<RectTransform>().rect.height/2)); 
            Debug.Log(" Pos Canvas" + a + "/" + b);

        this.transform.localPosition = new Vector3((float)a, (float)b, 0.0f);


       
    }
    public struct Point
    {
        public double X;
        public double Y;
    }


    // p.X -> longitud
    // p.Y -> latitud
    // left upper corner
    public Point TileToWorldPos(double tile_x, double tile_y, int zoom)
    {
        Point p = new Point();
        double n = System.Math.PI - ((2.0 * System.Math.PI * tile_y) / System.Math.Pow(2.0, zoom));

        p.X = ((tile_x / System.Math.Pow(2.0, zoom) * 360.0) - 180.0);
        p.Y = (180.0 / System.Math.PI * System.Math.Atan(System.Math.Sinh(n)));

        return p;
    }

    public double DrawCubeY(double targetLat, double minLat, double maxLat)
    {
        double pixelY = ((targetLat - minLat) / (maxLat - minLat));
        return pixelY;
    }

    public double DrawCubeX(double targetLong, double minLong, double maxLong)
    {
        double pixelX = ((targetLong - minLong) / (maxLong - minLong));
        return pixelX;
    }

   
    public void ShowInfo()
    {
        GameObject[] poiList = GameObject.FindGameObjectsWithTag("poi");
        GameObject textPoi = GameObject.Find("PoiText");
        textPoi.GetComponent<Text>().text = textDescription + textAddress;
        
    }

    

    //Simple way of calculating distance between to geolocated points. 
    //is used to calculate distance between user location and virtual object location
    int Distance(double latO, double lonO, double latUser, double lonUser)
    {
        //distance value meters
        int distance;


        double earthRadius = 6371;
        double radiantsFactor = System.Math.PI / 180;
        double diffLat = (latUser - latO) * radiantsFactor;
        double diffLng = (lonUser - lonO) * radiantsFactor;
        double a = System.Math.Pow(System.Math.Sin(diffLat / 2), 2) +
                                System.Math.Cos(latO * radiantsFactor) *
                                System.Math.Cos(latUser * radiantsFactor) *
                    System.Math.Pow(System.Math.Sin(diffLng / 2), 2);
        double c = 2 * System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1 - a));
        double res = earthRadius * c;

        distance = (int)(res * 1000);
        //distanceMeters = distance;
        //distanceUI.GetComponent<Text>().text =  distance + " meters";
        return distance;


    }

    public void Descripcio(){
	GameObject ob = GameObject.Find("TextDescripcio");
	//TextMeshPro textmeshPro = GetComponent<TextMeshPro>();
	ob.GetComponent<TMPro.TextMeshProUGUI>().text ="Nom: " + textDescription + " Adreça: "+textAddress;
	
	//ob.GetComponent<TextMeshPro>().SetText("hello");
	//textmeshPro.SetText("hello");
    }

}
