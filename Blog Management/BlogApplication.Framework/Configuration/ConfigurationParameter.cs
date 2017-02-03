using System;

namespace BlogApplication.Framework.Configuration
{
    public static class ConfigurationParameter
    {
        public static string GetParameter(string key, string defaultValue = "")
        {
            try
            {
                string returnValue = defaultValue;
                if (!string.IsNullOrEmpty(key) && System.Configuration.ConfigurationManager.AppSettings[key] != null)
                    returnValue = System.Configuration.ConfigurationManager.AppSettings[key];
                return returnValue;
            }
            catch (Exception)
            {
                
                return defaultValue;
            }
        }

        public static void SetParameter(string key, string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                    if (config.AppSettings.Settings[key] == null)
                        config.AppSettings.Settings.Add(key, value);
                    else
                        config.AppSettings.Settings[key].Value = value;
                    config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                    System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                }
            }
            catch { }
        }

        public static void AddKey(string key, string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                    if (config.AppSettings.Settings[key] == null)
                        config.AppSettings.Settings.Add(key, value);
                    else
                        config.AppSettings.Settings[key].Value = value;
                    config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                    System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                }
            }
            catch (Exception)
            {
                
                 
            }
        }

        public static void RemoveKey(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                    if (config.AppSettings.Settings[key] != null)
                    {
                        config.AppSettings.Settings.Remove(key);
                        config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                        System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                    }
                }
            }
            catch { }
        }

        public static string EncryptKey
        {
            get { return GetParameter("EncryptKey");}
        }

        //STMP PORT
        //Smtp server
        //smtp user
        //smtp password
        //Google
        //Facebook
        //Twitter
    }
}