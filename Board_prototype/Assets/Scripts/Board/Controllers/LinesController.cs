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
        RenderLineSignal line = signal;
        do
        {
            GameObject newLine = Instantiate(linePrefab);
            LineRenderer renderer = newLine.GetComponent<LineRenderer>();

            renderer.SetPositions(line.points);
            lines.Add(newLine);

            line = signal.nextLine;

        } while (line != null);
    }

    private void clear()
    {
        do
        {
            Destroy(lines[0]);
            lines.Remove(lines[0]);
        } while (lines.Count > 0);
    }


}
