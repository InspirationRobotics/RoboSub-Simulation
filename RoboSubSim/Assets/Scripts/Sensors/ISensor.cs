using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISensor
{
    JSONObject RequestObs(JSONObject json);
}
