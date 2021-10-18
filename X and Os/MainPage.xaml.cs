using System;
using System.Diagnostics;

using Xamarin.Forms;

namespace X_and_Os
{
    public partial class MainPage : ContentPage
    {
        const int NumberOfCells = 9;
        const int WidthOfGrid = 3;

        char playersTurn = 'X';

        int[] boardValues = new int[NumberOfCells];

        public MainPage()
        {
            InitializeComponent();
            ResetGame();
        }

        private void ClickedCell(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            int y = (int)button.GetValue(Grid.ColumnProperty);
            int x = (int)button.GetValue(Grid.RowProperty);

            int index = (y * WidthOfGrid) + x;

            if (boardValues[index] == 0)
            {
                Image visual = board.FindByName<Image>("img" + index);
                visual.Source = char.ToLower(playersTurn) + ".png";

                if (playersTurn == 'O')
                {
                    boardValues[index] = 1;
                    CheckForWinner();
                    playersTurn = 'X';
                }
                else
                {
                    boardValues[index] = 2;
                    CheckForWinner();
                    playersTurn = 'O';
                }

                turnLabel.Text = playersTurn + "'s Turn";

            }

        }

        private void CheckForWinner()
        {
            // --> Check rows 012
            // ooo            345
            // -->            678
            for (int i = 0; i < WidthOfGrid; i++)
            {
                int index = i * WidthOfGrid;

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
            for (int i = 0; i < WidthOfGrid; i++)
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
                var img = board.FindByName<Image>("img" + i);
                img.Source = null;
                boardValues[i] = 0;
            }

            playersTurn = 'X';
        }
    }
}
