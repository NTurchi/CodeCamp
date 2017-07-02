using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MyCodeCamp.Data.Entities;

namespace MyCodeCamp.Data
{
  public class CampDbInitializer
  {
    private CampContext _ctx;

    public CampDbInitializer(CampContext ctx)
    {
      _ctx = ctx;
    }

    public async Task Seed()
    {
      if (!_ctx.Camps.Any())
      {
        // Add Data
        _ctx.AddRange(_sample);
        await _ctx.SaveChangesAsync();
      }
    }

    List<Camp> _sample = new List<Camp>
    {
      new Camp()
      {
        Name = "Microsoft Seattle Code Camp",
        Moniker = "SEA2017",
        EventDate = DateTime.Today.AddDays(45),
        Length = 1,
        Description = "Microsoft Build",
        Location = new Location()
        {
          Address1 = "123 foo street",
          CityTown = "Seattle",
          StateProvince = "WA",
          PostalCode = "98101",
          Country = "USA"
        },
        Speakers = new List<Speaker>
        {
          new Speaker()
          {
            Name = "Turchi Nicolas",
            Bio = ".NET Developer",
            CompanyName = "Numen Solutions",
            GitHubName = "nturchi",
            TwitterName = "none",
            PhoneNumber = "00000000",
            HeadShotUrl = "https://media.licdn.com/mpr/mpr/shrinknp_400_400/AAEAAQAAAAAAAAxIAAAAJDc3MzlkZGMxLTU2OTEtNDNiNC1hNzY3LWQ4MWYzOGNkODU4ZQ.jpg",
            WebsiteUrl = "https://www.linkedin.com/in/nicolas-turchi-09391812a/",
            Talks = new List<Talk>()
            {
              new Talk()
              {
                Title =  "Microsoft Cognitive Services",
                Abstract = "How to improve your apps with Microsoft Cognitive Services",
                Category = ".NET development",
                Level = "50",
                Prerequisites = "Using API Experience",
                Room = "97",
                StartingTime = DateTime.Parse("14:30")
              },
              new Talk()
              {
                Title =  "Application deployment on Microsoft Azure",
                Abstract = "Deploy application on Microsoft Azure",
                Category = "Application Production",
                Level = "70",
                Prerequisites = "None",
                Room = "52",
                StartingTime = DateTime.Parse("13:00")
              },
            }
          },
        }
      }
    };

  }
}
