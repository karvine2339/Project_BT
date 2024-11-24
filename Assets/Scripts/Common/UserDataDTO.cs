using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataDTO { }

public class PlayerSessionDTO : UserDataDTO
{
    public Vector3 lastPosition;
    public float lastHP;
    public float lastMP;
}

public class PlayerDTO : UserDataDTO
{
    public string nickName;
    public int level;
    public int money;
    public float exp;
}
