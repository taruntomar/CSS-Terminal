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

namespace CISanityTester.GUIBase
{
    /// <summary>
    /// Interaction logic for ObjectTreeExplorer.xaml
    /// </summary>
    public partial class ObjectTreeExplorer : UserControl
    {
        
        List<HierarchicalObjectViewModel> TreeItemList;
        public ObjectTreeExplorer()
        {
            InitializeComponent();
            
            LoadTree();
        }
        public HierarchicalObjectViewModel AddTreeItem(string name)
        {
            HierarchicalObjectViewModel treeitem = new HierarchicalObjectViewModel();
            treeitem.Name = name;
            TreeItemList.Add(treeitem);

            return treeitem;
        }
        public bool DeleteTreeItem(string name)
        {
            var x = from item in TreeItemList where item.Name == name select item;
            if (x.Count() > 0)
            {
                TreeItemList.Remove(x.First<HierarchicalObjectViewModel>());
                return true;
            }
            
                return false;

        }
        protected void LoadTree()
        {
            ObjectTree.ItemsSource = null;
            TreeItemList = new List<HierarchicalObjectViewModel>();
            ObjectTree.ItemsSource = TreeItemList;

        }
        protected void ReloadTree()
        {
            ObjectTree.ItemsSource = null;
            ObjectTree.ItemsSource = TreeItemList;
        }

        protected virtual void TreeObjectRightClick(object sender, MouseButtonEventArgs e)
        {
            ((TreeViewItem)sender).IsSelected = true;
            //((HierarchicalObjectViewModel)sender).IsSelected = true;
        }


        protected virtual void Toolbar_Add_Button_Click(object sender, RoutedEventArgs e) { }
      

        protected virtual void Toolbar_Properties_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        protected virtual void Toolbar_Delete_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
