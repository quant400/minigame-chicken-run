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
    GameObject[] NPCPrefab;
    [SerializeField]
    Transform[] NPCSPawnPoints;

    private string chosenNFTName;

    private void Start()
    {
        //SinglePlayerScoreBoardScript.instance.PlayerJoined("1", 1);

        //span point chaged to index 4 intead of 0 will make it a single point intead of array if only one player in game decided
        //GameObject temp = Instantiate(characters[SingleplayerGameControler.instance.chosenAvatar], spawnPoint.position, Quaternion.identity);
        chosenNFTName = NameToSlugConvert(SingleplayerGameControler.instance.chosenNFT.name);
        GameObject resource = Resources.Load(Path.Combine("SinglePlayerPrefabs/Characters", chosenNFTName)) as GameObject;
        GameObject temp = Instantiate(resource, spawnPoint.position, Quaternion.identity);
        
        SpawnChickens();
        SpawnNpc();
    }


    void SpawnChickens()
    {
        int remainingToSpwan = SingleplayerGameControler.instance.GetChickenCount();
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
        yield return new WaitForSeconds(SingleplayerGameControler.instance.GetSpawnInterval());
        var temp=Instantiate(chickenPrefab, chickenSpawnPoints[Random.Range(0,10)].position, Quaternion.identity);
        temp.transform.parent = chickenHolder;
        StartCoroutine("SpawnRandomChicken");
    }

    void SpawnNpc()
    {
        for(int i=0;i<NPCSPawnPoints.Length;i++)
        {
            var randomNPCNo = Random.Range(0, NPCPrefab.Length);
            if (NPCPrefab[randomNPCNo].name == chosenNFTName)
            {
                randomNPCNo = randomNPCNo < NPCPrefab.Length ? randomNPCNo + 1 : 0;
            }
            Instantiate(NPCPrefab[randomNPCNo], NPCSPawnPoints[i].position, Quaternion.identity);
        }
    }

    public GameObject[] GetCharacterList()
    {
        return characters;
    }

    string NameToSlugConvert(string name)
    {
        string slug;
        slug = name.ToLower().Replace(" ", "-");
        return slug;

    }
    
}
