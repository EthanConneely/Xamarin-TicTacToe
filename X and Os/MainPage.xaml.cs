using System;

using Xamarin.Forms;

namespace X_and_Os
{
    public partial class MainPage : ContentPage
    {
        string[] imagePaths = { "empty.png", "o.png", "x.png" };

        const int NumberOfCells = 9;
        const int WidthOfBoard = 3;

        char playersTurn = 'x';

        // 0 = empty
        // 1 = o
        // 2 = x
        int[] boardValues = new int[NumberOfCells];
        ImageButton[] buttons = new ImageButton[NumberOfCells];

        public MainPage()
        {
            InitializeComponent();

            for (int i = 0; i < NumberOfCells; i++)
            {
                buttons[i] = (ImageButton)board.FindByName("cell" + i);
            }

            ResetGame();
        }

        private async void Cell_Clicked(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;

            int y = (int)button.GetValue(Grid.ColumnProperty);
            int x = (int)button.GetValue(Grid.RowProperty);

            int index = (y * WidthOfBoard) + x;


            if (boardValues[index] == 0)
            {
                button.TranslationY = -200;
                buttons[index].Source = playersTurn + ".png";

                if (playersTurn == 'o')
                {
                    boardValues[index] = 1;
                    CheckForWinner();
                    playersTurn = 'x';
                }
                else
                {
                    boardValues[index] = 2;
                    CheckForWinner();
                    playersTurn = 'o';
                }

                turnLabel.Text = playersTurn + "'s Turn";
                await button.TranslateTo(0, 0, 250);
            }

        }

        private void CheckForWinner()
        {
            // --> Check rows 012
            // ooo            345
            // -->            678
            for (int i = 0; i < WidthOfBoard; i++)
            {
                int index = i * WidthOfBoard;

                if (boardValues[index] == boardValues[index + 1] && boardValues[index + 1] == boardValues[index + 2] && boardValues[index] != 0)
                {
                    // We found a winner
                    GameOver(playersTurn);
                    return;
                }
            }

            // |x| Check cols 012
            // |x|            345
            // VxV            678
            for (int i = 0; i < WidthOfBoard; i++)
            {
                int index = i;

                if (boardValues[index] == boardValues[index + 3] && boardValues[index + 3] == boardValues[index + 6] && boardValues[index] != 0)
                {
                    // We found a winner
                    GameOver(playersTurn);
                    return;
                }
            }

            // Check diagonal tl to br
            // x   012
            //  x  345
            //   x 678
            if (boardValues[4] == boardValues[0] && boardValues[0] == boardValues[8] && boardValues[4] != 0)
            {
                // We found a winner
                GameOver(playersTurn);
                return;
            }

            // Check diagonal tr to bl
            //   o 012
            //  o  345
            // o   678
            if (boardValues[2] == boardValues[4] && boardValues[4] == boardValues[6] && boardValues[4] != 0)
            {
                // We found a winner
                GameOver(playersTurn);
                return;
            }

            // Check for tie
            for (int i = 0; i < NumberOfCells; i++)
            {
                if (boardValues[i] == 0)
                {
                    return;
                }
                else
                {
                    if (i == NumberOfCells - 1)
                    {
                        TieGame();
                    }
                }
            }
        }

        async void GameOver(char player)
        {
            await DisplayAlert(player + " has Won!", "Do you wanna play another?", "New Game");
            ResetGame();
        }

        async void TieGame()
        {
            await DisplayAlert("The game was a tie!", "Do you wanna play another?", "New Game");
            ResetGame();
        }

        void ResetGame()
        {
            for (int i = 0; i < NumberOfCells; i++)
            {
                boardValues[i] = 0;
                int index = boardValues[i];
                buttons[i].Source = imagePaths[index];
            }

            playersTurn = 'x';
        }
    }
}
