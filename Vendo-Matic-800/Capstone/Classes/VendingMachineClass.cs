using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class VendingMachineClass
    {
        UI userInterface = new UI();        
        public void Run()
        {           
            userInterface.Prompt();
        }
    }
}
