

public interface ITimerProgressBar
{
    void updateProgress(float newProgress);

    void dropProgress();

    void setConfig(float _time);
}
