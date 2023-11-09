//Camera���ӽ�
public enum CameraStyle
{
   None,
   FirstPerson,                      //��һ�˳�
   ThirdPersonNormal,                //�����˳������ӽ�
   ThirdPersonCombat,                //�����˳�ս���ӽ�
   ThirdPersonLock,                 //�����˳������ӽ�
   ThirdPersonTopDown                //�����˳Ƹ����ӽ�
}

//��Ʒ������
public enum ItemType
{
    Consumable,                      //����Ʒ��ʳ����ߵȣ�
    PrimaryWeapon,                   //��������
    SecondaryWeapon                  //��������
}

//���ӵ�����
public enum SlotType
{
    ConsumableBag,                    //��������Ʒ��
    EquipmentBag,                     //����װ����
    PrimaryWeapon,                    //����������
    SecondaryWeapon,                  //����������
    Action                            //�������
}

//��ʾ��������
public enum PanelType
{
    None,
    Inventory,     //ͬʱ��ʾ��ɫ�ͱ������
    Skill,         //ͬʱ��ʾ��ɫ�ͼ������
    QuestTask      //ͬʱ��ʾ��ɫ���������
}


//�˺�������
public enum DamageType
{
    Physical,      //�����˺�
    Magical,       //ħ���˺�
    True           //��ʵ�˺�

}

//���˵�״̬����
public enum EnemyState
{
    PartolState,        //Ѳ��״̬
    ChaseState,         //׷��״̬
    AttackState         //����״̬
}

//���ǵ���������
public enum PlayerWeaponType
{
    LightSword,
    GreatSword,
    Bow,
    Shield
}