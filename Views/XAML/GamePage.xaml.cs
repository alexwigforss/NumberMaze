namespace GridDemos.Views.XAML
{
    public partial class GamePage : ContentPage
    {
        public GamePage()
        {
            InitializeComponent();
            gameGrid.Add(new BoxView
            {
                ZIndex = -1,
                Color = Colors.Blue
            }, 5, 5);
        }
    }
}
