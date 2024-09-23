using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

public class Example
{
    static void Main()
    {
        Console.WriteLine($"{nameof(RuntimeInformation.OSArchitecture)}: {RuntimeInformation.OSArchitecture}");
        Console.WriteLine($"{nameof(RuntimeInformation.OSDescription)}: {RuntimeInformation.OSDescription}");
        Console.WriteLine($"{nameof(RuntimeInformation.FrameworkDescription)}: {RuntimeInformation.FrameworkDescription}");
        Console.WriteLine();

        Console.WriteLine("\r\nExists Certs Name and Location");
        Console.WriteLine("------ ----- -------------------------");

        foreach (StoreLocation storeLocation in (StoreLocation[])
            Enum.GetValues(typeof(StoreLocation)))
        {
            foreach (StoreName storeName in (StoreName[])
                Enum.GetValues(typeof(StoreName)))
            {
                X509Store store = new X509Store(storeName, storeLocation);

                try
                {
                    store.Open(OpenFlags.OpenExistingOnly);

                    Console.WriteLine("Yes    {0,4}  {1}, {2}",
                        store.Certificates.Count, store.Name, store.Location);
                }
                catch (CryptographicException)
                {
                    Console.WriteLine("No           {0}, {1}",
                        store.Name, store.Location);
                }
            }
            Console.WriteLine();
        }

            ShowStore(StoreName.My, StoreLocation.CurrentUser);
            ShowStore(StoreName.Root, StoreLocation.LocalMachine);
    }

    private static void ShowStore(StoreName storeName, StoreLocation location) {
        try {
            using (var store = new X509Store(storeName, location)) {
                Console.WriteLine($"Looking into {store.Name} at {store.Location}");
                store.Open(OpenFlags.OpenExistingOnly);
                Console.WriteLine($"Found {store.Certificates.Count} certificates");
                foreach (var cert in store.Certificates) {
                    var nameinfo = cert.GetNameInfo(X509NameType.DnsName, false);
                    Console.WriteLine($"{nameinfo} {cert.SubjectName.Name} - {cert.Thumbprint}");
                }
            }
        } catch {
            Console.WriteLine($"The store {storeName} does not exist!");
        }
        Console.WriteLine();
    }
}

/* This example produces output similar to the following:

Exists Certs Name and Location
------ ----- -------------------------
Yes       1  AddressBook, CurrentUser
Yes      25  AuthRoot, CurrentUser
Yes     136  CA, CurrentUser
Yes      55  Disallowed, CurrentUser
Yes      20  My, CurrentUser
Yes      36  Root, CurrentUser
Yes       0  TrustedPeople, CurrentUser
Yes       1  TrustedPublisher, CurrentUser

No           AddressBook, LocalMachine
Yes      25  AuthRoot, LocalMachine
Yes     131  CA, LocalMachine
Yes      55  Disallowed, LocalMachine
Yes       3  My, LocalMachine
Yes      36  Root, LocalMachine
Yes       0  TrustedPeople, LocalMachine
Yes       1  TrustedPublisher, LocalMachine

 */