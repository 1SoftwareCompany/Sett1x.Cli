using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elders.Pandora.Cli
{
    public static class LogStartup
    {
        public static void Boot()
        {
            log4net.Config.XmlConfigurator.Configure();
            log4net.LogManager.GetRepository().AddConsoleAppender();
        }

        public static void AddAppender(this log4net.Repository.ILoggerRepository loggerRepo, log4net.Appender.IAppender appender)
        {
            ((log4net.Repository.Hierarchy.Hierarchy)loggerRepo).Root.AddAppender(appender);
        }

        // <appender name="console" type="log4net.Appender.ColoredConsoleAppender">
        //      <mapping>
        //        <level value="ERROR" />
        //        <foreColor value="White" />
        //        <backColor value="Red, HighIntensity" />
        //      </mapping>
        //      <mapping>
        //        <level value="WARN" />
        //        <foreColor value="Yellow, HighIntensity" />
        //      </mapping>
        //      <mapping>
        //        <level value="INFO" />
        //        <foreColor value="Green" />
        //      </mapping>
        //      <layout type="log4net.Layout.PatternLayout">
        //        <conversionPattern value="%date %newline%message%newline%newline" />
        //      </layout>
        //  </appender>
        public static void AddConsoleAppender(this log4net.Repository.ILoggerRepository loggerRepo)
        {
            var appender = new log4net.Appender.ColoredConsoleAppender();
            appender.Name = "console";
            appender.Layout = new log4net.Layout.PatternLayout("%date %newline%message%newline%newline");

            var errorMapping = new log4net.Appender.ColoredConsoleAppender.LevelColors();
            errorMapping.ForeColor = log4net.Appender.ColoredConsoleAppender.Colors.Red;
            errorMapping.Level = log4net.Core.Level.Error;
            appender.AddMapping(errorMapping);

            var warnMapping = new log4net.Appender.ColoredConsoleAppender.LevelColors();
            warnMapping.BackColor = log4net.Appender.ColoredConsoleAppender.Colors.Yellow & log4net.Appender.ColoredConsoleAppender.Colors.HighIntensity;
            warnMapping.Level = log4net.Core.Level.Warn;
            appender.AddMapping(warnMapping);

            var infoMapping = new log4net.Appender.ColoredConsoleAppender.LevelColors();
            infoMapping.ForeColor = log4net.Appender.ColoredConsoleAppender.Colors.Green;
            infoMapping.Level = log4net.Core.Level.Info;
            appender.AddMapping(infoMapping);

            appender.ActivateOptions();

            loggerRepo.AddAppender(appender);

        }
    }
}
