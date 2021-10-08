using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using System.Collections;
using DG.Tweening;

public class MissionComplete : MonoBehaviour
{
    public Image imgBanner;
    public GameObject listTextGame;
    public GameObject parentItemsMission;
    public GameObject referenceItem; // Using for spawn
    public Sprite[] listIconGameMission;
    public Sprite[] listIconMoney;

    private List<ItemMissionComplete> listMission;

    private MissionCSVStructure GetMission(int idmission)
    {
        if (MasterCSV.missionCSV != null)
        {
            foreach (var item in MasterCSV.missionCSV)
            {
                if (item.id == idmission)
                {
                    return item;
                }
            }
        }
        return null;
    }

    private int  GetIdIconMission(string name)
    {
        int length = listIconGameMission.Length;
        for (int i = 0; i < length; i++)
        {
            if (listIconGameMission[i].name == name)
            {
                return i;
            }
        }
        return 0;
    }

    public IEnumerator LoadMissionItems(int idGame)
    {
        listMission = new List<ItemMissionComplete>();
        if (APIInformation.GetInstance != null && APIInformation.GetInstance.clear_mission_list != null)
        {
            int count = APIInformation.GetInstance.clear_mission_list.Count;
            float timeduration = 0.3f;
            for (int i = 0; i < count; i++)
            {
                Mission itemMission = APIInformation.GetInstance.clear_mission_list[i];
				GameObject obj = Instantiate(referenceItem, referenceItem.transform.position, Quaternion.identity) as GameObject;
                obj.transform.parent = parentItemsMission.transform;
                obj.transform.localScale = Vector3.one;
                ItemMissionComplete item = obj.GetComponent<ItemMissionComplete>();
                MissionCSVStructure mission = GetMission(itemMission.m_mission_id);
                int idmission = GetIdIconMission(mission.image_resource);
                item.SetInfo(listIconGameMission[idmission], mission.name, mission.reward_id);

                item.imgMark.gameObject.SetActive(false);
                item.imgMark.transform.DOScale(
                    new Vector3(3, 3, 3), timeduration)
                    .SetDelay(timeduration * i)
                    .SetAutoKill(true).From()
                    .OnComplete(PlayAnimMark).
                    SetUpdate(true);
                listMission.Add(item);
            }
            APIInformation.GetInstance.clear_mission_list.Clear();
            PlayAnimMark();
        }
        yield return null;
    }

    private void PlayAnimMark()
    {
        if (listMission != null && listMission.Count > 0)
        {
			ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se03_stamp);
            listMission[0].imgMark.gameObject.SetActive(true);
            listMission.RemoveAt(0);
        }
    }

    public void BackToHome()
    {
		ComponentConstant.SOUND_MANAGER.Play(SoundEnum.se01_ok);
        if (ComponentConstant.SCENE_COMBINE_MANAGER != null)
        {
			//if(APIInformation.GetInstance.next_clear_gacha == 10)
			if (APIInformation.GetInstance.gacha_result.cardinfo_list != null) {
				ComponentConstant.GACHA_PLAYER.unityAction = AnimationCompleteHandler;
				ComponentConstant.GACHA_PLAYER.Play (8, true);
			}
			else
           	 ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene(SceneEnum.Home);
        }
    }

	private void AnimationCompleteHandler (int type, bool isChanged)
	{
		ComponentConstant.SCENE_COMBINE_MANAGER.LoadScene(SceneEnum.Home);
	}
}
