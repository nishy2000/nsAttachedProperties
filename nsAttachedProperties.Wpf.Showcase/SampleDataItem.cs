/* ==============================
** Copyright 2021, 2022 nishy software
**
**      First Author : nishy software
**		Create : 2021/12/07
** ============================== */

namespace NishySoftware.Wpf.AttachedProperties.Showcase
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Prism.Mvvm;

    public class SampleDataItem : BindableBase
    {
        int _id;
        public int Id
        {
            get { return this._id; }
            set { this.SetProperty(ref this._id, value); }
        }

        string _name;
        public string Name
        {
            get { return this._name; }
            set { this.SetProperty(ref this._name, value); }
        }

        string _description;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        bool? _active;
        public bool? Active
        {
            get { return this._active; }
            set { this.SetProperty(ref this._active, value); }
        }

        string _selected;
        public string Selected
        {
            get { return this._selected; }
            set { this.SetProperty(ref this._selected, value); }
        }
    }
}
