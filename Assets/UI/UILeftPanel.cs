using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CelestialBody;

public class UILeftPanel : MonoBehaviour
{
    public Material UIMainMaterial;
    public Material UICautionMaterial;
    public Material UIWarningMaterial;
    public Color MainColor = new Color(0, 1, 0);
    public Color CautionColor = new Color(1, 1, 0);
    public Color WarningColor = new Color(1, 0, 0);
    

    public void UpdatePlanetConditionData(Planet planet)
    {
        UpdateIndicatorGas(planet);
        UpdateIndicatorNumbers(planet);
        UpdateIndicatorBars(planet);

    }



    void UpdateIndicatorGas(Planet planet)
    {
        Text UIGasText = this.transform.Find("UIGasPanel/UIGas").GetComponent<UnityEngine.UI.Text>();
        string gasList = planet.Atm.PrimaryGas.Name.ToString();
        string gasListParsed = gasList.Replace("-", "\n");
        UIGasText.text = gasListParsed;
    }

    void UpdateIndicatorNumbers(Planet planet)
    {
        Text UITemperature = this.transform.Find("UILevels/UITemperature/UINum").GetComponent<UnityEngine.UI.Text>();
        Text UIPressure = this.transform.Find("UILevels/UIPressure/UINum").GetComponent<UnityEngine.UI.Text>();
        Text UIRadiation = this.transform.Find("UILevels/UIRadiation/UINum").GetComponent<UnityEngine.UI.Text>();

        string temperatureUnit = " °C";
        string pressureUnit = " bar";
        string radiationUnit = " Sv/y";

        float temperature = planet.Atm.TemperatureB + planet.Atm.TemperatureG;
        UITemperature.text = temperature.ToString("F0") + temperatureUnit;

        float pressure = planet.Atm.Pressure;
        if (pressure < 0.1f)
        {
            pressure *= 1000;
            pressureUnit = " mbar/y";
        }
        UIPressure.text = pressure.ToString("F1") + pressureUnit;

        float radiation = planet.Atm.Radiation;
        if (radiation < 0.1f)
        {
            radiation *= 1000;
            radiationUnit = " mSv/y";
        }
        UIRadiation.text = radiation.ToString("F1") + radiationUnit;
    }

    void UpdateIndicatorBars(Planet planet)
    {
        float temperature = planet.Atm.TemperatureG + planet.Atm.TemperatureB;
        float temperatureBarLevel = PlanetConditions.GetTempIndicatorLevel(temperature);

        float pressure = planet.Atm.Pressure;
        float pressureBarLevel = PlanetConditions.GetPresIndicatorLevel(pressure);

        float radiation = planet.Atm.Radiation;
        float radiationBarLevel = PlanetConditions.GetRadIndicatorLevel(radiation);

        Image UITemperatureLevel = this.transform.Find("UILevels/UITemperature/UIBar").gameObject.GetComponent<Image>();
        Image UIPressureLevel = this.transform.Find("UILevels/UIPressure/UIBar").gameObject.GetComponent<Image>();
        Image UIRadiationLevel = this.transform.Find("UILevels/UIRadiation/UIBar").gameObject.GetComponent<Image>();

        UITemperatureLevel.fillAmount = temperatureBarLevel;
        UIPressureLevel.fillAmount = pressureBarLevel;
        UIRadiationLevel.fillAmount = radiationBarLevel;

        UITemperatureLevel.material = GetBarColorMaterial("temperature", temperatureBarLevel);
        UIPressureLevel.material = GetBarColorMaterial("pressure", pressureBarLevel);
        UIRadiationLevel.material = GetBarColorMaterial("radiation", radiationBarLevel);

    }

    Material GetBarColorMaterial(string bar, float flevel)
    {

        int colorLevel = PlanetConditions.GetConditionLevel(bar, flevel);

        Material ColorMaterial = UIMainMaterial;
        Material[] Materials = new Material[] { UIMainMaterial, UICautionMaterial, UIWarningMaterial };

        ColorMaterial = Materials[colorLevel];

        return ColorMaterial;
    }
}
