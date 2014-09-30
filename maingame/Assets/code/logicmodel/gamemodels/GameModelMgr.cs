class GameModelMgr
{
    public void Init(IGameForModel game)
    {
        this.game = game;
    }
    IGameForModel game;
    public IGameModel Create(string type)
    {
        switch (type.ToLower())
        {
            case "script":
                return new ScriptModel(game);
            case "uitool":
                return new UIToolModel(game);

            case "blockscene":
                return new BlockSceneModel(game);
        }
        return null;
    }
}