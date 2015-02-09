using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GP3_Project
{
    class MenuItem
    {
        public delegate void OnSelectEvent();

        public Texture2D normalState;
        public Texture2D selectedState;
        public OnSelectEvent onSelectEvent;
        public bool isSelected;

        public MenuItem(Texture2D normalState, Texture2D selectedState, OnSelectEvent onSelectEvent)
        {
            this.normalState = normalState;
            this.selectedState = selectedState;
            this.onSelectEvent = onSelectEvent;
            isSelected = false;
        }
    }
}
