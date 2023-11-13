using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;


//һ��Vector3 �����л�����
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

//����Sprite �����л�����
[System.Serializable]
public class SerializeSprite
{
    public string spriteAddress;
}