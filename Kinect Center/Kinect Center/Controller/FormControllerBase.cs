using Kinect_Center.Business.Classes;
using Kinect_Center.Business.Enums;
using MyComponents.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect_Center.Controller
{
    public abstract class FormControllerBase
    {
        public ContentForm Form { get; protected set; }

        public T GetView<T>() where T : ContentForm
        {
            return (T)Form;
        }
    }
}
