using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MachineInfomation  {

    public int IDMachine { set; get; }
    public string machineNameVie { set; get; }
    public string machineNameEng { set; get; }
    public int machineCost { set; get; }
    public List<int> listProductOfMachine { set; get; }
    public List<int> listRationFail { set; get; }
    public MachineInfomation(int IDMachine, string machineNameVie, string machineNameEng, int machineCost, List<int> listProductOfMachine, List<int> listRationFail)
    {
        this.IDMachine = IDMachine;
        this.machineNameVie = machineNameVie;
        this.machineNameEng = machineNameEng;
        this.machineCost = machineCost;
        this.listProductOfMachine = listProductOfMachine;
        this.listRationFail = listRationFail;
    }
}
