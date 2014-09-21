using UnityEngine;
using System.Collections;
using System;

public class screen_init : IScreenController
{
    public void OnNavInitBegin()
    {
        Debug.Log("screen=" + screen);
        //screen.Inited();
    }

    public void OnNavExitBegin()
    {
        //screen.Exited();
    }

    public void OnNavUpdate()
    {
        
    }




    public IGameForModel game
    {
        get;
        private set;
    }

    public IScreenForModel screen
    {
        get;
        private set;
    }



    public void OnInited()
    {
        throw new NotImplementedException();
    }
    public void OnUpdate()
    {
        throw new NotImplementedException();
    }
    public void OnExited()
    {
        throw new NotImplementedException();
    }
}
