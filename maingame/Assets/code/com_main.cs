using UnityEngine;
using System.Collections;

//主程序入口
public class com_main : MonoBehaviour {

	// Use this for initialization
	void Start () {
        game = new Game();
        Debug.LogWarning("GameCore Ver:" + game.ver);
        game.Init(rootUI,rootScene,"screen_init");

        
	}
    public Font font;
    public GameObject rootUI;
    public GameObject rootScene;
    IGameForControl game;
	// Update is called once per frame
	void Update () {
        if (font != null && font.dynamic)//保证我们的字体是像素化的
        {
            font.material.mainTexture.filterMode = FilterMode.Point;
        }
        game.Update(Time.deltaTime);
	}


}
