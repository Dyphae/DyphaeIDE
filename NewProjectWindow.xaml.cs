using System;
using System.Collections.Generic;
using System.IO;
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

namespace DyphaeIDE
{
    public partial class NewProjectWindow : Window
    {
        static string MCversion = "1.18.2";

        public NewProjectWindow()
        {
            InitializeComponent();
        }

        private void ChangeMinecraftVersion(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            MCversion = (string)button.Content;

            MCVersionLabel.Content = "Minecraft Version: " + MCversion;
        }

        private void CreateNewProject(object sender, RoutedEventArgs e)
        {
            DyphaeLanguage.Program.RunArgs(new string[] { "new", ProjectName.Text, Description.Text, Namespace.Text, MCversion, ProjectDirectory.Text });

            string appPath = Environment.ExpandEnvironmentVariables(@"%AppData%\DyphaeIDE");
            string projects = File.ReadAllText(appPath + @"\projects.txt");
            using (FileStream fs = File.Create(appPath + @"\projects.txt"))
            {
                Byte[] title = new UTF8Encoding(true).GetBytes(projects + ProjectDirectory.Text + @"\" + ProjectName.Text + "\n");
                fs.Write(title, 0, title.Length);
            }

            Global.currentProjectDir = ProjectDirectory.Text + @"\" + ProjectName.Text;
            EditorWindow editorWindow = new EditorWindow();
            editorWindow.Show();

            this.Close();
        }
    }
}
