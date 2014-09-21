using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IScriptModel:IGameModel
{
    CSLE.CLS_Environment getScriptEnv();
}
public interface IUIToolModel:IGameModel
{
    GameObject createImage();
    GameObject createRawImage();
}
