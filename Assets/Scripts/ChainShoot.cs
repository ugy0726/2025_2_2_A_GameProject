using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainShoot : MonoBehaviour
{

    [SerializeField] float refreshRate = 0.1f;
    [SerializeField][Range(1, 10)] int maximunEnemieslnChain = 3;
    [SerializeField] float delayBetweenEachChain = 0.5f;
    [SerializeField] Transform playerFirePoint;
    [SerializeField] EmenyDetector playerEmenyDetector;
    [SerializeField] GameObject linRenderPrefab;

    bool shooting;
    bool shot;
    float counter = 1;
    GameObject currentClosestEnemy;

    List<GameObject> spawnedLineRenderers = new List<GameObject>();
    List<GameObject> enemieslnChain = new List<GameObject>();
    List<GameObject> activeEffects = new List<GameObject>();

    IEnumerable ChainReaction(GameObject closestEnemy)
    {
        yield return new WaitForSeconds(delayBetweenEachChain);

        if (counter == maximunEnemieslnChain)
        {
            yield return null;
        }
        else
        {
            if (shooting)
            {
                counter++;
                enemieslnChain.Add(closestEnemy);
                if (!enemieslnChain.Contains(closestEnemy.GetComponent<EmenyDetector>().GetClosestEnemy()))
                {
                    NewLineRenderer(closestEnemy.transform.transform, closestEnemy.GetComponent<EmenyDetector>().GetClosestEnemy().transform);
                    StartCoroutine(ChainReaction(closestEnemy.GetComponent<EmenyDetector>().GetClosestEnemy()));
                }
            }
        }
    }

void NewLineRenderer(Transform startPos, Transform endPos, bool getClosestEnmeyToPlayer = false)
    {
        GameObject lineR = Instantiate(linRenderPrefab);
        spawnedLineRenderers.Add(lineR);
    }

    IEnumerable UpdateLineRenderer(GameObject lineR, Transform startPos, Transform endPos, bool GetClosestEnemyToPlayer = false)
    {
        if(shooting && shoot && lineR != null)
        {
            lineR.GetComponent<LineRendererController>().SetPosition(startPos, endPos);
            yield return new WaitForSeconds(refreshRate);

            if (GetClosestEnemyToPlayer)
            {
                StartCoroutine(UpdateLineRenderer(lineR, startPos, playerEmenyDetector.GetClosestEnemy().transform, true));
                if(currentClosestEnemy !=playerEmenyDetector.GetClosesEnemy())
                {

                }

            }
            else
            {
                StartCorout(UpdateLineRenderer(lineR, startPos, endPos));
            }
        }
    }

}
