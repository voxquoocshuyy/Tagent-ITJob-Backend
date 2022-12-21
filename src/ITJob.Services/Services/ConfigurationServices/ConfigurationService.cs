using AutoMapper;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.ViewModels.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace ITJob.Services.Services.ConfigurationServices;

public class ConfigurationService : IConfigurationService
{
    private readonly IMapper _mapper;
    public ConfigurationService(IMapper mapper)
    {
        _mapper = mapper;
    }
    public string Configuration(CongfigurationModel configurationModel)
    {
        if (configurationModel == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter configuration data!!! ");
        }
        var scoreOfSkill = configurationModel.ScoreOfSkill;
        var scoreOfWorkingStyle = configurationModel.ScoreOfWorkingStyle;
        var scoreOfJobPosition = configurationModel.ScoreOfJobPosition;
        var likeDailyLimit = configurationModel.LikeDailyLimit;
        var shareDailyLimit = configurationModel.ShareDailyLimit;
        var exchangeRate = configurationModel.ExchangeRate;
        var earnByLike = configurationModel.EarnByLike;
        var earnByShare = configurationModel.EarnByShare;
        var earnByMatch = configurationModel.EarnByMatch;
        var upgrade = configurationModel.Upgrade;

        var jsonString = File.ReadAllText(@"C:\ITJob-BE\appsettings.json");

        if (jsonString == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Configuration is null!!! ");
        }
        // Convert the JSON string to a JObject:
        var jObject = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString) as JObject;
        
        // Select a nested property using a single string:
        var scoreSkillJToken = jObject!.SelectToken("SystemConfiguration.ScoreSkill");
        if (scoreOfSkill != null)
        {
            // Update the value of the property: 
            scoreSkillJToken!.Replace(scoreOfSkill);
        }
        
        // Select a nested property using a single string:
        var scoreWorkingStyleJToken = jObject!.SelectToken("SystemConfiguration.ScoreWorkingStyle");
        if (scoreOfWorkingStyle != null)
        {
            // Update the value of the property: 
            scoreWorkingStyleJToken!.Replace(scoreOfWorkingStyle);
        }
        
        // Select a nested property using a single string:
        var scoreJobPositionJToken = jObject!.SelectToken("SystemConfiguration.ScoreJobPosition");
        if (scoreOfJobPosition != null)
        {
            // Update the value of the property: 
            scoreJobPositionJToken!.Replace(scoreOfJobPosition);
        }
        
        // Select a nested property using a single string:
        var likeDailyLimitJToken = jObject!.SelectToken("SystemConfiguration.LikeDailyLimit");
        if (likeDailyLimit != null)
        {
            // Update the value of the property: 
            likeDailyLimitJToken!.Replace(likeDailyLimit);
        }
        
        // Select a nested property using a single string:
        var shareDailyLimitJToken = jObject!.SelectToken("SystemConfiguration.ShareDailyLimit");
        if (shareDailyLimit != null)
        {
            // Update the value of the property: 
            shareDailyLimitJToken!.Replace(shareDailyLimit);
        }
        
        // Select a nested property using a single string:
        var exchangeRateJToken = jObject!.SelectToken("SystemConfiguration.ExchangeRate");
        if (exchangeRate != null)
        {
            // Update the value of the property: 
            exchangeRateJToken!.Replace(exchangeRate);
        }
        
        // Select a nested property using a single string:
        var earnByLikeJToken = jObject!.SelectToken("SystemConfiguration.EarnByLike");
        if (earnByLike != null)
        {
            // Update the value of the property: 
            earnByLikeJToken!.Replace(earnByLike);
        }
        // Select a nested property using a single string:
        var earnByShareJToken = jObject!.SelectToken("SystemConfiguration.EarnByShare");
        if (earnByShare != null)
        {
            // Update the value of the property: 
            earnByShareJToken!.Replace(earnByShare);
        }
        
        // Select a nested property using a single string:
        var earnByMatchJToken = jObject!.SelectToken("SystemConfiguration.EarnByMatch");
        if (earnByMatch != null)
        {
            // Update the value of the property: 
            earnByMatchJToken!.Replace(earnByMatch);
        }
        
        // Select a nested property using a single string:
        var upgradeJToken = jObject!.SelectToken("SystemConfiguration.Upgrade");
        if (upgrade != null)
        {
            // Update the value of the property: 
            upgradeJToken!.Replace(upgrade);
        }
        
        // Convert the JObject back to a string:
        string updatedJsonString = jObject.ToString();
        File.WriteAllText(@"C:\ITJob-BE\appsettings.json", updatedJsonString);
        
        return "Update configuration successfully!!!";
    }

    public async Task<CongfigurationModel> GetConfiguration()
    {
        var jsonString = File.ReadAllText(@"C:\ITJob-BE\appsettings.json");
        if (jsonString == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Configuration is null!!! ");
        }
        // Convert the JSON string to a JObject:
        var jObject = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString) as JObject;
        
        // Select a nested property using a single string:
        var scoreSkillJToken = jObject!.SelectToken("SystemConfiguration.ScoreSkill");

        // Select a nested property using a single string:
        var scoreWorkingStyleJToken = jObject!.SelectToken("SystemConfiguration.ScoreWorkingStyle");

        // Select a nested property using a single string:
        var scoreJobPositionJToken = jObject!.SelectToken("SystemConfiguration.ScoreJobPosition");
        
        // Select a nested property using a single string:
        var likeDailyLimitJToken = jObject!.SelectToken("SystemConfiguration.LikeDailyLimit");
        
        // Select a nested property using a single string:
        var shareDailyLimitJToken = jObject!.SelectToken("SystemConfiguration.ShareDailyLimit");
        
        // Select a nested property using a single string:
        var exchangeRateJToken = jObject!.SelectToken("SystemConfiguration.ExchangeRate");
        
        // Select a nested property using a single string:
        var earnByLikeJToken = jObject!.SelectToken("SystemConfiguration.EarnByLike");
        
        // Select a nested property using a single string:
        var earnByShareJToken = jObject!.SelectToken("SystemConfiguration.EarnByShare");
        
        // Select a nested property using a single string:
        var earnByMatchJToken = jObject!.SelectToken("SystemConfiguration.EarnByMatch");
        
        // Select a nested property using a single string:
        var upgradeJToken = jObject!.SelectToken("SystemConfiguration.Upgrade");

        var configuration = new CongfigurationModel()
        {
            ScoreOfSkill = (int?)scoreSkillJToken,
            ScoreOfWorkingStyle = (int?)scoreWorkingStyleJToken,
            ScoreOfJobPosition = (int?)scoreJobPositionJToken,
            LikeDailyLimit = (int?)likeDailyLimitJToken,
            ShareDailyLimit = (int?)shareDailyLimitJToken,
            ExchangeRate = (int?)exchangeRateJToken,
            EarnByLike = (int?)earnByLikeJToken,
            EarnByShare = (int?)earnByShareJToken,
            EarnByMatch = (int?)earnByMatchJToken,
            Upgrade = (int?)upgradeJToken,
        };
        var result = _mapper.Map<CongfigurationModel>(configuration);
        return result;
    }
}