﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DPSCalc : MonoBehaviour
{

    float startTime = 0;

    float damageDone = 0;

    float lastUpdate = 0;

    public Text m_DPSDisplay;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > lastUpdate + 1.0f)
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

        m_DPSDisplay.text = "DPS: " + (int)(damageDone / (Time.time - startTime));
    }
}
