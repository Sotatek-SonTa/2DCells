using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using static GameStatic;

public partial class NetworkManager : Singleton<NetworkManager>
{

    APIRequest apiRequest = new();
    void Start()
    {
        // UserDataOOP user = new UserDataOOP();
        // user.userId = "userID1234";
        // user.userName = "exampleUser";
        // user.password = "examplePassword";
        // user.credit = 100;

        // PostNewUserToServer(user);
    }
    #region Get
    public void GetMutationDataFromServer()
    {
        StartCoroutine(CreateWebGetRequest(HOST + GET_MUTATION_API, (string data) =>
        {
            DataManager.Instance.GetMutationData(data);
        }));
    }
    public void GetEnemyDataFromSever(){
        StartCoroutine(CreateWebGetRequest(HOST + GET_ENEMY_API,(string data) => 
        {
            DataManager.Instance.GetEnemydata(data);
        }));
    }
    #endregion
    
    #region Post
    public void PostNewUserToServer(UserDataOOP newUser)
    {
        APIRequest apiRequest = new();
        apiRequest.url = HOST + "/api/Users";
        string jsonData = JsonConvert.SerializeObject(newUser);
        apiRequest.body = jsonData;
        StartCoroutine(CreateWebPostRequest(apiRequest, (string data) =>
        {
            Debug.Log("done");
        }));
    }
    #endregion
}