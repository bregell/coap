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

namespace CoAp_Analyzer_GUI
{
    /// <summary>
    /// Interaction logic for MesureList.xaml
    /// </summary>
    public partial class MesureList : UserControl
    {
        public MesureList()
        {
            InitializeComponent();
            this.DataContext = SharedData._workerList;
        }
    }
}
