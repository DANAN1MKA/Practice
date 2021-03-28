using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LinesController : MonoBehaviour
{
    [Inject] SignalBus signalBus;
    [Inject] BoardProperties config;

    private GameObject linePrefab;

    private List<GameObject> lines;
    private LiensList pointsList;

    private void Awake()
    {
        signalBus.Subscribe<TimerHandlerSignal>(clear);
        signalBus.Subscribe<RenderLineSignal>(getPoints);
        signalBus.Subscribe<AnimationCompletedSignal>(render);

        linePrefab = config.linePrefab;
        lines = new List<GameObject>();
    }

    private void getPoints(RenderLineSignal signal)
    {
        pointsList = signal.list;
    }

    private void render()
    {
        while (pointsList != null)
        {
            GameObject newLine = Instantiate(linePrefab);
            LineRenderer renderer = newLine.GetComponent<LineRenderer>();

            renderer.SetPositions(pointsList.points);
            //renderer.material = config.pool[pointsList.type];

            lines.Add(newLine);

            pointsList = pointsList.nextLine;
        } 
    }

    private void clear()
    {
        foreach(GameObject curr in lines)
        {
            Destroy(curr);
        }

        lines.Clear();
        pointsList = null;
    }


}
