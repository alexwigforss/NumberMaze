using System.Diagnostics;

namespace GridDemos.Views.XAML
{
    public partial class GamePage : ContentPage
    {
        int nrOfRemNr;
        string heroFileName = "walk_attack2.png";

        List<Pickup> pickups = new List<Pickup>();
        List<Enemy> enemyList = new List<Enemy>();

        Level level;
        Hero hero;
        Enemy enemy;
        Enemy enemy2;
        Enemy enemy3;
        Enemy enemy4;

        public GamePage()
        {
            nrOfRemNr = 0;
            InitializeComponent();
            level = new Level("Template");
            hero = new Hero("namnet", new Vector2D(6, 5), level);
            enemy = new Enemy("fiende", new Vector2D(3, 8), 1, 1, 1, level);
            enemy2 = new Enemy("fiende2", new Vector2D(5, 1), 2, 1, 1, level);
            //enemy3 = new Enemy("fiende3", new Vector2D(4, 1), 3, 1, 1, level);
            //enemy4 = new Enemy("fiende4", new Vector2D(4, 1), 4, 1, 1, level);
            enemyList.Add(enemy);
            enemyList.Add(enemy2);
            //enemyList.Add(enemy3);
            //enemyList.Add(enemy4);
            //enemyList.Add(new Enemy("fiende3", new Vector2D(4,1), 3, 1, 1));
            gameGrid.Add(new Image
            {
                StyleId = "heroImage",
                Source = ImageSource.FromFile(heroFileName),
                ZIndex = 1,
            }, hero.Position.X, hero.Position.Y);
            hero.remIndex = gameGrid.Count - 1;

            foreach (Enemy e in enemyList)
            {
                gameGrid.Add(new Image
                {
                    StyleId = "EnemyImage",
                    Source = ImageSource.FromFile("orc.png"),
                    ZIndex = 1,
                }, e.Position.X, e.Position.Y);
                e.remIndex = gameGrid.Count - 1;
            }

            DrawMap();
            msg.Text = "";

            heropos.Text = $"X:{hero.Position.X},Y:{hero.Position.Y},Strength:{hero.Strength}, Lives:{hero.liv}";
        }

        private async Task gameoverAsync()
        {
            bool answer = await DisplayAlert("Game over!","Would you like to go back to the menu?", "Yes", "Quit");
            if (answer)
            {
                await Navigation.PopAsync();
            }
            else System.Environment.Exit(0);
        }

        private bool DrawMap()
        {
            for (int i = 0; i <= level.BpArray.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= level.BpArray.GetUpperBound(1); j++)
                {
                    if (level.BpArray[j, i] == 'T')
                    {
						gameGrid.Add(new Image
						{
							StyleId = "obstacle",
							Source = ImageSource.FromFile("fruit_tree2.png"),
							ZIndex = 1,
						}, i, j);
						gameGrid.Add(new BoxView
						{
							ZIndex = 0,
							Color = Colors.DarkGreen
						}, i, j);
					}
					else if (level.BpArray[j, i] == 't')
					{
						gameGrid.Add(new Image
						{
							StyleId = "obstacle",
							Source = ImageSource.FromFile("tree1.png"),
							ZIndex = 1,
						}, i, j);
						gameGrid.Add(new BoxView
						{
							ZIndex = 0,
							Color = Colors.DarkGreen
						}, i, j);
					}
					else if (level.BpArray[j, i] == 'R')
					{
						gameGrid.Add(new Image
						{
							StyleId = "obstacle",
							Source = ImageSource.FromFile("ruins1.png"),
							ZIndex = 1,
						}, i, j);
						gameGrid.Add(new BoxView
						{
							ZIndex = 0,
							Color = Colors.DarkGreen
						}, i, j);
					}
					else if (level.BpArray[j, i] == 'W')
					{
						gameGrid.Add(new Image
						{
							StyleId = "obstacle",
							Source = ImageSource.FromFile("wall.png"),
							ZIndex = 1,
						}, i, j);
						gameGrid.Add(new BoxView
						{
							ZIndex = 0,
							Color = Colors.DarkGreen
						}, i, j);
					}
					else if (level.BpArray[j, i] == 'S')
					{
						gameGrid.Add(new Image
						{
							StyleId = "obstacle",
							Source = ImageSource.FromFile("rock.png"),
							ZIndex = 1,
						}, i, j);
						gameGrid.Add(new BoxView
						{
							ZIndex = 0,
							Color = Colors.DarkGreen
						}, i, j);
					}
					else if (level.BpArray[j, i] == 'F')
					{
						gameGrid.Add(new Image
						{
							StyleId = "obstacle",
							Source = ImageSource.FromFile("fern.png"),
							ZIndex = 1,
						}, i, j);
						gameGrid.Add(new BoxView
						{
							ZIndex = 0,
							Color = Colors.DarkGreen
						}, i, j);
					}
					else if (level.BpArray[j, i] == 'b')
					{
						gameGrid.Add(new Image
						{
							StyleId = "obstacle",
							Source = ImageSource.FromFile("bush.png"),
							ZIndex = 1,
						}, i, j);
						gameGrid.Add(new BoxView
						{
							ZIndex = 0,
							Color = Colors.DarkGreen
						}, i, j);
					}
					else if (char.IsNumber(level.BpArray[j, i]))
					{
						gameGrid.Add(new Label
						{
							Text = level.BpArray[j, i].ToString() + "\n" + (gameGrid.Count - 1),
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center,
							ZIndex = 1,
						}, i, j);
                        pickups.Add(new Pickup(level.BpArray[j, i], new Vector2D(i, j), gameGrid.Count-1));
					}
				}
            }
            return true;
        }

