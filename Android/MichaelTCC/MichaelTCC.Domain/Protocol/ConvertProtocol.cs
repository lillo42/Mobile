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
            sb.Append("S;");
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

            sb.Append(Convert.ToInt32(michaelProtocol.Button1));
            sb.Append(";CR");

            return sb.ToString();
        }

        internal static IDataReceiveProtocol BytesToIDataReceiveProtocol(byte[] data)
        {
            string dataString = Encoding.UTF8.GetString(data, 0, data.Length);
            string[] campos = dataString.Split(';');
            if(campos.Length != 7)
                return new DataReceiveProtocol();
            return new DataReceiveProtocol
            {
                Campo1 = campos[1],
                Campo2 = campos[2],
                Campo3 = campos[3],
                Campo4 = campos[4],
                Campo5 = campos[5],
            };
        }
    }
}