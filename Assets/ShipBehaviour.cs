using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour
{
    public GameObject TroopPrefab;
    
    private Vector3 _landingPosition;
    private Vector3 _startingPosition;

    private bool _landed = false;
    private bool _unloaded = false;

    private int numTroopersToSpawn = 3;

    private Coroutine LanderBrainRoutine;

    void Start()
    {
        _startingPosition = transform.position;
        var gameBoard = GameManager.Instance.GameBoard;
        var randomCell = gameBoard.GetRandomCell();
        _landingPosition = randomCell.transform.position;

        LanderBrainRoutine = StartCoroutine(LanderBrain());
    }

    private IEnumerator LanderBrain()
    {
        DOTween.To(() => transform.position, x => transform.position = x, _landingPosition, Globals.LanderTravelTime)
            .SetEase(Ease.InOutQuad)
            .onComplete += OnLanded;

        yield return new WaitUntil(() => _landed);

        for (int numMarinesSpawned = 0; numMarinesSpawned < numTroopersToSpawn; numMarinesSpawned++)
        {
            yield return new WaitForSeconds(Globals.UnloadDelay);
            SpawnTrooper();
        }
        yield return new WaitForSeconds(Globals.UnloadDelay);

        _unloaded = true;

        DOTween.To(() => transform.position, x => transform.position = x, _startingPosition, Globals.LanderTravelTime)
            .SetEase(Ease.InOutQuad)
            .onComplete += OnFlewAway;
    }

    private void OnLanded()
    {
        _landed = true;
    }

    private void OnFlewAway()
    {
        
    }

    private void SpawnTrooper()
    {
        Instantiate(TroopPrefab, transform.position, Quaternion.identity);
    }
}
