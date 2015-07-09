using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime;
using System.Text;
using System.Threading;

namespace ParallelTestRunner.Process2
{
    internal delegate void UserCallBack(string data);

    [SuppressMessage("StyleCop.CSharp.NamingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    internal class AsyncStreamReader : IDisposable
    {
        internal const int DefaultBufferSize = 1024;

        private const int MinBufferSize = 128;
        
        private Stream stream;
        
        private Encoding encoding;
        
        private Decoder decoder;
        
        private byte[] byteBuffer;
        
        private char[] charBuffer;
        
        private int maxCharsPerBuffer;
        
        private Process2 process;
        
        private UserCallBack userCallBack;
        
        private bool cancelOperation;
        
        private ManualResetEvent eofEvent;
        
        private Queue messageQueue;
        
        private StringBuilder sb;
        
        private bool lastCarriageReturn;
        
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        internal AsyncStreamReader(Process2 process, Stream stream, UserCallBack callback, Encoding encoding)
            : this(process, stream, callback, encoding, 1024)
        {
        }

        internal AsyncStreamReader(Process2 process, Stream stream, UserCallBack callback, Encoding encoding, int bufferSize)
        {
            Init(process, stream, callback, encoding, bufferSize);
            messageQueue = new Queue();
        }

        public virtual Encoding CurrentEncoding
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return encoding;
            }
        }

        public virtual Stream BaseStream
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            get
            {
                return stream;
            }
        }

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public virtual void Close()
        {
            Dispose(true);
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal void BeginReadLine()
        {
            if (cancelOperation)
            {
                cancelOperation = false;
            }

            if (sb == null)
            {
                sb = new StringBuilder(1024);
                stream.BeginRead(byteBuffer, 0, byteBuffer.Length, new AsyncCallback(ReadBuffer), null);
                return;
            }

            FlushMessageQueue();
        }

        internal void CancelOperation()
        {
            cancelOperation = true;
        }

        internal void WaitUtilEOF()
        {
            if (eofEvent != null)
            {
                eofEvent.WaitOne();
                eofEvent.Close();
                eofEvent = null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && stream != null)
            {
                stream.Close();
            }

            if (stream != null)
            {
                stream = null;
                encoding = null;
                decoder = null;
                byteBuffer = null;
                charBuffer = null;
            }

            if (eofEvent != null)
            {
                eofEvent.Close();
                eofEvent = null;
            }
        }
        
        private void Init(Process2 process, Stream stream, UserCallBack callback, Encoding encoding, int bufferSize)
        {
            this.process = process;
            this.stream = stream;
            this.encoding = encoding;
            userCallBack = callback;
            decoder = encoding.GetDecoder();

            if (bufferSize < 128)
            {
                bufferSize = 128;
            }

            byteBuffer = new byte[bufferSize];
            maxCharsPerBuffer = encoding.GetMaxCharCount(bufferSize);
            charBuffer = new char[maxCharsPerBuffer];
            cancelOperation = false;
            eofEvent = new ManualResetEvent(false);
            sb = null;
            lastCarriageReturn = false;
        }

        private void ReadBuffer(IAsyncResult ar)
        {
            int num;
            try
            {
                num = stream.EndRead(ar);
            }
            catch (IOException)
            {
                num = 0;
            }
            catch (OperationCanceledException)
            {
                num = 0;
            }

            if (num == 0)
            {
                lock (messageQueue)
                {
                    if (sb.Length != 0)
                    {
                        messageQueue.Enqueue(sb.ToString());
                        sb.Length = 0;
                    }

                    messageQueue.Enqueue(null);
                }

                try
                {
                    FlushMessageQueue();
                    return;
                }
                finally
                {
                    eofEvent.Set();
                }
            }

            int chars = decoder.GetChars(byteBuffer, 0, num, charBuffer, 0);
            sb.Append(charBuffer, 0, chars);
            GetLinesFromStringBuilder();
            stream.BeginRead(byteBuffer, 0, byteBuffer.Length, new AsyncCallback(ReadBuffer), null);
        }
        
        private void GetLinesFromStringBuilder()
        {
            int i = 0;
            int num = 0;
            int length = sb.Length;

            if (lastCarriageReturn && length > 0 && sb[0] == '\n')
            {
                i = 1;
                num = 1;
                lastCarriageReturn = false;
            }

            while (i < length)
            {
                char c = sb[i];
                if (c == '\r' || c == '\n')
                {
                    string obj = sb.ToString(num, i - num);
                    num = i + 1;
                    if (c == '\r' && num < length && sb[num] == '\n')
                    {
                        num++;
                        i++;
                    }

                    lock (messageQueue)
                    {
                        messageQueue.Enqueue(obj);
                    }
                }

                i++;
            }

            if (sb[length - 1] == '\r')
            {
                lastCarriageReturn = true;
            }

            if (num < length)
            {
                sb.Remove(0, num);
            }
            else
            {
                sb.Length = 0;
            }

            FlushMessageQueue();
        }

        private void FlushMessageQueue()
        {
            while (messageQueue.Count > 0)
            {
                lock (messageQueue)
                {
                    if (messageQueue.Count > 0)
                    {
                        string data = (string)messageQueue.Dequeue();
                        if (!cancelOperation)
                        {
                            userCallBack(data);
                        }
                    }

                    continue;
                }
            }
        }
    }
}
