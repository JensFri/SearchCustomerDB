﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchCustomerOrders
{
    class Program
    {
        static Boolean Boproduct = true;
        static double zwischenSpeicher = 0.0;
        static decimal wDiscount = 0;
        static decimal woDiscount = 0;
        static double sumWDiscount = 0.0;
        static double sumWoDiscount = 0.0;
        static double discount = 0.0;
        static double total = 0.0;
        static double totalDiscount = 0.0;

        static void Main(string[] args)
        {

            String eingabe = String.Empty;
            Boolean ende = true;


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
                        Console.Write("Geben Sie den Customer ein nach dem Sie suchen:\t");
                        eingabe = Console.ReadLine();

                        SearchCustomer(eingabe);

                        break;
                    case "2":
                        Boproduct = true;
                        while (Boproduct)
                        {
                            Console.Write("Geben sie eine CustomerID ein:\t");
                        eingabe = Console.ReadLine();                       
                            ShowOrder(eingabe);                            
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


        private static void SearchCustomer(String eingabe)
        {
            using (var db = new NorthwindContext())
            {


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
        }

        private static void ShowOrder(String eingabe)
        {
            using (var db = new NorthwindContext())
            {
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

                    var orderid = db.Order_Details.Where(x => x.OrderID == item.OrderID);
                    var withDiscount = orderid.Where(x => x.Discount > 0);
                    var withoutDiscount = orderid.Where(x => x.Discount == 0);

                    if (withDiscount.Count() != 0)
                    {
                        if (withDiscount.Count() > 1)
                        {

                            for (int i = 0; i < withDiscount.Count(); i++)
                            {
                                var test = withDiscount.OrderBy(x => x.OrderID).Skip(i).Take(1);
                                wDiscount = test.Sum(x => x.UnitPrice * x.Quantity);
                                zwischenSpeicher = Convert.ToDouble(wDiscount);
                                discount = test.Sum(x => zwischenSpeicher * x.Discount);
                                sumWDiscount = test.Sum(x => zwischenSpeicher - discount);
                                totalDiscount = totalDiscount + sumWDiscount;
                            }
                        }
                        else
                        {
                            wDiscount = withDiscount.Sum(x => x.UnitPrice * x.Quantity);
                            zwischenSpeicher = Convert.ToDouble(wDiscount);
                            discount = withDiscount.Sum(x => zwischenSpeicher * x.Discount);
                            sumWDiscount = withDiscount.Sum(x => zwischenSpeicher - discount);
                            totalDiscount = totalDiscount + sumWDiscount;
                        }
                    }

                    if (withoutDiscount.Count() != 0)
                    {
                        woDiscount = withoutDiscount.Sum(x => x.UnitPrice * x.Quantity);
                        sumWoDiscount = Convert.ToDouble(woDiscount);
                    }

                    total = totalDiscount + sumWoDiscount;

                    Console.WriteLine("{0}\t {1}\t {2}\t {3:0.00}", item.OrderID, item.ShipName, item.ShipAddress, total);

                    sumWoDiscount = 0.0;
                    totalDiscount = 0.0;
                }

                Console.Write("Geben Sie eine OrderID ein, um die genauen Details der Bestellung anzusehen.\t");
                eingabe = Console.ReadLine();
                int id = Convert.ToInt32(eingabe);

              
                var bestellungen = db.Order_Details.Where(x => x.OrderID == id);


                foreach (var item in bestellungen)
                {
                    var products = db.Products.Where(x => x.ProductID == item.ProductID);

                    foreach (var product in products)
                    {
                        Console.WriteLine("{0}\t {1}\t {2}\t {3}\t ", item.OrderID, product.ProductName, item.Quantity, item.UnitPrice);
                    }
                }

                Console.WriteLine("1. Zurück zur Bestellübersicht.\n");
                Console.WriteLine("2. Zurück zum Menü.\n");
                eingabe = Console.ReadLine();

              if(eingabe != "1")
                {
                    Boproduct = false;
                }
            }
        }
    }
}



    

