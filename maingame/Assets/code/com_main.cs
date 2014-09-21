using UnityEngine;
using System.Collections;

//主程序入口
public class com_main : MonoBehaviour {

	// Use this for initialization
	void Start () {
        game = new Game();
        Debug.LogWarning("GameCore Ver:" + game.ver);
        game.Init();
	}
    IGameForControl game;
	// Update is called once per frame
	void Update () {
        game.Update(Time.deltaTime);
	}


}