        private void Remove()
        {
            gameGrid.RemoveAt(hero.remIndex);
            //if (enemy.remIndex > hero.remIndex) enemy.remIndex -= 1;
            foreach (Pickup pick in pickups)
            {
                if (pick.RemNum > hero.remIndex) pick.LowerIndex();
            }
            //gameGrid.RemoveAt(enemy.remIndex);

            int lastremoved = gameGrid.Count;
            foreach (Enemy e in enemyList)
            {
                if (e.remIndex > hero.remIndex) e.remIndex -= 1;
                if (e.remIndex > lastremoved) e.remIndex -= 1;
                gameGrid.RemoveAt(e.remIndex);
                lastremoved = e.remIndex;
                foreach (Pickup pick in pickups)
                {
                    if (pick.RemNum > e.remIndex) pick.LowerIndex();
                }
            }
        }

       

        private int CollideEnemy()
        {
            int result = 0;
            foreach (Enemy e in enemyList)
            {
                if (hero.Position.X == e.Position.X && hero.Position.Y == e.Position.Y)
                {
                    result = e.Id;
                }
            }
            return result;
        }
        private async Task Lose()
        {
            await DisplayAlert("Fight", "You Lost the fight", "OK");
        }
        private int strid(int enemyid)
        {
            int result = 1;
            foreach (Enemy e in enemyList)
            {
                if (e.Id == enemyid)
                {
                    if (hero.Strength >= e.Strength)
                    {
                        e.dead = true;
                    }
                    else
                    {
                        hero.liv -= 1;
                        heropos.Text = $"X:{hero.Position.X},Y:{hero.Position.Y}, Strength:{hero.Strength}, Lives:{hero.liv}";
                        if (hero.liv < 0)
                        {
                            _ = gameoverAsync();
                        }
                        else
                        {
                            _ = Lose();
                            hero.Position = new Vector2D(6, 5);
                        }
                    }
                }
            }
            return result;
        }

