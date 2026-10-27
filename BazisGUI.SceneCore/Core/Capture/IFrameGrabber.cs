using BazisGUI.SceneCore.Camera;

namespace BazisGUI.SceneCore.Capture
{
    public interface IFrameGrabber
    {
        byte[] Capture(Viewport viewport);
    }
}
