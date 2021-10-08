using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;


namespace Assets.Scripts.Factory.ReadDataMission
{
    public class FactoryDataMission : ItemAbstract
    {
        public PositionUnLockInData positionUnlock;
        public List<MachineInData> machinedatas;
        public int targetProducts = 0;

        MachineInData machine;
        int indexMachine;
        public void getDataFromXML(XmlNode factoryNode)
        {
            if (factoryNode.Attributes["targetPosition"] != null)
                positionUnlock = new PositionUnLockInData(Convert.ToInt16(factoryNode.Attributes[0].Value), Convert.ToInt16(factoryNode.Attributes["targetPosition"].Value));
            else
                positionUnlock = new PositionUnLockInData(Convert.ToInt16(factoryNode.Attributes[0].Value), 0);
            if (factoryNode.Attributes["fail"] != null)
                positionUnlock.isCanBreak = Convert.ToBoolean(factoryNode.Attributes["fail"].Value);
            if (factoryNode.Attributes["targetProductions"] != null)
                targetProducts = Convert.ToInt16(factoryNode.Attributes["targetProductions"].Value);

            machinedatas = new List<MachineInData>();

            //Index de xac dinh vi tri cua may' trong mang machinedatas(Sau nay khong fai dung for)
            indexMachine = 0;
            foreach (XmlNode childNode in factoryNode.ChildNodes)
            {
                machine = new MachineInData(Convert.ToInt16(childNode.Attributes["id"].Value));
                if (childNode.Attributes["targetNumber"] != null)
                    machine.targetNumber = Convert.ToInt16(childNode.Attributes["targetNumber"].Value);
                if (childNode.Attributes["startLevel"] != null)
                {
                    machine.startLevel = Convert.ToInt16(childNode.Attributes["startLevel"].Value);
                    machine.currentLevel = machine.startLevel;
                }
                if (childNode.Attributes["targetLevel"] != null)
                    machine.targetLevel = Convert.ToInt16(childNode.Attributes["targetLevel"].Value);
                if (childNode.Attributes["maxLevel"] != null)
                    machine.maxLevel = Convert.ToInt16(childNode.Attributes["maxLevel"].Value);
                if (machine.maxLevel < machine.targetLevel) machine.maxLevel = machine.targetLevel;
                if (childNode.Attributes["startNumber"] != null)
                {
                    machine.startNumber = Convert.ToInt16(childNode.Attributes["startNumber"].Value);
                    machine.currentNumber = machine.startNumber;
                    //Debug.Log(machine.startNumber);
                }
                machine.index = indexMachine;
                indexMachine++;
                machinedatas.Add(machine);
            }
            machinedatas.Sort(
                delegate(MachineInData i1, MachineInData i2)
                {
                    return i1.iDMachine.CompareTo(i2.iDMachine);
                }
                );
        }
        public override int getType()
        {
            return 0;
        }


        public override int getTarget()
        {
            if (typeShow == 0)
            {
                return 0;
            }
            else
            {
                return targetProducts;
            }
        }
    }

    public class MachineInData : ItemAbstract
    {
        public int iDMachine;
        public int startNumber;
        public int targetNumber;
        public int startLevel;
        public int targetLevel;
        public int maxLevel;

        public MachineInData(int IDMachine)
        {
            this.iDMachine = IDMachine;
            startNumber = 0;
            targetNumber = 0;
            startLevel = 1;
            targetLevel = 1;
            maxLevel = 1;
            currentLevel = 1;
            currentNumber = 0;
            currentLevel = 1;
        }

        public override int getType()
        {
            return iDMachine;
        }


        public override int getTarget()
        {
            if (typeShow == 0)
            {
                return targetLevel;
            }
            else
            {
                return targetNumber;
            }
        }
    }

    public class PositionUnLockInData : ItemAbstract
    {
        public int positionUnLockBegin;
        public int targetPositionUnlock;
        public bool isCanBreak;

        public PositionUnLockInData(int positionUnLockBegin, int targetPositionUnlock)
        {
            this.positionUnLockBegin = positionUnLockBegin;
            this.targetPositionUnlock = targetPositionUnlock;
            currentNumber = positionUnLockBegin;
        }
        public override int getType()
        {
            return 0;
        }


        public override int getTarget()
        {
            if (typeShow == 0)
            {
                return 0;
            }
            else
            {
                return targetPositionUnlock;//mục tiêu mở đc bao nhiêu ô
            }
        }
    }

}
