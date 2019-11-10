using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> backgroundGen(List<Sprite> sbackgrounds, GameObject cam, bool parallaxY, bool parallaxX, Material mat, GameObject backgroundPref)
    {

        List<int> orderLayer = new List<int>();
        List<int> parallaxEffectX = new List<int>();
        List<int> parallaxEffectY = new List<int>();
        List<Material> backgroundMaterials = new List<Material>();
        List<GameObject> backgrounds = new List<GameObject>();
        int parallaxN = 1 / sbackgrounds.Count;

        for (int i = 0; i < sbackgrounds.Count; i++)
        {
            backgroundPref.GetComponent<SpriteRenderer>.orderLayer = i;
            orderLayer.Add(i);
            backgroundMaterials.Add(mat);
            if (parallaxY)
            {
                if (i == 0)
                    parallaxEffectY.Add(1);
                else if (i == sbackgrounds.Count - 1)
                    parallaxEffectY.Add(0);
                else
                    parallaxEffectY.Add(parallaxN * i);
            }

            if (parallaxX)
            {
                if (i == 0)
                    parallaxEffectX.Add(1);
                else if (i == sbackgrounds.Count - 1)
                    parallaxEffectX.Add(0);
                else
                    parallaxEffectX.Add(parallaxN*i);

            }      
        }

        return backgrounds;
    }
}
