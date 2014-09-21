class GameModelMgr
{
    public void Init()
    {

    }
    public IGameModel Create(string type)
    {
        switch (type.ToLower())
        {
            case "script":
                return new ScriptModel();
            case "uitool":
                return new UIToolModel();
        }
        return null;
    }
}