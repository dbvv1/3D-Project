//Camera的视角
public enum CameraStyle
{
   None,
   FirstPerson,                      //第一人称
   ThirdPersonNormal,                //第三人称正常视角
   ThirdPersonCombat,                //第三人称战斗视角
   ThirdPersonLock,                 //第三人称锁定视角
   ThirdPersonTopDown                //第三人称俯视视角
}

//物品的类型
public enum ItemType
{
    Consumable,                      //消耗品（食物，道具等）
    PrimaryWeapon,                   //主手武器
    SecondaryWeapon                  //副手武器
}

//格子的类型
public enum SlotType
{
    ConsumableBag,                    //背包消耗品格
    EquipmentBag,                     //背包装备格
    PrimaryWeapon,                    //主手武器格
    SecondaryWeapon,                  //副手武器格
    Action                            //快捷栏格
}

//显示面板的类型
public enum PanelType
{
    None,
    Inventory,     //同时显示角色和背包面板
    Skill,         //同时显示角色和技能面板
    QuestTask      //同时显示角色和任务面板
}


//伤害的类型
public enum DamageType
{
    Physical,      //物理伤害
    Magical,       //魔法伤害
    True           //真实伤害

}

//敌人的状态类型
public enum EnemyState
{
    PartolState,        //巡逻状态
    ChaseState,         //追逐状态
    AttackState         //攻击状态
}

//主角的武器类型
public enum PlayerWeaponType
{
    LightSword,
    GreatSword,
    Bow,
    Shield
}