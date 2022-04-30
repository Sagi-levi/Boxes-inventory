using BLService.InnerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLService
{
    public interface ICommunicator
    {
        bool OnQuestion(string message);
        void OnMessage(string message);

    }
}
