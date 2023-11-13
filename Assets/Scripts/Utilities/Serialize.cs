using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;


//一：Vector3 的序列化问题
public class SerializeVector3
{
    public float x, y, z;

    public SerializeVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
    
}

//二：Sprite 的序列化问题
[System.Serializable]
public class SerializeSprite
{
    public string spriteAddress;
}