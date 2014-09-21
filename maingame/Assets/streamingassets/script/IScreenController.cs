//这个桥接类型，在脚本的世界里和程序的世界里同时存在
interface IScreenController
{
    IGameForModel game
    {
        get;
    }
    IScreenForModel screen
    {
        get;
    }

    void OnInited();
    void OnUpdate();
    void OnExited();
}
interface IScreenControllerAsync : IScreenController
{
    void OnNavInitBegin();
    void OnNavExitBegin();
    void OnNavUpdate();
}