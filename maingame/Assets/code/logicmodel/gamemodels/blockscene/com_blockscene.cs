using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
class com_blockscene : MonoBehaviour
{
    Texture2D texMainmap;

    public void Init(int w,int h)
    {
        Mesh m = new Mesh();
        
        Vector3[] verts = new Vector3[4];
        verts[0] = new Vector3(0, 0, 0);
        verts[1] = new Vector3(w, 0, 0);
        verts[2] = new Vector3(0, h, 0);
        verts[3] = new Vector3(w, h, 0);
        m.vertices = verts;
        
        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector3(0, 1);
        uv[3] = new Vector2(1, 1);
        m.uv = uv;
        
        Vector3[] normals = new Vector3[4];
        normals[0] = new Vector3(0, 0, -1);
        normals[1] = new Vector3(0, 0, -1);
        normals[2] = new Vector3(0, 0, -1);
        normals[3] = new Vector3(0, 0, -1);
        m.normals = normals;

        int[] tris = new int[6];
        tris[0] = 0;
        tris[1] = 2;
        tris[2] = 1;
        tris[3] = 1;
        tris[4] = 2;
        tris[5] = 3;
        m.triangles = tris;
        
        this.GetComponent<MeshFilter>().mesh = m;

        Material mat = new Material(Shader.Find("Unlit/Transparent"));
        texMainmap = new Texture2D(w, h, TextureFormat.ARGB32, false, false);
        texMainmap.filterMode = FilterMode.Point;
        
        Genmap();
        mat.mainTexture = texMainmap;

        
        this.GetComponent<MeshRenderer>().material = mat;
    }

    static System.Random ran = null;
    static double ranNumber(double min, double max)
    {
        if (ran == null)
        {
            ran = new System.Random();
        }
        return min + (max - min) * ran.NextDouble();
    }
    static int ranNumberInt(double min, double max)
    {
        if (ran == null)
        {
            ran = new System.Random();
        }
        return (int)(min + (max - min) * ran.NextDouble());
    }

