using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CISanityTester.Sessions
{
    public class BufferString
    {
        private StringBuilder buffer;
        public int MaxCapacity { get; set; }        
        public int Capacity { get; set; }
        public int Length { get; set; }
        public string Text
        {
            get { return buffer.ToString(); }
        }
     
        
        public BufferString(int maxcapacity=10000)
        {
            buffer = new StringBuilder();
            MaxCapacity = maxcapacity;
            Capacity = maxcapacity;
            Length = 0;
        }

        public void Append(string str)
        {
            //case1: 10, 2
            // case2: 8,3
            
            if(str.Length >= Capacity)
            TrimBeginning(str.Length -Capacity);

            buffer.Append(str);
            Capacity -= str.Length;

     
        }

        public void Append(byte[] bucket, int len)
        {
            if (bucket[0] == 8 || (bucket[0] == 27 && bucket[1] == 91 && bucket[2] == 48 && bucket[3] == 68 && bucket[4] == 27))
                DeleteLastChar();
            else
                Append(Encoding.ASCII.GetString(bucket, 0, len));
        }

        private void TrimBeginning(int length)
        {
            buffer.Remove(0, length);
            Capacity += length;
        }

        public void DeleteLastChar()
        {
            if (Length == 0)
                return;
            buffer.Remove(Length - 1, 1);
            Capacity++; 
        }

        public void Clear()
        {
            buffer.Clear();
            Capacity = MaxCapacity;
        }

    }
}
