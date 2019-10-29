using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public GameObject cam;
    public Texture2D t2d;
    public float speed;
    public float height;
    private Sprite spr;
    private SpriteRenderer sr;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 controlPos;
    float count = 0.0f;
    private double horizontalSize;
    private double verticalSize;
    // Start is called before the first frame update
    void Start()
    {

        verticalSize = cam.GetComponent<Camera>().orthographicSize * 2.0;
        horizontalSize = verticalSize * Screen.width / Screen.height;
        sr = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
        sr.sortingOrder = 1;
        transform.position = new Vector3(1.5f, 1.5f, 0.0f);
        spr = Sprite.Create(t2d, new Rect(0.0f, 0.0f, t2d.width, t2d.height), new Vector2(0.5f, 0.5f), 100.0f);

        sr.sprite = spr;

        startPos = new Vector3(cam.transform.position.x - (int)horizontalSize/2, cam.transform.position.y, cam.transform.position.z+1);
        endPos = new Vector3(cam.transform.position.x + (int)horizontalSize / 2, cam.transform.position.y, cam.transform.position.z+1);
        controlPos = startPos + (endPos - startPos) / 2 + Vector3.up * height;
    }

    // Update is for curve of the sun/moon
    void Update()
    {
        float startdist = cam.transform.position.x - (int)horizontalSize / 2;
        float enddist = cam.transform.position.x + (int)horizontalSize / 2;
        float currentx = startPos.x;
        float currentEndx = endPos.x;
        float startdiff = startdist - currentx;
        float enddiff = enddist - currentEndx;
        startPos.x = startPos.x + startdiff;
        endPos.x = endPos.x + enddiff;
        if (count < 1.0f)
        {
            count += speed * Time.deltaTime;

            Vector3 m1 = Vector3.Lerp(startPos, controlPos, count);
            Vector3 m2 = Vector3.Lerp(controlPos, endPos, count);
            transform.position = Vector3.Lerp(m1, m2, count);
        }
    }
}
