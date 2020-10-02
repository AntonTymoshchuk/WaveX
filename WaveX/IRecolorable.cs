using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WaveX
{
    public interface IRecolorable
    {
        void RecolorTo(Color Color);
        void SmoothRedrawing();
    }
}
