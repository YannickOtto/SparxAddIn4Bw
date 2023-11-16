using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace MyAddin.Installer
{
    [RunInstaller(true)]
    public partial class Installer_Addinn : System.Configuration.Install.Installer
    {
        public Installer_Addinn()
        {
            InitializeComponent();
        }
    }
}
