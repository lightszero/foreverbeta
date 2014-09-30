using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class screen_init : IScreenController
{




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
        Debug.Log("OnInited");
        screen.SetUpdateRate(1);
        Debug.Log("game:" + game);
        Debug.Log("game.rootUI:" + game.rootUI);
        GameObject label = game.rootUI.transform.Find("ScreenLabel").gameObject;
        Text text = label.GetComponent<Text>();
        text.text = "Init Test.\n初始化测试";

        IUIToolModel tool = game.InitModel("UITool") as IUIToolModel;//初始化UI工具模块

        var img = tool.createImage();
        //RectTransform t = img.transform as RectTransform;
        //t.localScale = Vector3.one;
        //t.localPosition = Vector3.zero;
        //t.anchorMin = Vector2.zero;
        //t.anchorMax = Vector2.zero;

        IBlockSceneModel scene =game.InitModel("blockscene") as IBlockSceneModel;
        var bs = scene.createMap(256, 64);
        //game.GetModel("UITool") as IToolModel;

        //game.rootUI.transform as RectTransform;
    }
    public void OnUpdate()
    {
        Debug.Log("OnUpdate");
    }
    public void OnExited()
    {
        Debug.Log("OnExited");
    }
}
