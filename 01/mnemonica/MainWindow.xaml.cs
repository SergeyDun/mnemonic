using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;

using System.Windows;

using System.Windows.Controls;

using System.Windows.Data;

using System.Windows.Documents;

using System.Windows.Input;

using System.Windows.Media;

using System.Windows.Media.Imaging;

using System.Windows.Navigation;

using System.Windows.Shapes;

namespace mnemonica

{

    /// <summary>

    /// Логика взаимодействия для MainWindow.xaml

    /// </summary>

    public partial class MainWindow : Window

    {

        public Mnemonica mnemonica;

        Rectangle[] rectangles;

        Rectangle[] pictures;

        public MainWindow()

        {

            InitializeComponent();

            preparationArea(); //подготовка поля

            Game();

        }

        public void preparationArea()

        {

            rectangles = new Rectangle[20];

            pictures = new Rectangle[20];

            int i = 0;

            while (i < 20)

            {

                for (int y = 0; y < 4; y++)

                {

                    for (int x = 0; x < 5; x++)

                    {

                        pictures[i] = new Rectangle();

                        pictures[i].Name = "p" + i.ToString();

                        pictures[i].Fill = new SolidColorBrush(Colors.YellowGreen);

                        pictures[i].Margin = new Thickness(2);

                        Grid.SetRow(pictures[i], y);

                        Grid.SetColumn(pictures[i], x);

                        grid.Children.Add(pictures[i]);

                        rectangles[i] = new Rectangle();

                        rectangles[i].Name = "r" + i.ToString();

                        rectangles[i].Fill = new SolidColorBrush(Colors.DeepSkyBlue);

                        rectangles[i].Margin = new Thickness(2);

                        rectangles[i].MouseEnter += new MouseEventHandler(mouseEnter);

                        rectangles[i].MouseLeave += new MouseEventHandler(mouseLeave);

                        rectangles[i].MouseLeftButtonUp += new MouseButtonEventHandler(mouseLeftButtonUp);

                        Grid.SetRow(rectangles[i], y);

                        Grid.SetColumn(rectangles[i], x);

                        grid.Children.Add(rectangles[i]);

                        i++;

                    }

                }

            }

        }

        public void Game()

        {

            for (int i = 0; i < 20; i++)

                rectangles[i].Visibility = Visibility.Visible;

            mnemonica = new Mnemonica();

            mnemonica.newGame();

            mnemonica.setPictures(pictures);

        }

        private void mouseEnter(object sender, MouseEventArgs e)

        {

            Rectangle rect = (Rectangle)sender;

            SolidColorBrush blackBrush = new SolidColorBrush(Colors.DarkBlue);

            rect.StrokeThickness = 1;

            rect.Stroke = blackBrush;

        }

        private void mouseLeave(object sender, MouseEventArgs e)

        {

            Rectangle rect = (Rectangle)sender;

            rect.StrokeThickness = 0;

        }

        private void mouseLeftButtonUp(object sender, MouseButtonEventArgs e)

        {

            Rectangle rect = (Rectangle)sender;

            rect.Visibility = Visibility.Hidden;

            int n = Convert.ToInt32(rect.Name.Remove(0, 1));

            switch (mnemonica.Motion(n))

            {

                case 0:

                    mnemonica.motion++;

                    break;

                case 1:

                    if (mnemonica.checkWithPrevious(n))

                    {

                        if (mnemonica.combination < 10)

                            mnemonica.motion = 0;

                        else

                        {

                            congratulations.Visibility = Visibility.Visible;

                            int second, minute;

                            if (DateTime.Now.Second - mnemonica.now.Second < 0)

                                second = mnemonica.now.Second - DateTime.Now.Second;

                            else

                                second = DateTime.Now.Second - mnemonica.now.Second;

                            if (DateTime.Now.Minute - mnemonica.now.Minute < 0)

                                minute = mnemonica.now.Minute - DateTime.Now.Minute;

                            else

                                minute = DateTime.Now.Minute - mnemonica.now.Minute;

                            string min = minute.ToString(); string sec = second.ToString();

                            if (minute < 10)

                            {

                                min = "0" + minute;

                            }

                            if (second < 10)

                                sec = "0" + second;

                            time.Content = min + ":" + sec;

                        }

                    }

                    else

                        mnemonica.motion++;

                    break;

                case 2:

                    System.Threading.Thread.Sleep(200);

                    rectangles[mnemonica.n1].Visibility = Visibility.Visible;

                    rectangles[mnemonica.n2].Visibility = Visibility.Visible;

                    mnemonica.motion = 1;

                    mnemonica.n1 = n;

                    break;

            }

        }

        private void newGame(object sender, KeyEventArgs e)

        {

            congratulations.Visibility = Visibility.Hidden;

            Game();

        }

    }

    public class Mnemonica

    {

        public int[] locationOfImages = new int[20] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public int motion, n1, n2, combination;

        public DateTime now;

        public void newGame()

        {

            combination = 0;

            motion = 0;

            now = DateTime.Now;

            Random random = new Random();

            int n = 10;

            int temp = random.Next(0, 19); ;

            for (int i = 0; i < 20; i++)

                locationOfImages[i] = 0;

            while (n > 0)

            {

                while (locationOfImages[temp] != 0)

                {

                    if (temp < 19)

                        temp++;

                    else

                        temp = 0;

                }

                locationOfImages[temp] = n;

                temp = temp + random.Next(1, 20);

                do

                {

                    if (temp < 19)

                        temp++;

                    else

                        temp = temp - 19;

                } while (locationOfImages[temp] != 0);

                locationOfImages[temp] = n;

                n--;

            }

        }

        public void setPictures(Rectangle[] pictures)

        {

            ImageBrush myBrush;

            for (int i = 0; i < 20; i++)

            {

                myBrush = new ImageBrush();

                BitmapImage bi = new BitmapImage(new Uri("pack://application:,,/Resources/i" + (locationOfImages[i] - 1) + ".jpg"));

                myBrush.ImageSource = bi;

                pictures[i].Fill = myBrush;

            }

        }

        public int Motion(int i)

        {

            switch (motion)

            {

                case 0:

                    n1 = i;

                    break;

                case 1:

                    n2 = i;

                    break;

            }

            return motion;

        }

        public bool checkWithPrevious(int n)

        {

            if (locationOfImages[n1] == locationOfImages[n])

            {

                combination++;

                return true;

            }

            else

                return false;

        }

    }

}