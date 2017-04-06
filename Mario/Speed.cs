using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario
{
    class Speed
    {
        int horizontalSpeed;
        int verticalSpeed;

        void setVerticalSpeed(int v)
        {
            verticalSpeed = v;

        }

        void setHorizontalSpeed(int h)
        {
            horizontalSpeed = h;

        }
        Speed(int h = 0 ,int v = 0)
        {
            verticalSpeed = v;
            horizontalSpeed = h;
        }
    }
}
