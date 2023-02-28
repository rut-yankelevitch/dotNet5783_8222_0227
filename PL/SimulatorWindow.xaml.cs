using BlApi;
using BlImplementation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static Simulator.Simulator;

namespace PL;

/// <summary>
/// Interaction logic for SimulatorWindow.xaml
/// </summary>
public partial class SimulatorWindow : Window
{
    IBl bl;
    BackgroundWorker worker;

    #region the closing button
    private const int GWL_STYLE = -16;
    private const int WS_SYSMENU = 0x80000;

    public event PropertyChangedEventHandler? PropertyChanged;
    [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    void ToolWindow_Loaded(object sender, RoutedEventArgs e)
    {
        var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
        SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
    }
    #endregion
    bool isTimerRun;
    Duration duration;
    DoubleAnimation doubleanimation;
    ProgressBar ProgressBar;
    private int seconds;
    DispatcherTimer _timer;
    TimeSpan _time;



    public int TreatmentTime
    {
        get { return (int)GetValue(TreatmentTimeProperty); }
        set { SetValue(TreatmentTimeProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Time.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TreatmentTimeProperty =
        DependencyProperty.Register("TreatmentTime", typeof(int), typeof(Window), new PropertyMetadata(0));


    public BO.OrderStatus OrderStatus
    {
        get { return (BO.OrderStatus)GetValue(OrderStatusProperty); }
        set { SetValue(OrderStatusProperty, value); }
    }
    // Using a DependencyProperty as the backing store for OrderStatus.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OrderStatusProperty =
        DependencyProperty.Register("OrderStatus", typeof(BO.OrderStatus), typeof(Window), new PropertyMetadata(null));


    public int OrderId
    {
        get { return (int)GetValue(OrderIdProperty); }
        set { SetValue(OrderIdProperty, value); }
    }
    // Using a DependencyProperty as the backing store for OrderId.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OrderIdProperty =
        DependencyProperty.Register("OrderId", typeof(int), typeof(Window), new PropertyMetadata(0));


    public string Time
    {
        get { return (string)GetValue(TimeProperty); }
        set { SetValue(TimeProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Time.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TimeProperty =
        DependencyProperty.Register("Time", typeof(string), typeof(Window), new PropertyMetadata(null));


    public static readonly DependencyProperty ProgressBarValueProperty =
        DependencyProperty.Register("ProgressBarValue", typeof(double), typeof(MainWindow), new PropertyMetadata(0.0));
    public double ProgressBarValue
    {
        get { return (double)GetValue(ProgressBarValueProperty); }
        set { SetValue(ProgressBarValueProperty, value); }
    }


    public string Timer
    {
        get { return (string)GetValue(TimerProperty); }
        set { SetValue(TimerProperty, value); }
    }
    // Using a DependencyProperty as the backing store for Timer.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty TimerProperty =
        DependencyProperty.Register("Timer", typeof(string), typeof(Window), new PropertyMetadata(null));



    private void timer(int sec)
    {
        _time = TimeSpan.FromSeconds(sec);

        _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
        {
            Timer = string.Format("{0:D2}", _time.Seconds);
            if (_time == TimeSpan.Zero) _timer.Stop();
            _time = _time.Add(TimeSpan.FromSeconds(-1));
        }, Application.Current.Dispatcher);

        _timer.Start();
    }
    public SimulatorWindow(IBl Bl)
    {
        InitializeComponent();
        bl = Bl;
        Loaded += ToolWindow_Loaded;
        workerStart();
    }
    void ProgressBarStart(int s)
    {
        if (ProgressBar != null) SBar.Items.Remove(ProgressBar);
        ProgressBar = new ProgressBar();
        ProgressBar.IsIndeterminate = false;
        ProgressBar.Orientation = Orientation.Horizontal;
        ProgressBar.Width = 500;
        ProgressBar.Height = 30;
        duration = new Duration(TimeSpan.FromSeconds(s * 2));
        doubleanimation = new DoubleAnimation(200.0, duration);
        ProgressBar.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);
        SBar.Items.Add(ProgressBar);
    }

    void workerStart()
    {
        isTimerRun = true;
        worker = new BackgroundWorker();
        worker.DoWork += WorkerDoWork!;
        worker.WorkerReportsProgress = true;
        worker.WorkerSupportsCancellation = true;
        worker.ProgressChanged += workerProgressChanged!;
        worker.RunWorkerCompleted += RunWorkerCompleted!;
        worker.RunWorkerAsync();
    }


    void WorkerDoWork(object sender, DoWorkEventArgs e)
    {
        PropsChangedEvent += progressChanged!;
        StopEvent += stop!;
        Run();
        while (!worker.CancellationPending && isTimerRun)
        {
            worker.ReportProgress(1);
            Thread.Sleep(1000);
        }
    }


    void workerProgressChanged(object sender, ProgressChangedEventArgs e) =>Time = DateTime.Now.ToString("h:mm:ss");

    private void stopSimulatorBtn_Click(object sender, RoutedEventArgs e) => stop(sender, EventArgs.Empty);

    void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) => this.Close();

    void stop(object sender, EventArgs e)
    {
        if (worker.WorkerSupportsCancellation == true)
            worker.CancelAsync();

        if (isTimerRun)
            isTimerRun = false;
        Stop();
        PropsChangedEvent-= progressChanged!;
        StopEvent-= stop!;
        if (!CheckAccess())
            Dispatcher.BeginInvoke(stop, sender, e);
        else
        {
            Close();
        }
    }


    void progressChanged(object sender, EventArgs e)
    {
        OrderEventArgs orderEventArgs = (OrderEventArgs)e;
        seconds = orderEventArgs.seconds / 1000;
        if (!CheckAccess())
        {
            Dispatcher.BeginInvoke(progressChanged, sender, e);
        }
        else
        {
            timer(seconds - 1);
            ProgressBarStart(seconds);
            OrderId = orderEventArgs.order.ID;
            TreatmentTime = seconds;
            OrderStatus = (BO.OrderStatus)orderEventArgs.order.Status!;
        }
    }
}
