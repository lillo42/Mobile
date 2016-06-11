using System;
using System.Text;
using MichaelTCC.Infrastructure.Protocol;

namespace MichaelTCC.Domain.Protocol
{
    internal class ConvertProtocol
    {
        internal static string MichaelProtocolToString(IMichaelProtocol michaelProtocol)
        {
            var sb = new StringBuilder();
            sb.Append(";");
            sb.Append(Convert.ToInt32(michaelProtocol.Up));
            sb.Append(";");
            sb.Append(Convert.ToInt32(michaelProtocol.Down));
            sb.Append(";");
            sb.Append(Convert.ToInt32(michaelProtocol.Left));
            sb.Append(";");
            sb.Append(Convert.ToInt32(michaelProtocol.Right));
            sb.Append(";");
            sb.Append(Convert.ToInt32(michaelProtocol.iOS));
            sb.Append(";");
            sb.Append(Convert.ToInt32(michaelProtocol.Triangle));
            sb.Append(";");
            sb.Append(Convert.ToInt32(michaelProtocol.A));
            sb.Append(";");
            sb.Append(Convert.ToInt32(michaelProtocol.X));
            sb.Append(";");

            foreach (float values in michaelProtocol.SensorValues)
            {
                sb.Append(string.Format("{0:0.00}", values));
                sb.Append(";");
            }

            return sb.ToString();
        }
    }
}