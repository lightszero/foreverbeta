using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IScriptModel:IGameModel
{
    CSLE.CLS_Environment getScriptEnv();
}
public interface IUIToolModel:IGameModel
{

}
