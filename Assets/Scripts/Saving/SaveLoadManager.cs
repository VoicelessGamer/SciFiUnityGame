using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadManager {

    public static void saveStarSystem(OrbitalDetails orbitalDetails) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/orbitalDetails.sav", FileMode.Create);

        SerializableOrbitDetails serializableOrbitDetails = new SerializableOrbitDetails(orbitalDetails);

        bf.Serialize(stream, serializableOrbitDetails);
        stream.Close();
    }

    public static OrbitalDetails loadStarSystem() {
        if(File.Exists(Application.persistentDataPath + "/orbitalDetails.sav")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/orbitalDetails.sav", FileMode.Open);

            SerializableOrbitDetails serializableOrbitDetails = (SerializableOrbitDetails)bf.Deserialize(stream);

            OrbitalDetails orbitalDetails = deserializeOrbitDetails(serializableOrbitDetails);

            stream.Close();

            return orbitalDetails;
        }

        return null;
    }

    public static OrbitalDetails deserializeOrbitDetails(SerializableOrbitDetails serializableOrbitDetails) {
        List<OrbitalDetails> orbitingBodies = new List<OrbitalDetails>();

        if(serializableOrbitDetails.orbitingBodies.Count > 0) {
            for(int i = 0; i < serializableOrbitDetails.orbitingBodies.Count; i++) {
                orbitingBodies.Add(deserializeOrbitDetails(serializableOrbitDetails.orbitingBodies[i]));
            }
        }

        if(serializableOrbitDetails.foci1 != null) {
            return new OrbitalDetails(serializableOrbitDetails.radius,
                serializableOrbitDetails.mass,
                serializableOrbitDetails.semiMinorAxis,
                serializableOrbitDetails.semiMajorAxis,
                serializableOrbitDetails.eccentricity,
                new Vector3(serializableOrbitDetails.foci1[0], serializableOrbitDetails.foci1[1], serializableOrbitDetails.foci1[2]),
                new Vector3(serializableOrbitDetails.foci2[0], serializableOrbitDetails.foci2[1], serializableOrbitDetails.foci2[2]),
                new Vector3(serializableOrbitDetails.centre[0], serializableOrbitDetails.centre[1], serializableOrbitDetails.centre[2]),
                new Vector3(serializableOrbitDetails.localCentreVector[0], serializableOrbitDetails.localCentreVector[1], serializableOrbitDetails.localCentreVector[2]),
                serializableOrbitDetails.currentTheta,
                serializableOrbitDetails.cosineEllipseRotation,
                serializableOrbitDetails.sineEllipseRotation,
                serializableOrbitDetails.distanceFromFoci,
                orbitingBodies);
        } else {
            return new OrbitalDetails(serializableOrbitDetails.radius,
                serializableOrbitDetails.mass,
                orbitingBodies);
        }
    }

    public static void saveTileMappings(Dictionary<int, int[,]> tileMapping) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/tileMappings.sav", FileMode.Create);

        TileMapping mapping = new TileMapping(tileMapping);

        bf.Serialize(stream, mapping);
        stream.Close();
    }

    public static Dictionary<int, int[,]> loadTileMappings() {
        if(File.Exists(Application.persistentDataPath + "/tileMappings.sav")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/tileMappings.sav", FileMode.Open);

            Dictionary<int, int[,]> tileMapping = ((TileMapping)bf.Deserialize(stream)).mapping;
            stream.Close();

            return tileMapping;
        }

        return new Dictionary<int, int[,]>();
    }
}

[Serializable]
public class SerializableOrbitDetails {
    public float radius;
    public float mass;
    public float semiMinorAxis;
    public float semiMajorAxis;
    public float eccentricity;
    public float[] foci1;
    public float[] foci2;
    public float[] centre;
    public float[] localCentreVector;
    public float currentTheta;
    public float cosineEllipseRotation;
    public float sineEllipseRotation;
    public float distanceFromFoci;
    public List<SerializableOrbitDetails> orbitingBodies;

    public SerializableOrbitDetails(OrbitalDetails orbitalDetails) {
        radius = orbitalDetails.getRadius();
        mass = orbitalDetails.getMass();
        semiMinorAxis = orbitalDetails.getSemiMinorAxis();
        semiMajorAxis = orbitalDetails.getSemiMajorAxis();
        eccentricity = orbitalDetails.getEccentricity();
        foci1 = orbitalDetails.getFoci1();
        foci2 = orbitalDetails.getFoci2();
        centre = orbitalDetails.getCentre();
        localCentreVector = orbitalDetails.getLocalCentreVector();
        currentTheta = orbitalDetails.getCurrentTheta();
        cosineEllipseRotation = orbitalDetails.getCosineEllipseRotation();
        sineEllipseRotation = orbitalDetails.getSineEllipseRotation();
        distanceFromFoci = orbitalDetails.getDistanceFromFoci();

        List<SerializableOrbitDetails> children = new List<SerializableOrbitDetails>();
        foreach(OrbitalDetails currentOrbitalDetails in orbitalDetails.getOrbitingBodies()) {
            children.Add(new SerializableOrbitDetails(currentOrbitalDetails));
        }
        orbitingBodies = children;
    }
}

[Serializable]
public class TileMapping {
    public Dictionary<int, int[,]> mapping;

    public TileMapping(Dictionary<int, int[,]> mapping) {
        this.mapping = mapping;
    }
}