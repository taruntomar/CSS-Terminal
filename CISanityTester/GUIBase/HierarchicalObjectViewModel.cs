using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace CSSTerminal.GUIBase
{
    public class HierarchicalObjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Tag { get; set; }
        public ContextMenu ContextMenuObj { get; set; }
       
        public ObservableCollection<HierarchicalObjectViewModel> HierarchicalObjects { get; set; }
      

        public HierarchicalObjectViewModel()
        {
            HierarchicalObjects = new ObservableCollection<HierarchicalObjectViewModel>();
            Image = @"..\Resources\Images\Application\TreeNode.bmp";
            
        }
        public IEnumerable Items
        {
            get
            {
                var items = new CompositeCollection();
                items.Add(new CollectionContainer { Collection = HierarchicalObjects });
            
                return items;
            }
        }

        public HierarchicalObjectViewModel AddItem(string name)
        {
            HierarchicalObjectViewModel item = new HierarchicalObjectViewModel();
            item.Name = name;
            HierarchicalObjects.Add(item);
            return item;
        }

        
    }
}
