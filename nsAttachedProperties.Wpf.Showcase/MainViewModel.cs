/* ==============================
** Copyright 2022 nishy software
**
**      First Author : nishy software
**		Create : 2022/02/25
** ============================== */

namespace NishySoftware.Wpf.AttachedProperties.Showcase
{
    using Prism.Mvvm;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MainViewModel : BindableBase
    {
        #region EditBoxValue
        string _editBoxBalue;
        public string EditBoxValue
        {
            get { return this._editBoxBalue; }
            set { this.SetProperty(ref this._editBoxBalue, value); }
        }
        #endregion

        #region EditBoxValue1
        string _editBoxBalue1;
        public string EditBoxValue1
        {
            get { return this._editBoxBalue1; }
            set { this.SetProperty(ref this._editBoxBalue1, value); }
        }
        #endregion

        #region EditBoxValue2
        string _editBoxBalue2;
        public string EditBoxValue2
        {
            get { return this._editBoxBalue2; }
            set { this.SetProperty(ref this._editBoxBalue2, value); }
        }
        #endregion

        #region EditBoxValue3
        string _editBoxBalue3;
        public string EditBoxValue3
        {
            get { return this._editBoxBalue3; }
            set { this.SetProperty(ref this._editBoxBalue3, value); }
        }
        #endregion

        #region EditBoxValue4
        string _editBoxBalue4;
        public string EditBoxValue4
        {
            get { return this._editBoxBalue4; }
            set { this.SetProperty(ref this._editBoxBalue4, value); }
        }
        #endregion

        #region EditBoxValue5
        string _editBoxBalue5;
        public string EditBoxValue5
        {
            get { return this._editBoxBalue5; }
            set { this.SetProperty(ref this._editBoxBalue5, value); }
        }
        #endregion

        #region EditBoxValue6
        string _editBoxBalue6;
        public string EditBoxValue6
        {
            get { return this._editBoxBalue6; }
            set { this.SetProperty(ref this._editBoxBalue6, value); }
        }
        #endregion
    }
}
