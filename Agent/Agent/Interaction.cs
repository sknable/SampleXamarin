using System;
using SODA;
using Gtk;

namespace Agent
{
    public partial class Interaction : Gtk.Window
    {

        private ParticitionEventArgs _interactionEvent;
        private SODAClient _agent;

        public Interaction(ParticitionEventArgs interactionEvent,SODAClient agent) : 
				base(Gtk.WindowType.Toplevel)
        {
            this.Build();

            _agent = agent;
            _interactionEvent = interactionEvent;         
            this.Title = interactionEvent.Interaction.id + " - " + _interactionEvent.Interaction.customerName;
           
        }


        public void OnParentHidden(object obj,EventArgs args)
        {
            this.Destroy();
        }

        protected void OnFinishPress (object obj, EventArgs args)
        {
            _agent.CIM.getWrapupCodes(_interactionEvent.Interaction.id);
            _agent.CIM.finishParticipation(_interactionEvent.Interaction.id, new string[] {"Done"});
        }
    }
}

