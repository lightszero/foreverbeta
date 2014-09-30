using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


class UIToolModel : IUIToolModel
{
    IGameForModel game;
    public UIToolModel(IGameForModel game)
    {
        this.game = game;
    }
    public GameObject createImage()
    {
        
        GameObject obj = new GameObject("image");
        obj.AddComponent<RectTransform>();
        obj.AddComponent<CanvasRenderer>();
        var img =obj.AddComponent<Image>();
        obj.transform.parent = this.game.rootUI.transform;
        obj.transform.localScale = Vector3.one;

                var tex= Resources.Load("back2") as Texture2D;

        img.sprite =Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),Vector2.zero
            ,100,1,SpriteMeshType.FullRect,new Vector4(3,3,3,3));
        img.type = Image.Type.Sliced;

        return obj;
    }
    public GameObject createRawImage()
    {

        GameObject obj = new GameObject("rawimage");
        obj.AddComponent<RectTransform>();
        obj.AddComponent<CanvasRenderer>();
        obj.AddComponent<RawImage>();
        obj.transform.parent = this.game.rootUI.transform;

        
        return obj;
    }


    public void Update(float delta)
    {

    }

    public void BeginInit()
    {
        inited = true;
    }

    public bool inited
    {
        get;
        private set;
    }

    public void BeginExit()
    {
        exited = true;
    }

    public bool exited
    {
        get;
        private set;
    }
}
