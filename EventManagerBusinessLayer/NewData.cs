using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Business
{
    public class NewData
    {
        private string headlineValue;
        private string supportValue;
        private string openerValue;

        public string Headline
        {
            get { return headlineValue; }
            set { headlineValue = value; }
        }
        public string Support
        {
            get { return supportValue; }
            set { supportValue = value; }
        }
        public string Opener
        {
            get { return openerValue; }
            set { openerValue = value; }
        }

    }
}
