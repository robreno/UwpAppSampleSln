using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Microsoft.Toolkit.Mvvm.Input;
using System.Diagnostics;

namespace UwpSample.ViewModels
{
    /// <summary>
    /// UI properties for our list items.
    /// </summary>
    public class ListItemData
    {
        /// <summary>
        /// Gets and sets the list item content string.
        /// </summary>
        public string ListItemText { get; set; }
        /// <summary>
        /// Gets and sets the list item icon.
        /// </summary>
        public Symbol ListItemIcon { get; set; }
    }

    public class CommandViewModel : ViewModelBase
    {
        // Item collections
        public ObservableCollection<ListItemData> ListItemLeft { get; } =
           new ObservableCollection<ListItemData>();
        public ObservableCollection<ListItemData> ListItemRight { get; } =
           new ObservableCollection<ListItemData>();

        public ListItemData listItem;

        public CommandViewModel()
        {
            MoveLeftCommand = new RelayCommand(new Action(MoveLeft), CanExecuteMoveLeftCommand);
            MoveRightCommand = new RelayCommand(new Action(MoveRight), CanExecuteMoveRightCommand);
            LoadItems();
        }

        public RelayCommand MoveLeftCommand { get; }
        public RelayCommand MoveRightCommand { get; }

        /// <summary>
        ///  Populate our list of items.
        /// </summary>
        public void LoadItems()
        {
            for (var x = 0; x <= 4; x++)
            {
                listItem = new ListItemData();
                listItem.ListItemText = "Item " + (ListItemLeft.Count + 1).ToString();
                listItem.ListItemIcon = Symbol.Emoji;
                ListItemLeft.Add(listItem);
            }
        }


        /// <summary>
        /// Move left command valid when items present in the list on right.
        /// </summary>
        /// <returns>True, if count is greater than 0.</returns>
        private bool CanExecuteMoveLeftCommand()
        {
            return ListItemRight.Count > 0;
        }

        /// <summary>
        /// Move right command valid when items present in the list on left.
        /// </summary>
        /// <returns>True, if count is greater than 0.</returns>
        private bool CanExecuteMoveRightCommand()
        {
            return ListItemLeft.Count > 0;
        }


        /// <summary>
        /// The command implementation to execute when the Move item right button is pressed.
        /// </summary>
        public void MoveRight()
        {
            try
            {
                if (ListItemLeft.Count > 0)
                {
                    listItem = new ListItemData();
                    ListItemRight.Add(listItem);
                    listItem.ListItemText = "Item " + ListItemRight.Count.ToString();
                    listItem.ListItemIcon = Symbol.Emoji;
                    ListItemLeft.RemoveAt(ListItemLeft.Count - 1);

                    OnPropertyChanged(nameof(MoveRight));
                    OnPropertyChanged(nameof(MoveLeft));
                }
                return;
            }
            catch(Exception e)
            {
                Debug.WriteLine("Error!");
            }
            finally
            {
                Debug.WriteLine("Completion!");
            }
        }

        public void MoveLeft()
        {
            try
            {
                if (ListItemRight.Count > 0)
                {
                    listItem = new ListItemData();
                    ListItemLeft.Add(listItem);
                    listItem.ListItemText = "Item " + ListItemLeft.Count.ToString();
                    listItem.ListItemIcon = Symbol.Emoji;
                    ListItemRight.RemoveAt(ListItemRight.Count - 1);

                    OnPropertyChanged(nameof(MoveRight));
                    OnPropertyChanged(nameof(MoveLeft));
                }
                return;
            }
            catch(Exception e)
            {
                Debug.WriteLine("Error!");
            }
            finally
            {
                Debug.WriteLine("Completion!");
            }
        }
    }
}
