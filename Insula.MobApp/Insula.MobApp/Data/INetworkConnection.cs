using System;
using System.Collections.Generic;
using System.Text;

namespace Insula.MobApp.Data
{
    public interface INetworkConnection
    {
        bool IsConnected { get; }
        void CheckNetWorkConnection();
    }
}
