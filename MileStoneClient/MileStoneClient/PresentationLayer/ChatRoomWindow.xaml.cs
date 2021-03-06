﻿using System;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using MileStoneClient.BusinessLayer;
using MileStoneClient.Logger;

namespace MileStoneClient.PresentationLayer
{
    /// <summary>
    /// Interaction logic for ChatRoom.xaml
    /// </summary>
    public partial class ChatRoomWindow : Window
    {
        private const int msgLength = 100;
        private ChatRoom chatRoom;
        private MainWindow mainWindow;
        private ObservableObject obs;
        private bool isOptionsVisible;
        private Options op;
        private string orderChoice, filterChoice, sortChoice;
        private Action sortAction;
        private string[] filterInfo;
        private int order;
        private List<GuiMessage> msgs;
        private List<string> groups;
        private DispatcherTimer dispatcherTimer;
        private ListBox listBox;
        private bool showUpdateWindow;

        public ChatRoomWindow(MainWindow mainWindow, ChatRoom chatRoom, ObservableObject obs)
        {
            Log.Instance.info("ChatRoom window opened"); //log
            this.obs = obs;
            InitializeComponent();
            this.chatRoom = chatRoom;
            this.mainWindow = mainWindow;
            this.DataContext = obs;
            groups = chatRoom.getGroups();
            op = new Options(this, groups, obs);
            sortAction = new SortByTime();
            filterInfo = new string[3];
            filterInfo[0] = "NONE";
            filterInfo[1] = "";
            filterInfo[2] = "";
            msgs = new List<GuiMessage>();
            isOptionsVisible = false;
            orderChoice = "ascending";
            filterChoice = "none";
            sortChoice = "time";
            obs.BtnSendIsEnabled = false;

            //initiate timer
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 2);
            dispatcherTimer.Start();
        }

        /// <summary>
        /// print messages with timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //if another sort/filter/order was chosen
            if (op.IsChanged & op.IsLegalData)
            {
                Log.Instance.info("Option for filter/sort/order changed");

                orderChoice = op.OrderChoice;
                filterChoice = op.FilterChoice;
                sortChoice = op.SortChoice;
                op.IsChanged = false;
                filterInfo = new string[3];
                filterInfo[0] = "";
                filterInfo[1] = "";
                filterInfo[2] = "";

                //list == order,sort,filter
                switch (orderChoice)
                {
                    case "ascending":
                        order = 0;
                        break;
                    case "decending":
                        order = 1;
                        break;
                }

                // adds the choosen sort to the action list
                switch (sortChoice)
                {
                    case "name":
                        sortAction = new SortByName();
                        break;
                    case "all":
                        sortAction = new SortByGNT();
                        break;
                    case "time":
                        sortAction = new SortByTime();
                        break;
                }
                // adds the choosen filter to the action list
                switch (filterChoice)
                {
                    case "none":
                        filterInfo[0] = "NONE";
                        break;
                    case "group":
                        if (op.GroupChoice != null)
                        {
                            filterInfo[0] = "ByGroup";
                            filterInfo[1] = op.GroupChoice;
                        }
                        break;
                    case "user":
                        if (op.UserChoice != null && op.GroupChoice != null)
                        {
                            filterInfo[0] = "ByUser";
                            filterInfo[1] = op.GroupChoice;
                            filterInfo[2] = op.UserChoice;
                        }
                        break;
                }
                if (op.UserChoice != null && op.GroupChoice != null)
                    getMessagesList();
            }
            else
                getMessagesList();
            // check if there are messages in the current filter, if there isn't display a message
            if (msgs.Count == 0 & op.Ok)
            {
                MessageBox.Show("There are no messages");
                Log.Instance.info("No messages to display");
                op.Ok = false;
            }
        }

        /// <summary>
        /// enables the user to log out of the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogOut(object sender, RoutedEventArgs e)
        {
            Log.Instance.info("User logged out"); //log
            this.chatRoom.logOut();
            Close();
            this.mainWindow.init();
            this.mainWindow.Show();
        }

        /// <summary>
        /// enables the user to send message 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Send(object sender, RoutedEventArgs e)
        {
            bool isLegalMessage = obs.TxtSendContent.Length <= msgLength;

            if (!isLegalMessage)
            {
                MessageBox.Show("Message length should be 100 letters or less", "Invalid message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (obs.TxtSendContent.Equals("") | isMsgOnlySpaces())
            {
                MessageBox.Show("Message cannot be empty", "Invalid message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                chatRoom.send(obs.TxtSendContent);
                obs.TxtSendContent = "";
                Log.Instance.info("Message sent");
            }
        }

        /// <summary>
        /// enables the user to exit program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit(object sender, RoutedEventArgs e)
        {
            Log.Instance.info("User logged out and exited program");
            chatRoom.logOut();
            System.Environment.Exit(0);
        }

        /// <summary>
        /// enables the user to exit program open options menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Options(object sender, RoutedEventArgs e)
        {
            //close options menu
            if (isOptionsVisible)
            {
                obs.IsOptionVisible = null;
                isOptionsVisible = false;
            }
            //open options menu
            else
            {
                obs.IsOptionVisible = op;
                isOptionsVisible = true;
            }
        }

        /// <summary>
        /// if the selection changed in the list box - allow to the current user to edit his message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Message message;
            int index = obs.ListBoxSelectedIndex;
            if (showUpdateWindow)
            {
                if ((msgs.Count > 0 & index >= 0) && (msgs[index].UserName.Equals(chatRoom.CurrUser.Nickname) & msgs[index].G_id.Equals(chatRoom.CurrUser.G_id)))
                {
                    message = new Message(obs, chatRoom, msgs[index]);
                    message.Show();
                }
            }

        }

        /// <summary>
        /// initiate listBox wuth the messages list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateMessages(object sender, RoutedEventArgs e)
        {
            msgs = chatRoom.getMessages(0, sortAction, filterInfo);
            for (int i = 0; i < msgs.Count; i++)
            {
                showUpdateWindow = false;
                obs.Messages.Add(msgs[i].toString());
            }

            ListBox msgList = sender as ListBox;
            listBox = msgList;
            msgList.Items.MoveCurrentToLast();
            msgList.ScrollIntoView(msgList.Items.CurrentItem);
            showUpdateWindow = true;
        }

        /// <summary>
        /// return list of the users in a given group
        /// </summary>
        /// <param name="g_id"></param>
        /// <returns></returns>
        public List<string> getMembersOf(string g_id)
        {
            if (g_id != null)
                return chatRoom.getMembersOf(g_id);
            return null;
        }

        /// <summary>
        /// display all the messages
        /// </summary>
        private void getMessagesList()
        {
            obs.Messages.Clear();
            msgs = chatRoom.getMessages(order, sortAction, filterInfo);
            // convers all the Gui Messages to a string
            for (int i = 0; i < msgs.Count; i++)
            {
                obs.Messages.Add(msgs[i].toString());
            }
        }


        /// <summary>
        /// allows the user to send a message by pressing enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Send(sender, e);
        }

        public List<GuiMessage> Msgs
        {
            get { return msgs; }
        }

        /// <summary>
        /// if the message that sent contains only spaces
        /// </summary>
        /// <returns></returns>        
        private bool isMsgOnlySpaces()
        {
            bool ans = true;
            string tMsg = obs.TxtSendContent;

            for (int i = 0; i < tMsg.Length; i++)
                if (tMsg[i] != ' ')
                    ans = false;
            return ans;
        }
    }
}



