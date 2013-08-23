using System;
using SODA;
using Gtk;

namespace Agent
{
    public partial class Interaction : Gtk.Window
    {

        private ParticitionEventArgs _interactionEvent;

        public Interaction(ParticitionEventArgs interactionEvent) : 
				base(Gtk.WindowType.Toplevel)
        {
            this.Build();

            _interactionEvent = interactionEvent;         
            this.Title = interactionEvent.Interaction.id + " - " + _interactionEvent.Interaction.customerName;
           


        }


        public void OnParentHidden(object obj,EventArgs args)
        {
            this.Destroy();
        }

    }
}

