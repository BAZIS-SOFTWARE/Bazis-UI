using SceneCore.Camera;

namespace SceneCore.Capture
{
    public interface IFrameGrabber
    {
        byte[] Capture(Viewport viewport);
    }
}