        private void Doit(int dir = -1)
        {
            int stridres = 0;

            if ((dir == 0)&&(heroFileName == "walk_attack2.png"))
                heroFileName = "walk_attack2left.png";
            else if ((dir == 3)&&(heroFileName == "walk_attack2left.png"))
                heroFileName = "walk_attack2.png";


            if (CollideEnemy() != 0) stridres = strid(CollideEnemy());
            
            heropos.Text = $"X:{hero.Position.X},Y:{hero.Position.Y}, Strength:{hero.Strength}, remInd:{hero.remIndex}";
            //heropos.Text = $"X:{hero.Position.X},Y:{hero.Position.Y}, Strength:{hero.Strength}, Lives:{hero.liv}";
                gameGrid.Add(new Image
                {
                    Source = ImageSource.FromFile(heroFileName),
                    ZIndex = 1,
                    //StyleId = "test",
                    //ClassId = "test",
                }, hero.Position.X, hero.Position.Y);

            hero.remIndex = gameGrid.Count - 1;
            //Image element = this.FindByName<Image>("test");
            foreach (Enemy e in enemyList)
            {
                if (!e.dead)
                {
                    e.Move();
                    if (CollideEnemy() != 0) stridres = strid(CollideEnemy());


                    gameGrid.Add(new Image
                    {
                        Source = ImageSource.FromFile("orc.png"),
                        ZIndex = 1,
                        //StyleId = "test",
                        //ClassId = "test",
                    }, e.Position.X, e.Position.Y);
                    e.remIndex = gameGrid.Count - 1;
                }
            }
        }
        private void Button_Left_Clicked(object sender, EventArgs e)
        {
            bool wasNum = false;
            Remove();
            hero.Move(0,out wasNum);
            if (wasNum)
            {
                RemoveNumByPos(hero.Position);
            }
            Doit(0);
        }
        private void Button_Up_Clicked(object sender, EventArgs e)
        {
            bool wasNum = false;
            Remove();
            hero.Move(1, out wasNum);
            if (wasNum)
            {
                RemoveNumByPos(hero.Position);
            }
            Doit();
        }
        private void Button_Down_Clicked(object sender, EventArgs e)
        {
            bool wasNum = false;
            Remove();
            hero.Move(2, out wasNum);
            if (wasNum)
            {
                RemoveNumByPos(hero.Position);
            }
            Doit();
        }
        private void Button_Right_Clicked(object sender, EventArgs e)
        {
            bool wasNum = false;
            Remove();
            hero.Move(3, out wasNum);
            if (wasNum)
            {
                RemoveNumByPos(hero.Position);
            }
            Doit(3);
        }
        public void RemoveNumByPos(Vector2D plpos)
        {
            //int lastRemoved = 0;
            foreach (Pickup p in pickups)
            {
                if ((p.Vector.X == plpos.X) && (p.Vector.Y == plpos.Y))
                {
                    gameGrid.RemoveAt(p.RemNum);
                    foreach (Pickup pick in pickups)
                    {
                        if (pick.RemNum > p.RemNum) pick.LowerIndex();
                    }
                    return;
                }
            }
        }
    }

    public struct Vector2D
    {
        public int X;
        public int Y;

        public Vector2D(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class Actor
    {
        public string Name { set; get; }
        public int remIndex;

        Vector2D position;
        public Vector2D Position
        {
            get { return position; }
            set { position = value; }
        }
        public Actor(string name, Vector2D position)
        {
            Name = name;
            Position = position;
        }

        public void Attack() { }
        public virtual void Move() { }
        public bool CollideWall(Vector2D position, Level level, int direction)
        {
            bool collide = false;
            if (direction == 0 && level.BpArray[position.Y, position.X - 1] != ' ' && !Char.IsNumber(level.BpArray[position.Y, position.X - 1]))
            {
                collide = true;
            }
            if (direction == 1 && level.BpArray[position.Y - 1, position.X] != ' ' && !Char.IsNumber(level.BpArray[position.Y - 1, position.X]))
            {
                collide = true;
            }
            if (direction == 2 && level.BpArray[position.Y + 1,position.X] != ' ' && !Char.IsNumber(level.BpArray[position.Y + 1, position.X]))
            {
                collide = true;
            }
            if (direction == 3 && level.BpArray[position.Y,position.X + 1] != ' ' && !Char.IsNumber(level.BpArray[position.Y, position.X + 1]))
            {
                collide = true;
            }
            return collide;
        }
        public int CollideWallOrNum(Vector2D position, Level level, int direction)
        {
            if (direction == 0 && level.BpArray[position.Y, position.X - 1] != ' ')
            {
                if (char.IsNumber(level.BpArray[position.Y, position.X - 1]))
                {
                    int slask = level.BpArray[position.Y, position.X - 1];
                    return slask - 48;
                }
                return -1;
            }
            if (direction == 1 && level.BpArray[position.Y - 1, position.X] != ' ' )
            {
                if (char.IsNumber(level.BpArray[position.Y - 1, position.X]))
                {
                    int slask = level.BpArray[position.Y - 1, position.X];
                    return slask - 48;
                }
                return -1;
            }
            if (direction == 2 && level.BpArray[position.Y + 1,position.X] != ' ')
            {
                if (char.IsNumber(level.BpArray[position.Y + 1, position.X]))
                {
                    int slask = level.BpArray[position.Y + 1, position.X];
                    return slask - 48;
                }
                return -1;
            }
            if (direction == 3 && level.BpArray[position.Y,position.X + 1] != ' ')
            {
                if (char.IsNumber(level.BpArray[position.Y, position.X + 1]))
                {
                    int slask = level.BpArray[position.Y, position.X + 1];
                    return slask - 48;
                }
                return -1;
            }
            return 0;
        }


    }

    class Hero : Actor
    {
        public List<Pickup> Inventory { set; get; }
        public int Strength { get => strength; set => strength = value; }
        private Level lvl;
        internal Level Lvl { get => lvl; set => lvl = value; }

        static int preDir = 2;
        private int strength;

        public int liv = 3;
        public Hero(string name, Vector2D position, Level lvl) : base(name, position)
        //public Hero(string name, Vector2D position) : base(name, position)
        {
            Inventory = new List<Pickup>();
            Strength = 0;
            Lvl = lvl;
        }

        public void PickupItem(Pickup item)
        {
            Inventory.Add(item);
        }

        public int Move(int direction, out bool wNum)
        {
            wNum = false;
            //FIXME Ge actor tillgång till level och byt ut new level till lvl
            //Left
            if (direction == 0 && Position.X > 0 && CollideWallOrNum(Position, lvl, 0) == 0)
            {
                Position = new Vector2D(Position.X - 1, Position.Y);
            }
            else if(direction == 0 && CollideWallOrNum(Position, lvl, 0) > 0)
            {
                Strength += CollideWallOrNum(Position, lvl, 0);
                Position = new Vector2D(Position.X - 1, Position.Y);
                lvl.BpArray[Position.Y,Position.X] = ' ';
                wNum = true;
            }
            //Up
            if (direction == 1 && Position.Y > 0 && CollideWallOrNum(Position, lvl, 1) == 0)
            {
                Position = new Vector2D(Position.X, Position.Y - 1);
            }
            else if(direction == 1 && CollideWallOrNum(Position, lvl, 1) > 0)
            {
                Strength += CollideWallOrNum(Position, lvl, 1);
                Position = new Vector2D(Position.X, Position.Y - 1);
                lvl.BpArray[Position.Y, Position.X] = ' ';
                wNum = true;
            }
            //Down
            if (direction == 2 && Position.Y < 9 && CollideWallOrNum(Position, lvl, 2) == 0)
            {
                Position = new Vector2D(Position.X, Position.Y + 1);
            }
            else if(direction == 2 && CollideWallOrNum(Position, lvl, 2) > 0)
            {
                Strength += CollideWallOrNum(Position, lvl, 2);
                Position = new Vector2D(Position.X, Position.Y + 1);
                lvl.BpArray[Position.Y, Position.X] = ' ';
                wNum = true;
            }
            //Right
            if (direction == 3 && Position.X < 9 && CollideWallOrNum(Position, lvl, 3) == 0)
            {
                Position = new Vector2D(Position.X + 1, Position.Y);
            }
            else if(direction == 3 && CollideWallOrNum(Position, lvl, 3) > 0)
            {
                Strength += CollideWallOrNum(Position, lvl, 3);
                Position = new Vector2D(Position.X + 1, Position.Y);
                lvl.BpArray[Position.Y, Position.X] = ' ';
                wNum = true;
            }
            return direction;
            //preDir = direction;
        }

        public void CollideEnemy() 
        {
        }
    }

    class Enemy : Actor
    {
        public int Id { set; get; }
        public int Strength { set; get; }
        public int Behaviour { set; get; }
        public int direction;
        public bool dead = false;
        private Level lvl;
        internal Level Lvl { get => lvl; set => lvl = value; }

        public Enum Behaviours;

        public Enemy(string name, Vector2D vector, int id, int str, int behaviour, Level lvl) : base(name, vector)
        {
            Id = id;
            Strength = str;
            Behaviour = behaviour;
            Lvl = lvl;
        }

        public void RemoveSelf() { }

        public void CollideOther() { }

        public override void Move()
        {
            Random rnd = new Random();
            direction = rnd.Next(0,4);
            //Up
            if (direction == 0 && Position.X > 0 && !CollideWall(Position, lvl, 0))
            {
                Position = new Vector2D(Position.X - 1, Position.Y);
            }
            //Up
            else if (direction == 1 && Position.Y > 0 && !CollideWall(Position, lvl, 1))
            {
                Position = new Vector2D(Position.X, Position.Y - 1);
            }
            //Down
            else if (direction == 2 && Position.Y < 9 && !CollideWall(Position, lvl, 2))
            {
                Position = new Vector2D(Position.X, Position.Y + 1);
            }
            //Right
            else if (direction == 3 && Position.X < 9 && !CollideWall(Position, lvl, 3))
            {
                Position = new Vector2D(Position.X + 1, Position.Y);
            }
            else direction += 0;
        }
    }

    class Pickup
    {
        public int Value;
        public Vector2D Vector;
        public int RemNum;

        public Pickup(Vector2D vector)
        {
            Vector = vector;
        }

        /// <summary>
        /// PICKUP
        /// </summary>
        /// <param name="value"></param>
        /// <param name="vector"></param>
        /// <param name="remNum"></param>
        public Pickup(int value, Vector2D vector, int remNum)
        {
            Value = value;
            Vector = vector;
            RemNum = remNum;
        }
        public void LowerIndex()
        {
            RemNum--;
        }
        public string Name { get; }
    }

    //class Number : Pickup
    //{
    //    public Number(int value, Vector2D vector) : base(remNum)
    //    {
    //        Value = value;
    //    }

    //}

    class Operator : Pickup
    {
        public char[] Signs { set; get; }

        public Operator(string name, Vector2D vector, char[] signs) : base(vector)
        {
            Signs = signs;
        }

    }

    class Level
    {
        string blueprint;
        //List<string> bpLines = new List<string>();
        string[] bpLines;
        char[,] bpArray;
        public string Name { set; get; }
        public string Blueprint { get { return blueprint; } set { blueprint = value; } }
        public char[,] BpArray { get => bpArray; set => bpArray = value; }
        static List<string> LevelArray = new List<string>();
        public int Difficulty { set; get; }
        public static int Width { set; get; }
        public static int Height { set; get; }

        //public Level(string name, string blueprint, int difficulty, int width, int height)
        public Level(string name)
        {
            Width = 10;
            Height = 10;
            Name = name;
            bpLines = new string[]
            {
            "T bTFFSFFT",
            "t    2 11T",
            "t  TTRW11F",
            "T  TSTW11T",
            "F  T     F",
            "T       TS",
            "T TR    FT",
            "F FTW  TFF",
            "T   W  bTS",
            "bbTbSSTSST"};
            bpArray = StringArrToCharArr(bpLines);
            Difficulty = 0;
        }
        public Level(string name, string[] bpl)
        {
            Width = 10;
            Height = 10;
            Name = name;
            bpLines = bpl;
            bpArray = StringArrToCharArr(bpLines);
            Difficulty = 0;
        }

        public static char[,] StringArrToCharArr(string[] stringarr)
        {
            // from stringArray TO char 2DcharArray
            char[,] chararr = new char[Width, Height];
            for (int i = 0; i <= chararr.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= chararr.GetUpperBound(1); j++)
                {
                    chararr[i, j] = stringarr[i][j];
                }
                if (i < chararr.GetUpperBound(0)) { }
            }

            return chararr;
        }

        public static string CharArrToString(char[,] chararr)
        {
            // from 2DcharArray BACK TO string
            string landstring = "";
            for (int i = 0; i <= chararr.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= chararr.GetUpperBound(1); j++)
                {
                    landstring += chararr[i, j];
                }
                if (i < chararr.GetUpperBound(0)) landstring += "\n";
            }

            return landstring;
        }

    }
}
