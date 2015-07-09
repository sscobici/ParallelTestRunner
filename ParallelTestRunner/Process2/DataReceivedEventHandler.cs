using System;

namespace ParallelTestRunner.Process2
{
    /// <summary>Represents the method that will handle the <see cref="E:System.Diagnostics.Process.OutputDataReceived" /> event or <see cref="E:System.Diagnostics.Process.ErrorDataReceived" /> event of a <see cref="T:System.Diagnostics.Process" />.</summary>
    /// <param name="sender">The source of the event. </param>
    /// <param name="e">A <see cref="T:System.Diagnostics.DataReceivedEventArgs" /> that contains the event data. </param>
    /// <filterpriority>2</filterpriority>
    public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs e);
}
