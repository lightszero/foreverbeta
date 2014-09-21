using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class ScreenProxy
{

    public ScreenProxy(string scenenname, IGameForModel game)
    {
        this.name = scenenname;
        this.game = game;
    }
    IGameForModel game;
    public string name
    {
        get;
        private set;
    }
    bool bInUpdate = false;
    bool bExUpdate = false;

    public void BeginInit()
    {
        if (screen == null)
        {
            screen = new Screen(name, game);
            screen.BeginInit();
            bInUpdate = true;
        }
    }
    public bool inited
    {
        get
        {
            if (screen != null)
                return screen.inited;
            else
                return false;
        }
    }
    public void Update()
    {
        if (screen != null && (bInUpdate || bExUpdate))
        {
            screen.UpdateNav();
            if (bInUpdate && inited)
            {
                bInUpdate = false;
            }
            if (bExUpdate && exited)
            {
                bExUpdate = false;
            }
        }
    }
    public Screen screen
    {
        get;
        private set;
    }

    public void BeginExit()
    {
        if (screen != null)
        {
            screen.BeginExit();
            bExUpdate = true;
        }
    }
    public bool exited
    {
        get
        {
            if (screen != null)
                return screen.exited;
            else
                return false;
        }
    }

}
public interface IScreenForModel
{
    void SetUpdateRate(int fps);
    void Inited();
    void Exited();
}
class Screen : IScreenForModel
{
    public Screen(string scenenname, IGameForModel game)
    {
        controller = new ScreenController(scenenname, game, this);
        this.name = scenenname;
    }
    ScreenController controller;
    public string name
    {
        get;
        private set;
    }
    float timer = 0;
    public void Update(float deltatimer)
    {
        timer += deltatimer;
        if (inited && !exited && updaterate > 0 && timer > 1.0f / (float)updaterate)
        {
            timer -= 1.0f / (float)updaterate;
            controller.OnUpdate();
        }
    }
    public void BeginInit()
    {
        controller.OnNavInitBegin();
    }
    public bool inited
    {
        get;
        private set;
    }
    public void BeginExit()
    {
        controller.OnNavExitBegin();
    }
    public bool exited
    {
        get;
        private set;
    }
    public void UpdateNav()//更新导航任务，也就是beginInit和beginExit的内容
    {
        controller.OnNavUpdate();
    }
    int updaterate = 0;
    public void SetUpdateRate(int fps)
    {
        updaterate = fps;
    }

    public class ScreenController : IScreenController
    {
        CSLE.CLS_Environment env;
        CSLE.CLS_Content content;
        public ScreenController(string name, IGameForModel game, IScreenForModel screen)
        {
            this.game = game;
            this.screen = screen;


            IScriptModel script = game.GetModel("script") as IScriptModel;
            env = script.getScriptEnv();
            var type = env.GetTypeByKeywordQuiet(name) as CSLE.CLS_Type_Class;
            content = new CSLE.CLS_Content(env);
            scriptThis = type.function.New(content, null).value as CSLE.SInstance;
            scriptThis.member["game"] = new CSLE.CLS_Content.Value();
            scriptThis.member["game"].value = game;
            scriptThis.member["game"].type = typeof(IGameForModel);
            scriptThis.member["screen"] = new CSLE.CLS_Content.Value();
            scriptThis.member["screen"].value = screen;
            scriptThis.member["screen"].type = typeof(IScreenForModel);


            var typeasync = env.GetTypeByKeywordQuiet("IScreenControllerAsync") as CSLE.CLS_Type_Class;
            try
            {
                havetypeasync = (type.ConvertTo(content, scriptThis, typeasync.type) != null);
            }
            catch
            {
                havetypeasync = false;
            }
            Debug.Log(havetypeasync);
        }
        bool havetypeasync = false;
        CSLE.SInstance scriptThis;



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





        public void OnNavInitBegin()
        {
            if (havetypeasync)
            {
                scriptThis.type.MemberCall(content, scriptThis, "OnNavInitBegin", null);
            }
            else
            {
                screen.Inited();
            }
        }

        public void OnNavExitBegin()
        {
            if (havetypeasync)
            {
                scriptThis.type.MemberCall(content, scriptThis, "OnNavExitBegin", null);
            }
            else
            {
                screen.Exited();
            }
        }

        public void OnNavUpdate()
        {
            if (havetypeasync)
            {
                scriptThis.type.MemberCall(content, scriptThis, "OnNavUpdate", null);
            }

        }

        public void OnInited()
        {
            scriptThis.type.MemberCall(content, scriptThis, "OnInited", null);
        }
        public void OnUpdate()
        {
            scriptThis.type.MemberCall(content, scriptThis, "OnUpdate", null);
        }
        public void OnExited()
        {
            scriptThis.type.MemberCall(content, scriptThis, "OnExited", null);
        }
    }




    public void Inited()
    {
        this.inited = true;
        controller.OnInited();
    }

    public void Exited()
    {
        this.exited = true;
        controller.OnExited();
    }
}

