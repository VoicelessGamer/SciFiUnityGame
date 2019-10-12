using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section {
    private int sectionId;
    private GameObject gameObject;

    public Section(int sectionId, GameObject gameObject) {
        this.sectionId = sectionId;
        this.gameObject = gameObject;
    }

    public int getSectionId() {
        return sectionId;
    }

    public GameObject getGameObject() {
        return gameObject;
    }
}
