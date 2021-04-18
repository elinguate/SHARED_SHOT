using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DPSCalc : MonoBehaviour
{

    float startTime = 0;

    float damageDone = 0;

    float lastUpdate = 0;

    public float resetTime = 1.0f;

    public Text m_DPSDisplay;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > lastUpdate + resetTime)
        {
            startTime = Time.time;
            damageDone = 0;
            lastUpdate = 0;
        }
    }

    public void AddDamage(float _dmg)
    {
        lastUpdate = Time.time;
        damageDone += _dmg;

        float val = (damageDone / (Time.time - startTime));
        string toDisp = (val) < 0 ? "0" : (Mathf.Round(val * 100f) / 100f).ToString();

        m_DPSDisplay.text = "DPS: " + toDisp;
    }
}
