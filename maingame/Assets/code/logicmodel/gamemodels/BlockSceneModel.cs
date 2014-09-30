using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


    class BlockSceneModel:IBlockSceneModel
    {
        IGameForModel game;
        public BlockSceneModel(IGameForModel game)
        {
            this.game = game;
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

        public GameObject createMap(int x, int y)
        {
            GameObject obj = new GameObject("blockscene");//;; GameObject.CreatePrimitive(PrimitiveType.Quad);
            obj.transform.parent = game.rootScene.transform;
            var bs= obj.AddComponent<com_blockscene>();
            bs.Init(x,y);
            return obj;
        }
    }

