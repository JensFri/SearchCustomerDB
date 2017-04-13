using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchCustomerOrders
{
    class Program
    {
        static void Main(string[] args)
        {

            String eingabe = String.Empty;
            Boolean ende = true;
            double zwischenSpeicher = 0.0;
            decimal withoutDiscount = 0;
            double withDiscount = 0.0;
            double discount = 0.0;



            while (ende)
            {
                Console.WriteLine("Menü\n");
                Console.WriteLine("1. Nach Customer suchen:\t");
                Console.WriteLine("2. CustomerID eingeben:\t");
                Console.WriteLine("3. Quit Application\n");


                eingabe = Console.ReadLine();

                switch (eingabe)
                {
                    case "1":
                        using (var db = new NorthwindContext())
                        {
                            Console.Write("Geben Sie den Customer ein nach dem Sie suchen:\t");
                            eingabe = Console.ReadLine();

                            
                            var customer = db.Customers.Where(u => u.CompanyName.Contains(eingabe) || u.ContactName.Contains(eingabe));
                            
                            
                           

                            if (customer.Count() == 0)
                            {
                                Console.WriteLine("\nEs wurden keine Customer mit dem Namen {0} gefunden\n\n", eingabe);
                            }

                           
                       
                            foreach (var item in customer)
                            {
                                Console.WriteLine("\n{0}\t {1}\t {2}", item.CustomerID, item.CompanyName, item.ContactName);
                                Console.WriteLine("_______________________________________________________________________\n\n");
                            }
                        }
                        break;
                    case "2":
                        using (var db = new NorthwindContext())
                        {
                            Console.Write("Geben sie eine CustomerID ein:\t");
                            eingabe = Console.ReadLine();

                            var customer = db.Orders.Where(u => u.CustomerID == eingabe);

                            if (customer.Count() == 0)
                            {
                                Console.WriteLine("\nEs wurden keine Bestellungen mit der CustomerID {0} gefunden\n\n", eingabe);
                            }
                            else
                            {
                                Console.WriteLine("\nAlle Bestellungen zugehörig der CustomerID:");
                            }

                            

                            foreach (var item in customer)
                            {

                                //var orderid = db.Order_Details.Where(x => x.OrderID == item.OrderID);
                                //var total = orderid.Sum(x => x.UnitPrice * x.Quantity);
                                //var test = Convert.ToDouble(total);
                                //var total1 = orderid.Sum(x => test / (x.Discount / 100));

                                var orderid = db.Order_Details.Where(x => x.OrderID == item.OrderID);   
                                
                                                             
                                withoutDiscount = orderid.Sum(x => x.UnitPrice * x.Quantity);                                
                                zwischenSpeicher = Convert.ToDouble(withoutDiscount);


                                discount = orderid.Sum(x => zwischenSpeicher * x.Discount);

                                if (discount != 0)
                                {
                                    withDiscount = orderid.Sum(x => zwischenSpeicher - discount);



                                    Console.WriteLine("{0}\t {1}\t {2}\t {3:0.00}", item.OrderID, item.ShipName, item.ShipAddress, withDiscount);
                                }
                                else
                                {
                                    Console.WriteLine("{0}\t {1}\t {2}\t {3:0.00}", item.OrderID, item.ShipName, item.ShipAddress, withoutDiscount);
                                }
                            }
                        }
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Die Eingabe stimmt nicht mit den Zahlen im Menü überein(1,2,3)\n");
                        break;
                }
            }



        }
    }
}
    

