using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public GameObject cam;
    public Texture2D t2d;
    private Sprite spr;
    private SpriteRenderer sr;
    
    private double horizontalSize;
    private double verticalSize;

    public float rotateSpeed;
    public float radius;
    public bool affectTime;
    private Vector2 _centre;
    public float fullDay;
    private float startOfNight;

    public float _timeofDay;

    // Start is called before the first frame update
    void Start()
    {

        verticalSize = cam.GetComponent<Camera>().orthographicSize * 2.0;
        horizontalSize = verticalSize * Screen.width / Screen.height;
        sr = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
        sr.sortingOrder = 1;
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0.0f);
        spr = Sprite.Create(t2d, new Rect(0.0f, 0.0f, t2d.width, t2d.height), new Vector2(0.5f, 0.5f), 100.0f);

        sr.sprite = spr;

        _centre = transform.position;

        Time.timeScale = 1.0f;
        startOfNight = fullDay / 3;
    }

    // Update is for curve of the sun/moon
    void Update()
    {
        _centre = new Vector2(cam.transform.position.x, cam.transform.position.y);
        _timeofDay += rotateSpeed * Time.deltaTime;
        var offset = new Vector2(Mathf.Sin(_timeofDay), Mathf.Cos(_timeofDay)) * radius;

        transform.position = _centre + offset;
        
        if (_timeofDay >= fullDay)
            _timeofDay = 0;
        Debug.Log(_timeofDay);

        if (affectTime)
        {
            if (_timeofDay >= startOfNight && _timeofDay <= (fullDay - startOfNight))
            {
                Debug.Log("Night Time");
            }
            else
            {
                Debug.Log("Day Time");
            }
        }

    }
}
