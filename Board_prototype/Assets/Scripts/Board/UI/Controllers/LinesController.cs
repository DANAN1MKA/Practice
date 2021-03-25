using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class LinesController : MonoBehaviour
{
    [Inject] SignalBus signalBus;
    [Inject] BoardProperties config;

    private GameObject linePrefab;

    private List<GameObject> lines;

    private void Awake()
    {
        signalBus.Subscribe<TimerHandlerSignal>(clear);
        signalBus.Subscribe<RenderLineSignal>(render);

        linePrefab = config.linePrefab;
        lines = new List<GameObject>();
    }

    private void render(RenderLineSignal signal)
    {
        LiensList line = signal.list;
        do
        {
            GameObject newLine = Instantiate(linePrefab);
            LineRenderer renderer = newLine.GetComponent<LineRenderer>();

            renderer.SetPositions(line.points);
            renderer.material = config.pool[line.type];

            lines.Add(newLine);

            line = line.nextLine;

        } while (line != null);
    }

    private void clear()
    {
        foreach(GameObject curr in lines)
        {
            Destroy(curr);
        }

        lines.Clear();
    }


}
