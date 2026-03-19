using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rekenspel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random random = new Random();
        private int currentAnswer;
        private int score = 0;
        private int currentHighScore = 0;
        private int levens = 3;
        private string highScoreFile = "highscore.txt";

        public MainWindow()
        {
            InitializeComponent();
            LoadHighScore();
            GenerateNewSum();
        }

        private void LoadHighScore()
        {
            try
            {
                if (System.IO.File.Exists(highScoreFile))
                {
                    string text = System.IO.File.ReadAllText(highScoreFile);
                    if (int.TryParse(text, out int savedHighScore))
                    {
                        currentHighScore = savedHighScore;
                        HighScoreTextBlock.Text = $"High Score: {currentHighScore}";
                    }
                }
            }
            catch { }
        }

        private void SaveHighScore()
        {
            try
            {
                System.IO.File.WriteAllText(highScoreFile, currentHighScore.ToString());
            }
            catch { }
        }

        private void GenerateNewSum()
        {
            // Kies willekeurige operatie
            string[] operations = { "+", "-", "*", "/" };
            string operation = operations[random.Next(operations.Length)];

            int num1, num2;

            switch (operation)
            {
                case "+":
                    num1 = random.Next(1, 101); // 1-100
                    num2 = random.Next(1, 101);
                      currentAnswer = num1 + num2;
                    break;
                case "-":
                    num1 = random.Next(50, 151); // 50-150
                    num2 = random.Next(1, num1 + 1);
                    currentAnswer = num1 - num2;
                    break;
                case "*":
                    num1 = random.Next(1, 11); // 1-10
                    num2 = random.Next(1, 11);
                    currentAnswer = num1 * num2;
                    break;
                case "/":
                    num2 = random.Next(1, 11); // 1-10
                    currentAnswer = random.Next(1, 11); // 1-10
                    num1 = currentAnswer * num2; // Zorg voor geheel getal
                    break;
                default:
                    num1 = 0;
                    num2 = 0;
                    currentAnswer = 0;
                    break;
            }

            SomBlock.Text = $"{num1} {operation} {num2} = ?";
            AnswerTextBox.Text = "";
            FeedbackTextBlock.Visibility = Visibility.Collapsed;
        }

        private void CheckAntwoord_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(AnswerTextBox.Text, out int userAnswer))
            {
                if (userAnswer == currentAnswer)
                {
                    score++;
                    if (score > currentHighScore)
                    {
                        currentHighScore = score;
                        HighScoreTextBlock.Text = $"High Score: {currentHighScore}";
                        SaveHighScore();
                    }
                    ScoreTextBlock.Text = $"Score {score}";
                    MainGrid.Background = Brushes.Green;
                    GenerateNewSum();
                }
                else
                {
                    levens--;
                    LevensTextBlock.Text = $"❤️Levens {levens}";
                    MainGrid.Background = Brushes.Red;


                    if (levens <= 0)
                    {
                        if (score > currentHighScore)
                        {
                            currentHighScore = score;
                            HighScoreTextBlock.Text = $"High Score: {currentHighScore}";
                            SaveHighScore();
                        }

                        MessageBox.Show($"Game Over! Je behaalde score is: {score}\nDe huidige High Score is: {currentHighScore}", "Game Over");
                      
                        score = 0;
                        levens = 3;
                        MainGrid.Background = Brushes.LightGoldenrodYellow;
                        ScoreTextBlock.Text = $"Score {score}";
                        LevensTextBlock.Text = $"❤️Levens {levens}";
                        GenerateNewSum();
                    }
                }
            }
            else
            {
                FeedbackTextBlock.Text = "Voer een geldig getal in.";
                FeedbackTextBlock.Foreground = Brushes.Orange;
                FeedbackTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Background = Brushes.LightGoldenrodYellow;
            GenerateNewSum();
        }
    }
}