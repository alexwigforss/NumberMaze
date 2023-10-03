

namespace _2DGraphicsDrawing;

public class GraphicsDrawable : IDrawable
{
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeColor = Colors.Aqua;
        canvas.StrokeSize = 1;
        canvas.DrawCircle(150, 100, 50);
    }
}