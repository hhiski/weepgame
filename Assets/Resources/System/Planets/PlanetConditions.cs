using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlanetConditions
{

    public static int GetConditionLevel(string conditionType, float flevel){
        int level = (int)((10 * flevel) - 1);
        int conditionLevel = 0;

        //0 = safe, 1 = caution, 2 = warning
        int[] TempRange = new int[] { 1, 1, 1, 0, 0, 1, 1, 2, 2, 2 };
        int[] PresRange = new int[] { 1, 1, 1, 1, 0, 0, 1, 1, 2, 2 };
        int[] RadiRange = new int[] { 0, 0, 0, 0, 1, 1, 2, 2, 2, 2 };
        if (conditionType == "temperature") { conditionLevel = TempRange[level]; }
        if (conditionType == "pressure") { conditionLevel = PresRange[level]; }
        if (conditionType == "radiation") { conditionLevel = RadiRange[level]; }

        return conditionLevel;
    }


    public static  float GetRadIndicatorLevel(float radiation)
    {

        float interpolatedRad = (Mathf.Log(radiation, 10) + 5) / 10;
        float radLevel = (Mathf.Round(interpolatedRad * 10)) / 10;
        return radLevel;
    }
    public static float GetPresIndicatorLevel(float pressure)
    {
        float presLevel = 0;
        if (pressure <= 0.003906f) { presLevel = 0.1f; }
        else if (pressure <= 0.015625f) { presLevel = 0.2f; }
        else if (pressure <= 0.0625f) { presLevel = 0.3f; }
        else if (pressure <= 0.25f) { presLevel = 0.4f; }
        else if (pressure <= 1) { presLevel = 0.5f; }
        else if (pressure <= 4) { presLevel = 0.6f; }
        else if (pressure <= 16) { presLevel = 0.7f; }
        else if (pressure <= 64) { presLevel = 0.8f; }
        else if (pressure <= 256) { presLevel = 0.9f; }
        else { presLevel = 1; }

        return presLevel;

    }
    public static float GetTempIndicatorLevel(float temperature)
    {

        float tempLevel = 0;
        if (temperature <= -200) { tempLevel = 0.1f; }
        else if (temperature <= -100) { tempLevel = 0.2f; }
        else if (temperature <= -50) { tempLevel = 0.3f; }
        else if (temperature <= 0) { tempLevel = 0.4f; }
        else if (temperature <= 50) { tempLevel = 0.5f; }
        else if (temperature <= 100) { tempLevel = 0.6f; }
        else if (temperature <= 200) { tempLevel = 0.7f; }
        else if (temperature <= 400) { tempLevel = 0.8f; }
        else if (temperature <= 800) { tempLevel = 0.9f; }
        else { tempLevel = 1; }

        return tempLevel;
    }
}
