using System;
using SODA;
using Gtk;

namespace Agent
{
    public partial class Interaction : Gtk.Window
    {
        public Interaction() : 
				base(Gtk.WindowType.Toplevel)
        {
            this.Build();
        }
    }
}

