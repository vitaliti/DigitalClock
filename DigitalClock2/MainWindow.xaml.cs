﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace DigitalClock2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int top = 1;
        int right = 1;
        int left = 1;
        int down = 1;
        string key = "Left";
        string isModified = "false";
        int itemNumber = 0;
        Label itemSelected = null;
        double heigth;
        double width;
        public MainWindow()
        {
            InitializeComponent();

            System.Windows.Threading.DispatcherTimer moveTimer = new System.Windows.Threading.DispatcherTimer();
            moveTimer.Tick += moveTimer_Tick;
            moveTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            moveTimer.Start();

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (isModified == "true")
            {
                //Calculate own time
                int seconds = int.Parse(seconds12.Content.ToString()) + 1;
                if (seconds == 60)
                {
                    seconds12.Content = 0;
                    int minuteFirst = int.Parse(minute1.Content.ToString()) * 10;
                    int minuteSecond = int.Parse(minute2.Content.ToString());
                    int minutes = minuteFirst + minuteSecond + 1;
                    if (minutes == 60)
                    {
                        minute1.Content = 0;
                        minute2.Content = 0;
                        int hourFirst = int.Parse(hour1.Content.ToString()) * 10;
                        int hourSecond = int.Parse(hour2.Content.ToString());
                        int hours = hourFirst + hourSecond + 1;
                        if (hours == 24)
                        {
                            hour1.Content = 0;
                            hour2.Content = 0;
                        }
                        else
                        {
                            hour1.Content = Math.Floor(hours / 10.0);
                            hour2.Content = hours % 10;
                        }
                    }
                    else
                    {
                        minute1.Content = Math.Floor(minutes / 10.0);
                        minute2.Content = minutes % 10;
                    }
                }
                else
                {
                    seconds12.Content = seconds;
                }
            }
            else if (isModified == "false")
            {
                //Get computer time
                string date = DateTime.Now.ToLocalTime().ToString();
                string[] arr = date.Split(' ', ':');
                int hours = int.Parse(arr[2]);
                int minutes = int.Parse(arr[3]);
                int seconds = int.Parse(arr[4]);
                seconds12.Content = seconds;

                minute1.Content = Math.Floor(minutes / 10.0);
                minute2.Content = minutes % 10;
                hour1.Content = Math.Floor(hours / 10.0);
                hour2.Content = hours % 10;
            }
            else
            {
                //stoped
            }

        }

        private void moveTimer_Tick(object sender, EventArgs e)
        {
            switch (key)
            {
                case "Left":
                    itemsGrid.Margin = new Thickness(left, top, right += 1, down);
                    break;
                case "Right":
                    itemsGrid.Margin = new Thickness(left, top, right -= 1, down);
                    break;
                case "Down":
                    itemsGrid.Margin = new Thickness(left, top += 1, right, down);
                    break;
                case "Up":
                    itemsGrid.Margin = new Thickness(left, top -= 1, right, down);
                    break;
                default:
                    /* Stop */
                    break;
            }

            heigth = ActualHeight - 200;
            width = ActualWidth - 220;
            //Checks if clock is out of bounds
            if (right < 0)
            {
                key = "Left";
            }
            else if (right > width)
            {
                key = "Right";
            }
            else if (top < 0)
            {
                key = "Down";
            }
            else if (top > heigth)
            {
                key = "Up";
            }

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //Senses and saves the key that has been pressed
            if (e.Key.ToString() == "Up")
            {
                key = "Up";
            }
            if (e.Key.ToString() == "Down")
            {
                key = "Down";
            }
            if (e.Key.ToString() == "Left")
            {
                key = "Left";
            }
            if (e.Key.ToString() == "Right")
            {
                key = "Right";
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            key = "";
        }

        private void reset_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.isModified = "false";
            hideArrows();
            itemSelected = null;
            itemNumber = 0;
        }

        private void hideArrows()
        {
            arrowh1.Visibility = Visibility.Hidden;
            arrowh2.Visibility = Visibility.Hidden;
            arrowm1.Visibility = Visibility.Hidden;
            arrowm2.Visibility = Visibility.Hidden;
        }

        private void select_MouseDown(object sender, MouseButtonEventArgs e)
        {
            hideArrows();
            this.isModified = "";
            if (itemNumber == 0)
            {
                itemSelected = hour1;
                arrowh1.Visibility = Visibility.Visible;
            }
            else if (itemNumber == 1)
            {
                itemSelected = hour2;
                arrowh2.Visibility = Visibility.Visible;
            }
            else if (itemNumber == 2)
            {
                itemSelected = minute1;
                arrowm1.Visibility = Visibility.Visible;
            }
            else
            {
                itemSelected = minute2;
                arrowm2.Visibility = Visibility.Visible;
                itemNumber = -1;
            }
            itemNumber++;
        }

        private void start_MouseDown(object sender, MouseButtonEventArgs e)
        {
            hideArrows();
            this.isModified = "true";
            itemSelected = null;
            itemNumber = 0;
        }

        private void minus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (itemSelected != null)
            {
                int num = int.Parse(itemSelected.Content.ToString());

                if (num != 0)
                {
                    itemSelected.Content = num - 1;
                }
            }
        }

        private void plus_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (itemSelected != null)
            {
                int num = int.Parse(itemSelected.Content.ToString());
                string name = itemSelected.Name.ToString();
                if (name == "hour1")
                {
                    int hour2Time = int.Parse(hour2.Content.ToString());
                    if (num < 1)
                    {
                        itemSelected.Content = num + 1;
                    }
                    else
                    {
                        if (hour2Time < 4 && num < 2)
                        {
                            itemSelected.Content = num + 1;
                        }
                    }

                }
                else if (name == "hour2")
                {
                    int hour1Time = int.Parse(hour1.Content.ToString());
                    if (hour1Time == 2)
                    {
                        if (num != 3)
                        {
                            itemSelected.Content = num + 1;
                        }
                    }
                    else
                    {
                        if (num != 9)
                        {
                            itemSelected.Content = num + 1;
                        }
                    }
                }
                else if (name == "minute1")
                {
                    if (num != 5)
                    {
                        itemSelected.Content = num + 1;
                    }
                }
                else if (name == "minute2")
                {
                    if (num != 9)
                    {
                        itemSelected.Content = num + 1;
                    }
                }
            }
        }
    }
}