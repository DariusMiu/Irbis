struct SmoothFramerate
{
    int samples;
    int currentFrame;
    double[] frametimes;
    double currentFrametimes;

    public double Framerate
    {
        get
        {
            return (samples / currentFrametimes);
        }
    }

    public SmoothFramerate(int Samples)
    {
        samples = Samples;
        currentFrame = 0;
        frametimes = new double[samples];
        currentFrametimes = 0d;
    }

    public void Update(double timeSinceLastFrame)
    {
        currentFrame++;
        if (currentFrame >= frametimes.Length) { currentFrame = 0; }

        currentFrametimes -= frametimes[currentFrame];
        frametimes[currentFrame] = timeSinceLastFrame;
        currentFrametimes += frametimes[currentFrame];
    }
}
