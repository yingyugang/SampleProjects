using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Assets.Scripts.Town
{
    public class TownDataMission : ItemAbstract
    {
        public List<StaffData> staffsData;
        public TargetTraining targetTraning;
        public bool isCanSick;
        public int targetNumberLoterry;
        public List<int> buildingsOpen;
        public List<int> itemsInShop;
        public List<int> typesMedia;
        public List<int> typesResearch;

        int tempcount, countIndex;
        StaffData tempStaff;
        public void GetDataFromXML(XmlNode node)
        {
            countIndex = 0;
            staffsData = new List<StaffData>();
            isCanSick = false;
            buildingsOpen = new List<int>();
            itemsInShop = new List<int>();
            typesMedia = new List<int>();
            typesResearch = new List<int>();
            targetTraning = new TargetTraining();
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                tempcount = Convert.ToInt16(node.ChildNodes[i].Attributes["id"].Value);
                buildingsOpen.Add(tempcount);
                if (tempcount == 0) //staff building
                {
                    if (node.ChildNodes[i].Attributes["fail"] != null)
                        isCanSick = Convert.ToBoolean(node.ChildNodes[i].Attributes["fail"].Value);
                    if (node.ChildNodes[i].Attributes["number"] != null)
                    {
                        targetTraning.targetNumber = Convert.ToInt16(node.ChildNodes[i].Attributes["number"].Value);
                        targetTraning.targetLevel = Convert.ToInt16(node.ChildNodes[i].Attributes["level"].Value);
                    }
                    foreach (XmlNode miniNode in node.ChildNodes[i].ChildNodes)
                    {
                        if (miniNode.Attributes["id"] == null)
                        {
                            Debug.Log("Why null");
                            break;
                        }
                        tempStaff = new StaffData(Convert.ToInt16(miniNode.Attributes["id"].Value));
                        if (miniNode.Attributes["startNumber"] != null)
                            tempStaff.isHired = Convert.ToBoolean(miniNode.Attributes["startNumber"].Value);
                        if (miniNode.Attributes["startLevel"] != null)
                        {
                            tempStaff.startLevel = Convert.ToInt16(miniNode.Attributes["startLevel"].Value);
                            tempStaff.currentLevel = tempStaff.startLevel;
                        }
                        if (miniNode.Attributes["targetLevel"] != null)
                            tempStaff.targetLevel = Convert.ToInt16(miniNode.Attributes["targetLevel"].Value);
                        if (miniNode.Attributes["maxLevel"] != null)
                            tempStaff.maxLevel = Convert.ToInt16(miniNode.Attributes["maxLevel"].Value);
                        if (tempStaff.maxLevel < tempStaff.targetLevel) tempStaff.maxLevel = tempStaff.targetLevel;
                        tempStaff.index = countIndex;
                        countIndex++;
                        staffsData.Add(tempStaff);
                    }
                }
                else if (tempcount == 1)//supermarket building
                {
                    foreach (XmlNode miniNode in node.ChildNodes[i].ChildNodes)
                    {
                        itemsInShop.Add(Convert.ToInt16(miniNode.Attributes["id"].Value));
                    }
                }
                else if (tempcount == 2)//communication building
                {
                    foreach (XmlNode miniNode in node.ChildNodes[i].ChildNodes)
                    {
                        typesMedia.Add(Convert.ToInt16(miniNode.Attributes["id"].Value));
                    }
                }
                else if (tempcount == 3)//research building
                {
                    foreach (XmlNode miniNode in node.ChildNodes[i].ChildNodes)
                    {
                        typesResearch.Add(Convert.ToInt16(miniNode.Attributes["id"].Value));
                    }
                }
                else if (tempcount == 4)//lottery building
                {
                    if (node.ChildNodes[i].Attributes["targetNumber"] != null)
                        targetNumberLoterry = Convert.ToInt16(node.ChildNodes[i].Attributes["targetNumber"].Value);
                    else targetNumberLoterry = 0;
                }
            }
        }
        public override int getTarget()
        {
            if (typeShow == 0)
            {
                return 0;
            }
            else
            {
                return targetNumberLoterry;//mục tiêu quay sổ số bao nhiêu lần                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
            }
        }
        public override int getType()
        {
            return 0;
        }
    }

    public class StaffData : ItemAbstract
    {
        public int idStaff;
        public int startLevel;
        public int targetLevel;
        public int maxLevel;
        public bool isHired;
        public StaffData(int id)
        {
            idStaff = id;
            startLevel = 1;
            maxLevel = 1;
            targetLevel = 1;
            currentLevel = startLevel;
            isHired = false;
        }
        public StaffData(int id, int start, int max, int target, bool hired)
        {
            idStaff = id;
            startLevel = start;
            maxLevel = max;
            targetLevel = target;
            isHired = hired;
            currentLevel = startLevel;
        }

        public override int getTarget()
        {
            if (typeShow == 0)
            {
                return targetLevel;
            }
            else
            {
                return 0;
            }
        }

        public override int getType()
        {
            return idStaff;
        }
    }

    public class TargetTraining : ItemAbstract
    {
        public int targetNumber;
        public int targetLevel;
        public TargetTraining()
        {
            targetNumber = 0;
            currentNumber = 0;
            targetLevel = 1;
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

        public override int getType()
        {
            return 0;
        }
    }
}
