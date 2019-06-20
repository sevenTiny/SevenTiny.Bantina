﻿/*********************************************************
 * CopyRight: 7TINY CODE BUILDER. 
 * Version: 5.0.0
 * Author: 7tiny
 * Address: Earth
 * Create: 2018-02-18 21:10:34
 * Modify: 2018-02-18 21:10:34
 * E-mail: dong@7tiny.com | sevenTiny@foxmail.com 
 * GitHub: https://github.com/sevenTiny 
 * Personal web site: http://www.7tiny.com 
 * Technical WebSit: http://www.cnblogs.com/7tiny/ 
 * Description: 
 * Thx , Best Regards ~
 *********************************************************/
using SevenTiny.Bantina.Configuration;
using System;
using System.Linq;

namespace SevenTiny.Bantina.Logging.Infrastructure
{
    [ConfigName("Logging")]
    internal class LoggingConfig : MySqlRowConfigBase<LoggingConfig>
    {
        [ConfigProperty]
        public string Group { get; set; }
        [ConfigProperty]
        public string Directory { get; set; }
        [ConfigProperty]
        public int Level_Info { get; set; }
        [ConfigProperty]
        public int Level_Debug { get; set; }
        [ConfigProperty]
        public int Level_Warn { get; set; }
        [ConfigProperty]
        public int Level_Error { get; set; }
        [ConfigProperty]
        public int Level_Fatal { get; set; }

        private static LoggingConfig _loggingConfig;

        public static LoggingConfig Get()
        {
            if (_loggingConfig != null)
            {
                return _loggingConfig;
            }
            //load group config
            _loggingConfig = Instance?.FirstOrDefault(t => t.Group.Equals(AppSettingsConfigHelper.GetAppName()));
            if (_loggingConfig != null)
            {
                return _loggingConfig;
            }
            //if group not found,load root
            _loggingConfig = Instance?.FirstOrDefault(t => t.Group.Equals("Default"));
            if (_loggingConfig != null)
            {
                return _loggingConfig;
            }
            throw new EntryPointNotFoundException("[AppName] group not fount and [Default] goup not found also.please set least one group in logging config.");
        }
    }
    internal static class LoggingConfigHelper
    {
        public static bool CheckLevelOpen(LoggingLevel loggingLevel)
        {
            switch (loggingLevel)
            {
                case LoggingLevel.Info: return LoggingConfig.Get().Level_Info == 1;
                case LoggingLevel.Debug: return LoggingConfig.Get().Level_Debug == 1;
                case LoggingLevel.Warn: return LoggingConfig.Get().Level_Warn == 1;
                case LoggingLevel.Error: return LoggingConfig.Get().Level_Error == 1;
                case LoggingLevel.Fatal: return LoggingConfig.Get().Level_Fatal == 1;
                default: return false;
            }
        }
    }
}
