using System;
using System.Collections.Generic;



namespace New_folder
{

    public class PlaneEventArgs : EventArgs
    {
        public String nameEventArgs { get; set; }
    }

    public delegate void UserNotifications(string name, Airplane plane);

    public class Monitor
    {
        
        public void OnLandingHandler(object o, PlaneEventArgs args)
        {
            Console.WriteLine("A plane has landed --- {0}", args.nameEventArgs);
        }

        public void OnTakeoffHandler(object o, PlaneEventArgs args)
        {
            Console.WriteLine("A plane has taken off ----- {0}", args.nameEventArgs);
        }

        public void OnDelayHandler(object o, PlaneEventArgs args)
        {
            Console.WriteLine($"Plane {args.nameEventArgs} was delayed !");
        }
    }

    public static class Extensions
    {
        public static void DelayFlight(this Airport airport, Airplane plane)
        {
            airport.PlaneDelay?.Invoke(null, new PlaneEventArgs { nameEventArgs = plane.name });
            plane.delayed = true;
        }
        public static void ListPlanes(this Airport airport)
        {
            foreach (Airplane plane in airport.planes)
            {
                Console.WriteLine("{0} - {1} is at {2} airport!", plane.name, plane.capacity, airport.name);
            }
        }
    }

    public class Airplane
    {
        
        public String name { get; set; }
        public int capacity { get; set; }

        public bool delayed = false;
    }
    public class Airport
    {
        public EventHandler<PlaneEventArgs> PlaneLanding;
        public EventHandler<PlaneEventArgs> PlaneTakeoff;
        public EventHandler<PlaneEventArgs> PlaneDelay;
       
        public String name { get; set; }
        public List<Airplane> planes = new List<Airplane>();

        public void Incoming(Airplane plane)
        {
            PlaneLanding?.Invoke(this, new PlaneEventArgs { nameEventArgs = plane.name });
            planes.Add(plane);
        }
        public void Outgoing(Airplane plane)
        {
            PlaneTakeoff?.Invoke(this, new PlaneEventArgs { nameEventArgs = plane.name });
            planes.Remove(plane);
        }
    }

    public class User
    {
        
        public event UserNotifications UserPlaneDelay;
        public string name { get; set; }

        public void onUserPlaneDelay(object o, PlaneEventArgs args)
        {
            Console.WriteLine($"Sorry {name} your plane with model {args.nameEventArgs} was delayed !");

        }
        public void ListPlanes(Airport airport)
        {
            airport.ListPlanes();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Monitor monitor = new Monitor();
            Airport skopski = new Airport();
            skopski.name = "Skopski";
            Airplane mig = new Airplane();
            mig.name = "MIG";
            mig.capacity = 2;

            skopski.PlaneLanding += monitor.OnLandingHandler;
            skopski.PlaneTakeoff += monitor.OnTakeoffHandler;
            skopski.PlaneDelay += monitor.OnDelayHandler;


            User dame = new User();
            dame.name = "Dame";
            skopski.PlaneDelay += dame.onUserPlaneDelay;
           
           
         
           

            Console.WriteLine("Ovde sletuva avion");
            skopski.Incoming(mig);
            Console.WriteLine("Ovde user lista avioni");
            dame.ListPlanes(skopski);
            Console.WriteLine("Ovde se pravi delay na flight");
            skopski.DelayFlight(mig);
       
            Console.WriteLine("Ovde aerodromot lista avioni");
            skopski.ListPlanes();
            Console.WriteLine("Ovde poletuva avionot");
            
            skopski.Outgoing(mig);
            Console.ReadKey();
        }
    }
}
