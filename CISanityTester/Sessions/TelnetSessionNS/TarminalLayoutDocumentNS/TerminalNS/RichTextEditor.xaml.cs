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

namespace CISanityTester.Sessions
{
    /// <summary>
    /// Interaction logic for ConsoleTextBox.xaml
    /// </summary>
    public partial class TerminalRichTextEditor :TextBox
    {
        public Border Caret { get; set; }
        public delegate void OnKeyDownDelegate(KeyEventArgs e);
        public event OnKeyDownDelegate KeyDownEvent;
        private int capacity;
        public int MaxCapacity { get; set; }
     
        public TerminalRichTextEditor()
        {
          
            InitializeComponent();
            //ininitalize console textbox
            Focusable = true;
            MaxCapacity = 10000;
            capacity = MaxCapacity;
           
            //ConsoleTextBox.AddHandler(KeyDownEvent, new RoutedEventHandler(HandleHandledKeyDown), true);
            //ConsoleTextBox.TextInput += ConsoleTextBox_TextInput;
        }
        
        
        protected override void OnSelectionChanged(RoutedEventArgs e)
        {
            base.OnSelectionChanged(e);
            MoveCustomCaret();
        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            Caret.Visibility = Visibility.Collapsed;

        }
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            Caret.Visibility = Visibility.Visible;
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            //base.OnKeyDown(e);
            if (KeyDownEvent != null)
                KeyDownEvent(e);
           
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
        }

        public void SetFocus()
        {
            //Keyboard.Focus(this);
           // OnMouseLeftButtonDown(new MouseButtonEventArgs());
            Focus();
            FocusManager.SetFocusedElement(this, this);
            UpdateCaret();
        }

        internal void MoveCustomCaret()
        {
            //
            //this.CaretPosition.GetCharacterRect(LogicalDirection.Forward);
            var caretLocation = GetRectFromCharacterIndex(CaretIndex).Location;

            if (!double.IsInfinity(caretLocation.X))
            {
                Canvas.SetLeft(Caret, caretLocation.X);
            }

            if (!double.IsInfinity(caretLocation.Y))
            {
                Canvas.SetTop(Caret, caretLocation.Y);
            }
        }
        public void UpdateCaret()
        {
            this.Select(Text.Length,0);
            //this.Selection.Select(run.Text.Length,);
            MoveCustomCaret();
        }
        public void Write(string text)
        {
            if (capacity < text.Length)
            {
                Text = Text.Remove(0, (text.Length - capacity));
                capacity += (text.Length - capacity);
            }
            AppendText(text);

            capacity -= text.Length;

            ScrollToEnd();
            UpdateCaret();
            InvalidateVisual();
        }
        public void Write(byte[] bucket, int len)
        {
            if (bucket[0] == 8 || (bucket[0] == 27 && bucket[1] == 91 && bucket[2] == 48 && bucket[3] == 68 && bucket[4] == 27))
                RemoveLastChar();
            else
                Write(Encoding.ASCII.GetString(bucket, 0, len));
        }
    internal void RemoveLastChar()
        {
            
                Text = Text.Remove(Text.Length - 1);
                ScrollToEnd();
                UpdateCaret();
        }
    }
}
