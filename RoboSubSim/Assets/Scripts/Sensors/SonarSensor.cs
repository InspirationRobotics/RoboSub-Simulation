using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class SonarSensor : MonoBehaviour, ISensor
{
    float maxRange;

    // are there layers we don't want to collide with?
    public string[] layerMaskNames;
    int collMask = 0;

    void Awake()
    {
        int v = 0;
        foreach (string layerName in layerMaskNames)
        {
            int layer = LayerMask.NameToLayer(layerName);
            v |= 1 << layer;
        }

        collMask |= ~v;

    }


    float GetSonarDistance()
    {

        Ray ray = new Ray(transform.position, transform.up);
        RaycastHit hit;
        float distance = -1;

        if (Physics.Raycast(ray, out hit, maxRange, collMask))
        {
            distance = hit.distance;
        }
        return distance;


    }

    public JSONObject RequestObs(JSONObject json)
    {
        json.AddField(transform.name, (float)Math.Round(GetSonarDistance(), 2));
        return json;
    }
}
