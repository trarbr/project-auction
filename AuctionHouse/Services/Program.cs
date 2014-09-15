using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    class Program
    {
        static void Main(string[] args)
        {
            // on the server-side, this needs to be the startup project, and this method has to 
            // set up the server, like in https://github.com/trarbr/3-semester-src/blob/master/SocketOpgave5/ThreadedServer/Program.cs
            AuctionServer server = new AuctionServer(13370);
            server.Run();
        }
    }
}
