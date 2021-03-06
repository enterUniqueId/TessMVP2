﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TessMVP2.Model.Interfaces;
using TessMVP2.Presenter.Interfaces;


namespace TessMVP2.Model.Interfaces
{
    interface IOutlookModel : IModel<IMyPresenterOutlookCallbacks>
    {
        List<string> Hits { get; }
        void GetContacts();
    }
}
