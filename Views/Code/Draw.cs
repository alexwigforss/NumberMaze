namespace _2DGraphicsDrawing;

public class TestDrawable : IDrawable
{
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeColor = Colors.Aqua;
        canvas.StrokeSize = 1;
        canvas.DrawCircle(150, 100, 50);
        canvas.DrawRoundedRectangle(200, 200, 100, 100, 25);
    }
}