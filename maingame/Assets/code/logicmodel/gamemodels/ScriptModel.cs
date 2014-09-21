using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class ScriptModel : IScriptModel, CSLE.ICLS_Logger
{
    public void Update(float delta)
    {

    }
    CSLE.CLS_Environment env = null;
    public void BeginInit()
    {
        env = new CSLE.CLS_Environment(this);
        Log_Warn("C#Light Ver:" + env.version);

        //System
        env.RegType(new CSLE.RegHelper_Type(typeof(NotImplementedException), "NotImplementedException"));
        env.RegType(new CSLE.RegHelper_Type(typeof(Exception), "Exception"));
        env.RegType(new CSLE.RegHelper_Type(typeof(object), "object"));

        //Framework
        env.RegType(new CSLE.RegHelper_Type(typeof(IScreenForModel), "IScreenForModel"));
        env.RegType(new CSLE.RegHelper_Type(typeof(IGameForModel), "IGameForModel"));

        env.RegType(new CSLE.RegHelper_Type(typeof(IGameModel), "IGameForModel"));
        env.RegType(new CSLE.RegHelper_Type(typeof(IScriptModel), "IScriptModel"));
        env.RegType(new CSLE.RegHelper_Type(typeof(IUIToolModel), "IUIToolModel"));

        //UnityEngine
        env.RegType(new CSLE.RegHelper_Type(typeof(UnityEngine.Debug), "Debug"));
        env.RegType(new CSLE.RegHelper_Type(typeof(UnityEngine.GameObject), "GameObject"));
        env.RegType(new CSLE.RegHelper_Type(typeof(UnityEngine.Object), "Object"));
        env.RegType(new CSLE.RegHelper_Type(typeof(UnityEngine.Transform), "Transform"));
        env.RegType(new CSLE.RegHelper_Type(typeof(UnityEngine.UI.Text), "Text"));
 
        

        //直接编译模式
        string[] files=  System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/script", "*.cs", System.IO.SearchOption.AllDirectories);
        Dictionary<string, IList<CSLE.Token>> tokens = new Dictionary<string, IList<CSLE.Token>>();
        foreach(var f in files)
        {
            string code = System.IO.File.ReadAllText(f);
            var t = env.ParserToken(code);
            tokens[f] = t;
        }

        env.Project_Compiler(tokens, true);


        inited = true;
    }

    public bool inited
    {
        get;
        private set;
    }

    public void BeginExit()
    {
        env = null;
        exited = true;
    }

    public bool exited
    {
        get;
        private set;
    }

    public void Log(string str)
    {
        Debug.Log(str);
    }

    public void Log_Warn(string str)
    {
        Debug.LogWarning(str);
    }

    public void Log_Error(string str)
    {
        Debug.LogError(str);
    }

    public CSLE.CLS_Environment getScriptEnv()
    {
        return env;
    }
}
