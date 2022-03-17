using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;

namespace DyphaeIDE
{
    public partial class MainWindow : Window
    {
        IDictionary<string, string> projectPathDictionary = new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadProjects(object sender, RoutedEventArgs e)
        {
            // Create project path
            string appPath = Environment.ExpandEnvironmentVariables(@"%AppData%\DyphaeIDE");
            if (!Directory.Exists(appPath))
            {
                Directory.CreateDirectory(appPath);
            }
            if (!File.Exists(appPath + @"\projects.txt"))
            {
                using (FileStream fs = File.Create(appPath + @"\projects.txt"))
                {
                    Byte[] title = new UTF8Encoding(true).GetBytes("");
                    fs.Write(title, 0, title.Length);
                }
            }

            string projects = File.ReadAllText(appPath + @"\projects.txt");
            string[] projectList = projects.Split("\n");
            foreach (string project in projectList)
            {
                if (project != "")
                {
                    string projectData = File.ReadAllText(project + @"\dyphae.project");
                    string[] projectDataArray = projectData.Split("\n");
                    string[] projectDataArrayList = new string[4];
                    int counter = 0;
                    foreach (var projectDataPiece in projectDataArray)
                    {
                        string[] splitData = projectDataPiece.Split(":");
                        projectDataArrayList[counter] = splitData[1];
                        counter += 1;
                    }

                    projectPathDictionary.Add(projectDataArrayList[0], project);
                    ProjectList.Items.Add(newProjectButton(projectDataArrayList[0], project));
                }
            }
        }

        Button newProjectButton(String buttonName, String buttonTitle)
        {
            Button button = new Button();
            var bc = new BrushConverter();

            button.Content = buttonName;
            button.Background = Brushes.Black;
            button.Foreground = Brushes.White;
            button.BorderBrush = (Brush)bc.ConvertFrom("#FF252525");
            button.Height = 40;
            button.FontSize = 14;
            button.Click += OpenProject;

            return button;
        }

        private void OpenProject(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Global.currentProjectDir = projectPathDictionary[button.Content.ToString()];

            EditorWindow editorWindow = new EditorWindow();
            editorWindow.Show();

            this.Close();
        }

        private void CreateNewProject(object sender, RoutedEventArgs e)
        {
            NewProjectWindow newProjectWindow = new NewProjectWindow();
            newProjectWindow.Show();
            this.Close();
        }
    }
}
