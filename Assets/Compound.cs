using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

public enum elementType { Lantanoid, Scandium, Oxygen, Bor1, Bor2 };

[Serializable]
public class ClickData
{
    public string name;
    public float a, b, c, x, y, z;
    public bool beta;
    public ClickData ang1, ang3;
    public ClickData()
    {

    }
    public ClickData(float a, float b, float c, float x, float y, float z)
    {
        this.a = a;
        this.b = b;
        this.c = b;
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

[System.Serializable]
public class Compound
{
    public float a;
    public float b;
    public float c;
    public string name;
    public List<Element> elList;
    public Compound()
    {
        elList = new List<Element>();
    }
}

[System.Serializable]
public class Element
{
    public elementType type;
    public string elenemtName;
    public float xn, yn, zn, xd,yd,zd;
    public float x { get { return xn / xd; } }
    public float y { get { return yn / yd; } }
    public float z { get { return zn / zd; } }
}
