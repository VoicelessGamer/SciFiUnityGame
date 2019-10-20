using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquids 
{
    private int sectionId;
    private List<GameObject> gameObject;

    public Liquids(int sectionId, List<GameObject> gameObject)
    {
        this.sectionId = sectionId;
        this.gameObject = gameObject;
    }

    public int getSectionId()
    {
        return this.sectionId;
    }

    public List<GameObject> getGameObject()
    {
        return this.gameObject;
    }
}
