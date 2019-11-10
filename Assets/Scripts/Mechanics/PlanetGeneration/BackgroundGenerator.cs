using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Background> backgroundGen(List<Sprite> sbackgrounds, bool parallaxY, bool parallaxX, GameObject backgroundPref)
    {        
        List<Background> backgrounds = new List<Background>();
        int parallaxN = 1 / sbackgrounds.Count;

        for (int i = 0; i < sbackgrounds.Count; i++)
        {
            Vector3 pos;
            Vector3 posLeft;
            if (i == 0)
            {
                pos = new Vector3(0, 0, 0);
                posLeft = new Vector3(0, 0, 0);
            }
            else
            {
                pos = new Vector3(i * (float)sbackgrounds[i].bounds.size.x, 0, 0);
                posLeft = new Vector3(-i * (float)sbackgrounds[i].bounds.size.x, 0, 0);
            }
            Background background;
            Background backgroundLeft;
            int parallaxEffectY;
            int parallaxEffectX;
            if (parallaxY)
            {
                if (i == 0)
                    parallaxEffectY = 1;
                else if (i == sbackgrounds.Count - 1)
                    parallaxEffectY = 0;
                else
                    parallaxEffectY = parallaxN * i;
            }
            else { parallaxEffectY = 1; }

            if (parallaxX)
            {
                if (i == 0)
                    parallaxEffectX = 1;
                else if (i == sbackgrounds.Count - 1)
                    parallaxEffectX = 0;
                else
                    parallaxEffectX = parallaxN * i;

            }
            else { parallaxEffectX = 1; }

            background = new Background(pos, i, parallaxEffectX, parallaxEffectY, backgroundPref, sbackgrounds[i]);
            backgrounds.Add(background);
            if (i != 0)
            {
                backgroundLeft = new Background(posLeft, i, parallaxEffectX, parallaxEffectY, backgroundPref, sbackgrounds[i]);
                backgrounds.Add(backgroundLeft);
            }
        }

        return backgrounds;
    }
}
