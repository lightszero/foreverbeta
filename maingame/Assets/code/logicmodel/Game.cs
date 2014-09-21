using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
interface IGameForModel
{
    string ver
    {
        get;
    }
    IGameModel GetModel(string type);
    IGameModel InitModel(string type);

    void UnloadModel(string type);

    void ChangeScreen(string code);
}
interface IGameForControl
{
    string ver
    {
        get;
    }
    void Init();
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
            if(mode==null)
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
    public void Init()
    {
        modelmgr = new GameModelMgr();
        modelmgr.Init();
    }
    public void Update(float delta)
    {
        string release = null;
        foreach(var m in models)
        {
            m.Value.Update(delta);
            if (m.Value.inited)
            {

            }
            if(m.Value.exited&&release!=null)
            {
                release = m.Key;
            }
        }
        if(release!=null)
        {
            models.Remove(release);
        }
    }
    public void BeginExit()
    {
        foreach(var m in models.Values)
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


    public void ChangeScreen(string code)
    {
        //throw new NotImplementedException();
    }
}

class GameModelMgr
{
    public void Init()
    {

    }
    public IGameModel Create(string type)
    {
        return null;
    }
}