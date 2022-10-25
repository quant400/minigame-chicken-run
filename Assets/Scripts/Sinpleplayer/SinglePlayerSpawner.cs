using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;
using System.IO;

public class SinglePlayerSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject chickenPrefab;
    [SerializeField]
    GameObject[] characters;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    Transform[] chickenSpawnPoints;
    [SerializeField]
    Transform chickenHolder;
    [SerializeField]
    GameObject[] NPCModels;
    [SerializeField]
    GameObject npcBasePrefab;
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    Transform[] NPCSPawnPoints;
    [SerializeField]
    int startingChickensForLevel;
    [SerializeField]
    float spawnInterval;

    private string chosenNFTName;

    private void Start()
    {

        SetUpChar();

        SpawnChickens();
        SpawnNpc();
    }

    void SetUpChar()
    {
        // changed for holloween
        gameplayView.instance.chosenNFT.name = "pumpkin";
        //end
        chosenNFTName = NameToSlugConvert(gameplayView.instance.chosenNFT.name); 
        GameObject temp = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        temp.GetComponent<SetUpSkin>().SetUpChar(chosenNFTName);
    }

    void SpawnChickens()
    {
        int remainingToSpwan = startingChickensForLevel;//gameplayView.instance.GetChickenCount();
        int index = 0;
        while(remainingToSpwan>0)
        {
            var temp=Instantiate(chickenPrefab, chickenSpawnPoints[index].position, Quaternion.identity);
            temp.transform.parent = chickenHolder;
            index++;
            if (index >= 19)
                index = 0;
            remainingToSpwan--;
        }
        StartCoroutine("SpawnRandomChicken");
    }

    IEnumerator SpawnRandomChicken()
    {
        yield return new WaitForSeconds(spawnInterval);
        var temp=Instantiate(chickenPrefab, chickenSpawnPoints[Random.Range(0,10)].position, Quaternion.identity);
        temp.transform.parent = chickenHolder;
        StartCoroutine("SpawnRandomChicken");
    }

    void SpawnNpc()
    {
        for (int i=0;i<NPCSPawnPoints.Length;i++)
        {
           
            List<int> randomNPCs = new List<int>();
            var randomNPCNo = Random.Range(0, NPCModels.Length);
            while (randomNPCs.Contains(randomNPCNo) || NPCModels[randomNPCNo].name == chosenNFTName)
                randomNPCNo = Random.Range(0, NPCModels.Length);

            GameObject temp = Instantiate(npcBasePrefab, NPCSPawnPoints[i].position, Quaternion.identity);
            temp.GetComponent<SetUpSkin>().SetUpChar(NPCModels[randomNPCNo].name);
        }
    }

    public GameObject[] GetCharacterList()
    {
        return characters;
    }

    string NameToSlugConvert(string name)
    {
        string slug;
        slug = name.ToLower().Replace(".", "").Replace("'", "").Replace(" ", "-");
        return slug;

    }
    
}
