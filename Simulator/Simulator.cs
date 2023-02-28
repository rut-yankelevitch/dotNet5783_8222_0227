using BlApi;
using System.Diagnostics;


namespace Simulator;
public static class Simulator
{
    private static IBl bl = BlApi.Factory.Get();

    private static BO.Order order = new();
    private static Thread? myThread { get; set; }

    private static bool doWork = true;

    private static Stopwatch? myStopWatch { get; set; }

    public class OrderEventArgs : EventArgs
    {
        public BO.Order order;
        public int seconds;
        public OrderEventArgs(int newSecond, BO.Order newOrder)
        {
            order = newOrder;
            seconds = newSecond;
        }
    }

    public static event EventHandler? StopEvent;

    public static event EventHandler<OrderEventArgs>? PropsChangedEvent;

    public static void Run()
    {
        doWork = true;
        myThread = new Thread(new ThreadStart(Simulation));
        myThread.Start();
    }


    private static void Simulation()
    {
        while (doWork)
        {
            int? orderID = bl.Order.SelectOrder();
            if (orderID == null)
            {
                StopEvent!("", EventArgs.Empty);
                break;
            }
            Random rnd = new Random();
            int seconds = rnd.Next(8000, 15000);
            order = bl.Order.GetOrderById((int)orderID);
            PropsChangedEvent!("", new OrderEventArgs(seconds, order));
            while (doWork && seconds > 0)
            {
                Thread.Sleep(1000);
                seconds-=1000;
            }
            if (doWork)
            {
                if (order.Status == BO.OrderStatus.Confirmed_Order)
                    bl.Order.UpdateSendOrderByManager(order.ID);
                else
                    bl.Order.UpdateSupplyOrderByManager(order.ID);
            }
        }
    }

    public static void Stop()
    {
        doWork = false;
    }
}


