﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TessMVP2.Presenter.Interfaces.View
{
    public interface IPresenterFormFieldControlCallbacks : IPresenter
    {
        void OnBtnCommitClick();
        void OnForm2Closed();
    }
}
