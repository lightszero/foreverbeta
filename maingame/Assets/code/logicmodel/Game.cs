using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public interface IGameModel
{
    void Update(float delta);
    void BeginInit();

    bool inited
    {
        get;
    }


    void BeginExit();
    bool exited
    {
        get;
    }
}
public interface IGameForModel
{
    string ver
    {
        get;
    }
    IGameModel GetModel(string type);
    IGameModel InitModel(string type);

    GameObject rootUI
    {
        get;
    }
    GameObject rootScene
    {
        get;
    }
    void UnloadModel(string type);

    void NavTo(string ScreenName);

    void NavBack();

    bool haveNavTarget
    {
        get;
    }
}
interface IGameForControl
{
    string ver
    {
        get;
    }
    void Init(GameObject rootUI, GameObject rootScene, string firstscreen);
    void Update(float delta);

    void BeginExit();

    bool exited
    {
        get;
    }
}
class Game : IGameForModel, IGameForControl
{
    public string ver
    {
        get
        {
            return "0.001Alpha";
        }
    }
    //获取游戏模块
    public IGameModel GetModel(string type)
    {
        if (models.ContainsKey(type) == false)
        {
            return null;
        }
        else
        {
            return models[type];
        }
    }
    //初始化游戏模块
    public IGameModel InitModel(string type)
    {
        if (models.ContainsKey(type) == false)
        {
            var mode = modelmgr.Create(type);
            if (mode == null)
            {
                throw new Exception("Model Type do not exist.");
            }
            models[type] = mode;
            mode.BeginInit();
            return mode;
        }
        else
        {
            throw new Exception("Model has exist.");
        }
    }
    //卸载游戏模块
    public void UnloadModel(string type)
    {
        if (models.ContainsKey(type) == false)
        {
            throw new Exception("Model do not exist.");
        }
        else
        {
            models[type].BeginExit();
        }
    }
    Dictionary<string, IGameModel> models = new Dictionary<string, IGameModel>();
    public void Init(GameObject rootUI, GameObject rootScene, string firstscene)
    {
        modelmgr = new GameModelMgr();
        modelmgr.Init();

        //初始必备的模块先初始化
        InitModel("script");

        NavTo(firstscene);
    }
    public void Update(float delta)
    {
        string release = null;
        foreach (var m in models)
        {
            m.Value.Update(delta);
            if (m.Value.inited)
            {

            }
            if (m.Value.exited && release != null)
            {
                release = m.Key;
            }
        }
        if (release != null)
        {
            models.Remove(release);
        }

        //刷新所有屏幕状态
        ScreenProxy remove = null;
        foreach (var p in screens)
        {
            if (p.exited)
                remove = p;
            p.Update();
        }
        if (remove != null)
            screens.Remove(remove);
        //刷新当前Screen
        if (current != null)
        {
            current.Update(delta);
        }
        //是否要导航
        if (navTarget != null)
        {
            if (navTarget.inited)
            {
                current = navTarget.screen;
            }
        }

    }
    public void BeginExit()
    {
        foreach (var m in models.Values)
        {
            m.BeginExit();
        }
    }
    public bool exited
    {
        get
        {
            return models.Count > 0;
        }
    }
    GameModelMgr modelmgr;

    public GameObject rootUI
    {
        get;
        private set;
    }
    public GameObject rootScene
    {
        get;
        private set;
    }

    public void NavTo(string ScreenName)
    {
        if (navTarget != null) return;
        if (current != null && current.name == ScreenName) return;
        foreach (var s in screens)
        {
            if (s.name == ScreenName)
            {
                navTarget = s;
                s.BeginInit();
                continue;
            }
            if (navTarget != null)
            {
                s.BeginExit();
            }
        }
        if (navTarget == null)
        {
            navTarget = new ScreenProxy(ScreenName, this);
            navTarget.BeginInit();
            screens.Add(navTarget);
        }
    }

    public void NavBack()
    {
        if (current == null) return;
        if (navTarget != null) return;

        string lastname = null;
        foreach (var s in screens)
        {
            if (s.name == current.name)
            {
                if (lastname != null)
                    NavTo(lastname);
                return;
            }
            else
            {
                lastname = s.name;
            }
        }
    }
    Screen current = null;
    public ScreenProxy navTarget
    {
        get;
        private set;
    }
    public bool haveNavTarget
    {
        get
        {
            return navTarget != null;
        }

    }
    List<ScreenProxy> screens = new List<ScreenProxy>();
}

class GameModelMgr
{
    public void Init()
    {

    }
    public IGameModel Create(string type)
    {
        switch (type)
        {
            case "script":
                return new ScriptModel();
        }
        return null;
    }
}