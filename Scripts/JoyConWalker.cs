using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


public class JoyConWalker : MonoBehaviour
{
    private ChromeDriver _driver;
    private List<Joycon>    m_joycons;
    private Joycon          m_joyconL;
    private Joycon          m_joyconR;
    private Joycon.Button?  m_pressedButtonL;
    private Joycon.Button?  m_pressedButtonR;
    int count = 0;
    float LlastUpper = 0;
    float LlastLower = 0;
    float Llast = 0;
    bool LisUp = true;
    float theta = 0.08f; //閾値。設定は微妙なので要調整。
    void Start()
    {
        var path = Application.streamingAssetsPath;
        _driver = new ChromeDriver(path);

        _driver.Navigate().GoToUrl(Application.dataPath + "/StreetView.html");

        m_joycons = JoyconManager.Instance.j;
        if ( m_joycons == null || m_joycons.Count <= 0 ) return;
        m_joyconL = m_joycons.Find( c =>  c.isLeft );
        m_joyconR = m_joycons.Find( c => !c.isLeft );
    }

    void Update()
    {
        var orientationL = m_joyconL.GetVector();
        var current = orientationL.z;
        if(LisUp)
        {
            if(current >= Llast)
            {
                LlastUpper = current;
            }
            else
            {
                if(LlastUpper - LlastLower >= theta)
                {
                    Proceed();
                }
                LisUp = false;
                LlastLower = current;
            }
        }
        else
        {
            if(current <= Llast)
            {
                LlastLower = current;
            }
            else
            {
                if(LlastUpper - LlastLower >= theta)
                {
                    Proceed();
                }
                LisUp = true;
                LlastUpper = current;
            }            
        }
        Llast = current;


    }

    void Proceed()
    {
        count++;
        Debug.Log(count);
        _driver.FindElement(By.Id("count")).Click();

    }

    void OnDestroy()
    {
        _driver.Dispose();
    }
}