    void GenWorld(int width, int height, Action<int, int, Color32> _drawfunc)
    {
        float line1min = 0.47f;
        float line1max = 0.6f;
        
        float sPower = (float)height / 1024.0f;
        //决定两根世界线
        double worldLine1 = (double)height *(line1min+line1max)  *0.5f* ranNumber(0.9, 1.1);
        double worldLine2 = (double)height * (line1max+0.2f) * ranNumber(0.9, 1.1);

        //查找线路
        double line1Min = worldLine1;
        double line1Max = worldLine1;
        double line2Min = worldLine2;
        double line2Max = worldLine2;
        //保存每一列的line1,line2
        double[] wl1 = new double[width];
        double[] wl2 = new double[width];
        int seed = 0;//随机种子
        int seedlen = 0;//随机种子使用步长
        for (int x = 0; x < width; x++)
        {


            float floatx = (float)x / (float)width;
            if (seedlen <= 0)
            {
                seed = ranNumberInt(0, 5);//随机出五种情况
                seedlen = (int)(ranNumber(5, 40) * sPower);//这种情况的处理步长
                if (seed == 0)
                {
                    seedlen = (int)(seedlen * ranNumber(1, 6) * sPower);
                }
            }
            seedlen--;
            if (seed == 0)//上上下下摆动，下挖的几率比较高
            {
                while (ranNumber(0, 7) == 0)
                {
                    worldLine1 += (double)ranNumber(-1, 2) * sPower;
                }
            }
            else if (seed == 1)//先高再低，高的几率比较高
            {
                while (ranNumberInt(0, 4) == 0)//高出
                {
                    worldLine1 -= 1.0 * sPower;
                }
                while (ranNumberInt(0, 10) == 0)//挖下
                {
                    worldLine1 += 1.0 * sPower;
                }
            }
            else if (seed == 2)//先低再高，低得的几率比较高
            {
                while (ranNumberInt(0, 4) == 0)
                {
                    worldLine1 += 1.0 * sPower;
                }
                while (ranNumberInt(0, 10) == 0)
                {
                    worldLine1 -= 1.0 * sPower;
                }
            }
            else if (seed == 3)//先高再低，大角度版
            {
                while (ranNumberInt(0, 2) == 0)
                {
                    worldLine1 -= 1.0 * sPower;
                }
                while (ranNumberInt(0, 6) == 0)
                {
                    worldLine1 += 1.0 * sPower;
                }
            }
            else if (seed == 4)//先低再高，大角度版
            {
                while (ranNumberInt(0, 2) == 0)
                {
                    worldLine1 += 1.0 * sPower;
                }
                while (ranNumberInt(0, 5) == 0)
                {
                    worldLine1 -= 1.0 * sPower;
                }
            }


            //限制范围

            if (worldLine1 < (double)height * line1min)
            {
                worldLine1 = (double)height * line1min;
                seedlen = 0;
            }
            else
            {
                if (worldLine1 > (double)height * line1max)
                {
                    worldLine1 = (double)height * line1max;
                    seedlen = 0;
                }
            }
            //在两边靠近边界的地方限制深度
            if ((floatx < 0.1 || floatx > 0.9) && worldLine1 > (double)height * 0.25)
            {
                worldLine1 = (double)height * (line1max+line1min)*0.5f;
                seedlen = 1;
            }



            //插值为自循环的
            if (floatx >= 0.75f)
            {
                float needx = 1.0f - floatx;
                float v = (floatx - 0.75f) / 0.25f;

                double n1 = wl1[(int)(needx * (float)width)] - wl1[0];
                worldLine1 = worldLine1 * (1 - v) + (wl1[0] - n1) * v;
                //double n2 = wl2[(int)(needx * (float)width)] - wl2[0];
                //worldLine2 = worldLine2 * (1 - v) + (wl2[0] - n2) * v;
            }

            //深层地表根据表层生成
            while (ranNumberInt(0, 3) == 0)//一定几率摆动
            {
                worldLine2 += (float)ranNumberInt(-2, 3) * sPower;
            }
            //小小的调整
            if (worldLine2 < worldLine1 + (double)height * 0.05)
            {
                worldLine2 += 1.0 * sPower;
            }
            if (worldLine2 > worldLine1 + (double)height * 0.35)
            {
                worldLine2 -= 1.0 * sPower;
            }

            ////插值为自循环的
            if (floatx >= 0.75f)
            {
                float needx = 1.0f - floatx;
                float v = (floatx - 0.75f) / 0.25f;

                //double n1 = wl1[(int)(needx * (float)width)] - wl1[0];
                //worldLine1 = worldLine1 * (1 - v) + (wl1[0] - n1) * v;
                double n2 = wl2[(int)(needx * (float)width)] - wl2[0];
                worldLine2 = worldLine2 * (1 - v) + (wl2[0] + n2) * v;
            }
            wl1[x] = worldLine1;
            wl2[x] = worldLine2;
            line1Min = line1Min < worldLine1 ? line1Min : worldLine1;
            line1Max = line1Max > worldLine1 ? line1Max : worldLine1;
            line2Min = line2Min < worldLine1 ? line2Min : worldLine1;
            line2Max = line2Max > worldLine1 ? line2Max : worldLine1;
            for (int y = 0; y < height; y++)
            {
                if (y < (int)worldLine1)
                {
                    _drawfunc(x, y,new Color32(0,0,0,0));
                }
                else if (y < (int)worldLine2)
                {
                    _drawfunc(x, y, new Color32(128,128,0,255));
                }
                else
                {
                    _drawfunc(x, y, new Color32(192,192,9,255));
                }
            }
        }
    }

    void Genmap()
    {
        Color32[] data =new Color32[texMainmap.width*texMainmap.height];
        Action<int, int, Color32> df =(int x,int y,Color32 c)=>
            {
                data[(texMainmap.height-1-y)*texMainmap.width+x] =c;
            };

        GenWorld(texMainmap.width, texMainmap.height, df);

        texMainmap.SetPixels32(data, 0);
        texMainmap.Apply();
    }
}

