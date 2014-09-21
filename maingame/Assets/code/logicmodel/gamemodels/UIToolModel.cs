using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class UIToolModel : IUIToolModel
    {


        public void Update(float delta)
        {
          
        }

        public void BeginInit()
        {
            inited = true;
        }

        public bool inited
        {
            get;
            private set;
        }

        public void BeginExit()
        {
            exited = true;
        }

        public bool exited
        {
            get;
            private set;
        }
    }
