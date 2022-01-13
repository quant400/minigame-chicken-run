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

    private void Start()
    {
        //SinglePlayerScoreBoardScript.instance.PlayerJoined("1", 1);

        //span point chaged to index 4 intead of 0 will make it a single point intead of array if only one player in game decided
        //GameObject temp = Instantiate(characters[SingleplayerGameControler.instance.chosenAvatar], spawnPoint.position, Quaternion.identity);
        string name = SingleplayerGameControler.instance.chosenNFT.name;
        GameObject resource = Resources.Load(Path.Combine("SinglePlayerPrefabs/Characters", name)) as GameObject;
        GameObject temp = Instantiate(resource, spawnPoint.position, Quaternion.identity);
        
        SpawnChickens();
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



    public GameObject[] GetCharacterList()
    {
        return characters;
    }
}
