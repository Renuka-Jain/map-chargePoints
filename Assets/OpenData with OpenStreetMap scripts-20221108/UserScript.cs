using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserScript : MonoBehaviour
{

    [SerializeField]
    GameObject mapCanvas;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MapLocation());
    }

    public IEnumerator MapLocation()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(5);
            

	double lon = 2.15223;
	double lat = 41.40645;

	 double x = Math.Floor(MapManager.TileX);
            double y = Math.Floor(MapManager.TileY);
            int zoom = MapManager.zoom;

            double a = DrawCubeX(lon, TileToWorldPos(x, y, zoom).X, TileToWorldPos(x + 1, y, zoom).X);
            double b = DrawCubeY(lat, TileToWorldPos(x, y + 1, zoom).Y, TileToWorldPos(x, y, zoom).Y);
        Debug.Log("width/2 " + mapCanvas.GetComponent<RectTransform>().rect.width / 2);
        Debug.Log("x :" + mapCanvas.transform.localPosition.x);
        Debug.Log("height/2 " + mapCanvas.GetComponent<RectTransform>().rect.height / 2);
        Debug.Log("y :" + mapCanvas.transform.localPosition.y);
        a = (a* mapCanvas.GetComponent<RectTransform>().rect.width) + (mapCanvas.transform.localPosition.x - (mapCanvas.GetComponent<RectTransform>().rect.width/2));
        b = (b* mapCanvas.GetComponent<RectTransform>().rect.height) + (mapCanvas.transform.localPosition.y - (mapCanvas.GetComponent<RectTransform>().rect.height/2)); 
            Debug.Log(" Pos Canvas" + a + "/" + b);

        this.transform.localPosition = new Vector3((float)a, (float)b, 0.0f);
       
            
        }
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
